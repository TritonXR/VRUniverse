using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Controller : MonoBehaviour {

	public Text panel;

	public GameObject planet1, planet2;
	private planet1_des planet1_script;
	private planet2_des planet2_script;


	// Use this for initialization
	void Start () {
	
		planet1_script = planet1.GetComponent<planet1_des> ();
		planet2_script = planet2.GetComponent<planet2_des> ();

	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.A)) {
			panel.text = "Name:" + planet1_script.name+ "\n" 
				+ "Year: " + planet1_script.year + "\n" 
				+ "Description: " + planet1_script.description;

		}

		if (Input.GetKeyUp (KeyCode.A)) {
			panel.text = "";
		}

		if (Input.GetKeyDown (KeyCode.B)) {
			panel.text = "Name:" + planet2_script.name+ "\n" 
				+ "Year: " + planet2_script.year + "\n" 
				+ "Description: " + planet2_script.description;

		}

		if (Input.GetKeyUp (KeyCode.B)) {
			panel.text = "";
		}


	}
}
