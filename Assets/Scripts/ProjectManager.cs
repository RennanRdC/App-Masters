using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectManager : MonoBehaviour
{
    public List<JsonItems> Collections;
    // Start is called before the first frame update
    void Start()
    {
        Collections = JsonReader.InitializeJSON();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
