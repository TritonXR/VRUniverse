using UnityEngine;
using UnityEngine.UI;
using System.IO;

/*
 * Name: TravelInteractable.cs
 * Description: Contains methods called when user points and pulls the trigger at a confirmation menu. Handles traveling to the planet
 * Utilized on: Planet.prefab
 */

public class TravelInteractable : MonoBehaviour, PointableObject
{
    //Check if searching for yes or no answer
    public bool isYes; 

    //Image component to show the highlight of the pointed selection
    private Image highlight;

    //Access to the planet executable string
    public string executableString;

    void Start()
    {
        highlight = GetComponent<Image>();
    }

    /*
     * PointerStart: Called whenever the user points at the travel menu button.
     */
    public void PointerStart()
    {
        //Enable the highlight to show it is being hovered on
        highlight.enabled = true;
        
    }

    /*
     * PointerStop: Called whenever the user stops pointing at the travel menu button.
     */
    public void PointerStop()
    {
        //Disable the highlight
        highlight.enabled = false;
    }

    public void PointerClick()
    {
        if(isYes)
        {
            BeginTravel();
        }
        else
        {
            GetComponentInParent<PlanetController>().DeselectPlanet();
            LeverScript lever = LeverScript.GetInstance();
            lever.SetThrottle(lever.GetDefaultThrottle());
        }
    }

    /*
     * BeginTravel: Save current year user is in and travel to the planet executable 
     * Parameters: None
     */
    public void BeginTravel()
    {
        //Save the current year in an output text file

        //Write the year index to the following path
        string path = "VRClubUniverse_Data/saveData.txt";
        string currentYear = UniverseSystem.GetInstance().GetCurrentYear();
        Debug.Log("Writing current year to saveData file, year Index: " + currentYear);
        File.WriteAllText(path, currentYear);

        //Begin loading the executable
        Debug.Log("loading executable: " + executableString);
        ExecutableSwitch.LoadExe(executableString);
        
    }

}
