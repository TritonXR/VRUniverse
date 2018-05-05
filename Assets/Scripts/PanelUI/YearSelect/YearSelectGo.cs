using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class YearSelectGo : MonoBehaviour, PointableObject
{

    public int YearValue
    {
        get
        {
            return yearValue;
        }

        set
        {
            yearValue = value;
            yearIndex = UniverseSystem.GetInstance().GetYearIndex(yearValue);

            if (yearIndex == -1)
            {
                yearString = "--";
                yearText.color = disabledColor;
            }
            else
            {
                yearString = value.ToString() + " (" + YearSelectMain.GetInstance().NumberOfPlanets(value) + ")";
                yearText.color = IsCurrentYear() ? currentYearColor : defaultColor;
            }

			yearText.text = yearString;
        }
    }

    [SerializeField] private Text yearText;
    [SerializeField] private Color defaultColor;
    [SerializeField] private Color highlightColor;
    [SerializeField] private Color disabledColor;
    [SerializeField] private Color currentYearColor;

    private int yearValue;
    private int yearIndex;

    private string yearString;

    void Start()

    {
    }

    public void PointerEnter()
    {
        if (yearIndex != -1)
        {
            yearText.color = highlightColor;
        }
    }

    public void PointerClick()
    {
        if (yearIndex != -1 && !UniverseSystem.GetInstance().IsCurrentlyTraveling())
        {
            StartCoroutine(UniverseSystem.GetInstance().TeleportToYear(yearIndex));
        }
    }

    public void PointerExit()
    {
        if (yearIndex != -1)
        {
            yearText.color = IsCurrentYear() ? currentYearColor : defaultColor;
        }
        return;
    }

    private bool IsCurrentYear()
    {
        int result = -1;
        if (Int32.TryParse(UniverseSystem.GetInstance().GetCurrentYear(), out result))
        {
            return result == yearValue;
        }
        else return false;
    }
}
