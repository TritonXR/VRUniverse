using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class YearSelection : MonoBehaviour
{
	private static readonly int CURRENT_YEAR = DateTime.Now.Year;
    public int SelectedYear { get; private set; }
	private int displayedYear;
	private Text yearText;

    private void Start()
    {
        SelectedYear = CURRENT_YEAR;
        displayedYear = SelectedYear;
        yearText = GetComponent<Text>();
        updateYearText();
    }

    public void attemptToChangeYears()
    {
        if (displayedYear != SelectedYear)
        {
            SelectedYear = displayedYear;
            updateYearText();
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
		yearText.color = displayedYear == SelectedYear ? Color.green : Color.black;
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
