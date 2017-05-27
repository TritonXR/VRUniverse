using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporarySceneChanger : MonoBehaviour {

    private ExecutableSwitch switchLoader;

	// Use this for initialization
	void Start () {
        switchLoader = GetComponentInChildren<ExecutableSwitch>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.A))
        {
            switchLoader.LoadExecutable("datapath to Crash");
        }
        else if (Input.GetKeyDown(KeyCode.B))
        {
            switchLoader.LoadExecutable("datapath to waterworks");
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            switchLoader.LoadExecutable("data path to island adventure");
        }
	}
}
