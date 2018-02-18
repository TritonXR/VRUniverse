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
	//private static readonly int CURRENT_YEAR = DateTime.Now.Year; No longer needed because you start in lobby
    public int SelectedYearIndex { get; set; } // [0; 1; 2] --> UniverseSystem.list_years[0],[1],[2]

    public bool isTravelling { private get; set; }

    private const string LOBBY_YEAR_STRING = "Year";
    public string displayedYearString; // ["Year"; "0"; "1"; "2"]
	private Text yearText;

    private int minimumYear, maximumYear;

    private List<int> listYearNames; // [2015; 2016; 2017]

    public bool tutorial_firstSelection = true;

    private void Start()
    {

        //Initialize list of years that contains the name 
        listYearNames = new List<int>();

        // Access the list of years and insert the int year into a list
        /*for (int i = 0; i < list_years.Count; i++)
        {
            //Adds the year names into a list
            listYearNames.Add(int.Parse(list_years[i].yr_name));
        }*/ 

		//String that shows on the controller
		if (string.IsNullOrEmpty(displayedYearString))
		{
			SelectedYearIndex = -1; // The starting year should be -1 which is the lobby index
			displayedYearString = LOBBY_YEAR_STRING;
		}

        //displayedYear = SelectedYear; No longer needed because you start in lobby
        yearText = GetComponent<Text>(); //The text of the year selection controller

        // Set the text on the controller and the color
        updateYearText();

        /*
         * Can't pass in 2015/2016/2017 year names to the universe system. It teleports based on the index of the number in the year list
         * e.g. in [2015,2016,2017], 2015 is index 0, 2016 is 1, 2017 is 2.
         */
    }
    
    public bool isAbleToTravel()
    {
        return (displayedYearString != SelectedYearIndex.ToString()) && (displayedYearString != LOBBY_YEAR_STRING);
    }

    public void changeYears()
    {
        SelectedYearIndex = int.Parse(displayedYearString);
        updateYearText();
    }

    private void checkTutorial()
    {
        if (tutorial_firstSelection)
        {
            tutorial_firstSelection = false;
        }
    }

    public void nextYear()
	{

        // Disable "navigate a year" tutorial and enable "select a year"
        checkTutorial();

        // Check if there are years to travel to and not travelling
        if (listYearNames.Count != 0 && !isTravelling)
        {

            // Handle case if user is in lobby
            if (displayedYearString == LOBBY_YEAR_STRING)
            {
                // display the first year available
                displayedYearString = "0";
            }
            else
            {
                int newYear;
                // if the current year is NOT the maximum index and user can go to the next year, 
                /*if (int.Parse(displayedYearString) < (UniverseSystem.list_years.Count - 1))
                {
                    // increment the displayed year
                    newYear = int.Parse(displayedYearString) + 1;
                }
                // else if the year is maximum
                else
                {
                    // wrap around to beginning
                    newYear = 0;
                }
                displayedYearString = newYear.ToString();*/
            }
        }
        // Update the text on the screen
		updateYearText();
	}

	public void prevYear()
	{

        // Disable "navigate a year" tutorial and enable "select a year"
        checkTutorial();

        // Check if there are years to travel to and not travelling
        if (listYearNames.Count != 0 && !isTravelling)
        {
            // Handle case if user is in lobby
            if (displayedYearString == LOBBY_YEAR_STRING)
            {
                // implement some way of notifying the user that they reached the end of the list
                Debug.Log("Failure: Can't go further past YEAR");
            }
            else
            {
                int newYear;
                // if the current year is NOT the minimum index and user can go to the previous year, 
                if (int.Parse(displayedYearString) != 0)
                {
                    // decrement the displayed year
                    newYear = int.Parse(displayedYearString) - 1;
                }
                // else if the year is minimum
                else
                {
                    // wrap around to end
                    newYear = listYearNames.Count - 1;
                }
                displayedYearString = newYear.ToString();
            }
        }
        // Update the text on the screen
        updateYearText();
	}

	public void updateYearText()
	{
        Debug.Log("Updating year to: " + displayedYearString);
        // Since lobby has the string "Year" which is not an int, only show the word Year
        if (displayedYearString == LOBBY_YEAR_STRING)
        {
            yearText.text = displayedYearString;
        }
        else // Set the text of the displayed string to the controller year selection by grabbing the name from the index
        {
			//Debug.Log("0:" + listYearNames[0]);
			//Debug.Log("1: " + listYearNames[1]);
			//Debug.Log("Text: " + yearText.text);
            yearText.text = listYearNames[int.Parse(displayedYearString)].ToString();
        }

        // If the string displayed equals the index that was previously selected OR user is in the lobby, turn the text green. Or keep it black
        if (displayedYearString == SelectedYearIndex.ToString() || displayedYearString == LOBBY_YEAR_STRING)
        {
            yearText.color = Color.green;
        }
        else
        {
            yearText.color = Color.black;
        }
	}

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            nextYear();
        } else if (Input.GetKeyDown(KeyCode.T))
        {
            prevYear();
        } else if (Input.GetKeyDown(KeyCode.U))
        {
            changeYears();
        }

    //	if (Input.GetKeyDown(KeyCode.A) && displayedYear != selectedYear)
    //	{
    //		selectedYear = displayedYear;
    //		updateYearText();
    //		Debug.Log("Traveling to year " + selectedYear);
    //	}
    }
}
