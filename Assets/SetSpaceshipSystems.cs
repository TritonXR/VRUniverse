using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSpaceshipSystems : MonoBehaviour {

    public GameObject InstructionsMenu;
    public GameObject FloatingMenu;
    public GameObject LeverParticles;

	// Use this for initialization
	void Start () {
        Controller[] scriptController = ListOfPlanetsController.listOfPlanets.GetComponentsInChildren<Controller>();
        for (int i = 0; i < scriptController.Length; i++)
        {
            scriptController[i].FloatingMenu = FloatingMenu;
            scriptController[i].InstructionsMenu = InstructionsMenu;
            FloatingMenu.GetComponent<SetCenterMenuInPlanet>().SetForm();
            LeverParticles.GetComponent<SetLeverParticles>().SetLeverActivity();
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
