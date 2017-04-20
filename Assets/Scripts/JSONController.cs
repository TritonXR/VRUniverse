using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class JSONController : MonoBehaviour {

	private string jsonString;
    public GameObject PlanetParent;

    public string yearToRead;

    //public static JSONController JSONControlObject = null;

    /*
    private void Awake()
    {
        if (JSONControlObject == null) {
            JSONControlObject = this;
        } else if (JSONControlObject != this) {
            Destroy(this);
        }
    }*/


    // Use this for initialization
    void Start()
	{
        /*
        PlanetParent = ListOfPlanetsController.listOfPlanets;

        string filePath = "/Planet_" + yearToRead + ".json";
        Debug.Log("the file path is: " + filePath);

		jsonString = File.ReadAllText(Application.persistentDataPath + filePath);
        Debug.Log("Reading: " + jsonString);
        PlanetJS[] universe = JsonHelper.FromJson<PlanetJS>(jsonString);

        PlanetData[] listOfPlanets = PlanetParent.GetComponentsInChildren<PlanetData>();
        
        for (int i = 0; i < listOfPlanets.Length; i++)
        {
            listOfPlanets[i].title = universe[i].Name;
            listOfPlanets[i].creator = universe[i].Creator;
            listOfPlanets[i].year = universe[i].Year.ToString();
            listOfPlanets[i].description = universe[i].Description;
            listOfPlanets[i].des_tag = new string[universe[i].Tags.Length];
            for (int j = 0; j < universe[i].Tags.Length; j++)
            {
                listOfPlanets[i].des_tag[j] = universe[i].Tags[j];
            }

            string imageName = "/" + universe[i].Image;

						byte[] bytes = File.ReadAllBytes(Application.persistentDataPath + imageName);
						Texture2D texture = new Texture2D(0, 0);
						texture.LoadImage(bytes);

						Rect rect = new Rect(0, 0, texture.width, texture.height);

						//listOfPlanets[i].GetComponent<UnityEngine.UI.Image>().sprite = sprite;
						listOfPlanets[i].image = Sprite.Create(texture, rect, new Vector2(0.5f, 0.5f));


        } */



    }

}


[System.Serializable]
public class PlanetJS{
	public string Name;
	public string Creator;
	public string Description;
	public int Year;
	public string[] Tags;
    public string Image;
}

/*
public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.PlanetJS;

    }

    public static string ToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.PlanetJS = array;
        return JsonUtility.ToJson(wrapper);
    }

    public static string ToJson<T>(T[] array, bool prettyPrint)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.PlanetJS = array;
        return JsonUtility.ToJson(wrapper, prettyPrint);
    }

    [System.Serializable]
    private class Wrapper<T>
    {
        public T[] PlanetJS;

        public object[] Array { get; internal set; }
    }
}*/