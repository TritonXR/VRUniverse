using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IUI : MonoBehaviour {
    //offsets of various menus relative to the planet in viewing space (neg/pos effects: x => left/right, y=down/up, z=closer/farther)
    [SerializeField]
    private Vector3 PlanetMenu_Offset = new Vector3(0, 0, 0);
    [SerializeField]
    private Vector3 TravelMenu_Offset = new Vector3(0, 0, 0);
    //Menu's controlled by hovering and selecting
    private GameObject Planet_Menu; //Entire Planet_Menu attached to each planet that shows planet data

    //Travel menu where user selects whether they want to travel or not. accessible by travelinteractable by clicking no
    private GameObject Travel_Selection;

    //Planet reference
    private PlanetController Planet;
	
	// Update is called once per frame
	void Update () {
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

    // Show description
    public void ShowDetail(PlanetController planet, GameObject planet_menu, GameObject travel_selection)
    {
        if (Planet == planet)
        {
            return;
        }
        if (Planet)
        {
            Planet.DeselectPlanet();
            Planet = planet;
            Planet_Menu = planet_menu;
            Travel_Selection = travel_selection;
        }

        //Activate the planet description
        Planet_Menu.SetActive(true);

        //Activate the travel confirmation menu
        Travel_Selection.SetActive(true);
    }

    // Hide panel
    public void HidePanel()
    {
        if (Planet) {
            Planet = null;
            Planet_Menu = null;
            Travel_Selection = null;

            //Hide planet description
            Planet_Menu.SetActive(false);
            //Disable the confirmation travel panel
            Travel_Selection.SetActive(false);
            //Dehighlight planet
            Planet.DeselectPlanet();
        }
    }
}
