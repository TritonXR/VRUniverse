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