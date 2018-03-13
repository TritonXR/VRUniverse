using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YearSelectMain : MonoBehaviour {

    private static YearSelectMain instance;

    [SerializeField] private YearSelectGo[] yearButtons;
    [SerializeField] private int primaryYearButtonIndex;

    private int minYear, maxYear;

	// Use this for initialization
	void Awake () {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else instance = this;
	}

    void Start()
    {
        StartCoroutine(EndOfStartFrame());
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void IncrementYear(int increment)
    {
        UniverseSystem.GetInstance().GetYearRange(out minYear, out maxYear);
        if (yearButtons.Length >= 1)
        {
            int adjustedPrimaryYear = yearButtons[primaryYearButtonIndex].YearValue + increment;
            if (adjustedPrimaryYear <= maxYear && adjustedPrimaryYear >= minYear)
            {
                foreach (YearSelectGo year in yearButtons)
                {
                    year.YearValue = year.YearValue + increment;
                }
            }
        }
    }

    public void SetPrimaryYear(int year)
    {
        for (int index = 0; index < yearButtons.Length; index++)
        {
            yearButtons[index].YearValue = year - primaryYearButtonIndex + index;
        }
    }

    public static YearSelectMain GetInstance()
    {
        return instance;
    }

    IEnumerator EndOfStartFrame()
    {
        yield return new WaitForEndOfFrame();
        UniverseSystem.GetInstance().GetYearRange(out minYear, out maxYear);

        for(int index = 0; index < yearButtons.Length; index++)
        {
            yearButtons[index].YearValue = maxYear - primaryYearButtonIndex + index;
        }
    }
}
