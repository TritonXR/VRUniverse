using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class TravelInteractable : MonoBehaviour
{

    public bool isYes; //check if searching for yes or no answer

    //Image component to show the highlight of the pointed selection
    private Image highlight;

    //Access to the planet executable string
    public string executableString;

    //Bool to detect if currently implementing trigger action
    public bool processingPress = false;

    public void StartHoverButton(GameObject obj)
    {
        highlight = obj.GetComponent<Image>();
        highlight.enabled = true;
        
    }

    public void StopHoverButton(GameObject obj)
    {
        if (!highlight)
        {
            highlight = obj.GetComponent<Image>();
        }

        highlight.enabled = false;
        highlight = null;
    }

    public void BeginTravel()
    {
        //save the current year in an output text file
        YearSelection yearSelection = Camera.main.transform.root.GetComponentInChildren<YearSelection>(true);

        string path = "VRClubUniverse_Data/saveData.txt";
        string currentYear = yearSelection.SelectedYearIndex.ToString();
        Debug.Log("writing current year to saveData file, year Index: " + currentYear);
        File.WriteAllText(path, currentYear);

        Debug.Log("loading executable: " + executableString);
        ExecutableSwitch.LoadExe(executableString);
        
    }

}
