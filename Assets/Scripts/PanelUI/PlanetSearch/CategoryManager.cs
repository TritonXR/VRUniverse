using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        StartCoroutine(EndOfStartFrame());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public bool CheckIfSelected(string category) {
		return selectedCategories.Contains(category);
	}

	public void ToggleSelected(string category) {
		if (selectedCategories.Contains(category)) {
			selectedCategories.Remove(category);
		} else {
			selectedCategories.Add(category);
		}

		string[] tags = selectedCategories.ToArray();

		List<PlanetData> searchResults = new List<PlanetData>();

        if (tags.Length > 0)
        {
            searchResults = database.Select(tags);
        }
        else
        {
            searchResults = database.SelectAllPlanets();
        }

		ResultDisplay.GetInstance().DisplaySearchResults(searchResults);
	}

	public void SetVisible(bool visible)
	{
		renderedCanvas.enabled = visible;
		foreach (BoxCollider col in buttonColliders) {
			col.enabled = visible;
		}
	}

	public static CategoryManager GetInstance() {
		return instance;
	}

	public void ResetAll(){
		for (int i = 0; i < categoryButtons.Length; i++) {
			categoryButtons[i].Deselect();
		}
        selectedCategories.Clear();
		ResultDisplay.GetInstance().DisplaySearchResults(database.SelectAllPlanets());
	}

	public List<string> getSelectedCategories(){
		return this.selectedCategories;
	}

    private IEnumerator EndOfStartFrame()
    {
        yield return new WaitForEndOfFrame();
        ResultDisplay.GetInstance().DisplaySearchResults(database.SelectAllPlanets());
    }
}
