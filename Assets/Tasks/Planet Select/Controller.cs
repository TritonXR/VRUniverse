using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using VRTK;

public class Controller : VRTK_InteractableObject
{

    public Text Title, Creator, Description, Year, Tag;

    public Image imageDes;

    private PlanetData planet_script;

    public GameObject UsingObject;


    // Use this for initialization
    void Start()
    {
    }

    public override void StartUsing(GameObject currentUsingObject)
    {
        Debug.Log("Planet_seclected");
        base.StartUsing(currentUsingObject);
        UsingObject = currentUsingObject;
        planet_script = gameObject.GetComponent<PlanetData>();

        Debug.Log("start getting info");
        Title.text = planet_script.title;
        Creator.text = planet_script.creator;
        Description.text = planet_script.description;
        Year.text = planet_script.year;
        Tag.text = planet_script.des_tag;

        imageDes.sprite = planet_script.image;

        Debug.Log("completed");


    }

    public override void StopUsing(GameObject previousUsingObject)
    {
        Debug.Log("stop");

        base.StopUsing(previousUsingObject);
        StartUsing(UsingObject);
    }
    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}