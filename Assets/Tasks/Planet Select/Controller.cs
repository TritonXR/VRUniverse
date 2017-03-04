using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using VRTK;

public class Controller : VRTK_InteractableObject
{

    //Planet Select Assets
    public Text Title, Creator, Description, Year, Tag;
    public Image imageDes;
    private PlanetData planet_script;
    public GameObject UsingObject;

    //PointerPreview Assets
    public Text myText;
    public Image panel;
    public SteamVR_TrackedController rightController;

    protected void Start()
    {
        //Debug.Log("hello");
        panel.enabled = false;
        myText.enabled = false;

        planet_script = gameObject.GetComponent<PlanetData>();
        myText.text = planet_script.title;

        panel.transform.LookAt(Camera.main.transform);
        //panel.transform.Rotate(Vector3.up - Vector3(0, 180, 0));
        panel.transform.localEulerAngles = new Vector3(180, 0, 180);
    }

    private void HandleTriggerClicked(object sender, ClickedEventArgs e)
    {

        planet_script = gameObject.GetComponent<PlanetData>();

        Title.text = planet_script.title;
        Creator.text = planet_script.creator;
        Description.text = planet_script.description;
        Year.text = planet_script.year;

        string tagText = "";
        for (int i = 0; i < planet_script.des_tag.Length; i++)
        {
            if (i == planet_script.des_tag.Length - 1)
            {
                tagText = tagText + planet_script.des_tag[i];
            }
            else
            {
                tagText = tagText + planet_script.des_tag[i] + ", ";
            }

        }
        Tag.text = tagText;

        imageDes.sprite = planet_script.image;
    }

    public override void StartUsing(GameObject currentUsingObject)
    {
        base.StartUsing(currentUsingObject);

        rightController.TriggerClicked += HandleTriggerClicked;


        /*
        UsingObject = currentUsingObject;
        planet_script = gameObject.GetComponent<PlanetData>();

        Title.text = planet_script.title;
        Creator.text = planet_script.creator;
        Description.text = planet_script.description;
        Year.text = planet_script.year;

        string tagText = "";
        for (int i = 0; i < planet_script.des_tag.Length; i++)
        {
            if (i == planet_script.des_tag.Length - 1) {
                tagText = tagText + planet_script.des_tag[i];
            } else
            {
                tagText = tagText + planet_script.des_tag[i] + ", ";
            }
                
        }
        Tag.text = tagText;

        imageDes.sprite = planet_script.image;
        */

        planet_script = gameObject.GetComponent<PlanetData>();
        myText.text = planet_script.title;

        panel.enabled = true;
        myText.enabled = true;





    }

    public override void StopUsing(GameObject previousUsingObject)
    {
        base.StopUsing(previousUsingObject);

        StartUsing(UsingObject);

        panel.enabled = false;
        myText.enabled = false;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}