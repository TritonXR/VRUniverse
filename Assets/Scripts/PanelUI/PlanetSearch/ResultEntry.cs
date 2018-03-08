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

    // Use this for initialization
    void Start () {
        entryBorder = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void DisplayPlanet(PlanetData planet)
    {
        if(planet.title.Equals(""))
        {
            planetYearText.text = planetTitleText.text = "";
            planetImage.enabled = false;
            entryBorder.enabled = false;
        }
        else
        {
            planetTitleText.text = "Name: " + planet.title;
            planetYearText.text = "Year: " + planet.year;
            planetImage.enabled = true;
            planetImage.sprite = planet.image;
            entryBorder.enabled = true;
        }
    }

    public void PointerEnter()
    {
        return;
    }

    public void PointerClick()
    {
        return;
    }

    public void PointerExit()
    {
        return;
    }
}
