using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class YearTravel : MonoBehaviour {

    public GameObject cameraRig;
    public GameObject Spaceship;
    public GameObject VRTK;
     
    void Awake()
    {
        //DontDestroyOnLoad(cameraRig);
        //DontDestroyOnLoad(Spaceship);
        //DontDestroyOnLoad(VRTK);
    }

	// Use this for initialization
	void Start () {
		
	}


	
	// Update is called once per frame
	void Update () {
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
