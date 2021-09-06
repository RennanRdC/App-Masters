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
    public static List<JsonItems> InitializeJSON()
	{
        List<JsonItems> Collections = new List<JsonItems>();

        TextAsset jsonFile = Resources.Load<TextAsset>("JSON/collections");

        JsonCollections collectionsInJson = JsonUtility.FromJson<JsonCollections>(jsonFile.text);

        foreach (JsonElement jsonElement in collectionsInJson.collections)
        {
            TextAsset collectionJsonFile = Resources.Load<TextAsset>("JSON/items-" + jsonElement.id);

            JsonItems subcollectionsInJson = JsonUtility.FromJson<JsonItems>(collectionJsonFile.text);
            Collections.Add(subcollectionsInJson);
        }

        return Collections;
    }

    public static JsonCollections GetCollections()
	{
        TextAsset jsonFile = Resources.Load<TextAsset>("JSON/collections");

        JsonCollections collectionsInJson = JsonUtility.FromJson<JsonCollections>(jsonFile.text);

        return collectionsInJson;
    }


    public static JsonItems GetItens(string id)
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("JSON/items-" + id);

        JsonItems collectionsInJson = JsonUtility.FromJson<JsonItems>(jsonFile.text);

        return collectionsInJson;
    }
}




