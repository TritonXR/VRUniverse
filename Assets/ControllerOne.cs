using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using VRTK;
using UnityEngine.SceneManagement;

public class ControllerOne : VRTK_InteractableObject
{

    //Planet Data Input on Planet Menu
    [SerializeField] private Text Planet_Title, Planet_Creator, Planet_Description, Planet_Year, Planet_Tag;
    [SerializeField] private Image Planet_Image;

    //Reference to the Planet Data on each Planet
    private Planet Planet_Data;

    //VRTK Hovering Check
    private GameObject objectUsing;

    //Menu's controlled by hovering and selecting
    [SerializeField] private GameObject Planet_Menu; //Entire Planet_Menu attached to each planet that shows planet data
    [SerializeField] private GameObject RadialBar_Menu; //Entire Radial Bar that indicates selection of planet

    //Reference to the right controller to detect the trigger clicked
    private SteamVR_TrackedController rightController;

    //Reference that detects if the trigger has been clicked before
    private bool hasClickedTrigger;

    private void toggleMenu(bool status)
    {
        Planet_Menu.SetActive(status);
    }

    protected void Start()
    {

        hasClickedTrigger = false;

        //rightController = Camera.main.transform.root.GetComponentInChildren<SteamVR_TrackedController>();

        //Turn off floating menu panel by default
        toggleMenu(false);

        //Also turn off the radial bar by default
        RadialBar_Menu.SetActive(false);
        
        //Gets the data from the planet
        Planet_Data = gameObject.GetComponent<Planet>();

        //Positions the menu to the left of the planet and looking at initial camera
        Planet_Menu.transform.position = new Vector3(Planet_Data.transform.position.x + 10, Planet_Data.transform.position.y, Planet_Data.transform.position.z);
        //Planet_Menu.transform.position = Planet_Data.transform.position + Planet_Data.transform.TransformDirection(new Vector3(15, 0, 0));
        Planet_Menu.transform.LookAt(Camera.main.transform); //looks at the camera
        Planet_Menu.transform.Rotate(new Vector3(0, 180, 0)); //but it needs to be flipped around for some reason

        //Set text to planet info
        Planet_Title.text = Planet_Data.title;        
        Planet_Creator.text = Planet_Data.creator;
        Planet_Description.text = Planet_Data.description;
        Planet_Year.text = Planet_Data.year;
        string tagText = "";
        for (int i = 0; i < Planet_Data.des_tag.Length; i++)
        {
            if (i == Planet_Data.des_tag.Length - 1)
            {
                tagText = tagText + Planet_Data.des_tag[i];
            }
            else
            {
                tagText = tagText + Planet_Data.des_tag[i] + ", ";
            }

        }
        Planet_Tag.text = tagText;
        Planet_Image.sprite = Planet_Data.image;



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

        //if (rightController == null)
        //{
        //Debug.Log("right controller is null AGAIN");
        //    rightController = PlanetTravel.camerarig.GetComponentInChildren<SteamVR_TrackedController>();
        //}


        //if (!hasClickedTrigger)
        //{
        //Debug.LogWarning("Setting trigger clicked to " + name);
        //    hasClickedTrigger = true;
        //    rightController.TriggerClicked += HandleTriggerClicked;

        //}

        //Saves the object that is currently being hovered on
        objectUsing = currentUsingObject;

        //Turn on menu when hovering
        toggleMenu(true);

    }

    public override void StopUsing(GameObject previousUsingObject)
    {
        base.StopUsing(previousUsingObject);

        StartUsing(objectUsing);

        //Turn off floating menu panel when not using
        toggleMenu(false);

        //Reset circular status
        //radialBar.fillAmount = 0;

        /*
        if (hasClickedTrigger)
        {
            //Debug.LogWarning("Getting rid of trigger clicked to " + name);
            hasClickedTrigger = false;
            rightController.TriggerClicked -= HandleTriggerClicked;   
        }
        */
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}