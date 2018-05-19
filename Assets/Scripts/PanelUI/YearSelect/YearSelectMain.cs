using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO; // Important for getting files from directory
using System;

public class YearSelectMain : MonoBehaviour {

    private static YearSelectMain instance;

    [SerializeField] private YearSelectGo[] yearButtons;

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
            int adjustedMaxYear = yearButtons[0].YearValue + increment;
            int adjustedMinYear = yearButtons[yearButtons.Length - 1].YearValue + increment;
            if (adjustedMinYear <= maxYear && adjustedMaxYear >= minYear)
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
            yearButtons[index].YearValue = year - index;
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

        SetPrimaryYear(maxYear);
    }

	public int NumberOfPlanets (int year){
		#if UNITY_EDITOR
		string jsonString = File.ReadAllText(Application.dataPath + "/../Website/data/VRClubUniverseData/Vive/" + year.ToString() + ".json");
		#elif UNITY_STANDALONE
		string jsonString = File.ReadAllText(Application.dataPath + "/../VRClubUniverseData/Vive/" + year.ToString() + ".json");
		#endif
		PlanetJSON[] universe = JsonHelper.FromJson<PlanetJSON>(jsonString);

		return universe.Length;
	}
}
