using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using VRTK;

public class Controller : VRTK_InteractableObject {

	public Text panel;

    public GameObject planet1;
	private planet1_des planet1_script;


	// Use this for initialization
	void Start () {
	
		planet1_script = planet1.GetComponent<planet1_des> ();

	}

    public override void StartUsing(GameObject currentUsingObject)
    {
        base.StartUsing(currentUsingObject);
        panel.text = "Name:" + planet1_script.name + "\n"
                + "Year: " + planet1_script.year + "\n"
                + "Description: " + planet1_script.description;
        Debug.Log("Using: text is: " + panel.text);
    }

    public override void StopUsing(GameObject previousUsingObject)
    {
        Debug.Log("Stop using");
        base.StopUsing(previousUsingObject);

    }
    // Update is called once per frame
    protected override void Update () {
        base.Update();

		/*if (Input.GetKeyDown (KeyCode.A)) {
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
*/

	}
}
