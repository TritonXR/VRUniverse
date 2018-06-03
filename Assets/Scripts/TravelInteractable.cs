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
    //Image component to show the highlight of the pointed selection
    [SerializeField] private Image highlight;

    //Access to the planet executable string
    [SerializeField] private string executableString;

    [SerializeField] public UniverseSystem USystem;

    void Start()
    {
        
    }

    /*
     * PointerStart: Called whenever the user points at the travel menu button.
     */
    public void PointerEnter()
    {
        //Enable the highlight to show it is being hovered on
        highlight.enabled = true;
        
    }

    /*
     * PointerStop: Called whenever the user stops pointing at the travel menu button.
     */
    public void PointerExit()
    {
        //Disable the highlight
        highlight.enabled = false;
    }

    public void PointerClick()
    {
        BeginTravel();
    }

    /*
     * BeginTravel: Save current year user is in and travel to the planet executable 
     * Parameters: None
     */
    public void BeginTravel()
    {
        //Save the current year in an output text file

        //Write the year index to the following path
        string path;
#if UNITY_EDITOR
            if (USystem.GetOculusBool()) path = Application.dataPath + "/../Website/data/VRClubUniverseData/Oculus/saveData.txt"; //saveData not exist on my side
        else path = Application.dataPath + "/../Website/data/VRClubUniverseData/Vive/saveData.txt"; //saveData not exist on my side
#elif UNITY_STANDALONE
            if (USystem.GetOculusBool()) path = Application.dataPath + "/../VRClubUniverseData/Oculus/saveData.txt"; //Change everything under VRClubUniverseData to VR../Vive/
            else path = Application.dataPath + "/../VRClubUniverseData/Vive/saveData.txt"; //Change everything under VRClubUniverseData to VR../Vive/
        
            
#endif

        string currentYear = UniverseSystem.GetInstance().GetCurrentYear();
        Debug.Log("Writing current year to saveData file, year Index: " + currentYear);
        File.WriteAllText(path, currentYear);

        //Begin loading the executable
        Debug.Log("loading executable: " + executableString);
        ExecutableSwitch.LoadExe(executableString);
        
    }

    public void SetExeString(string exe)
    {
        executableString = exe;
    }

    public string GetExeString()
    {
        return executableString;
    }

}
