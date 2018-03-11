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
                yearString = value.ToString();
                yearText.color = defaultColor;
            }

            yearText.text = yearString;
        }
    }

    [SerializeField] private Text yearText;
    [SerializeField] private Color defaultColor;
    [SerializeField] private Color highlghtColor;
    [SerializeField] private Color disabledColor;

    private int yearValue;
    private int yearIndex;

    private string yearString;

    void Start()
    {
        
    }

    public void PointerEnter()
    {
        if(yearIndex != -1) yearText.color = highlghtColor;
        return;
    }

    public void PointerClick()
    {
		if (yearIndex != -1)
			StartCoroutine (UniverseSystem.GetInstance ().TeleportToYear (yearIndex));
    }

    public void PointerExit()
    {
        if (yearIndex != -1) yearText.color = defaultColor;
        return;
    }
}
