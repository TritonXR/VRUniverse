using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System.IO;

public class Jsonexample : MonoBehaviour {
    public Character player = new Character(0,"molly",1127,false,new int[] { 7,4,8,21,12,15});
    JsonData playerJson;
    
    // Use this for initialization
	void Start () {
        playerJson = JsonMapper.ToJson(player);
        File.WriteAllText(Application.dataPath+"/Player.json",playerJson.ToString());
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

public class Character {

    public int id;
    public string name;
    public int health;
    public bool aggressive;
    public int[] stats;

    public Character(int id, string name, int health, bool aggressive, int[] stats) {

        this.id = id;
        this.name = name;
        this.health = health;
        this.aggressive = aggressive;
        this.stats = stats;


    }

}
