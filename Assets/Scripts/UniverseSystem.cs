using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO; // Important for getting files from directory
using System;

/*
 * Name: UniverseSystem.cs
 * Description: UniverseSystem manages the controllers of MainUniverse and handles the creation of years
 * Utilized on: Main hierarchy UniverseSystem
 */

public class UniverseSystem : MonoBehaviour {

    const string LOBBY_YEAR_STRING = "Year";

	private static UniverseSystem instance; //part of singleton pattern

	//Gameobject acting as list of years
	private GameObject years;
	private List<Year> list_years;

	/*
	 * PlanetPosition Variables
	 */
	public Material planetMaterial;
	public int numOfPlanets = 0;
	private int tracker = 0;
	private float inputRadius = (float)500 / 3;
	private Vector3 planetPosition;

	//Integer that stores the year user is currently located in.
	private int atYear = -1;

	//Reference to the planet gameobject that will have the planet component
	[SerializeField] private GameObject prefab_planet;
    [SerializeField] private GameObject HyperSpeedController;

    //Holds original skybox color
    private Color origSkyboxColor;

    private bool CurrentlyTraveling;

	//Holds tutorial menus
	public GameObject tutorial_YearSelection;
	public GameObject tutorial_YearTravel;
	public GameObject tutorial_PlanetSelection;
	public GameObject tutorial_PlanetTravel;

	void Awake()
	{
		if (instance != null && instance != this)
		{
			Destroy(this);
		}
		else instance = this;
	}
    
    /*
     * Start: Initialize years, skybox, and load previously saved year if existing
     * Parameters: None
     */
	void Start() {

		//Initialize list_years
		list_years = new List<Year>();

		//Creates the years object and handles initializing the list of years
		ReadYearsFromJSON();

		//Set activate the tutorial for selecting a year on the left controller
		if (tutorial_YearSelection != null) tutorial_YearSelection.SetActive(true);

		//Set the original color of the application to original color
		ResetSkyboxColor();

		//Path where the save data is located
		string path = "VRClubUniverse_Data/saveData.txt";

        CurrentlyTraveling = false;

		//Teleport to previously saved year if exist
		if (File.Exists(path))
		{
			ReloadYear(path);
		}

	}

	/*
     * ReloadYear: If the user is returning from traveling to a planet, reload the year that they originally came from
     * Parameters: string path - the path to the save data text file where the year is stored
     */
	private void ReloadYear(string path)
	{
		//Read the year text stored in the file in path
		string readText = File.ReadAllText(path);

		//Delete the file to restart the experience in the originally state
		File.Delete(path);

		//Teleport to year but skip the hyperspeed
		StartCoroutine(TeleportToYear(int.Parse(readText), false));

		//Handle jump in tutorial system
		if (tutorial_YearSelection != null) tutorial_YearSelection.SetActive(false);
		if (tutorial_PlanetSelection != null) tutorial_PlanetSelection.SetActive(true);
	}

	/*
     * ResetSkyboxColor: Reset the color of the skybox to the original color
     * Parameters: None
     */
	private void ResetSkyboxColor()
	{
		//Set the color to an original no RGB value change
		origSkyboxColor = new Color(1, 1, 1);

		//Get reference to the skybox and set the tint to be original
		Material skybox = RenderSettings.skybox;
		skybox.SetColor("_Tint", origSkyboxColor);
		RenderSettings.skybox = skybox;
	}

	/*
     * GetYears: Gets the JSON files and detects what years should be in the list to travel to
     * Parameters: None
     */
	public void ReadYearsFromJSON()
	{

		//Initialize the years gameobject to act as parent for all the years inputed
		years = new GameObject();

		//Set the name of the game object to be Years
		years.name = "Years";

		//Set UniverseSystem as the parent object of the years gameobject
		years.transform.parent = gameObject.transform;

		//Get the information of a directory in persistent data path
		DirectoryInfo dir = new DirectoryInfo("VRClubUniverse_Data");

		//TESTING
		Debug.Log("Reading JSON files from VRClubUniverse_Data");

		//Get the file info by getting files with JSON path to get list of JSON files in directory
		FileInfo[] info = dir.GetFiles("*.json");

		//For each JSON file in the persistent data path
		foreach (FileInfo f in info)
		{

			//Get the name of the file
			string nameOfFile = f.Name;

			//Instantiate a Year object with a Year component on it
			GameObject year = new GameObject();

			//Add a Year component on the new year game object to declare it a year object
			Year currYear = year.AddComponent<Year>();

			//Set the parent of the new year object to be the years gameobject
			year.transform.parent = years.transform;

			//Remove the .json from the name
			string[] tempYearName = nameOfFile.Split('.');

			//Set the name of the new Year object to be the name of the file
			currYear.yr_name = tempYearName[0];

			//Set the name of the new Year object to be the name of the year being read
			year.name = tempYearName[0];

			//Add the Year object to a list of Years
			list_years.Add(year.GetComponent<Year>());

		}
	}

