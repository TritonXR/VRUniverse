using UnityEngine;
using UnityEngine.UI;
using System.IO;

/*
 * Name: TravelInteractable.cs
 * Description: Contains methods called when user points and pulls the trigger at a confirmation menu. Handles traveling to the planet
 * Utilized on: Planet.prefab
 */

public class TravelInteractable : MonoBehaviour
{
    //Check if searching for yes or no answer
    public bool isYes; 

    //Image component to show the highlight of the pointed selection
    private Image highlight;

    //Access to the planet executable string
    public string executableString;

    /*
     * StartHoverButton: Called whenever the user points at the travel menu button.
     * Parameters: GameObject obj - reference to the button, should be itself
     */
    public void StartHoverButton(GameObject obj)
    {
        //Gets reference to the highlight
        highlight = obj.GetComponent<Image>();

        //Enable the highlight to show it is being hovered on
        highlight.enabled = true;
        
    }

    /*
     * StopHoverButton: Called whenever the user stops pointing at the travel menu button.
     * Parameters: GameObject obj - reference to the button, should be itself
     */
    public void StopHoverButton(GameObject obj)
    {
        //Gets reference to the highlight if it doesn't exist
        if (!highlight)
        {
            highlight = obj.GetComponent<Image>();
        }

        //Disable the highlight
        highlight.enabled = false;
        highlight = null;
    }

    /*
     * BeginTravel: Save current year user is in and travel to the planet executable 
     * Parameters: None
     */
    public void BeginTravel()
    {
        //Save the current year in an output text file
        YearSelection yearSelection = Camera.main.transform.root.GetComponentInChildren<YearSelection>(true);

        //Write the year index to the following path
        string path = "VRClubUniverse_Data/saveData.txt";
        string currentYear = yearSelection.SelectedYearIndex.ToString();
        Debug.Log("Writing current year to saveData file, year Index: " + currentYear);
        File.WriteAllText(path, currentYear);

        //Begin loading the executable
        Debug.Log("loading executable: " + executableString);
        ExecutableSwitch.LoadExe(executableString);
        
    }

}
