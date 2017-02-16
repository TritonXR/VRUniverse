using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using VRTK;

public class Controller : VRTK_InteractableObject {

	public Text panel;

    public GameObject planet1;
	public PlanetData planet1_script;


	// Use this for initialization
	void Start () {
        //Debug.Log("Started");
		planet1_script = planet1.GetComponent<PlanetData> ();

	}

    public override void StartUsing(GameObject currentUsingObject)
    {
        base.StartUsing(currentUsingObject);
        Debug.Log("Name: " + planet1_script.planet_name);
        //panel.text = "Name:" + planet1_script.planet_name + "\n"
        //        + "Year: " + planet1_script.year + "\n"
        //        + "Description: " + planet1_script.description;
        //Debug.Log("Using: text is: " + panel.text);
    }

    public override void StopUsing(GameObject previousUsingObject)
    {
        //Debug.Log("Stop using");
        base.StopUsing(previousUsingObject);
        panel.text = "";
    }
    // Update is called once per frame
    protected override void Update () {
        base.Update();
	}
}
