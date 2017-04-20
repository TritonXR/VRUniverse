using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Year object contains a name and list of planets within the year 
 */

public class Year : MonoBehaviour {

    public string yr_name;
    public List<Planet> list_planets;
    public GameObject planets;


    void Awake()
    {


        // Initialize the planets list that will hold the list of planets in the year being read from the JSON file
        list_planets = new List<Planet>();

        // Initialize the planets gameobject that will act as parent for all the planets in the year
        planets = new GameObject();

        // Set the name of the planets game object to know what the game object is called
        planets.name = "Planets";

        // Set the parent of the planets gameobject to be the year gameobject
        planets.transform.parent = gameObject.transform;
    }

	// Use this for initialization
	void Start () {


	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
