using UnityEngine;
using UnityEngine.UI;

/*
 * Name: PlanetController.cs
 * Description: Contains methods called when user points and pulls the trigger at a planet. Distributes information to its floating menu and determining where the executable is
 * Utilized on: Planet.prefab that is instantiated with every planet/project
 */

public class PlanetController : MonoBehaviour
{
    //offsets of various menus relative to the planet in viewing space (neg/pos effects: x => left/right, y=down/up, z=closer/farther)
    [SerializeField] private Vector3 PlanetMenu_Offset = new Vector3(0, 0, 0);
    [SerializeField] private Vector3 TravelMenu_Offset = new Vector3(0, 0, 0);

    //Planet Data Input on Planet Menu
    [SerializeField] private Text Planet_Title, Planet_Creator, Planet_Description, Planet_Year, Planet_Tag;
    [SerializeField] private Image Planet_Image;

    //Access to the Planet's Executable File to Travel to
    private string Planet_Executable;

    //Reference to the Planet Data on each Planet
    private Planet Planet_Data;

    //Menu's controlled by hovering and selecting
    [SerializeField] private GameObject Planet_Menu; //Entire Planet_Menu attached to each planet that shows planet data

    //Travel menu where user selects whether they want to travel or not. accessible by travelinteractable by clicking no
    [SerializeField] private GameObject Travel_Selection;

    //Reference to the right controller to detect the trigger clicked
    private SteamVR_TrackedController rightController;

    //Check if this is the first time the user is selecting a planet (so as not to repeat the tutorial everytime)
	private bool tutorial_firstSelection = true;

    //Contains the tutorials to describe controls on the right controller (the pointer for selecting planets)
	private Image[] tutorialsOnRightController;

    /*
     * ToggleMenu: Toggles the planet description floating menu whenever the user points at a planet
     * Parameters: bool status - true if the menu should be visible
     *                           false if the menu should be invisible
     */
	private void ToggleMenu(bool status)
    {
        Planet_Menu.SetActive(status);
    }

    /*
     * Start: Sets planet information, positions, scales, references to controller, planets, menus, and tutorials
     * Parameters: None
     */
    protected void Start()
    {
      
        //Find the right controller
        SetRightController();

        //Turn off floating menu panel by default
        ToggleMenu(false);

        //Turn off travel menu by default
        Travel_Selection.SetActive(false);
        
        //Gets the data from the planet
        Planet_Data = gameObject.GetComponent<Planet>();

        //Positions the menu to the left of the planet and looking at initial camera
        //PositionMenus();

        //Shrink the floating menus by a proportional size
        ScaleMenus(0.58f);

        //Set text to planet info
        SetPlanetInfoText();

        //Set executable string for TravelInteracble
        SetExecutableString();

		// Get Tutorials on right controller
		tutorialsOnRightController = rightController.GetComponentsInChildren<Image>(true);

	}

    protected void Update()
    {
        PositionMenus();
    }

    /*
     * SetPlanetInfoText: Sets the text on the description panel that appears by the planet when the user points their laser at it
     * Parameters: None
     */
    private void SetPlanetInfoText()
    {
        //Sets the text for the different data components on the menu
        Planet_Title.text = Planet_Data.title;
        Planet_Creator.text = Planet_Data.creator;
        Planet_Description.text = Planet_Data.description;
        Planet_Year.text = Planet_Data.year;

        //Must be handled differently because tags are stored as an array and we must concatenate them
        string tagText = "";
        for (int i = 0; i < Planet_Data.des_tag.Length; i++)
        {
            if (i == Planet_Data.des_tag.Length - 1)
            {
                tagText = tagText + Planet_Data.des_tag[i];
            }
            else
            {
                tagText = tagText + Planet_Data.des_tag[i] + ", ";
            }

        }

        Planet_Tag.text = tagText;
        Planet_Image.sprite = Planet_Data.image; //Uses the image component to set the sprite of what the picture should be
        Planet_Executable = Planet_Data.executable;
    }

    /*
     * SetExecutableString: Sets the path of where the executable is for the planet/project
     * Parameters: None
     */
    private void SetExecutableString()
    {
        //Gets all the interactable buttons in the travel confirmation menu of this planet
        TravelInteractable[] travelPanels = GetComponentsInChildren<TravelInteractable>(true);

        for (int i = 0; i < travelPanels.Length; i++)
        {
            //If the panel is a Yes button, set the string in which to travel to 
            if (travelPanels[i].isYes)
            {
                travelPanels[i].executableString = "/../VRClubUniverse_Data/VR_Demos/" + int.Parse(Planet_Year.text) + "/" + Planet_Executable + "/" + Planet_Executable + ".exe";
            }
        }
    }

