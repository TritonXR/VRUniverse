using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO; // Important for getting files from directory

/*
 * UniverseSystem manages the controllers of MainUniverse. Contains a list of Years.
 */

public class UniverseSystem : MonoBehaviour {

    /*
     * TODO: attach this to the controller years. the menu on the controller will access the 
     * list_years LIST to know what years are available to travel to
     */
    //gameobject acting as list of years
    public GameObject years;
    public List<Year> list_years;

    /*
     * TODO: Need to figure out how to position the planets systematically
     * For now, just add 5 in the X axis (Temporary)
     */
    private Vector3 planetPosition;
    [SerializeField] private float temporaryPlanetPositionScale;

	// Use this for initialization
	void Start () {

        // Creates the years object and handles initializing the list of years
        GetYears();

	}

    /*
     * Gets the JSON files and detects what years should be in the list to travel to
     */
    public void GetYears()
    {

        // Initialize the years gameobject to act as parent for all the years inputed
        years = new GameObject();

        // Set the name of the game object to be Years
        years.name = "Years";

        // Set UniverseSystem as the parent object of the years gameobject
        years.transform.parent = gameObject.transform;

        // Get the information of a directory in persistent data path
        DirectoryInfo dir = new DirectoryInfo(Application.persistentDataPath);

        // TESTING
        Debug.Log("Reading JSON files from " + Application.persistentDataPath);

        // Get the file info by getting files with JSON path to get list of JSON files in directory
        FileInfo[] info = dir.GetFiles("*.json");

        // For each JSON file in the persistent data path
        foreach (FileInfo f in info)
        {

            // Get the name of the file
            string nameOfFile = f.Name;

            // TESTING
            Debug.Log("Creating year: " + nameOfFile);

            // Instantiate a Year object with a Year component on it
            GameObject year = new GameObject();

            // Add a Year component on the new year game object to declare it a year object
            Year currYear = year.AddComponent<Year>();

            // Set the parent of the new year object to be the years gameobject
            year.transform.parent = years.transform;

            // Set the name of the new Year object to be the name of the file
            currYear.yr_name = nameOfFile;

            // Set the name of the new Year object to be the name of the year being read
            year.name = nameOfFile;

            // Add the Year object to a list of Years
            list_years.Add(year.GetComponent<Year>());

        }

    }

    /*
     * Creates the year gameobject and instantiates the planets
     * Passes in the index of the selected year to grab from List<Year>
     */
    public void CreateYear(int yearIndex)
    {

        // Open the JSON file with the name yr_name parameter passed in

        // Create a JSONPlanet array and read the JSON file

        // Initialize the planetPosition vector3 to systematically place planets in universe
        planetPosition = new Vector3(0.0f, 0.0f, 0.0f);

        // For each object in the JSONPlanet array

            // Instantiate a Planet object with a Planet component on it
            GameObject planet = new GameObject();

            // Add a Planet component on the new planet game object to declare it a planet object
            planet.AddComponent<Planet>();

            // Get the year object from the List via Index
            Year year = list_years[yearIndex];

            // Set the parent of the new Planet object to be the Year gameobject
            planet.transform.parent = year.transform;

            // Set the planet's name
            // Set the planet's creator
            // Set the planet's description
            // Set the planet's tags
            // Set the planet's year
            // Set the planet's image

            // Set the planet's position to the current planetPosition vector3
            planet.transform.position = planetPosition;

            // TEMPORARY: Increment new planet position to be +scale on X direction
            planetPosition = new Vector3(planetPosition.x + temporaryPlanetPositionScale, planetPosition.y, planetPosition.z);

    }

}
