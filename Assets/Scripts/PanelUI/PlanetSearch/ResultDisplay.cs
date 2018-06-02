using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

// used to manage all of the search results
public class ResultDisplay : MonoBehaviour {

    private static ResultDisplay instance = null;

    [SerializeField] private ResultEntry[] entry_list;
    [SerializeField] private ResultShift upButton;
    [SerializeField] private ResultShift downButton;
	[SerializeField] private Canvas resultsCanvas;

    private List<PlanetData> planetSearchResults;
    private List<int> planetsWaitingForImages;
    private int topEntryIndex, maxTopEntryIndex; //current index of the entry displayed at the top; largest index that can be at the top
    private PlanetData dummyPlanet; // planet data used to signify empty search results
	private BoxCollider[] buttonColliders;

    private bool isLoadingImages;
    private bool restartImageLoading;

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
        planetsWaitingForImages = new List<int>();
        topEntryIndex = 0;

        dummyPlanet.title = dummyPlanet.creator = dummyPlanet.description = dummyPlanet.executable = dummyPlanet.year = "";
        dummyPlanet.des_tag = new string[0];
        dummyPlanet.image = null;

		buttonColliders = GetComponentsInChildren<BoxCollider>();

        isLoadingImages = false;
        restartImageLoading = false;

		StartCoroutine(PostStartBehaviors());
    }

    // Update is called once per frame
    void Update() {

    }

    // displays the passed planet data
    public void DisplaySearchResults(List<PlanetData> searchResults)
    {
        //saves data for future use
        planetSearchResults = searchResults;
        topEntryIndex = 0;
        maxTopEntryIndex = planetSearchResults.Count - entry_list.Length;

        //displays the data
        UpdateDisplayedEntries();

        //starts loading the planet image data
        List<string> imagePaths = new List<string>();
        List<PassSprite> callbacks = new List<PassSprite>();
        planetsWaitingForImages.Clear();
        for(int index = 0; index < planetSearchResults.Count; index++)
        {
            PlanetData planet = planetSearchResults[index];
            planetsWaitingForImages.Add(index);
            imagePaths.Add(ExecutableSwitch.GetFullPath(planet.image_name, planet.executable, planet.year));
            callbacks.Add(ReceiveSprite);
        }
        ImageLoader.GetInstance().LoadImages(imagePaths, callbacks);
    }

    // shifts the displayed results up or down
    public void ShiftDisplayedResults(int shiftUpAmount)
    {
        topEntryIndex += shiftUpAmount;

        //clamps the shift
        if (topEntryIndex > maxTopEntryIndex)
        {
            topEntryIndex = maxTopEntryIndex;
        }
        else if (topEntryIndex < 0)
        {
            topEntryIndex = 0;
        }

        //updates the displayed data
        UpdateDisplayedEntries();
    }

    // passes data to the search result scripts to be displayed
    public void UpdateDisplayedEntries()
    {
        if (planetSearchResults == null) // case: no data has been passed yet
        {
            foreach (ResultEntry entry in entry_list)
            {
                entry.DisplayPlanet(dummyPlanet); // display empty data
            }

            // disable up/down buttons
            upButton.SetButtonEnabled(false);
            downButton.SetButtonEnabled(false);
        }
        else
        {
            for (int index = 0; index < entry_list.Length; index++) {
				if (index + topEntryIndex < planetSearchResults.Count) { // display data where possible
					entry_list[index].DisplayPlanet(planetSearchResults[index + topEntryIndex]);
				} else { // display empty data if not enough actual data
					entry_list[index].DisplayPlanet(dummyPlanet);
				}
            }

            // only allow scrolling up and down so far
            upButton.SetButtonEnabled(topEntryIndex > 0);
            downButton.SetButtonEnabled(topEntryIndex < maxTopEntryIndex);
        }
    }


    // enable or disable the canvas and its buttons
	public void SetVisible(bool visible) {
		resultsCanvas.enabled = visible;
		if (!visible) {
			foreach (BoxCollider col in buttonColliders)
				col.enabled = false;
		} else {
            // update entries on enable
			UpdateDisplayedEntries();
		}
	}

    // receives and distributes planet images when the loader produces them
    public void ReceiveSprite(Sprite image)
    {
        int planetIndex = planetsWaitingForImages[0];
        PlanetData planet = planetSearchResults[planetIndex];
        planet.image = image;
        planetSearchResults[planetIndex] = planet;
        planetsWaitingForImages.RemoveAt(0);

        if(planetIndex >= topEntryIndex && planetIndex < topEntryIndex + entry_list.Length)
        {
            entry_list[planetIndex - topEntryIndex].ReceiveSprite(image);
        }
    }

    // get a reference to the result display singleton
    public static ResultDisplay GetInstance()
    {
        return instance;
    }

    // update the display after all other start function have finished
	private IEnumerator PostStartBehaviors() {
		yield return new WaitForEndOfFrame();
		UpdateDisplayedEntries();
	}
}