    /*
     * PositionMenus: Sets the position of the planet information menu and the travel confirmation menu
     * Parameters: None
     */
    private void PositionMenus()
    {
        
        Vector3 planetOffset = transform.position - Camera.main.transform.position;
        Quaternion menuOffsetRot = Quaternion.FromToRotation(Vector3.forward, planetOffset.normalized);
        Vector3 pMenuOffset = menuOffsetRot * PlanetMenu_Offset;
        Vector3 tMenuOffset = menuOffsetRot * TravelMenu_Offset;
        
        Planet_Menu.transform.position = transform.position + pMenuOffset;
        Planet_Menu.transform.LookAt(Camera.main.transform); //looks at the camera
        Planet_Menu.transform.Rotate(new Vector3(0, 180, 0)); //but it needs to be flipped around for some reason

        Travel_Selection.transform.position = transform.position + tMenuOffset;
        Travel_Selection.transform.LookAt(Camera.main.transform);
        Travel_Selection.transform.Rotate(new Vector3(0, 180, 0));
    }

    /*
     * ScaleMenus: Decrease the size of the floating description and travel confirmation menu to fit the size of the planet
     * Parameters: float val - the value in which you want to decrease the current scale by
     */
    private void ScaleMenus(float val)
    {
        //Get the current scale of the planet
        Vector3 currentScale = Planet_Menu.transform.localScale;

        //Decrease the size of the floating menu in proportion to the current scale of the planet
        Planet_Menu.transform.localScale = new Vector3(currentScale.x * val, currentScale.y * val, currentScale.z * val);

        //Get the scale of the travel selection menu
        currentScale = Travel_Selection.transform.localScale;

        //Decrease the size the travel selection menu by a proportion
        Travel_Selection.transform.localScale = new Vector3(currentScale.x * val, currentScale.y * val, currentScale.z * val);
    }

    /*
     * SetRightController: Find the right controller by iterating through all the possible controllers and checking if they don't contain the YearSelection object (which belongs to the left controller)
     * Parameters: None
     */
    private void SetRightController()
    {
        //Get array of controllers starting from the CameraRig camera
        SteamVR_TrackedController[] controllerSearch = Camera.main.transform.root.GetComponentsInChildren<SteamVR_TrackedController>(true);

        //Iterate through array
        for (int i = 0; i < controllerSearch.Length; i++)
        {
            //Because YearSelection belongs in left controller, if you don't have it, then it is the right controller
            if (!(controllerSearch[i].GetComponentInChildren<YearSelection>(true)))
            {
                rightController = controllerSearch[i];
            }
        }
    }

    /*
     * SelectPlanet: When the user pulls the trigger while pointing at the planet, it should display the travel confirmation menu and update the tutorials if needed
     * Parameters: None
     */
    public void SelectPlanet()
    {
        //Activate the travel confirmation menu
        Travel_Selection.SetActive(true);

        //Disable any tutorials remaining if they exist
        if (tutorialsOnRightController[1].gameObject.activeSelf)
        {
            tutorialsOnRightController[1].gameObject.SetActive(false); //turn off label 4 for selecting a planet
        }

    }

    /*
     * DeselectPlanet: When the user clicks 'No' in response to whether they actually want to travel to the planet, the menu disappears
     * Parameters: None
     */
    public void DeselectPlanet()
    {
        //Disable the confirmation travel panel
        Travel_Selection.SetActive(false);
    }

    /*
     * StartPointing: Called when user's laser collides with the planet to toggle the description menu and make changes to tutorial on right controller
     * Parameters: None
     */
    public void StartPointing()
    {

        //Turn on menu when hovering
        ToggleMenu(true);

        //If this is the first time selecting a planet, perform change of tutorials
		if (tutorial_firstSelection)
		{
			tutorialsOnRightController[0].gameObject.SetActive(false); //turn off label 3 for pointing at a planet
			tutorialsOnRightController[1].transform.parent.gameObject.SetActive(true); //turn on label 4 for selecting a planet
			tutorial_firstSelection = false;
		}

	}

    /*
     * StopPointing: Calls when the user points their laser away from the planet to disable the description menu
     * Parameters: None
     */
    public void StopPointing()
    {

        //Turn off floating menu panel when not using
        ToggleMenu(false);
        
    }
    
}