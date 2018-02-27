using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YearSelectMain : MonoBehaviour {

    private static YearSelectMain instance;

    private List<YearSelectGo> yearButtons;

	// Use this for initialization
	void Start () {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else instance = this;

        yearButtons = new List<YearSelectGo>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void IncrementYear(int increment)
    {

    }

    public static YearSelectMain GetInstance()
    {
        return instance;
    }
}
