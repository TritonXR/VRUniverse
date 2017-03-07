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

    private bool hasClickedTrigger;

    //Access to Instructions Menu and Floating Menu
    public GameObject InstructionsMenu;
    public GameObject FloatingMenu;

    //Access to Particle System Lever
    public GameObject leverParticleSystem;

    protected void Start()
    {

        hasClickedTrigger = false;

        rightController = PlanetTravel.camerarig.GetComponentInChildren<SteamVR_TrackedController>();  
        if (rightController == null) {
            //Debug.Log("right controller is null");
            rightController = PlanetTravel.camerarig.GetComponentInChildren<SteamVR_TrackedController>();
        }  

        //Debug.Log("hello");
        panel.enabled = false;
        myText.enabled = false;

        planet_script = gameObject.GetComponent<PlanetData>();
        myText.text = planet_script.title;

        panel.transform.LookAt(Camera.main.transform);
        panel.transform.localEulerAngles = new Vector3(180, 0, 180);
    }

    private void HandleTriggerClicked(object sender, ClickedEventArgs e)
    {
        //Debug.Log("Name of Controller Before Error: " + name);
        planet_script = gameObject.GetComponent<PlanetData>();
       // Debug.Log("after Click");

        Title.text = planet_script.title;
        //Debug.Log("Testing After");

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

        if (hasClickedTrigger)
        {
            //Debug.Log("TRIGGER CLICKED IS TRUE");
            InstructionsMenu.SetActive(false);
            FloatingMenu.SetActive(true);
            leverParticleSystem.SetActive(true);
        } else
        {
            //Debug.Log("TRIGGER CLICKED IS FALSE");
            InstructionsMenu.SetActive(true);
            FloatingMenu.SetActive(false);
            leverParticleSystem.SetActive(false);
        }
    }

    public override void StartUsing(GameObject currentUsingObject)
    {
        base.StartUsing(currentUsingObject);
        if (rightController == null)
        {
            //Debug.Log("right controller is null AGAIN");
            rightController = PlanetTravel.camerarig.GetComponentInChildren<SteamVR_TrackedController>();
        }

        
        if (!hasClickedTrigger)
        {
            //Debug.LogWarning("Setting trigger clicked to " + name);
            rightController.TriggerClicked += HandleTriggerClicked;
            hasClickedTrigger = true;
        }


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

        if (hasClickedTrigger)
        {
            //Debug.LogWarning("Getting rid of trigger clicked to " + name);
            rightController.TriggerClicked -= HandleTriggerClicked;
            hasClickedTrigger = false;
            
        }
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}