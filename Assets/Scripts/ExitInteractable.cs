using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/*
 * Name: ExitInteractable.cs
 * Description: Contains methods called when user points and pulls the trigger at a confirmation menu. Handles exit the menu
 * Utilized on: Planet.prefab
 */

public class ExitInteractable : MonoBehaviour, PointableObject
{

    // Use this for initialization
    //Image component to show the highlight of the pointed selection
    [SerializeField] private Image highlight;
    [SerializeField] private Canvas updatedCanvas;
    [SerializeField] private PlanetController selectedPlanet;

    //Access to the planet executable string
    //[SerializeField] private string executableString;

    void Start()
    {
        //selectedPlanet = updatedCanvas.gameObject.GetComponent<PlanetDisplay>().targetplanet.gameObject.GetComponent<PlanetController>();
        //highlight.enabled = true;


    }

    /*
     * PointerStart: Called whenever the user points at the travel menu button.
     */
    public void PointerEnter()
    {
        //Enable the highlight to show it is being hovered on
        highlight.color = Color.red;
    }

    /*
     * PointerStop: Called whenever the user stops pointing at the travel menu button.
     */
    public void PointerExit()
    {
        highlight.color = Color.blue;

    }

    public void PointerClick()
    {
        Exit();
    }

    /*
     * Exit: Turn off the menu when triggered 
     * Parameters: None
     */
    public void Exit()
    {
        //selectedPlane = GetComponentInParent<>();
        updatedCanvas.enabled = false;
        PlanetController.DeselectPlanet();
    }



}

