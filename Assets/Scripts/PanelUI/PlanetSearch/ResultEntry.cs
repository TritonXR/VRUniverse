using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultEntry : MonoBehaviour, PointableObject {

    private Text entryText;

	// Use this for initialization
	void Start () {
        entryText = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void DisplayPlanet(PlanetData planet)
    {
        if(planet.title.Equals(""))
        {
            entryText.text = "";
        }
        else
        {
            entryText.text = planet.title + " (" + planet.year + ")";
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
