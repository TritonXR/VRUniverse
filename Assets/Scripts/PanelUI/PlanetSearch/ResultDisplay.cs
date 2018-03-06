using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultDisplay : MonoBehaviour {

    private static ResultDisplay instance = null;

    [SerializeField] private ResultEntry[] entry_list;
    [SerializeField] private ResultShift upButton;
    [SerializeField] private ResultShift downButton;

    private List<PlanetData> planetSearchResults;
    private int topEntryIndex, maxTopEntryIndex;
    private PlanetData dummyPlanet;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else instance = this;
    }

    // Use this for initialization
    void Start() {
        planetSearchResults = null;
        topEntryIndex = 0;

        dummyPlanet.title = dummyPlanet.creator = dummyPlanet.description = dummyPlanet.executable = dummyPlanet.year = "";
        dummyPlanet.des_tag = new string[0];
        dummyPlanet.image = null;

        UpdateDisplayedEntries();
    }

    // Update is called once per frame
    void Update() {

    }

    public void DisplaySearchResults(List<PlanetData> searchResults)
    {
        planetSearchResults = searchResults;
        topEntryIndex = 0;
        maxTopEntryIndex = planetSearchResults.Count - entry_list.Length;
        UpdateDisplayedEntries();
    }

    public void ShiftDisplayedResults(int shiftUpAmount)
    {
        topEntryIndex += shiftUpAmount;
        if (topEntryIndex > maxTopEntryIndex)
        {
            topEntryIndex = maxTopEntryIndex;
        }
        else if (topEntryIndex < 0)
        {
            topEntryIndex = 0;
        }

        UpdateDisplayedEntries();
    }

    public void UpdateDisplayedEntries()
    {
        if (planetSearchResults == null)
        {
            foreach (ResultEntry entry in entry_list)
            {
                entry.DisplayPlanet(dummyPlanet);
            }

            upButton.SetButtonEnabled(false);
            downButton.SetButtonEnabled(false);
        }
        else
        {
            for (int index = 0; index < entry_list.Length; index++) {
                entry_list[index].DisplayPlanet(planetSearchResults[index + topEntryIndex]);
            }

            upButton.SetButtonEnabled(topEntryIndex == 0);
            downButton.SetButtonEnabled(topEntryIndex == maxTopEntryIndex);
        }
    }

    public static ResultDisplay GetInstance()
    {
        return instance;
    }
}
