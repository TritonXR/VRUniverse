using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class TravelInteractable : MonoBehaviour
{

    public bool isYes; //check if searching for yes or no answer

    //constant strings to compare the text of the selection to
    //private string YES = "Yes";
    //private string NO = "No";

    //Image component to show the highlight of the pointed selection
    private Image highlight;

    //Reference to the right controller to detect the trigger clicked
    private SteamVR_TrackedController rightController;

    //Access to the planet executable string
    public string executableString;

    //Access to the object using the button previously
    private GameObject prevUsingObject;

    //Bool to detect if the panel is being pointed at
    private bool isPointing = false;

    //Bool to detect if currently implementing trigger action
    private bool processingPress = false;

    public void TravelMenu(GameObject obj)
    {
        highlight = obj.GetComponent<Image>();
        highlight.enabled = true;

        if (isYes)
        {
            //save the current year in an output text file
            YearSelection yearSelection = Camera.main.transform.root.GetComponentInChildren<YearSelection>(true);

            string path = "VRClubUniverse_Data/saveData.txt";
            string currentYear = yearSelection.SelectedYearIndex.ToString();
            Debug.Log("writing current year to saveData file, year Index: " + currentYear);
            File.WriteAllText(path, currentYear);

            Debug.Log("loading executable: " + executableString);
            ExecutableSwitch.LoadExe(executableString);

        }
        
        //If user selected no or it is after the yes select, then disable the highlights and objects
        highlight.enabled = false;
        highlight = null;
        gameObject.SetActive(false);
        
    }



    /*
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

        //attempt: rightController.TriggerClicked += HandleTriggerClicked_Travel; //add a handle trigger check 
        isPointing = true;

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

        //rightController.TriggerClicked -= HandleTriggerClicked_Travel; //remove a handle trigger check

    }


    private void HandleTriggerClicked_Travel(object sender, ClickedEventArgs e)
    {
        if (isYes)
        {
            Debug.Log("Loading Executable: " + executableString);
            //ExecutableSwitch.LoadExe(executableString);
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
                rightController = controllerSearch[i];
            }
        }
    }

    protected override void Update()
    {
        base.Update();
        if (isPointing && !processingPress)
        {
            if (rightController == null)
            {
                SetRightController();
            }

            
            if (rightController.triggerPressed)
            {
                processingPress = true;
                Debug.Log("Trigger is pressed");
                if (isYes)
                {
                    Debug.Log("Loading Executable: " + executableString);
                    //ExecutableSwitch.LoadExe(executableString);
                    processingPress = false;
                }
                else
                {
                    Debug.Log("no pressed");
                    GetComponentInParent<ControllerOne>().Travel_Selection.SetActive(false);
                    processingPress = false;
                }
            }
            
        }
    }
    */
}
