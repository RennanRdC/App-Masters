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

    //Project Mathods


    //Initialize the collections panel
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

    //Initialize the itens panel based on collection
    public void InitializeItens(int index)
    {
        foreach (Transform child in panels[1].transform.GetChild(0))
        {
            Destroy(child.gameObject);
        }

        itemSelected = -1;

        Items = JsonReader.GetItems(Collections.collections[index].id);

        for (int i = 0; i < Items.items.Length; i++)
        {
            GameObject button = GameObject.Instantiate(buttonPrefab);
            button.transform.SetParent(panels[1].transform.GetChild(0).transform);
            button.GetComponentInChildren<TextMeshProUGUI>().text = Items.items[i].title;
            button.GetComponent<Button>().onClick.AddListener(() => SelectItem(button.transform.GetSiblingIndex()));

            Image buttonImage = button.transform.GetChild(0).GetChild(0).GetComponent<Image>();
            Sprite sprite = Resources.Load<Sprite>("Images/" + Items.items[i].title);
            if (sprite)
            {
                buttonImage.sprite = sprite;
            }

        }
    }

    //Called when select a collection
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

    //Called when select a item
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






    //UI 


    //The "Next" ui button calls this function
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

    //The "Back" ui button calls this function
    public void BackPanel()
	{
        currentPanel--;
        SelectPanel(currentPanel);
        
        CheckNext();
    }

    //This method takes care of screen switching
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

    //Restart button function
    public void Restart()
	{
        SceneManager.LoadScene(0);
	}

    //Checks when "Next" button must be interactive
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











    //Mesh And Camera


    //Calls the method for generating the mesh
    public void GenerateMesh()
	{
        meshGenerator.GenerateMesh(float.Parse(widthInputField.text)/100, float.Parse(depthInputField.text)/100);
    }

    //Calculates the camera distance and position
    public void CalculateCamera()
	{
        float w = float.Parse(widthInputField.text)/100;
        float d = float.Parse(depthInputField.text)/100;

        float bigger = w > d ? w : d;


        

        Camera.main.transform.position = new Vector3(Camera.main.transform.position.x - w/2, Camera.main.transform.position.y, Camera.main.transform.position.z);
        Vector3 direction = Camera.main.transform.position - (meshGenerator.transform.position - new Vector3(w/2, 0, d/2));
        Camera.main.transform.position = Camera.main.transform.position + direction.normalized * bigger/2 + new Vector3(0,bigger,0);

    }

}
