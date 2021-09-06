using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ProjectManager : MonoBehaviour
{
    public JsonCollections Collections;
    public JsonItems Items;
    public List<GameObject> panels;

    public int currentPanel = 0;
    public int collectionSelected = -1;
    public int itemSelected = -1;

    public Button backButton;
    public Button nextButton;

    public TMP_InputField widthInputField;
    public TMP_InputField depthInputField;

    public TextMeshProUGUI selectedCollectionText;
    public TextMeshProUGUI selectedItemText;

    public Transform panelContainer;
    public GameObject panelPrefab;
    public GameObject buttonPrefab;

    public MeshGenerator meshGenerator;

    // Start is called before the first frame update
    void Start()
    {
        Collections = JsonReader.GetCollections();

        InitializePanels();


    }






	void InitializePanels()
	{
        SelectPanel(0);

		for (int i = 0; i < Collections.collections.Length; i++)
		{
            GameObject button = GameObject.Instantiate(buttonPrefab);
            button.transform.SetParent(panels[0].transform.GetChild(0).transform);
            button.GetComponentInChildren<TextMeshProUGUI>().text = Collections.collections[i].title;
            button.GetComponent<Button>().onClick.AddListener(() => SelectCollection(button.transform.GetSiblingIndex()));
        }

        CheckNext();
    }










    public void SelectCollection(int index)
	{
        collectionSelected = index;

        foreach (Transform child in panels[0].transform.GetChild(0))
        {
            child.GetComponent<Outline>().enabled = collectionSelected == child.GetSiblingIndex();
        }

        selectedCollectionText.text = Collections.collections[index].title;


        InitializeItens(index);

        CheckNext();
    }

    public void SelectItem(int index)
    {
        itemSelected = index;

        foreach (Transform child in panels[1].transform.GetChild(0))
        {
            child.GetComponent<Outline>().enabled = itemSelected == child.GetSiblingIndex();
        }

        selectedItemText.text = Items.items[index].title;

        CheckNext();
    }

    public void InitializeItens(int index)
	{
        foreach (Transform child in panels[1].transform.GetChild(0))
        {
            Destroy(child.gameObject);
        }

        itemSelected = -1;

        Items = JsonReader.GetItens(Collections.collections[index].id);

        for (int i = 0; i < Items.items.Length; i++)
        {
            GameObject button = GameObject.Instantiate(buttonPrefab);
            button.transform.SetParent(panels[1].transform.GetChild(0).transform);
            button.GetComponentInChildren<TextMeshProUGUI>().text = Items.items[i].title;
            button.GetComponent<Button>().onClick.AddListener(() => SelectItem(button.transform.GetSiblingIndex()));
        }
    }


	public void NextPanel()
	{
        currentPanel++;
        SelectPanel(currentPanel);

        CheckNext();

        if(currentPanel == 3)
		{
            GenerateMesh();
            
            CalculateCamera();


        }
    }

    public void BackPanel()
	{
        currentPanel--;
        SelectPanel(currentPanel);
    }

    public void SelectPanel(int index)
	{
        foreach (GameObject panel in panels)
        {
            panel.SetActive(false);
        }

        panels[index].SetActive(true);


        backButton.gameObject.SetActive(index > 0);

        if(currentPanel == 3)
		{
            backButton.gameObject.SetActive(false);
            nextButton.gameObject.SetActive(false);
        }

    }

    public void Restart()
	{
        SceneManager.LoadScene(0);
	}


    public void CheckNext()
	{
        if(currentPanel == 0 && collectionSelected == -1)
		{
            nextButton.interactable = false;
            return;
		}

        if (currentPanel == 1 && itemSelected == -1)
        {
            nextButton.interactable = false;
            return;
        }



        if( currentPanel == 2 && (string.IsNullOrEmpty(depthInputField.text) || string.IsNullOrEmpty(widthInputField.text)))
		{
            nextButton.interactable = false;
            return;
        }

        nextButton.interactable = true;
    }





    public void GenerateMesh()
	{
        meshGenerator.GenerateMesh(float.Parse(widthInputField.text), float.Parse(depthInputField.text));
    }


    public void CalculateCamera()
	{
        float w = float.Parse(widthInputField.text);
        float d = float.Parse(depthInputField.text);

        float bigger = w > d ? w : d;

        

        Camera.main.transform.position = new Vector3(Camera.main.transform.position.x - float.Parse(widthInputField.text) / 2, Camera.main.transform.position.y, Camera.main.transform.position.z);
        Vector3 direction = Camera.main.transform.position - (meshGenerator.transform.position - new Vector3(float.Parse(widthInputField.text) / 2, 0, float.Parse(depthInputField.text) / 2));
        Camera.main.transform.position = Camera.main.transform.position + direction.normalized * bigger/2 + new Vector3(0,bigger,0);

    }

}
