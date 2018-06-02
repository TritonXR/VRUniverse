using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
 * Name: SearchPanelsControl.cs
 * Description: Contains methods called when hit the toggle controlling turn on/off SearchResult Canvas and Category Canvas
 * Utilized on: SpaceshipTest.prefab
 */

public class SearchPanelsControl : MonoBehaviour {

    private Canvas[] newpanels;
    private bool turnup;
    private static SearchPanelsControl instance;
    [SerializeField] private Text displaytext;
    
    // Use this for initialization
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else instance = this;
    }

    // Initiate the Panels to be off
    void Start () {
        newpanels = UniverseSystem.GetInstance().panels;
        turnup = false;
        clearPanels();    
        displaytext.text = "Display: Off";
    }

    // Update is called once per frame
    void Update() {
        SetLabel(turnup);
    }

    // Display the CategoryCanvas and SearchResultCanvas
    public void displayPanels()
    {
        turnup = true;
        //SearchResultsCanvas -->  GetComponentsInChildren<ResultDisplay>
        //CategoiresCanvas -->  GetComponentsInChildren<CategoryManager>
        for (int index = 0; index < newpanels.Length; index++)
        {
            if (newpanels[index].GetComponent<CategoryManager>() != null) 
            {
                newpanels[index].enabled = true;
                BoxCollider[] SearchResultArray = newpanels[index].GetComponentsInChildren<BoxCollider>();
                for (int i = 0; i < SearchResultArray.Length; i++)
                {
                    SearchResultArray[i].enabled = true;
                }

            }
            else if (newpanels[index].GetComponentInChildren<ResultDisplay>() != null)
            {
                newpanels[index].enabled = true;
                BoxCollider[] CategoriesArray = newpanels[index].GetComponentsInChildren<BoxCollider>();
                for (int i = 0; i < CategoriesArray.Length; i++)
                {
                    CategoriesArray[i].enabled = true;
                }
            }
            else
            {
                continue;
            }


        }
    }

    // Turn off the CategoryCanvas and SearchResultCanvas
    public void clearPanels()
    {
        turnup = false;

        //SearchResultsCanvas -->  GetComponentsInChildren<ResultDisplay>
        //CategoiresCanvas -->  GetComponentsInChildren<CategoryManager>
        for (int index = 0; index < newpanels.Length; index++)
        {
            if (newpanels[index].GetComponent<CategoryManager>() != null) 
            {
                newpanels[index].enabled = false;
                BoxCollider[] SearchResultArray = newpanels[index].GetComponentsInChildren<BoxCollider>();
                for (int i = 0; i < SearchResultArray.Length; i++)
                {
                    SearchResultArray[i].enabled = false;
                }

            }
            else if (newpanels[index].GetComponentInChildren<ResultDisplay>() != null)
            {
                newpanels[index].enabled = false;
                BoxCollider[] CategoriesArray = newpanels[index].GetComponentsInChildren<BoxCollider>();
                for (int i = 0; i < CategoriesArray.Length; i++)
                {
                    CategoriesArray[i].enabled = false;
                }
            }
            else
            {
                continue;
            }
        }
    }

    //attach the button into action
    public void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("GameController"))
        {
            if (turnup == false)
            {
                displayPanels();
            }
            else
            {
                clearPanels();
            }
            SteamVR_Controller.Input((int)other.GetComponent<SteamVR_TrackedController>().controllerIndex).TriggerHapticPulse(1000);

        }
    }

    //set the label of toggle
    private void SetLabel(bool turnup)
    {
        if (turnup == false)
        {
            displaytext.text = "Display: Off";
        }
        else
        {
            displaytext.text = "Display: On";
        }
    }

    //helper function
    public bool GetIfPanelsEnabled()
    {
        return turnup;
    }

    //return instance of SearchPanels
    public static SearchPanelsControl GetInstance()
    {
        return instance;
    }



}
