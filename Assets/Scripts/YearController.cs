using UnityEngine;
using UnityEngine.UI;

/*
 * Name: YearController.cs
 * Description: Contains methods for detecting when the user presses on touchpad and control for the year selection menus
 * Utilized on: YearSelection and the left controller on CameraRig
 */

public class YearController : MonoBehaviour
{
    //Reference to the year selection menu on the left controller
    private YearSelection yearSelection;

    /*
     * Start: Initialize year selection and the left controller, as well as its delegates
     * Parameters: None
     */
    private void Start ()
    {
        //Initialize year selection by getting the component from the camerarig root
        yearSelection = Camera.main.transform.root.GetComponentInChildren<YearSelection>(true);

        //Find all the controllers and iterate through it until it finds the YearSelection component located on the left controller
        SteamVR_TrackedController[] leftController = Camera.main.transform.root.GetComponentsInChildren<SteamVR_TrackedController>(true);
        for (int i = 0; i < leftController.Length; i++)
        {
            //Once found, start adding trigger and pad delegates
            if (leftController[i].GetComponentInChildren<YearSelection>())
            {
                leftController[i].TriggerClicked += HandleTriggerClicked;
                leftController[i].PadClicked += HandlePadClicked;
            }
        }   
    }

    /*
     * HandleTriggerClicked: Called whenever the user pulls the trigger
     * Parameters: object sender - contains method info
     *             PointerEventArgs e - contains the hit information such as the gameobject being collided with
     */
    private void HandleTriggerClicked(object sender, ClickedEventArgs e)
    {
        //Check if the selection menu is at an actual year date
        if (yearSelection.isAbleToTravel())
        {
            //Begin changing years and starting teleport systems
            yearSelection.changeYears();
            UniverseSystem universeSystem = transform.root.GetComponent<UniverseSystem>();
            universeSystem.TeleportToYear(yearSelection.SelectedYearIndex);

            //Update tutorial system
            if (universeSystem.tutorial_YearTravel)
            {
                universeSystem.tutorial_YearTravel.GetComponentInChildren<Image>().gameObject.SetActive(false);
            }
            if (universeSystem.tutorial_PlanetSelection)
            {
                universeSystem.tutorial_PlanetSelection.SetActive(true);
            }
		}
        else
        {
            Debug.LogWarning("Unable to travel");
        }
    }

    /*
     * HandlePadClicked: Called whenever the user presses on the touchpad in the left or right side
     * Parameters: object sender - contains method info
     *             PointerEventArgs e - contains the hit information such as the gameobject being collided with
     */
    private void HandlePadClicked(object sender, ClickedEventArgs e)
    {
        //If the user clicks on the left side of the controller, decrement the year
        if (e.padX < 0)
        {
            yearSelection.prevYear();
        }

        //If the user clicks on the right side of the controller, increment the year
        else
        {
            yearSelection.nextYear();
        }
    }
}
