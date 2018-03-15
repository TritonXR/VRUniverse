using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultEntry : MonoBehaviour, PointableObject {

    [SerializeField] private Text planetTitleText;
    [SerializeField] private Text planetYearText;
    [SerializeField] private Image planetImage;

    private Image entryBorder;
	private PlanetData data;
	private BoxCollider buttonCollider;

    // Use this for initialization
    void Start () {
        entryBorder = GetComponent<Image>();
		data.title = data.creator = data.description = data.executable = data.year = "";
		data.des_tag = new string[0];
		data.image = null;
		buttonCollider = GetComponent<BoxCollider>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void DisplayPlanet(PlanetData planet)
    {
		data = planet;
        if(planet.title.Equals(""))
        {
            planetYearText.text = planetTitleText.text = "";
            planetImage.enabled = false;
            entryBorder.enabled = false;
			buttonCollider.enabled = false;
        }
        else
        {
            planetTitleText.text = "Name: " + planet.title;
            planetYearText.text = "Year: " + planet.year;
            planetImage.enabled = true;
            planetImage.sprite = planet.image;
            entryBorder.enabled = true;
			buttonCollider.enabled = true;
        }
    }

    public void PointerEnter()
    {
        return;
    }

    public void PointerClick()
    {
		if (!data.title.Equals ("")) {
			DetailedEntry infoPanel = DetailedEntry.GetInstance();
			infoPanel.UpdateInfo(data.title, data.creator, data.description, data.year, data.des_tag, data.image);
            infoPanel.GetTravelButton().SetExeString(data.executable);
			infoPanel.SetVisible (true);
			ResultDisplay.GetInstance().SetVisible(false);
			CategoryManager.GetInstance().SetVisible(false);
		}
    }

    public void PointerExit()
    {
        return;
    }
}
