using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using UnityEngine.UI;

public class TravelInteractable : VRTK_InteractableObject
{
    [SerializeField]
    private bool isYes; //check if searching for yes or no answer

    //constant strings to compare the text of the selection to
    private string YES = "Yes";
    private string NO = "No";

    //Image component to show the highlight of the pointed selection
    private Image highlight;

    //Reference to the right controller to detect the trigger clicked
    private SteamVR_TrackedController rightController;

    //Access to the planet executable string
    public string executableString;

    //Access to the object using the button previously
    private GameObject prevUsingObject;

    protected void Start()
    {
        highlight = GetComponent<Image>();

        SetRightController();
    }

    public override void StartUsing(GameObject currentUsingObject)
    {
        base.StartUsing(currentUsingObject);

        //activate the highlight
        highlight.enabled = true;

        //Double check that the controller has been set
        if (rightController == null)
        {
            SetRightController(); //if not, set the right controller
        }

        rightController.TriggerClicked += HandleTriggerClicked; //add a handle trigger check 

        //get current using object to be prev using object
        prevUsingObject = currentUsingObject;
       
    }

    public override void StopUsing(GameObject previousUsingObject)
    {
        base.StopUsing(previousUsingObject);

        //deactivate the highlight
        highlight.enabled = false;

        //Double check that the controller has been set
        if (rightController == null)
        {
            SetRightController(); //if not, set the right controller
        }

        rightController.TriggerClicked -= HandleTriggerClicked; //remove a handle trigger check

    }


    private void HandleTriggerClicked(object sender, ClickedEventArgs e)
    {
        if (isYes)
        {
            Debug.Log("Loading Executable: " + executableString);
            ExecutableSwitch.LoadExe(executableString);
        } else
        {
            StopUsing(prevUsingObject);
            GetComponentInParent<ControllerOne>().Travel_Selection.SetActive(false);
        }
    }

    //Helper for finding right controller
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
}
