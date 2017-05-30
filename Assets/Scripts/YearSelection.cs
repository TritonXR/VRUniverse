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
    public int SelectedYearIndex { get; private set; } // [0; 1; 2] --> UniverseSystem.list_years[0],[1],[2]
	private string displayedYearString; // ["Year"; "0"; "1"; "2"]
	private Text yearText;

    private int minimumYear, maximumYear;

    private List<int> listYearNames; // [2015; 2016; 2017]

    private void Start()
    {

        //Initialize list of years that contains the name 
        listYearNames = new List<int>();

        // Access the list of years and insert the int year into a list
        for (int i = 0; i < UniverseSystem.list_years.Count; i++)
        {
            //Adds the year names into a list
            listYearNames.Add(int.Parse(UniverseSystem.list_years[i].yr_name));
        }

        //SelectedYear = CURRENT_YEAR; No longer needed because you start in lobby
        SelectedYearIndex = -1; // The starting year should be -1 which is the lobby index

        //String that shows on the controller
        displayedYearString = "Year";

        //displayedYear = SelectedYear; No longer needed because you start in lobby
        yearText = GetComponent<Text>(); //The text of the year selection controller

        // Set the text on the controller and the color
        updateYearText();

        /*
         * Can't pass in 2015/2016/2017 year names to the universe system. It teleports based on the index of the number in the year list
         * e.g. in [2015,2016,2017], 2015 is index 0, 2016 is 1, 2017 is 2.
         */

    }

    public void attemptToChangeYears()
    {
      
        if (displayedYearString != SelectedYearIndex.ToString())
        {
            SelectedYearIndex = int.Parse(displayedYearString);
            updateYearText();
        } else
        {
            Debug.Log("[attempt] Same year");
        }
        
    }

    public void nextYear()
	{
        // Check if there are years to travel to
        if (listYearNames.Count != 0)
        {
            // Handle case if user is in lobby
            if (displayedYearString == "Year")
            {
                // display the first year available
                displayedYearString = "0";
                Debug.Log("[next] new year from lobby: " + displayedYearString);
            }
            else
            {
                // if the current year is NOT the maximum index and user can go to the next year, 
                if (int.Parse(displayedYearString) < (UniverseSystem.list_years.Count - 1))
                {
                    // increment the displayed year
                    int newYear = int.Parse(displayedYearString) + 1;
                    displayedYearString = newYear.ToString();
                    Debug.Log("[next] new year: " + displayedYearString);
                }
                // else if the year is maximum
                else
                {
                    // implement some way of notifying user that they reached the end of the list
                }
            }
        }

        // Update the text on the screen
		updateYearText();
	}

	public void prevYear()
	{

        // Check if there are years to travel to
        if (listYearNames.Count != 0)
        {
            // Handle case if user is in lobby
            if (displayedYearString == "Year")
            {
                // implement some way of notifying the user that they reached the end of the list
                Debug.Log("Failure: Can't go further past YEAR");
            }
            else
            {
                // if the current year is NOT the minimum index and user can go to the previous year, 
                if (int.Parse(displayedYearString) != 0)
                {
                    // decrement the displayed year
                    int newYear = int.Parse(displayedYearString) - 1;
                    displayedYearString = newYear.ToString();
                    Debug.Log("[prev] new year: " + displayedYearString);
                }
                // else if the year is minimum
                else
                {
                    // implement some way of notifying user that they reached the beginning of the list
                }
            }
        }

        // Update the text on the screen
        updateYearText();
	}

	private void updateYearText()
	{
        // Since lobby has the string "Year" which is not an int, only show the word Year
        if (displayedYearString == "Year")
        {
            yearText.text = displayedYearString;
        }
        else // Set the text of the displayed string to the controller year selection by grabbing the name from the index
        {
            yearText.text = listYearNames[int.Parse(displayedYearString)].ToString();
        }

        // If the string displayed equals the index that was previously selected OR user is in the lobby, turn the text green. Or keep it black
        if (displayedYearString == SelectedYearIndex.ToString() || displayedYearString == "Year")
        {
            yearText.color = Color.green;
        } else
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
            attemptToChangeYears();
        }

    //	if (Input.GetKeyDown(KeyCode.A) && displayedYear != selectedYear)
    //	{
    //		selectedYear = displayedYear;
    //		updateYearText();
    //		Debug.Log("Traveling to year " + selectedYear);
    //	}
    }
}
