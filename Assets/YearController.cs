using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YearController : MonoBehaviour
{
    private YearSelection yearSelection;

    private void Start ()
    {

        yearSelection = Camera.main.transform.root.GetComponentInChildren<YearSelection>(true);

        SteamVR_TrackedController[] leftController = Camera.main.transform.root.GetComponentsInChildren<SteamVR_TrackedController>(true);
        for (int i = 0; i < leftController.Length; i++)
        {
            if (leftController[i].GetComponentInChildren<YearSelection>())
            {
                leftController[i].TriggerClicked += HandleTriggerClicked;
            } else
            {
                Debug.Log("Error: Unable to find left controller");
            }
        }
        
        
    }

    private void HandleTriggerClicked(object sender, ClickedEventArgs e)
    {

        yearSelection.attemptToChangeYears();

        Debug.Log("Traveling to Year: " + yearSelection.SelectedYearIndex);

        // freeze radial menu

        // convert the year selection to the proper index
        transform.root.GetComponent<UniverseSystem>().StartCoroutine(transform.root.GetComponent<UniverseSystem>().TeleportToYear(yearSelection.SelectedYearIndex));
    }
}
