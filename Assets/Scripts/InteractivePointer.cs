using UnityEngine;

/*
 * Name: InteractivePointer.cs
 * Description: Contains methods for detecting when the user points or pulls the trigger on certain gameobjects
 * Utilized on: The right controller of the CameraRig object
 */

public class InteractivePointer : MonoBehaviour {

    //Contains references to the color when the user is not pointing at a specific gameobject versus when they are
    [SerializeField] private Color inactiveColor;
    [SerializeField] private Color activeColor;
    [SerializeField] private LeverScript lever;

    //Reference to the right controller
    private SteamVR_TrackedController controller;

    //Reference to the steamvr laser
    private SteamVR_LaserPointer laser;

    //Reference to the planet or menu to check for when interacting with objects
    private PointableObject targetObject;

    //Check to ensure user doesn't click on a gameobject multiple times
    private bool isTriggerClickable = false;

    /*
     * Start: Initializes controller, laser, and handles first pointer delegates to the laser
     * Parameters: None
     */
    void Start () {

        //Initialize controller and laser
        controller = GetComponent<SteamVR_TrackedController>();
        laser = GetComponent<SteamVR_LaserPointer>();

        //Handle delegates and tracking whether the pointer collides with an object
        laser.PointerIn += HandlePointerIn;
        laser.PointerOut += HandlePointerOut;

		//Set laser to inactive first
		SetPointerColor(false);
	}

	/*
     * SetPointerColor: Set the color of the laser
     * Parameters: bool active - true means the laser is active so should be activeColor
	 *			                 false means the laser is not active so should be inactiveColor
     */
	private void SetPointerColor(bool active)
	{
		//Ensure that the laser exists
		if (laser.pointer)
		{
			if (active)
			{
				//Only sets the color in the component. Need to directly access the pointer gameobject and set the material color
				laser.color = activeColor;
				laser.pointer.GetComponent<MeshRenderer>().material.color = activeColor;
			}
			else
			{
				laser.color = inactiveColor;
				laser.pointer.GetComponent<MeshRenderer>().material.color = inactiveColor;
			}
		}
	}

	/*
     * HandlePointerIn: Called whenever the user points at ANY gameobject. But only calls object methods if it is a planet or travelMenu.
     * Parameters: object sender - contains method info
     *             PointerEventArgs e - contains the hit information such as the gameobject being collided with
     */
	private void HandlePointerIn(object sender, PointerEventArgs e)
    {
        //Initialize references of the object the laser collided with
        targetObject = e.target.GetComponent<PointableObject>();

        //Check if the pointer is pointed at a planet
        if (targetObject != null) 
        {
			//Activate the laser to be green
			SetPointerColor(true);

            //Have planet react to the pointing
            targetObject.PointerStart();

            //Ensure that the user didn't already click on the planet
            if (!isTriggerClickable)
            {
                isTriggerClickable = true;

                //Set a delegate so that the user can click on the planet as long as they are pointing at it
                controller.TriggerClicked += HandlePointerClick;
            }
        }
    }

    /*
     * HandlePointerOut: Called whenever the user clicks on a gameobject with a PointableObject script
     * Parameters: object sender - contains method info
     *             PointerClickedEventArgsEventArgs e - contains the hit information such as the gameobject being collided with
     */
    private void HandlePointerClick(object sender, ClickedEventArgs e)
    {
        targetObject.PointerClick();
    }

    /*
     * HandlePointerOut: Called whenever the user points away from a gameobject. But only calls object methods if it is a planet or travelMenu.
     * Parameters: object sender - contains method info
     *             PointerEventArgs e - contains the hit information such as the gameobject being collided with
     */
    private void HandlePointerOut(object sender, PointerEventArgs e)
    {
        //Initialize references of the object the laser collided with
        targetObject = e.target.GetComponent<PointableObject>();

        //Check if the pointer stopped pointing at a planet
        if (targetObject != null)
        {
            //Deactivate the laser to be red
            SetPointerColor(false);

            //Have planet stopped reacting
            targetObject.PointerStart();

            //Ensure that the user didn't already click on the menu
            if (isTriggerClickable)
            {
                isTriggerClickable = false;
                
                //Disable delegate
                controller.TriggerClicked -= HandlePointerClick;
            }
        }
    }

    /*
     * HandlePlanetTriggerClicked: Called whenever the user pulls the trigger when pointing at a planet
     * Parameters: object sender - contains method info
     *             PointerEventArgs e - contains the hit information such as the gameobject being collided with
     *
    private void HandlePlanetTriggerClicked(object sender, ClickedEventArgs e)
    {
        //React to trigger click
        planet.SelectPlanet();
        lever.SetThrottle(0.0f);
    }

    /*
     * HandleMenuTriggerClicked: Called whenever the user pulls the trigger when pointing at a travel menu
     * Parameters: object sender - contains method info
     *             PointerEventArgs e - contains the hit information such as the gameobject being collided with
     *
    private void HandleMenuTriggerClicked(object sender, ClickedEventArgs e)
	{
        //If the button they clicked was the Yes confirmation, travel to the planet
        if (travelMenuButton.isYes)
        {
            travelMenuButton.BeginTravel();
        }

        //If the button they clicked was the No confimration, deselect the planet/hide the menu
        else
        {
            travelMenuButton.GetComponentInParent<PlanetController>().DeselectPlanet();
            lever.SetThrottle(lever.GetDefaultThrottle());
        }
	}*/
}

public interface PointableObject
{
    void PointerStart();
    void PointerClick();
    void PointerStop();
}
