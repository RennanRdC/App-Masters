using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class JsonElement
{
    public string id;
    public string title;
}

[System.Serializable]
public class JsonCollections
{
    public JsonElement[] collections;
}

[System.Serializable]
public class JsonItems
{
    public JsonElement[] items;
}



public static class JsonReader
{
   //Read Collections JSON
    public static JsonCollections GetCollections()
	{
        TextAsset jsonFile = Resources.Load<TextAsset>("JSON/collections");

        JsonCollections collectionsInJson = JsonUtility.FromJson<JsonCollections>(jsonFile.text);

        return collectionsInJson;
    }


    //Get Items based on ID
    public static JsonItems GetItems(string id)
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("JSON/items-" + id);

        JsonItems collectionsInJson = JsonUtility.FromJson<JsonItems>(jsonFile.text);

        return collectionsInJson;
    }
}




