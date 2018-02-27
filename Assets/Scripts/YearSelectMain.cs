using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YearSelectMain : MonoBehaviour {

    private static YearSelectMain instance;

    private List<YearSelectGo> yearButtons;

	// Use this for initialization
	void Awake () {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else instance = this;

        yearButtons = new List<YearSelectGo>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void IncrementYear(int increment)
    {
        foreach(YearSelectGo year in yearButtons)
        {
            year.YearValue = year.YearValue + increment;
        }
    }

    public void RegisterYearButton(YearSelectGo year)
    {
        if(!yearButtons.Contains(year)) yearButtons.Add(year);
    }

    public void DeregisterYearButton(YearSelectGo year)
    {
        if (yearButtons.Contains(year)) yearButtons.Remove(year);
    }

    public static YearSelectMain GetInstance()
    {
        return instance;
    }
}
