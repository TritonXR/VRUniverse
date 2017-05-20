using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YearController : MonoBehaviour
{
    private YearSelection yearSelection;

    private void Start ()
    {
        yearSelection = Camera.main.transform.root.GetComponentInChildren<YearSelection>();
        SteamVR_TrackedController leftController = Camera.main.transform.root.GetComponentInChildren<SteamVR_TrackedController>();
        leftController.TriggerClicked += HandleTriggerClicked;
    }

    private void HandleTriggerClicked(object sender, ClickedEventArgs e)
    {
        yearSelection.attemptToChangeYears();

        // freeze radial menu

        transform.root.GetComponent<UniverseSystem>().RadialMenu_TeleportToYear(yearSelection.SelectedYear);
    }
}
