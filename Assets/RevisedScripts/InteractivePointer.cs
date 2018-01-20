using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractivePointer : MonoBehaviour {

    [SerializeField]
    private Color inactiveColor;
    [SerializeField]
    private Color activeColor;


    private SteamVR_TrackedController controller;
    private SteamVR_LaserPointer laser;

    private ControllerOne planet;
    private TravelInteractable travelMenu;

    private bool isTriggerClickable = false; //check to ensure user doesn't click on a planet multiple times

	// Use this for initialization
	void Start () {
        controller = GetComponent<SteamVR_TrackedController>();
        laser = GetComponent<SteamVR_LaserPointer>();

        //Handle delegates and tracking whether the pointer collides with an object
        laser.PointerIn += HandlePointerIn;
        laser.PointerOut += HandlePointerOut;

        laser.color = inactiveColor;
	}

    private void HandlePointerIn(object sender, PointerEventArgs e)
    {

        planet = e.target.GetComponent<ControllerOne>();
        travelMenu = e.target.GetComponent<TravelInteractable>();


        if (planet) //check if the pointer is pointed at a planet
        {
            Debug.Log("POINTER IN on: planet!");
            laser.color = activeColor;
            planet.StartPointing(e.target.gameObject);

            if (!isTriggerClickable)
            {
                isTriggerClickable = true;
                controller.TriggerClicked += HandleTriggerClicked;
            }
        } else if (travelMenu)
        {
            Debug.Log("POINTER IN on: travel menu!");
            laser.color = activeColor;
            travelMenu.TravelMenu(e.target.gameObject);

            if (!isTriggerClickable)
            {
                isTriggerClickable = true;
                controller.TriggerClicked += HandleTriggerClicked;
            }
        }
    }

    private void HandlePointerOut(object sender, PointerEventArgs e)
    {
        planet = e.target.GetComponent<ControllerOne>();
        travelMenu = e.target.GetComponent<TravelInteractable>();

        if (planet) //check if the pointer stopped pointing at a planet
        {
            Debug.Log("POINTER OUT on: " + e.target.name);
            laser.color = inactiveColor;
            planet.StopPointing(e.target.gameObject);

            if (isTriggerClickable)
            {
                isTriggerClickable = false;
                controller.TriggerClicked -= HandleTriggerClicked;
            }
        } else if (travelMenu)
        {
            Debug.Log("POINTER out on: travel menu!");
            laser.color = inactiveColor;

            if (isTriggerClickable)
            {
                isTriggerClickable = false;
                controller.TriggerClicked -= HandleTriggerClicked;
            }
        }
    }

    private void HandleTriggerClicked(object sender, ClickedEventArgs e)
    {
        planet.SelectPlanet();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
