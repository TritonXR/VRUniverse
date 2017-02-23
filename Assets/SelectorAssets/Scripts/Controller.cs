using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using VRTK;

public class Controller : VRTK_InteractableObject {

	public Text panel;

	private PlanetData planet_script;

    public GameObject UsingObject;


	// Use this for initialization
	void Start () {
	}

    public override void StartUsing(GameObject currentUsingObject)
    {
        base.StartUsing(currentUsingObject);
        UsingObject = currentUsingObject;
        planet_script = gameObject.GetComponent<PlanetData>();
        panel.text = "Name:" + planet_script.planet_name + "\n"
                + "Year: " + planet_script.year + "\n"
                + "Description: " + planet_script.description;

    }

    public override void StopUsing(GameObject previousUsingObject)
    {
        base.StopUsing(previousUsingObject);
        panel.text = "";
        StartUsing(UsingObject);
    }
    // Update is called once per frame
    protected override void Update () {
        base.Update();
	}
}
