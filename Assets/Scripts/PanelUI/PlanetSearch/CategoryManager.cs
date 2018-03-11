using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CategoryManager : MonoBehaviour {

	private static CategoryManager instance;

	private List<string> selectedCategories;
	private Canvas renderedCanvas;
	private BoxCollider[] buttonColliders;

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

		//TODO: pass tags to database, get planet data
		string[] tags = selectedCategories.ToArray();

		List<PlanetData> searchResults = new List<PlanetData>();

		//TODO: remove these lines when you have actual results
		PlanetData testData;
		testData.creator = testData.description = testData.executable = testData.title = "Testing 123";
		testData.year = "2018";
		testData.des_tag = new string[2] {"Test1", "Test2"};
		testData.image = null;
		for (int i = 0; i < 10; i++) {
			testData.title = "Test " + i;
			searchResults.Add(testData);
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
}