	/*
     * CreateYear: Creates the year gameobject and instantiates the planets
     * Parameters: int yearIndex - Passes in the index of the selected year to grab from List<Year>
     */
	public void CreateYear(int yearIndex)
	{

		//Checks if the year the user is traveling to is the Home Year. No JSON file for the home year
		if (yearIndex != -1) {

			//Gets the name of the year in the list of years
			string yearName = list_years[yearIndex].yr_name;

			//Get the year object from the List via Index
			Year year = list_years[yearIndex];

			//Open the JSON file with the name yr_name parameter passed in
			string jsonString = File.ReadAllText("VRClubUniverse_Data/" + yearName + ".json");

			//TESTING
			Debug.Log("Jsonstring is: " + jsonString);

			//Create a JSONPlanet array and read the JSON file
			PlanetJSON[] universe = JsonHelper.FromJson<PlanetJSON>(jsonString);

			//For each object in the JSONPlanet array
			foreach (PlanetJSON json_planet in universe)
			{

				//Instantiate a Planet object with a Planet component on it
				GameObject planet = Instantiate(prefab_planet, planetPosition, Quaternion.identity);

				//Set the name of the planet game object in hierarchy
				planet.name = "Planet";

                //Add a Planet component on the new planet game object to declare it a planet object
                PlanetController currPlanet = planet.GetComponent<PlanetController>();

				//Set the parent of the new Planet object to be the Planets gameobject
				planet.transform.parent = year.planets.transform;

				//Set the planet's name
				currPlanet.data.title = json_planet.Name;

				//Set the planet's creator
				currPlanet.data.creator = json_planet.Creator;

				//Set the planet's description
				currPlanet.data.description = json_planet.Description;

                //Set the planet's year
                currPlanet.data.year = json_planet.Year.ToString();

				//Set the planet's executable path
				currPlanet.data.executable = json_planet.Executable;

				//Set the planet's tags by creating a string array of the same length
				currPlanet.data.des_tag = new string[json_planet.Tags.Length];

				//For each tag in the json planet
				for (int i = 0; i < json_planet.Tags.Length; i++)
				{
					// Set the tag of the current planet equal to the json tag
					currPlanet.data.des_tag[i] = json_planet.Tags[i];
				}

				//Get the planet's image with path
				string imageName = "/" + json_planet.Image;

				//Turn the image from path URL into a Sprite to set
				byte[] bytes = File.ReadAllBytes("VRClubUniverse_Data/VR_Demos/" + currPlanet.data.year + "/" + currPlanet.data.executable + "/" + imageName);
				Texture2D texture = new Texture2D(0, 0);
				texture.LoadImage(bytes);
				Rect rect = new Rect(0, 0, texture.width, texture.height);
				currPlanet.data.image = Sprite.Create(texture, rect, new Vector2(0.5f, 0.5f));

				//Adds the read planet into the year
				year.list_planets.Add(currPlanet);

				Renderer rend = currPlanet.GetComponent<Renderer>();
				Material material = Instantiate(rend.material);
				rend.material =material;
				ChangeValue val = GetComponentInChildren<ChangeValue>();
				val.change(rend, currPlanet.data.title, int.Parse(currPlanet.data.year));
            }

            OrbitManager orbitManager = OrbitManager.GetOrbitManager();
            if(orbitManager != null)
            {
                orbitManager.PopulateOrbit(year.list_planets.ToArray());
            }
            else
            {
                Debug.LogError("Could not find OrbitManager to populate planets.");
            }

        }

        // Handles case if the year traveling to is the new year
        else {

            // There shouldn't be any planets in the Home year

        }


	}

	/*
     * DestroyPlanets: Destroys the planets in the year the user was currently at
     * Parameters: int prevYear - reference to the year the user just left
     */
	public void DestroyPlanets(int prevYear)
	{
		Debug.Log("Destroying Planets from year: " + list_years[prevYear].yr_name);

        //Get the list of planets existing in the previous year
        PlanetController[] list_planets = list_years[prevYear].planets.GetComponentsInChildren<PlanetController>();

		//Iterate through the list and destroy the planet gameobjects
		for (int i = 0; i < list_planets.Length; i++)
		{
			Destroy(list_planets[i].gameObject);
		}

		//Clear the array of planets existing in that year
		list_years[prevYear].list_planets.Clear();
	}

	/*
     * Handles teleportation to a new year. Calls CreateYear and Destroys previous year
     * 
     * TeleportToYear: Handles teleportation to a new year. Calls CreateYear and Destroys previous year
     * Parameters: int newYear - the year to travel to so as to create the year 
     *             bool useAnimation - default is true. Won't use animation if going from a project back to a year
     */

