using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using VRTK;
using UnityEngine.SceneManagement;

public class Controller : VRTK_InteractableObject
{


    //Planet Select Assets
    public Text Title, Creator, CreatorTitle, Description, Year, YearTitle, Tag, TagTitle;
    public Image imageDes;
    private Planet planet_script;
    public GameObject UsingObject;
    public Image floatingPanel;
    public Image radialBar;

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

    private void toggleMenu(bool status)
    {
        Title.enabled = status;
        Creator.enabled = status;
        Description.enabled = status;
        Year.enabled = status;
        Tag.enabled = status;
        floatingPanel.enabled = status;
        imageDes.enabled = status;
        CreatorTitle.enabled = status;
        YearTitle.enabled = status;
        TagTitle.enabled = status;
    }

    protected void Start()
    {

        hasClickedTrigger = false;

        //rightController = PlanetTravel.camerarig.GetComponentInChildren<SteamVR_TrackedController>();
        rightController = Camera.main.transform.root.GetComponentInChildren<SteamVR_TrackedController>();

        //Debug.Log("hello");
        panel.enabled = false;
        myText.enabled = false;

        //Turn off floating menu panel by default
        toggleMenu(false);


        planet_script = gameObject.GetComponent<Planet>();
        myText.text = planet_script.title;        

        panel.transform.LookAt(Camera.main.transform);
        panel.transform.localEulerAngles = new Vector3(180, 0, 180);

        //Planet transforms (not needed?)
        planet_script.transform.LookAt(Camera.main.transform);
        planet_script.transform.localEulerAngles = new Vector3(180, 0, 180);

        //Positions the menu to the left of the planet and looking at initial camera
        FloatingMenu.transform.position = planet_script.transform.position + planet_script.transform.TransformDirection(new Vector3(15, 0, 0));
        FloatingMenu.transform.LookAt(Camera.main.transform);
        FloatingMenu.transform.Rotate(new Vector3(0, 180, 0));
        //FloatingMenu.transform.localEulerAngles = new Vector3(180, 0, 180);

        planet_script = gameObject.GetComponent<Planet>();

        //Set text to planet info
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

    private void HandleTriggerClicked(object sender, ClickedEventArgs e)
    {
        //radialBar.fillAmount += 0.1f;

        /*if (hasClickedTrigger)
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
        }*/
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
            hasClickedTrigger = true;
            rightController.TriggerClicked += HandleTriggerClicked;
            
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

        planet_script = gameObject.GetComponent<Planet>();
        myText.text = planet_script.title;

        panel.enabled = true;
        myText.enabled = true;

        //Turn on menu when hovering
        toggleMenu(true);       


    }

    public override void StopUsing(GameObject previousUsingObject)
    {
        base.StopUsing(previousUsingObject);

        StartUsing(UsingObject);

        panel.enabled = false;
        myText.enabled = false;

        //Turn off floating menu panel when not using
        toggleMenu(false);

        //Reset circular status
        //radialBar.fillAmount = 0;

        if (hasClickedTrigger)
        {
            //Debug.LogWarning("Getting rid of trigger clicked to " + name);
            hasClickedTrigger = false;
            rightController.TriggerClicked -= HandleTriggerClicked;
            
            
        }
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}