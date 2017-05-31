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

    //Access to the Planet's Executable File to Travel to
    private string Planet_Executable;

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
    private bool canClickOnTrigger;

    private void toggleMenu(bool status)
    {
        Planet_Menu.SetActive(status);
    }

    protected void Start()
    {

        //Set the use of trigger
        canClickOnTrigger = false;

        //Find the right controller
        SetRightController();

        //Turn off floating menu panel by default
        toggleMenu(false);

        //Also turn off the radial bar by default
        RadialBar_Menu.SetActive(false);
        
        //Gets the data from the planet
        Planet_Data = gameObject.GetComponent<Planet>();

        //Positions the menu to the left of the planet and looking at initial camera

        Vector3 temp = Vector3.Cross(Planet_Data.transform.position, Vector3.up);
        temp.Normalize();
        temp *= 5.2f;
        Vector3 closer = Vector3.Cross(Vector3.up, temp).normalized;
        Planet_Menu.transform.position = new Vector3(temp.x, temp.y, temp.z) + Planet_Data.transform.position - 2*closer;
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
        Planet_Executable = Planet_Data.executable;



    }

    private void SetRightController()
    {
        SteamVR_TrackedController[] controllerSearch = Camera.main.transform.root.GetComponentsInChildren<SteamVR_TrackedController>(true);
        for (int i = 0; i < controllerSearch.Length; i++)
        {
            if (!(controllerSearch[i].GetComponentInChildren<YearSelection>(true)))
            {
                Debug.Log("Established right controller input.");
                rightController = controllerSearch[i];
            }
        }
    }

    private void HandleTriggerClicked(object sender, ClickedEventArgs e)
    {
        //radialBar.fillAmount += 0.1f;

        if (canClickOnTrigger)
        {
            Debug.Log("Loading Executable: " + Planet_Executable);
            ExecutableSwitch.LoadExe("/../VRClubUniverse_Data/VR_Demos/" + int.Parse(Planet_Year.text) + "/" + Planet_Executable + "/" + Planet_Executable + ".exe");
            
        } else
        {
            Debug.Log("TRIGGER CLICKED IS FALSE");
            
        }
    }

    public override void StartUsing(GameObject currentUsingObject)
    {
        base.StartUsing(currentUsingObject);

        //Double check that the controller has been set
        if (rightController == null)
        {
            SetRightController(); //if not, set the right controller
        }

        //Check if the trigger isnt already pressed
        if (!canClickOnTrigger)
        {
            canClickOnTrigger = true; //mark it as pressed
            rightController.TriggerClicked += HandleTriggerClicked; //add a handle trigger check 
        }

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

        
        if (canClickOnTrigger)
        {
            //Debug.LogWarning("Getting rid of trigger clicked to " + name);
            canClickOnTrigger = false;
            rightController.TriggerClicked -= HandleTriggerClicked;   
        }
        
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}