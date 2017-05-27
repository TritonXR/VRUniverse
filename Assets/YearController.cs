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
                Debug.Log("Found. Added listener for left controller trigger");
                leftController[i].TriggerClicked += HandleTriggerClicked;
            }
        }
        
        
    }

    private void HandleTriggerClicked(object sender, ClickedEventArgs e)
    {

        Debug.Log("Trigger clicked");

        Debug.Log("Leaving year: " + yearSelection.SelectedYear);

        yearSelection.attemptToChangeYears();

        Debug.Log("going to year: " + yearSelection.SelectedYear);

        // freeze radial menu

        transform.root.GetComponent<UniverseSystem>().StartCoroutine(transform.root.GetComponent<UniverseSystem>().TeleportToYear(yearSelection.SelectedYear));
    }
}
