using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * OBSOLETE
 * TO BE REMOVED FROM FINAL PRODUCT
 */

public class PlanetTravel : MonoBehaviour {

    //public GameObject cameraRig;
    // public GameObject VRTK;

    public static GameObject camerarig = null;

    void Awake()
    {
       // DontDestroyOnLoad(cameraRig);
       // DontDestroyOnLoad(VRTK);
       if (camerarig == null) {
            camerarig = gameObject;
       } else if (camerarig != gameObject) {
            Destroy(gameObject);
       }

        DontDestroyOnLoad(gameObject);

    }

	// Use this for initialization
	void Start () {
		
	}


	
	// Update is called once per frame
	void Update () {

        //PLANET CLUSTER TRAVEL
		if (Input.GetKeyDown(KeyCode.A))
        {
            SceneManager.LoadScene("A Magical Wonderland");
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            SceneManager.LoadScene("Space Flight");
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            SceneManager.LoadScene("Underwater Rings");
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            SceneManager.LoadScene("Nature");
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            SceneManager.LoadScene("Fireland");
        }

        //YEAR CLUSTER TRAVEL
        if (Input.GetKeyDown(KeyCode.I))
        {
            SceneManager.LoadScene("MainUniverse");
        }
        if (Input.GetKeyDown(KeyCode.J))
        {
            SceneManager.LoadScene("2015");
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            SceneManager.LoadScene("2016");
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            SceneManager.LoadScene("2017");
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            SceneManager.LoadScene("2018");
        }
    }
}
