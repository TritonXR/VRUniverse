using System.Collections.Generic;
using UnityEngine;

/*
 * Name: Year.cs
 * Description: Year object contains a name and list of planets within the year 
 * Utilized on: UniverseSystem to create a list of years that contain planets
 */

public class Year : MonoBehaviour {

    //Holds the name of the year in string form
    public string yr_name;

    //Holds the list of planets existing in the year
    public List<PlanetController> list_planets;

    //The parent of planets in the hierarchy
    public GameObject planets;

    /*
     * Awake: Initialize list, gameobjects, names, and transform
     * Parameters: None
     */
    void Awake()
    {
        //Initialize the planets list that will hold the list of planets in the year being read from the JSON file
        list_planets = new List<PlanetController>();

        //Initialize the planets gameobject that will act as parent for all the planets in the year
        planets = new GameObject();

        //Set the name of the planets game object to know what the game object is called
        planets.name = "Planets";

        //Set the parent of the planets gameobject to be the year gameobject
        planets.transform.parent = gameObject.transform;
    }
}
