using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// used to manage selected categories and get search results from database interface script
public class CategoryManager : MonoBehaviour {

	private static CategoryManager instance;

	private List<string> selectedCategories;
	private Canvas renderedCanvas;
	private BoxCollider[] buttonColliders;
    private CategoryButton[] categoryButtons;

	private SQLiteTags database;

	void Awake()
	{
		if (instance != null && instance != this)
		{
			Destroy(this);
		}
		else instance = this;
	}

	// Use this for initialization
	void Start () {
		
		selectedCategories = new List<string>();
		renderedCanvas = GetComponent<Canvas>();
		buttonColliders = GetComponentsInChildren<BoxCollider>();
		categoryButtons = GetComponentsInChildren<CategoryButton> ();
		database = GetComponent<SQLiteTags> ();
		GenerateCategoryCounts ();

        StartCoroutine(EndOfStartFrame());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // checks if a given category is selected or not
	public bool CheckIfSelected(string category) {
		return selectedCategories.Contains(category);
	}

    // toggles whether a given category is selected or not and updates search results
	public void ToggleSelected(string category) {
		if (selectedCategories.Contains(category)) {
			selectedCategories.Remove(category);
		} else {
			selectedCategories.Add(category);
		}

		string[] tags = selectedCategories.ToArray();

		List<PlanetData> searchResults = new List<PlanetData>();

        if (tags.Length > 0) // perform logical AND search if there are categories selected
        {
            searchResults = database.Select(tags);
        }
        else // return all planets if no categories selected
        {
            searchResults = database.SelectAllPlanets();
        }

        // update the result display
		ResultDisplay.GetInstance().DisplaySearchResults(searchResults);
	}

    // get the number of selected categories
    public int GetNumSelected()
    {
        return selectedCategories.Count;
    }

    // set whether the panel is visible or not
	public void SetVisible(bool visible)
	{
		renderedCanvas.enabled = visible;

        //disable buttons while panel is invisible
		foreach (BoxCollider col in buttonColliders) {
			col.enabled = visible;
		}
	}

    // get singleton instance
	public static CategoryManager GetInstance() {
		return instance;
	}

    // reset so no categories are selected
	public void ResetAll(){
		for (int i = 0; i < categoryButtons.Length; i++) {
			categoryButtons[i].Deselect();
		}

        selectedCategories.Clear(); //clear selected planets

        //update results display
		ResultDisplay.GetInstance().DisplaySearchResults(database.SelectAllPlanets());
	}

    // get the list of selected categories
	public List<string> GetSelectedCategories(){
		return selectedCategories;
	} 

    // figure out how many planets are in each category
	public void GenerateCategoryCounts(){
        string[] tags = new string[1];

        // go through each button, get its category, find the planets in that category, and tell the button how many planets that is
		for (int i = 0; i < categoryButtons.Length; i++) {
            tags[0] = categoryButtons[i].GetCategory();

			categoryButtons[i].SetCount(database.Select(tags).Count);
		}
	}

    // after all the start functions have been called, search for all planets and display them
    private IEnumerator EndOfStartFrame()
    {
        yield return new WaitForEndOfFrame();
        ResultDisplay.GetInstance().DisplaySearchResults(database.SelectAllPlanets());
    }
}
