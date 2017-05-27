using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * Located on the Text component of the YearSelection Canvas
 */

public class YearSelection : MonoBehaviour
{
	private static readonly int CURRENT_YEAR = DateTime.Now.Year;
    public int SelectedYear { get; private set; }
	private int displayedYear;
	private Text yearText;

    private int minimumYear, maximumYear;

    private void Start()
    {

        /*
         * Access the list of years
         * Grab the minimum and maximum year
         */
        string[] tempYearName = UniverseSystem.list_years[0].yr_name.Split('.'); //have to remove the .json
        minimumYear = int.Parse(tempYearName[0]);
        Debug.Log("minim: " + minimumYear);

        tempYearName = UniverseSystem.list_years[UniverseSystem.list_years.Count - 1].yr_name.Split('.');
        maximumYear = int.Parse(tempYearName[0]);
        Debug.Log("maxim: " + maximumYear);


        SelectedYear = CURRENT_YEAR;
        displayedYear = SelectedYear;
        yearText = GetComponent<Text>();
        updateYearText();

        /*
         * Can't pass in 2015/2016/2017 year names to the universe system. It teleports based on the index of the number in the year list
         * e.g. in [2015,2016,2017], 2015 is index 0, 2016 is 1, 2017 is 2.
         */

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
