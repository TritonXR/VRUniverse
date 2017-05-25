using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YearController : MonoBehaviour
{
    private YearSelection yearSelection;

    private void Start ()
    {
        yearSelection = Camera.main.transform.root.GetComponentInChildren<YearSelection>();
        SteamVR_TrackedController[] leftController = Camera.main.transform.root.GetComponentsInChildren<SteamVR_TrackedController>();
        if (Camera.main)
        {
            Debug.Log("camera exists");
        } else
        {
            Debug.Log("camera not exists");
        }
        Debug.Log("The root is: " + Camera.main.transform.root.name);
        for (int i = 0; i < leftController.Length; i++)
        {
            if (leftController[i].GetComponentInChildren<YearSelection>())
            {
                leftController[i].TriggerClicked += HandleTriggerClicked;
            }
        }
        
    }

    private void HandleTriggerClicked(object sender, ClickedEventArgs e)
    {
        yearSelection.attemptToChangeYears();

        // freeze radial menu

        transform.root.GetComponent<UniverseSystem>().RadialMenu_TeleportToYear(yearSelection.SelectedYear);
    }
}
