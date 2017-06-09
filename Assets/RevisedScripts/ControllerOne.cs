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
    //obselete --- private Image RadialBar; //Entire Radial Bar that indicates selection of planet

    //Travel menu where user selects whether they want to travel or not. accessible by travelinteractable by clicking no
    public GameObject Travel_Selection;

    //Reference to the right controller to detect the trigger clicked
    private SteamVR_TrackedController rightController;

    //Reference that detects if the trigger has been clicked before
    private bool canClickOnTrigger;

    private bool triggerDown = false;

    //Previous reference to panel on travel selection menu to deselect highlight
    private Image prevTravelPanel;

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

        //getcomponentinchildren<image>
        // obselete--- RadialBar = rightController.GetComponentInChildren<Image>();
        // obselete--- if (RadialBar == null) Debug.LogWarning("radial bar not found");


        //Also turn off the radial bar by default
        // RadialBar_Menu.SetActive(false);
        /// obselete--- RadialBar.fillAmount = 0;
        /// 

        //Turn off travel menu by default
        Travel_Selection.SetActive(false);
        
        //Gets the data from the planet
        Planet_Data = gameObject.GetComponent<Planet>();

        //Positions the menu to the left of the planet and looking at initial camera

        Vector3 temp = Vector3.Cross(Planet_Data.transform.position, Vector3.up);
        temp.Normalize();
        temp *= 5.2f;
        Vector3 closer = Vector3.Cross(Vector3.up, temp).normalized;
        Planet_Menu.transform.position = new Vector3(temp.x, temp.y, temp.z) + Planet_Data.transform.position - 2*closer;
        Planet_Menu.transform.LookAt(Camera.main.transform); //looks at the camera
        Planet_Menu.transform.Rotate(new Vector3(0, 180, 0)); //but it needs to be flipped around for some reason

        Travel_Selection.transform.position = Planet_Data.transform.position * 0.7f + Vector3.up * 1.25f;
        Travel_Selection.transform.LookAt(Camera.main.transform);
        Travel_Selection.transform.Rotate(new Vector3(0, 180, 0));


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


        //Set executable string for TravelInteracble
        TravelInteractable[] travelPanels = GetComponentsInChildren<TravelInteractable>(true);
        for (int i = 0; i < travelPanels.Length; i++)
        {
            if (travelPanels[i].isYes)
            {
                travelPanels[i].executableString = "/../VRClubUniverse_Data/VR_Demos/" + int.Parse(Planet_Year.text) + "/" + Planet_Executable + "/" + Planet_Executable + ".exe";
            }
        }

    }

    private void SetRightController()
    {
        SteamVR_TrackedController[] controllerSearch = Camera.main.transform.root.GetComponentsInChildren<SteamVR_TrackedController>(true);
        for (int i = 0; i < controllerSearch.Length; i++)
        {
            if (!(controllerSearch[i].GetComponentInChildren<YearSelection>(true)))
            {
                rightController = controllerSearch[i];
            }
        }
    }

    private void HandleTriggerClicked(object sender, ClickedEventArgs e)
    {
        //radialBar.fillAmount += 0.1f;

        if (canClickOnTrigger)
        {
            //obselete--- StartCoroutine(PlanetTravelLoading());  

            Travel_Selection.SetActive(true);
            
        } else
        {

            //obselete--- StopAllCoroutines();
            //Debug.Log("TRIGGER CLICKED IS FALSE");
            //obselete--- RadialBar.fillAmount = 0;
            
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

        //Turn on menu when hovering
        toggleMenu(true);
      
    }

    /*
    public IEnumerator RadialBarLoading()
    {
        triggerDown = rightController.triggerPressed;
        Debug.LogWarning("The controller is" + triggerDown);
        if (triggerDown)
        {
            for (float i = 0; i < 15; i += Time.deltaTime)
            {
                Debug.LogWarning("Radial bar fill increased");
                //obselete--- RadialBar.fillAmount += 0.005f;
                yield return null;
            }
            Debug.Log("Loading Executable: " + Planet_Executable);
            ExecutableSwitch.LoadExe("/../VRClubUniverse_Data/VR_Demos/" + int.Parse(Planet_Year.text) + "/" + Planet_Executable + "/" + Planet_Executable + ".exe");
        }
        //obselete--- else  RadialBar.fillAmount = 0;
    }

    public IEnumerator PlanetTravelLoading()
    {
        yield return StartCoroutine(RadialBarLoading());
        Debug.LogWarning("End Radial Bar");
    }
    */

    public override void StopUsing(GameObject previousUsingObject)
    {

        base.StopUsing(previousUsingObject);

        //Turn off floating menu panel when not using
        toggleMenu(false);


        if (canClickOnTrigger)
        {
            canClickOnTrigger = false;
            rightController.TriggerClicked -= HandleTriggerClicked;
        }
        
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (rightController == null)
        {
            SetRightController();
        }

        Ray myRay = new Ray(rightController.transform.position, rightController.transform.forward);
        Debug.DrawRay(rightController.transform.position, rightController.transform.forward, Color.blue);

        RaycastHit hitObject;

        if (Physics.Raycast(myRay, out hitObject, Mathf.Infinity))
        {
            Collider hitCollider = hitObject.collider;

            if (hitCollider.gameObject.GetComponent<TravelInteractable>())
            {
                prevTravelPanel = hitCollider.gameObject.GetComponent<Image>();
                prevTravelPanel.enabled = true;

                if (rightController.triggerPressed)
                {
                    if (hitCollider.gameObject.GetComponent<TravelInteractable>().isYes)
                    {
                        Debug.Log("loading executable: " + hitCollider.gameObject.GetComponent<TravelInteractable>().executableString);
                        ExecutableSwitch.LoadExe(hitCollider.gameObject.GetComponent<TravelInteractable>().executableString);
                        prevTravelPanel.enabled = false;
                        prevTravelPanel = null;
                        Travel_Selection.SetActive(false);

                    }
                    else
                    {
                        Debug.Log("hiding travel selection");
                        prevTravelPanel.enabled = false;
                        prevTravelPanel = null;
                        Travel_Selection.SetActive(false);
                    }
                }

            } else
            {
                if (prevTravelPanel != null)
                {
                    prevTravelPanel.enabled = false;
                    prevTravelPanel = null;
                }
            }

        } else
        {
            if (prevTravelPanel != null)
            {
                prevTravelPanel.enabled = false;
                prevTravelPanel = null;
            }
        }
    }
}