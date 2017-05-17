using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class YearSelection : MonoBehaviour
{
	private static readonly int CURRENT_YEAR = DateTime.Now.Year;
	private int selectedYear = CURRENT_YEAR;
	private int displayedYear;
	private Text yearText;
	
	private void Start()
	{
		displayedYear = selectedYear;
		yearText = GetComponent<Text>();
		updateYearText();

        SteamVR_TrackedController leftController = transform.root.GetComponentInChildren<SteamVR_TrackedController>();
        leftController.TriggerClicked += HandleTriggerClicked;
    }

    private void HandleTriggerClicked(object sender, ClickedEventArgs e)
    {
        if (displayedYear != selectedYear)
        {
            selectedYear = displayedYear;
            updateYearText();
            Debug.Log("Traveling to year " + selectedYear);
        }
    }

    public void nextYear()
	{
		displayedYear++;
		updateYearText();
	}

	public void prevYear()
	{
		displayedYear--;
		updateYearText();
	}

	private void updateYearText()
	{
		yearText.text = displayedYear.ToString();
		yearText.color = displayedYear == selectedYear ? Color.green : Color.black;
	}

    //private void Update()
    //{
    //	if (Input.GetKeyDown(KeyCode.A) && displayedYear != selectedYear)
    //	{
    //		selectedYear = displayedYear;
    //		updateYearText();
    //		Debug.Log("Traveling to year " + selectedYear);
    //	}
    //}
}
