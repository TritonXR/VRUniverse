using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * OBSOLETE
 * TO BE REMOVED FROM FINAL PRODUCT
 */

public class SetLeverParticles : MonoBehaviour {

	// Use this for initialization
	void Start () {
        SetLeverActivity();
    }

    public void SetLeverActivity()
    {
        Controller[] scriptController = ListOfPlanetsController.listOfPlanets.GetComponentsInChildren<Controller>();
        for (int i = 0; i < scriptController.Length; i++)
        {
            scriptController[i].leverParticleSystem = gameObject;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
