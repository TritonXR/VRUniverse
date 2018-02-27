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
            yearString = value.ToString();
            yearText.text = yearString;
        }
    }

    [SerializeField] private int yearValue;
    [SerializeField] private Text yearText;

    private string yearString;

    void Start()
    {
        YearSelectMain.GetInstance().RegisterYearButton(this);
    }

    public void PointerStart()
    {
        return;
    }

    public void PointerClick()
    {
        UniverseSystem universe = UniverseSystem.GetInstance();
        universe.TeleportToYear(universe.GetYearIndex(YearValue));
    }

    public void PointerStop()
    {
        return;
    }
}
