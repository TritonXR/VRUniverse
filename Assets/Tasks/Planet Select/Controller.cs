using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using VRTK;

public class Controller : VRTK_InteractableObject
{

    public Text Title, Creator, Description, Year, Tag;

    public Image iamgeDes;

    private PlanetData planet_script;

    public GameObject UsingObject;


    // Use this for initialization
    void Start()
    {
    }

    public override void StartUsing(GameObject currentUsingObject)
    {
        base.StartUsing(currentUsingObject);
        UsingObject = currentUsingObject;
        planet_script = gameObject.GetComponent<PlanetData>();
        Title.text = planet_script.title;
        Creator.text = planet_script.creator;
        Description.text = planet_script.description;
        Year.text = planet_script.year;
        Tag.text = planet_script.des_tag;

    }

    public override void StopUsing(GameObject previousUsingObject)
    {
        base.StopUsing(previousUsingObject);
        StartUsing(UsingObject);
    }
    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}