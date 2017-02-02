using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using LitJson;


public class Readjson : MonoBehaviour {
    private string Jsonstring;
    private JsonData itemdata;
	// Use this for initialization
	void Start () {
        Jsonstring = File.ReadAllText(Application.dataPath + "/Player.json");
        itemdata = JsonMapper.ToObject(Jsonstring);
        Debug.Log(itemdata);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
