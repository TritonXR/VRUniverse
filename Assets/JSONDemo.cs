using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class JSONDemo : MonoBehaviour {

	string path;
	string jsonString;
    public planetsArray mainObject;
    public Planet innerObject;
    List<Planet> objectList = new List<Planet>();
    // Use this for initialization
  
    public Planet createSubObject(string Name, string Creator,string Description,
                                    int Year,string[]Tags)
    {
        Planet innerObject = new Planet();
        innerObject.Name = Name;
        innerObject.Creator = Creator;
        innerObject.Description = Description;
        innerObject.Year = Year;
        innerObject.Tags = Tags;
        return innerObject;
    }
    void Start()
	{
        /*objectList.Add(createSubObject("1Universe", "Molly", "sample", 2017,["White", "Star"]));
        objectList.Add(createSubObject("2Universe", "2Molly", "2sample", 2018,["2White", "2Star"]));
        mainObject.Universe = objectList.ToArray();
        */
        //string planetToJson = JsonHelper.ToJson(mainObject, true);
		path = Application.streamingAssetsPath+"/Planet.json";
		jsonString = File.ReadAllText(path);
        Debug.Log("string is: " + jsonString);
		Planet[] universe = JsonHelper.FromJson<Planet>(jsonString);
        Debug.Log(universe[0].Name);
    }

}

[System.Serializable]
public class planetsArray
{
    public Planet[] Universe;
}

[System.Serializable]

public class Planet{
	public string Name;
	public string Creator;
	public string Description;
	public int Year;
	public string[] Tags;
}

public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Items;
    }

    public static string ToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper);
    }

    public static string ToJson<T>(T[] array, bool prettyPrint)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper, prettyPrint);
    }

    [System.Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }
}