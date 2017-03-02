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
        //Debug.Log("1name of selected project" + gameObject.name);

        //currentUsingObject.GetComponent<VRTK_SimplePointer>().enableTeleport = false;

        //Debug.Log("setting the : " + currentUsingObject.name + " equal to false");
        base.StartUsing(currentUsingObject);

        //currentUsingObject.GetComponent<VRTK_SimplePointer>().enableTeleport = false;

        UsingObject = currentUsingObject;
        planet_script = gameObject.GetComponent<PlanetData>();

        //Debug.Log("2name of selected project" + gameObject.name);
        Title.text = planet_script.title;
        Creator.text = planet_script.creator;
        Description.text = planet_script.description;
        Year.text = planet_script.year;
        Tag.text = planet_script.des_tag;

        imageDes.sprite = planet_script.image;

        //Debug.Log("3name of selected project" + gameObject.name);


    }

    public override void StopUsing(GameObject previousUsingObject)
    {
        Debug.Log("stop");
        //previousUsingObject.GetComponent<VRTK_SimplePointer>().enableTeleport = false;
        base.StopUsing(previousUsingObject);

        //previousUsingObject.GetComponent<VRTK_SimplePointer>().enableTeleport = false;

        StartUsing(UsingObject);
    }
    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}