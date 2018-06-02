using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// used to manage individual search result entries
public class ResultEntry : MonoBehaviour, PointableObject {

    [SerializeField] private Text planetTitleText;
    [SerializeField] private Text planetYearText;
    [SerializeField] private Image planetImage;

    private Image entryBorder;
	private PlanetData data;
	private BoxCollider buttonCollider;

    private PassSprite detailedEntryCallback; // this is callback used to pass on images loaded by the image loader

    // Use this for initialization
    void Start () {
        entryBorder = GetComponent<Image>();
		data.title = data.creator = data.description = data.executable = data.year = "";
		data.des_tag = new string[0];
		data.image = null;
		buttonCollider = GetComponent<BoxCollider>();
        detailedEntryCallback = null;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // saves off and displays planet data
    public void DisplayPlanet(PlanetData planet)
    {
		data = planet; //saves off all the data
        detailedEntryCallback = null; // haven't switch to the detailed entry, so don't pass on sprite
        if(planet.title.Equals("")) // null planet case, display nothing
        {
            planetYearText.text = planetTitleText.text = "";
            planetImage.enabled = false;
            entryBorder.enabled = false;
			buttonCollider.enabled = false;
        }
        else // normal case, display relevant info
        {
            planetTitleText.text = "Name: " + planet.title;
            planetYearText.text = "Year: " + planet.year;
            if(planet.image != null)
            {
                planetImage.enabled = true;
                planetImage.sprite = planet.image;
            }
            else
            {
                planetImage.enabled = false;
            }
            
            entryBorder.enabled = true;
			buttonCollider.enabled = true;
        }
    }

    // called when user starts pointing at button
    public void PointerEnter()
    {
        return;
    }

    // called when user pulls trigger while pointing at this button
    public void PointerClick()
    {
        // switch to detailed entry view
		if (!data.title.Equals ("")) {
            // get detailed entry canvas, pass it data, enable it
			DetailedEntry infoPanel = DetailedEntry.GetInstance();
			infoPanel.UpdateInfo(data.title, data.creator, data.description, data.year, data.des_tag, data.image);
            infoPanel.GetTravelButton().SetExeString(ExecutableSwitch.GetFullPath(data.executable + ".exe", data.executable, data.year));
			infoPanel.SetVisible (true);

            // disable search results and category panel
			ResultDisplay.GetInstance().SetVisible(false);
			CategoryManager.GetInstance().SetVisible(false);

            detailedEntryCallback = infoPanel.ReceiveSprite; // pass on sprite if a sprite is received
		}
    }

    // called when user stops pointing at button
    public void PointerExit()
    {
        return;
    }

    // callback function used by image loader
    public void ReceiveSprite(Sprite image)
    {
        data.image = image;
        planetImage.sprite = image;
        planetImage.enabled = true;

        // this used to pass on the image to the detailed entry
        if (detailedEntryCallback != null) detailedEntryCallback(image);
    }
}
