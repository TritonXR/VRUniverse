using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class JSONDemo : MonoBehaviour {

	string path;
	string jsonString;
    public GameObject planetObject;
  
    // Use this for initialization
  
    void Start()
	{
       
        //string planetToJson = JsonHelper.ToJson(mainObject, true);
		path = Application.streamingAssetsPath+"/Planet.json";
		jsonString = File.ReadAllText(path);
        Debug.Log("string is: " + jsonString);
        Planet[] universe = JsonHelper.FromJson<Planet>(jsonString);
        Debug.Log(universe[1].Tags[0]);
        planetObject.GetComponent<PlanetData>().title = universe[0].Name;
        planetObject.GetComponent<PlanetData>().creator = universe[0].Creator;
        planetObject.GetComponent<PlanetData>().year = universe[0].Year;
        planetObject.GetComponent<PlanetData>().description = universe[0].Description;
        //planetObject.GetComponent<PlanetData>().image;
        planetObject.GetComponent<PlanetData>().des_tag = universe[0].Tags;


        Debug.Log("path is: " + Application.persistentDataPath);
    }

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
        return wrapper.Planet;

    }

    public static string ToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Planet = array;
        return JsonUtility.ToJson(wrapper);
    }

    public static string ToJson<T>(T[] array, bool prettyPrint)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Planet = array;
        return JsonUtility.ToJson(wrapper, prettyPrint);
    }

    [System.Serializable]
    private class Wrapper<T>
    {
        public T[] Planet;

        public object[] Array { get; internal set; }
    }
}