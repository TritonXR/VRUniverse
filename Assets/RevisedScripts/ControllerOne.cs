using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;

public class ControllerOne : MonoBehaviour
{

    //Planet Data Input on Planet Menu
    [SerializeField] private Text Planet_Title, Planet_Creator, Planet_Description, Planet_Year, Planet_Tag;
    [SerializeField] private Image Planet_Image;

    //Access to the Planet's Executable File to Travel to
    private string Planet_Executable;

    //Reference to the Planet Data on each Planet
    private Planet Planet_Data;

    //VRTK Hovering Check
    /* obselete
    private GameObject objectUsing;
    */

    //Menu's controlled by hovering and selecting
    [SerializeField] private GameObject Planet_Menu; //Entire Planet_Menu attached to each planet that shows planet data
    //obselete --- private Image RadialBar; //Entire Radial Bar that indicates selection of planet

    //Travel menu where user selects whether they want to travel or not. accessible by travelinteractable by clicking no
    public GameObject Travel_Selection;

    //Reference to the right controller to detect the trigger clicked
    private SteamVR_TrackedController rightController;

    //Reference that detects if the trigger has been clicked before
    //private bool canClickOnTrigger;

    //Previous reference to panel on travel selection menu to deselect highlight
    //private Image travelPanel;

	private bool tutorial_firstSelection = true;
	private Image[] tutorialsOnRightController;


	private void toggleMenu(bool status)
    {
        Planet_Menu.SetActive(status);
    }

    protected void Start()
    {
        

        //Set the use of trigger
        //OBSELTEcanClickOnTrigger = false;

        //Find the right controller
        SetRightController();

        //Turn off floating menu panel by default
        toggleMenu(false);

        //Turn off travel menu by default
        Travel_Selection.SetActive(false);
        
        //Gets the data from the planet
        Planet_Data = gameObject.GetComponent<Planet>();

        //Positions the menu to the left of the planet and looking at initial camera

        Vector3 temp = Vector3.Cross(Planet_Data.transform.position, Vector3.up);
        temp.Normalize();
		//temp *= 5.2f;
		temp *= 63f;
        Vector3 closer = Vector3.Cross(Vector3.up, temp).normalized;
        Planet_Menu.transform.position = new Vector3(temp.x, temp.y, temp.z) + Planet_Data.transform.position - 30*closer;
        Planet_Menu.transform.LookAt(Camera.main.transform); //looks at the camera
        Planet_Menu.transform.Rotate(new Vector3(0, 180, 0)); //but it needs to be flipped around for some reason

        Travel_Selection.transform.position = Planet_Data.transform.position * 0.7f + Vector3.up * 1.25f;
        Travel_Selection.transform.LookAt(Camera.main.transform);
        Travel_Selection.transform.Rotate(new Vector3(0, 180, 0));

		//Shrink the floating menus by a proportional size
		Vector3 currentScale = Planet_Menu.transform.localScale;
		Planet_Menu.transform.localScale = new Vector3(currentScale.x*0.58f, currentScale.y*0.58f, currentScale.z*0.58f);
		currentScale = Travel_Selection.transform.localScale;
		Travel_Selection.transform.localScale = new Vector3(currentScale.x * 0.58f, currentScale.y * 0.58f, currentScale.z * 0.58f);


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

		// Get Tutorials on right controller
		tutorialsOnRightController = rightController.GetComponentsInChildren<Image>(true);

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
    

    public void SelectPlanet()
    {
        Travel_Selection.SetActive(true);

        if (tutorialsOnRightController[1].gameObject.activeSelf)
        {
            tutorialsOnRightController[1].gameObject.SetActive(false); //turn off label 4
        }


        /*
        if (canClickOnTrigger)
        {

            Travel_Selection.SetActive(true);

			if (tutorialsOnRightController[1].gameObject.activeSelf)
			{
				tutorialsOnRightController[1].gameObject.SetActive(false); //turn off label 4
			}
            
        } */
    }

    //Called when user's laser collides with the planet
    public void StartPointing(GameObject currentUsingObject)
    {

        /*
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
        } */

        //Turn on menu when hovering
        toggleMenu(true);

		if (tutorial_firstSelection)
		{
			tutorialsOnRightController[0].gameObject.SetActive(false); //turn off label 3
			tutorialsOnRightController[1].transform.parent.gameObject.SetActive(true); //turn on label 4
			tutorial_firstSelection = false;
		}


	}

    // Calls when the user points their laser away from the planet
    public void StopPointing(GameObject previousUsingObject)
    {

        //Turn off floating menu panel when not using
        toggleMenu(false);

        /*
        if (canClickOnTrigger)
        {
            canClickOnTrigger = false;
            rightController.TriggerClicked -= SelectPlanet;
        }*/
        
    }

    /*
    public void TravelMenu(GameObject obj)
    {
        travelPanel = obj.GetComponent<Image>();
        //travelPanel.enabled = true;



        if (rightController.triggerPressed)
        {
            if (hitCollider.gameObject.GetComponent<TravelInteractable>().isYes)
            {
                //SAVING CURRENT YEAR
                YearSelection yearSelection = Camera.main.transform.root.GetComponentInChildren<YearSelection>(true);

                string path = "VRClubUniverse_Data/saveData.txt";
                string currentYear = yearSelection.SelectedYearIndex.ToString();
                Debug.Log("writing current year to saveData file, year Index: " + currentYear);
                File.WriteAllText(path, currentYear);

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
    }
    */

    // Update is called once per frame
    protected void Update()
    {
        /*
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
						//SAVING CURRENT YEAR
						YearSelection yearSelection = Camera.main.transform.root.GetComponentInChildren<YearSelection>(true);
						
						string path = "VRClubUniverse_Data/saveData.txt";
                        string currentYear = yearSelection.SelectedYearIndex.ToString();
                        Debug.Log("writing current year to saveData file, year Index: " + currentYear);
                        File.WriteAllText(path, currentYear);

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
        } */
    }
}