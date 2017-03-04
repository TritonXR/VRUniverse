using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class JSON : MonoBehaviour {

    Planets[] planetInstance;

	// Use this for initialization
	void Start () {
        planetInstance = new Planets[2];

        planetInstance[0] = new Planets();
        planetInstance[0].name = "Venus";
        planetInstance[0].creator = "Anish Kannan";
        planetInstance[0].description = "It's burning.";
        planetInstance[0].year = "3000";
        planetInstance[0].tags = "gear";

        planetInstance[1] = new Planets();
        planetInstance[1].name = "Pluto";
        planetInstance[1].creator = "NAS";
        planetInstance[1].description = "It's cold.";
        planetInstance[1].year = "1000";
        planetInstance[1].tags = "cardboard";

        string planetToJson = JsonHelper.ToJson(planetInstance, true);
        Debug.Log(planetToJson);

        Deserialize();

        /*
        string jsonString = "{\"name\":\"Mercury\",\"creator\":\"Temporary\",\"description\":\"Random\",\"year\":\"2101\",\"tags\":\"vr\"}";
        Planets planet = (Planets)JsonUtility.FromJson(jsonString, typeof(Planets));
        Debug.Log(planet.name);
        */
    }

    // Update is called once per frame
    void Update () {
		
	}

    void Deserialize()
    {
        /*
        string jsonString = "{\"name\":\"Mercury\",\"creator\":\"Temporary\",\"description\":\"Random\",\"year\":\"2101\",\"tags\":\"vr\"}";
        JsonUtility.FromJsonOverwrite(jsonString, planetInstance);
        Debug.Log(planetInstance.name); */

        string jsonString = "{\r\n    \"Planets\": [\r\n        {\r\n            \"name\": \"Mercury\",\r\n            \"creator\": \"Temporary\",\r\n            \"description\": \"random\",\r\n            \"year\": \"2101\",\r\n            \"tag\": \"vr\"\r\n        },\r\n        {\r\n            \"name\": \"Neptune\",\r\n            \"creator\": \"adf\",\r\n            \"description\": \"111\",\r\n            \"year\": \"000\",\r\n            \"tag\": \"lol\"\r\n        }\r\n    ]\r\n}";
        Debug.Log("json string: " + jsonString);
        Planets[] testInst = JsonHelper.FromJson<Planets>(jsonString);
        //planetInstance = JsonHelper.FromJson<Planets>(jsonString);
        //Debug.Log(planetInstance[0].name);
        //Debug.Log(planetInstance[1].name);
        Debug.Log(testInst[0].name);
        Debug.Log(testInst[1].name);
    }
}

[Serializable]
public class Planets
{
    public string name;
    public string creator;
    public string description;
    public string year;
    public string tags;
}