using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DetailedEntry : MonoBehaviour {

	private static DetailedEntry instance = null;

	[SerializeField] private Text Planet_Title, Planet_Creator, Planet_Description, Planet_Year, Planet_Tag;
	[SerializeField] private Image Planet_Image;
	[SerializeField] private TravelInteractable travelButton;

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
		buttonColliders = GetComponentsInChildren<BoxCollider>();
		renderedCanvas = GetComponent<Canvas>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void UpdateInfo(string title, string creator, string desc, string year, string[] tags, Sprite image)
	{
		//Sets the text for the different data components on the menu
		Planet_Title.text = title;
		Planet_Creator.text = creator;
		Planet_Description.text = desc;
		Planet_Year.text = year;

		//Must be handled differently because tags are stored as an array and we must concatenate them
		string tagText = "";
		for (int i = 0; i < tags.Length; i++)
		{
			if (i == tags.Length - 1)
			{
				tagText = tagText + tags[i];
			}
			else
			{
				tagText = tagText + tags[i] + ", ";
			}

		}

		Planet_Tag.text = tagText;
		Planet_Image.sprite = image; //Uses the image component to set the sprite of what the picture should be
	}

	public void SetVisible(bool visible)
	{
		renderedCanvas.enabled = visible;
		foreach (BoxCollider col in buttonColliders) {
			col.enabled = visible;
		}
	}

	public TravelInteractable GetTravelButton() {
		return travelButton;
	}

	public static DetailedEntry GetInstance()
	{
		return instance;
	}
}