    public IEnumerator TeleportToYear(int newYear, bool useAnimation = true)
    {
        CurrentlyTraveling = true;

		//Check if there have been planets created before
		if (atYear != -1)
		{
			//Destroy planets in the previous year
			DestroyPlanets(atYear);
		}

        atYear = newYear;

        //Start teleportation system traveling there by calling from Hyperspeed script
        if (useAnimation) yield return StartCoroutine(HyperSpeedController.GetComponentInChildren<Hyperspeed>().Travel(true));

        PlanetDisplay disp = PlanetDisplay.GetInstance();
        if (disp != null)
        {
            disp.SetVisible(false);
            disp.SetViewTarget(null);
        }

		//Create the new year with planets
		CreateYear(newYear);

		//Change the color of the skybox by hashing the year
		Material skybox = RenderSettings.skybox;
		skybox.SetInt("_Rotation", 20 * newYear);
		int currYearName = int.Parse(list_years[newYear].yr_name);
		int colorValueToChange = newYear % 3;
		Color newSkyboxColor;
		float newColorValue = (((float)currYearName * 42) % 255) / 255; //42 is arbitrary, but also... well, you've heard the joke by now
		if (colorValueToChange == 0) // change red value
		{
			newSkyboxColor = new Color(newColorValue, origSkyboxColor.g, origSkyboxColor.b);
			skybox.SetColor("_Tint", newSkyboxColor);
		} else if (colorValueToChange == 1) // change green value
		{
			newSkyboxColor = new Color(origSkyboxColor.r, newColorValue, origSkyboxColor.b);
			skybox.SetColor("_Tint", newSkyboxColor);
		} else if (colorValueToChange == 2) //change blue value
		{
			newSkyboxColor = new Color(origSkyboxColor.r, origSkyboxColor.g, newColorValue);
			skybox.SetColor("_Tint", newSkyboxColor);
		} else
		{
			Debug.LogWarning("Invalid Skybox Year Index");
		}
		RenderSettings.skybox = skybox;

		//Start teleportation system ending by Hyperspeed script call
		if(useAnimation) yield return StartCoroutine(HyperSpeedController.GetComponentInChildren<Hyperspeed>().Travel(false));

        CurrentlyTraveling = false;
	}

	public int GetNumYears()
	{
		return list_years.Count;
	}

	public Year GetYear(int index)
	{
		return list_years[index];
	}

    public int GetYearIndex(int yearValue)
    {
        int index = -1;
        for(int count = 0; count < list_years.Count; count++)
        {
            if(Int32.Parse(list_years[count].yr_name) == yearValue)
            {
                index = count;
                break;
            }
        }

        return index;
    }

    public void GetYearRange(out int min, out int max)
    {
        min = max = Int32.Parse(list_years[0].yr_name);
    
        for(int index = 1; index < list_years.Count; index++)
        {
            int yearValue = Int32.Parse(list_years[index].yr_name);
            if (yearValue < min) min = yearValue;
            else if (yearValue > max) max = yearValue;
        }
    }

    public string GetCurrentYear()
    {
        if (atYear == -1 || atYear > list_years.Count) return LOBBY_YEAR_STRING;
        return list_years[atYear].yr_name;
    }

    public bool IsCurrentlyTraveling()
    {
        return CurrentlyTraveling;
    }

    /*
     * TESTING INPUT TO GETTING YEAR AND PLANETS
     */
    /*
     * Update: TESTING INPUT FOR YEAR AND PLANETS
     * Parameters: None
     */
    void Update()
    {

        if (tutorial_YearTravel != null && tutorial_YearSelection != null && !tutorial_YearTravel.activeSelf)
        {
            tutorial_YearTravel.SetActive(true);
            tutorial_YearSelection.SetActive(false);
        }
    }

	public static UniverseSystem GetInstance()
	{
		return instance;
	}

}

/*
 * Name: PlanetJSON
 * Description: used for creating object when reading the planet JSON files
 * Utilized on: UniverseSystem
 */
[System.Serializable]
public class PlanetJSON
{
    public string Name;
    public string Creator;
    public string Description;
    public int Year;
    public string[] Tags;
    public string Image;
    public string Executable;
}

/*
 * Name: JsonHelper
 * Description: Acts to parse JSON objects into arrays
 * Utilized on: UniverseSystem
 */
public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.PlanetJSON;

    }

    public static string ToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.PlanetJSON = array;
        return JsonUtility.ToJson(wrapper);
    }

    public static string ToJson<T>(T[] array, bool prettyPrint)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.PlanetJSON = array;
        return JsonUtility.ToJson(wrapper, prettyPrint);
    }

    [System.Serializable]
    private class Wrapper<T>
    {
        public T[] PlanetJSON;

        public object[] Array { get; internal set; }
    }
}