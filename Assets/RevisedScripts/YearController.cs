using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

            universeSystem.tutorial_RadialMenu.SetActive(false);
            universeSystem.tutorial_TriggerMenu.GetComponentInChildren<YearInput>().gameObject.GetComponent<Text>().text = UniverseSystem.list_years[yearSelection.SelectedYearIndex].yr_name;
            universeSystem.tutorial_TriggerMenu.SetActive(true);

			universeSystem.tutorial_YearTravel.GetComponentInChildren<Image>().gameObject.SetActive(false);
            universeSystem.tutorial_PlanetSelection.SetActive(true);
		}
        else
        {
            Debug.Log("Unable to travel");
        }
    }
}
