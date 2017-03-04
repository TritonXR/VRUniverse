using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListOfPlanetsController : MonoBehaviour {

    public static GameObject listOfPlanets;

    private void Awake()
    {
        if (listOfPlanets == null) {
            listOfPlanets = gameObject;
        } else if (listOfPlanets != gameObject) {
            Destroy(gameObject);
        }
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
