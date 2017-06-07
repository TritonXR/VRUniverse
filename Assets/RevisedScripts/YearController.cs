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
            }
        }   
    }

    private void HandleTriggerClicked(object sender, ClickedEventArgs e)
    {
        if (yearSelection.isAbleToTravel())
        {
            yearSelection.changeYears();
            UniverseSystem universeSystem = transform.root.GetComponent<UniverseSystem>();
            universeSystem.StartCoroutine(universeSystem.TeleportToYear(yearSelection.SelectedYearIndex));
        }
        else
        {
            Debug.Log("Unable to travel");
        }
    }
}
