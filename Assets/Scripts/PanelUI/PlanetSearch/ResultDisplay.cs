using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ResultDisplay : MonoBehaviour {

    private static ResultDisplay instance = null;

    [SerializeField] private ResultEntry[] entry_list;
    [SerializeField] private ResultShift upButton;
    [SerializeField] private ResultShift downButton;
	[SerializeField] private Canvas resultsCanvas;

    private List<PlanetData> planetSearchResults;
    private List<int> planetsWaitingForImages;
    private int topEntryIndex, maxTopEntryIndex;
    private PlanetData dummyPlanet;
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

    public void DisplaySearchResults(List<PlanetData> searchResults)
    {
        planetSearchResults = searchResults;
        topEntryIndex = 0;
        maxTopEntryIndex = planetSearchResults.Count - entry_list.Length;
        UpdateDisplayedEntries();
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
				if (index + topEntryIndex < planetSearchResults.Count) {
					entry_list[index].DisplayPlanet(planetSearchResults[index + topEntryIndex]);
				} else {
					entry_list[index].DisplayPlanet(dummyPlanet);
				}
            }

            upButton.SetButtonEnabled(topEntryIndex > 0);
            downButton.SetButtonEnabled(topEntryIndex < maxTopEntryIndex);
        }
    }

	public void SetVisible(bool visible) {
		resultsCanvas.enabled = visible;
		if (!visible) {
			foreach (BoxCollider col in buttonColliders)
				col.enabled = false;
		} else {
			UpdateDisplayedEntries();
		}
	}

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

    public static ResultDisplay GetInstance()
    {
        return instance;
    }

	private IEnumerator PostStartBehaviors() {
		yield return new WaitForEndOfFrame();
		UpdateDisplayedEntries();
	}
}
