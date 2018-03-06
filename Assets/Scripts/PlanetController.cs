using UnityEngine;
using UnityEngine.UI;

/*
 * Name: PlanetController.cs
 * Description: Contains methods called when user points and pulls the trigger at a planet. Distributes information to its floating menu and determining where the executable is
 * Utilized on: Planet.prefab that is instantiated with every planet/project
 */

public struct PlanetData
{
    public string title, creator, year, description, executable;
    public string[] des_tag;
    public Sprite image;
}

public class PlanetController : MonoBehaviour, PointableObject
{
    private static PlanetController selectedPlanet = null;

    public PlanetData data;

    //Check if this is the first time the user is selecting a planet (so as not to repeat the tutorial everytime)
	private bool tutorial_firstSelection = true;

    /*
     * Start: Sets planet information, positions, scales, references to controller, planets, menus, and tutorials
     * Parameters: None
     */
    protected void Start()
    {

	}

    protected void Update()
    {
        
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
            if (travelPanels[i].IsYesButton())
            {
                //travelPanels[i].executableString = "/../VRClubUniverse_Data/VR_Demos/" + int.Parse(Planet_Year.text) + "/" + Planet_Executable + "/" + Planet_Executable + ".exe";
            }
        }
    }

    /*
     * SetRightController: Find the right controller by iterating through all the possible controllers and checking if they don't contain the YearSelection object (which belongs to the left controller)
     * Parameters: None
     */
    /*private void SetRightController()
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
    }*/

    /*
     * SelectPlanet: When the user pulls the trigger while pointing at the planet, it should display the travel confirmation menu and update the tutorials if needed
     * Parameters: None
     */
    public void SelectPlanet()
    {
        //Activate the travel confirmation menu
        //Travel_Selection.SetActive(true);

        //Disable any tutorials remaining if they exist
        /*if (tutorialsOnRightController[1].gameObject.activeSelf)
        {
            tutorialsOnRightController[1].gameObject.SetActive(false); //turn off label 4 for selecting a planet
        }*/

    }

    /*
     * DeselectPlanet: When the user clicks 'No' in response to whether they actually want to travel to the planet, the menu disappears
     * Parameters: None
     */
    public void DeselectPlanet()
    {
        //Disable the confirmation travel panel
        //Travel_Selection.SetActive(false);
    }
    

    /*
     * PointerStart: Called when user's laser collides with the planet to toggle the description menu and make changes to tutorial on right controller
     * Parameters: None
     */
    public void PointerEnter()
    {
        //Turn on menu when hovering
        //ToggleMenu(true);

        //If this is the first time selecting a planet, perform change of tutorials
        if (tutorial_firstSelection)
        {
            //tutorialsOnRightController[0].gameObject.SetActive(false); //turn off label 3 for pointing at a planet
            //tutorialsOnRightController[1].transform.parent.gameObject.SetActive(true); //turn on label 4 for selecting a planet
            tutorial_firstSelection = false;
        }
    }

    public void PointerClick()
    {
        if(selectedPlanet == this)
        {
            LeverScript lever = LeverScript.GetInstance();
            lever.SetThrottle(lever.GetDefaultThrottle());
            PlanetDisplay disp = PlanetDisplay.GetInstance();
            if (disp.GetViewTarget() == transform)
            {
                disp.SetVisible(false);
                disp.SetViewTarget(null);
                disp.GetTravelInteractable().SetExeString("");
            }

            selectedPlanet = null;
        }
        else
        {
            LeverScript.GetInstance().SetThrottle(0.0f);
            PlanetDisplay disp = PlanetDisplay.GetInstance();
            disp.SetVisible(true);
            disp.SetViewTarget(transform);
            disp.UpdateInfo(data.title, data.creator, data.description, data.year, data.des_tag, data.image);
            disp.GetTravelInteractable().SetExeString(@"../VRClubUniverse_Data/VR_Demos/" + data.year + @"/" + data.executable + @"/" + data.executable + @".exe");

            selectedPlanet = this;
        }
        
    }

    /*
     * PointerStop: Calls when the user points their laser away from the planet to disable the description menu
     * Parameters: None
     */
    public void PointerExit()
    {
        //Turn off floating menu panel when not using
        //ToggleMenu(false);
    }

}