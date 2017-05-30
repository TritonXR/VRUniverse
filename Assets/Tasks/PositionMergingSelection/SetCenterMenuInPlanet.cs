using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetCenterMenuInPlanet : MonoBehaviour {

    public Text title, creatorInput, description, yearInput, tagsInput;
    public Image image;

	// Use this for initialization
	void Start () {
        SetForm();
	}
	
    public void SetForm()
    {
        Controller[] scriptController = ListOfPlanetsController.listOfPlanets.GetComponentsInChildren<Controller>();
        for (int i = 0; i < scriptController.Length; i++)
        {
            scriptController[i].Title = title;
            scriptController[i].Creator = creatorInput;
            scriptController[i].Description = description;
            scriptController[i].Year = yearInput;
            scriptController[i].Tag = tagsInput;
            scriptController[i].imageDes = image;
        }
    }

	// Update is called once per frame
	void Update () {
		
	}
}
