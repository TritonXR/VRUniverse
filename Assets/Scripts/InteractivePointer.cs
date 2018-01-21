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

    //Reference to the right controller
    private SteamVR_TrackedController controller;

    //Reference to the steamvr laser
    private SteamVR_LaserPointer laser;

    //Reference to the planet or menu to check for when interacting with objects 
    private PlanetController planet;
    private TravelInteractable travelMenuButton;

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
        planet = e.target.GetComponent<PlanetController>();
		travelMenuButton = e.target.GetComponent<TravelInteractable>();

        //Check if the pointer is pointed at a planet
        if (planet) 
        {
			//Activate the laser to be green
			SetPointerColor(true);

            //Have planet react to the pointing
            planet.StartPointing();

            //Ensure that the user didn't already click on the planet
            if (!isTriggerClickable)
            {
                isTriggerClickable = true;

                //Set a delegate so that the user can click on the planet as long as they are pointing at it
                controller.TriggerClicked += HandlePlanetTriggerClicked;
            }
        }

        //Check if the pointer is pointed at a travelMenu
        else if (travelMenuButton)
        {
			//Activate the laser to be green
			SetPointerColor(true);

            //Have travel menu react to pointing
			travelMenuButton.StartHoverButton(e.target.gameObject);

            //Ensure that the user didn't already click on the menu
            if (!isTriggerClickable)
            {
                isTriggerClickable = true;

                //Set a delegate so that the user can click on the menu as long as they are pointing at it
                controller.TriggerClicked += HandleMenuTriggerClicked;
            }
        }
    }

    /*
     * HandlePointerOut: Called whenever the user points away from a gameobject. But only calls object methods if it is a planet or travelMenu.
     * Parameters: object sender - contains method info
     *             PointerEventArgs e - contains the hit information such as the gameobject being collided with
     */
    private void HandlePointerOut(object sender, PointerEventArgs e)
    {
        //Initialize references of the object the laser collided with
        planet = e.target.GetComponent<PlanetController>();
		travelMenuButton = e.target.GetComponent<TravelInteractable>();

        //Check if the pointer stopped pointing at a planet
        if (planet)
        {
			//Deactivate the laser to be red
			SetPointerColor(false);

            //Have planet stopped reacting
            planet.StopPointing();

            //Ensure that the user didn't already click on the menu
            if (isTriggerClickable)
            {
                isTriggerClickable = false;
                
                //Disable delegate
                controller.TriggerClicked -= HandlePlanetTriggerClicked;
            }
        }

        //Check if pointer stopped pointing at a travele menu
        else if (travelMenuButton)
        {
			//Deactivate the laser to be red
			SetPointerColor(false);

            //Have travel menu stopped reacting
            travelMenuButton.StopHoverButton(e.target.gameObject);

            //Ensure that the user didn't already click on the menu
            if (isTriggerClickable)
            {
                isTriggerClickable = false;

                //Disable delegate
                controller.TriggerClicked -= HandleMenuTriggerClicked;
            }
        }
    }

    /*
     * HandlePlanetTriggerClicked: Called whenever the user pulls the trigger when pointing at a planet
     * Parameters: object sender - contains method info
     *             PointerEventArgs e - contains the hit information such as the gameobject being collided with
     */
    private void HandlePlanetTriggerClicked(object sender, ClickedEventArgs e)
    {
        //React to trigger click
        planet.SelectPlanet();
    }

    /*
     * HandleMenuTriggerClicked: Called whenever the user pulls the trigger when pointing at a travel menu
     * Parameters: object sender - contains method info
     *             PointerEventArgs e - contains the hit information such as the gameobject being collided with
     */
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
        }
	}
}
