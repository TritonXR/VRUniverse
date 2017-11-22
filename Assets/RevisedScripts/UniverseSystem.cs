using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO; // Important for getting files from directory
using UnityEngine.UI;

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
    public static List<Year> list_years;

    /*
     * TODO: Need to figure out how to position the planets systematically
     * For now, just add 5 in the X axis (Temporary)
     */
    private Vector3 planetPosition;
    [SerializeField] private float temporaryPlanetPositionScale;

	/*
	 * PlanetPosition Variables
	 */
	public Material Planet1;
	public Material Planet2;
	public Material Planet3;
	public int NUM_OF_PLANETS = 0;
	private int tracker = 0;
	private float inputRadius = (float) 70 / 3;

    // Integer that stores the year user is currently located in.
    private int atYear = -1;

    // Reference to the planet gameobject that will have the planet component
    [SerializeField] private GameObject prefab_planet;

    // Holds original skybox color
    private Color origSkyboxColor;

    //Holds instruction menus
    public GameObject tutorial_RadialMenu;
    public GameObject tutorial_TriggerMenu;

    // Holds tutorial menus
    public GameObject tutorial_YearSelection;
    public GameObject tutorial_YearTravel;
    public GameObject tutorial_PlanetSelection;
    public GameObject tutorial_PlanetTravel;

	private YearSelection yearSelection;


	// Use this for initialization
	void Start () {

        // Initialize list_years
        list_years = new List<Year>();

        // Creates the years object and handles initializing the list of years
        GetYears();

        tutorial_YearSelection.SetActive(true);

		// Set the original color of the application to original color
		origSkyboxColor = new Color(1, 1, 1);
        Material skybox = RenderSettings.skybox;
        skybox.SetColor("_Tint", origSkyboxColor);
        RenderSettings.skybox = skybox;

        string path = "VRClubUniverse_Data/saveData.txt";

		yearSelection = Camera.main.transform.root.GetComponentInChildren<YearSelection>(true);

		//Teleport to previously saved year
		if (File.Exists(path))
        {
			string readText = File.ReadAllText(path);
            File.Delete(path);
            StartCoroutine(TeleportToYear(int.Parse(readText), false));

            yearSelection.displayedYearString = readText;
			yearSelection.SelectedYearIndex = int.Parse(readText);

			tutorial_RadialMenu.SetActive(false);
			tutorial_TriggerMenu.GetComponentInChildren<YearInput>().gameObject.GetComponent<Text>().text = list_years[yearSelection.SelectedYearIndex].yr_name;
			tutorial_TriggerMenu.SetActive(true);

            tutorial_YearSelection.SetActive(false);
            tutorial_PlanetSelection.SetActive(true);

        }

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
        //DirectoryInfo dir = new DirectoryInfo(Application.persistentDataPath);

        DirectoryInfo dir = new DirectoryInfo("VRClubUniverse_Data");

        // TESTING
        Debug.Log("Reading JSON files from VRClubUniverse_Data");

        // Get the file info by getting files with JSON path to get list of JSON files in directory
        FileInfo[] info = dir.GetFiles("*.json");

        // For each JSON file in the persistent data path
        foreach (FileInfo f in info)
        {

            // Get the name of the file
            string nameOfFile = f.Name;

            // Instantiate a Year object with a Year component on it
            GameObject year = new GameObject();

            // Add a Year component on the new year game object to declare it a year object
            Year currYear = year.AddComponent<Year>();

            // Set the parent of the new year object to be the years gameobject
            year.transform.parent = years.transform;

            // Remove the .json from the name
            string[] tempYearName = nameOfFile.Split('.');

            // Set the name of the new Year object to be the name of the file
            currYear.yr_name = tempYearName[0];

            // Set the name of the new Year object to be the name of the year being read
            year.name = tempYearName[0];

            // Add the Year object to a list of Years
            UniverseSystem.list_years.Add(year.GetComponent<Year>());

        }

    }

    /*
     * Creates the year gameobject and instantiates the planets
     * Passes in the index of the selected year to grab from List<Year>
     */
    public void CreateYear(int yearIndex)
    {

        // Checks if the year the user is traveling to is the Home Year. No JSON file for the home year
        if (yearIndex != -1) {

            // Gets the name of the year in the list of years
            string yearName = UniverseSystem.list_years[yearIndex].yr_name;

            // Open the JSON file with the name yr_name parameter passed in
            //string jsonString = File.ReadAllText(Application.persistentDataPath + "/" + yearName);
            string jsonString = File.ReadAllText("VRClubUniverse_Data/" + yearName + ".json");

            Debug.Log("Jsonstring is: " + jsonString);

            // Create a JSONPlanet array and read the JSON file
            PlanetJSON[] universe = JsonHelper.FromJson<PlanetJSON>(jsonString);

            // Initialize the planetPosition vector3 to systematically place planets in universe
            planetPosition = new Vector3(0.0f, 0.0f, 0.0f);

            // For each object in the JSONPlanet array
            foreach (PlanetJSON json_planet in universe)
            {

                // Instantiate a Planet object with a Planet component on it
                GameObject planet = Instantiate(prefab_planet, planetPosition, Quaternion.identity);

                // Set the name of the planet game object in hierarchy
                planet.name = "Planet";

                // Add a Planet component on the new planet game object to declare it a planet object
                Planet currPlanet = planet.AddComponent<Planet>();

                // Get the year object from the List via Index
                Year year = UniverseSystem.list_years[yearIndex];

                // Set the parent of the new Planet object to be the Planets gameobject
                planet.transform.parent = year.planets.transform;

                // Set the planet's name
                currPlanet.title = json_planet.Name;

                // Set the planet's creator
                currPlanet.creator = json_planet.Creator;

                // Set the planet's description
                currPlanet.description = json_planet.Description;

                // Set the planet's year
                currPlanet.year = json_planet.Year.ToString();

                // Set the planet's executable path
                currPlanet.executable = json_planet.Executable;

                // Set the planet's tags by creating a string array of the same length
                currPlanet.des_tag = new string[json_planet.Tags.Length];

                // For each tag in the json planet
                for (int i = 0; i < json_planet.Tags.Length; i++)
                {
                    // Set the tag of the current planet equal to the json tag
                    currPlanet.des_tag[i] = json_planet.Tags[i];
                }

                // Get the planet's image with path
                string imageName = "/" + json_planet.Image;

                // Turn the image from path URL into a Sprite to set
                //byte[] bytes = File.ReadAllBytes(Application.persistentDataPath + imageName);
                byte[] bytes = File.ReadAllBytes("VRClubUniverse_Data/VR_Demos/" + currPlanet.year + "/" + currPlanet.executable + "/" + imageName);
                Texture2D texture = new Texture2D(0, 0);
                texture.LoadImage(bytes);
                Rect rect = new Rect(0, 0, texture.width, texture.height);
                currPlanet.image = Sprite.Create(texture, rect, new Vector2(0.5f, 0.5f));

                // Set the planet's position to the current planetPosition vector3
                planet.transform.position = planetPosition;

                // TEMPORARY: Increment new planet position to be +scale on X direction
                //planetPosition = new Vector3(planetPosition.x + temporaryPlanetPositionScale, planetPosition.y, planetPosition.z);

                // Adds the read planet into the year
                year.list_planets.Add(currPlanet);

            }

            int listLength = UniverseSystem.list_years[yearIndex].list_planets.Count;
            //Debug.Log(" l  " + listLength);
            if (listLength <= 6)
            {
                tracker = 0;
                setupSphere(10f, listLength, Planet1, UniverseSystem.list_years[yearIndex].list_planets);
            }
            else if (listLength > 6 && listLength <= 11)
            {
                tracker = 0;
                setupSphere(10f, 6, Planet1, UniverseSystem.list_years[yearIndex].list_planets);
                setupSphere(15f, listLength - 6, Planet2, UniverseSystem.list_years[yearIndex].list_planets);
            }
            else if (listLength > 11)
            {
                tracker = 0;
                setupSphere(10f, 6, Planet1, UniverseSystem.list_years[yearIndex].list_planets);
                setupSphere(15f, 5, Planet2, UniverseSystem.list_years[yearIndex].list_planets);
                setupSphere(20f, listLength - 11, Planet3, UniverseSystem.list_years[yearIndex].list_planets);
            }

        }

        // Handles case if the year traveling to is the new year
        else {

            // There shouldn't be any planets in the Home year

        }


    }

    /*
     * Destroys the planets in the year the user was currently at
     */
    public void DestroyPlanets(int prevYear)
    {
        Debug.Log("Destroying Planets from year: " + UniverseSystem.list_years[prevYear].yr_name);
        Planet[] list_planets = UniverseSystem.list_years[prevYear].planets.GetComponentsInChildren<Planet>();
        for (int i = 0; i < list_planets.Length; i++)
        {
            Destroy(list_planets[i].gameObject);
        }
        UniverseSystem.list_years[prevYear].list_planets.Clear();
    }

	public Vector3 getCartesianFor(float radius, float inclination, float azimuth)
	{
		return new Vector3(radius * Mathf.Sin(inclination) * Mathf.Sin(azimuth), radius * Mathf.Cos(inclination), radius * Mathf.Sin(inclination) * Mathf.Cos(azimuth));
	}

	public void setupSphere(float inputY, int num, Material material, List<Planet> list)
	{
		GameObject bigSphere = GameObject.CreatePrimitive (PrimitiveType.Sphere);
		bigSphere.GetComponent<SphereCollider>().radius = inputRadius;
		bigSphere.transform.position = new Vector3(0, 0, 0);
		Renderer rend = bigSphere.GetComponent<Renderer>();
		rend.enabled = false;
		int old_tracker = tracker;

		float radius = bigSphere.GetComponent<SphereCollider>().radius * transform.localScale.x;

		float theta = (Mathf.PI / 2) - Mathf.Asin(inputY / radius);

        while (tracker < num + old_tracker)
		{
          
			//if (i < list.Count) {
                float sectorAngle = (Mathf.PI * 2) / num;
         
				Vector3 vect = getCartesianFor (radius, theta, tracker * sectorAngle);

                if (NUM_OF_PLANETS == 1)
                {
                    //Debug.Log("vector " + vect);
                }

                //Debug.Log("The vector is: " + vect);
                list[tracker].transform.position = vect;
				list [tracker].transform.localScale = new Vector3 (2, 2, 2);
				list [tracker].GetComponent<MeshRenderer> ().material = material;
				tracker++;
			//}
		}
		//tracker++;
        NUM_OF_PLANETS++;
	}

    /*
     * Handles teleportation to a new year. Calls CreateYear and Destroys previous year
     */
    public IEnumerator TeleportToYear(int newYear, bool useAnimation = true)
    {
        //YearSelection yearSelection = Camera.main.transform.root.GetComponentInChildren<YearSelection>(true);
        yearSelection.isTravelling = true;

        // Start teleportation system traveling there by calling from Hyperspeed script
        if(useAnimation) yield return StartCoroutine(GetComponentInChildren<Hyperspeed>().Travel(true));

        // Check if there have been planets created before
        if (atYear != -1)
        {
            // Destroy planets in the previous year
            DestroyPlanets(atYear);
        }

        // Create the new year with planets
        CreateYear(newYear);

        // CHANGE THE SKYBOX TODO
        Material skybox = RenderSettings.skybox;
        skybox.SetInt("_Rotation", 20 * newYear);
        int currYearName = int.Parse(list_years[newYear].yr_name);
		//Debug.Log("SkyColor CurrYearName: " + currYearName);
        int colorValueToChange = newYear % 3;
		//Debug.Log("SkyColor colorValueToChange: " + colorValueToChange);
		Color newSkyboxColor;
        float newColorValue = (( (float)currYearName * 42) % 255) / 255;
		//Debug.Log("SkyColor newColorValue: " + newColorValue);
		if (colorValueToChange == 0) // change red value
        {
			//Debug.Log("SkyColor change to red");
            newSkyboxColor = new Color(newColorValue, origSkyboxColor.g, origSkyboxColor.b);
            skybox.SetColor("_Tint", newSkyboxColor);
        } else if (colorValueToChange == 1) // change green value
        {
			//Debug.Log("SkyColor change to green");
			newSkyboxColor = new Color(origSkyboxColor.r, newColorValue, origSkyboxColor.b);
            skybox.SetColor("_Tint", newSkyboxColor);
        } else if (colorValueToChange == 2) //change blue value
        {
			//Debug.Log("SkyColor change to blue");
			newSkyboxColor = new Color(origSkyboxColor.r, origSkyboxColor.g, newColorValue);
            skybox.SetColor("_Tint", newSkyboxColor);
        } else
        {
            Debug.LogWarning("Invalid Skybox Year Index");
        }
        RenderSettings.skybox = skybox;

        // Start teleportation system ending by Hyperspeed script call
        if(useAnimation) yield return StartCoroutine(GetComponentInChildren<Hyperspeed>().Travel(false));

        // Set the year user is currently at to the new year
        atYear = newYear;
        yearSelection.isTravelling = false;
    }

    /*
     * TESTING RADIAL MENU WITH VRTK. GOING TO REMOVE RADIAL MENU IN THE FUTURE.
     * Need to have separate method to start a coroutine since doesn't start coroutines in 
     * VRTK radial menu settins.
     */
    public void RadialMenu_TeleportToYear(int newYear) 
    {

        StartCoroutine(TeleportToYear(newYear));

    }

    /*
     * TESTING INPUT TO GETTING YEAR AND PLANETS
     */
    void Update()
    {

        if (!tutorial_YearTravel.activeSelf && !yearSelection.tutorial_firstSelection)
        {
            tutorial_YearTravel.SetActive(true);
            tutorial_YearSelection.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.A)) // Create 2015
        {
            StartCoroutine(TeleportToYear(0));
        }
        else if (Input.GetKeyDown(KeyCode.B)) // Create 2016
        {
            StartCoroutine(TeleportToYear(1));
        }
        else if (Input.GetKeyDown(KeyCode.C)) // Create 2017
        {
            StartCoroutine(TeleportToYear(2));
        }
        else if (Input.GetKeyDown(KeyCode.D)) // Create 2018
        {
            StartCoroutine(TeleportToYear(3));
        }
    }

}

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
 * Acts to parse JSON objects into arrays
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