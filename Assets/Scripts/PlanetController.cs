using UnityEngine;
using UnityEngine.UI;

/*
 * Name: PlanetController.cs
 * Description: Contains methods called when user points and pulls the trigger at a planet. Distributes information to its floating menu and determining where the executable is
 * Utilized on: Planet.prefab that is instantiated with every planet/project
 */

public struct PlanetData
{
    public string title, creator, year, description, executable, image_name;
    public string[] des_tag;
    public Sprite image;
}

public class PlanetController : MonoBehaviour, PointableObject
{
    private static PlanetController selectedPlanet = null;

    public PlanetData data;

    //Check if this is the first time the user is selecting a planet (so as not to repeat the tutorial everytime)
	private bool tutorial_firstSelection = true;

	//Place the default shader of the planet here
	public Shader shader1;

	//Place the highlight shader here
	public Shader shader2;

	// renderer of this object
	Renderer rend;


    /*
     * Start: Sets planet information, positions, scales, references to controller, planets, menus, and tutorials
     * Parameters: None
     */
    protected void Start()
    {
		// Get the renderer on start
		rend = GetComponent<Renderer>();

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

        if (selectedPlanet != this)
        {
            Highlight("hover"); // highlights the planet
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

            Highlight("none"); // disables highlights
        }
        else
        {
            LeverScript.GetInstance().SetThrottle(0.0f);
            PlanetDisplay disp = PlanetDisplay.GetInstance();
            disp.SetVisible(true);
            disp.SetViewTarget(transform);
            disp.UpdateInfo(data.title, data.creator, data.description, data.year, data.des_tag, data.image);
            disp.GetTravelInteractable().SetExeString(ExecutableSwitch.GetFullPath(data.executable + ".exe", data.executable, data.year));

            if (selectedPlanet != null)
            {
                selectedPlanet.Highlight("none");
            }
            selectedPlanet = this;

            Highlight("selected");
        }

        
    }

    public static void DeselectPlanet()
    {
        if (selectedPlanet != null)
        {

            LeverScript lever = LeverScript.GetInstance();
            lever.SetThrottle(lever.GetDefaultThrottle());
            PlanetDisplay disp = PlanetDisplay.GetInstance();
            if (disp.GetViewTarget() == selectedPlanet.transform)
            {
                disp.SetVisible(false);
                disp.SetViewTarget(null);
                disp.GetTravelInteractable().SetExeString("");
            }
            selectedPlanet.Highlight("none");
            selectedPlanet = null;

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

        if (selectedPlanet != this)
        {
            Highlight("none"); // disables highlights
        }
    }

	public void Highlight(string version) {
		switch (version)
		{
		case "hover":
			rend.material.shader = shader2;
			rend.material.SetColor("_OutlineColor", new Color32(43, 255, 233, 255));
			rend.material.SetFloat("_Outline", (float)0.04);
			break;
		case "selected":
			rend.material.shader = shader2;
			rend.material.SetColor("_OutlineColor", new Color32(24, 249, 51, 255));
			rend.material.SetFloat("_Outline", (float)0.04);
			break;
		default:
			rend.material.shader = shader1;
			break;
		}
        //CheckPlanetTextures();
	} 

    public void ReceiveSprite(Sprite image)
    {
        Debug.Log("PlanetController received Sprite");
        data.image = image;
        if(selectedPlanet == this)
        {
            PlanetDisplay.GetInstance().UpdateInfo(data.title, data.creator, data.description, data.year, data.des_tag, data.image);
        }
    }

    /*
    public void CheckPlanetTextures()
    {
        GameObject system = UniverseSystem.GetInstance().gameObject;
        if (system == null)
        {
            Debug.Log("system is null");
        }
        ChangeValue val = system.GetComponentInChildren<ChangeValue>();
        if (val == null)
        {
            Debug.Log("change value is null");
        }
        Debug.Log("data title: " + data.title);
        Debug.Log("data year: " + data.year);
        Debug.Log("renderer: " + rend);
        val.change(rend, data.title, int.Parse(data.year));
    }
    */

}