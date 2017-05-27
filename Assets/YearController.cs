using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YearController : MonoBehaviour
{
    private YearSelection yearSelection;

    private void Start ()
    {
        //transform.root.GetComponent<UniverseSystem>().StartCoroutine(transform.root.GetComponent<UniverseSystem>().TeleportToYear(2015));
        Debug.Log("Started year controller");

        yearSelection = Camera.main.transform.root.GetComponentInChildren<YearSelection>(true);

        /*
        GameObject leftleft = Camera.main.transform.root.GetComponent<SteamVR_ControllerManager>().left;
        Debug.Log("leftleft: " + leftleft.name);

        SteamVR_TrackedController controlObj = leftleft.GetComponent<SteamVR_TrackedController>();
        if (controlObj)
        {
            Debug.Log("access to controller");
        }
        else
        {
            Debug.Log("noooo");
        }
        */




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

        //transform.root.GetComponent<UniverseSystem>().RadialMenu_TeleportToYear(yearSelection.SelectedYear);
        transform.root.GetComponent<UniverseSystem>().StartCoroutine(transform.root.GetComponent<UniverseSystem>().TeleportToYear(yearSelection.SelectedYear));
    }
}
