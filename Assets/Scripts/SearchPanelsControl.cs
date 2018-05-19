using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SearchPanelsControl : MonoBehaviour {

    // new panels = UniverseSystem.GetInstance().GetComponent<Canvas>();
    private Canvas[] newpanels;
    private bool turnup;
    [SerializeField] private Text displaytext;
    // private ToggleButton 

    // Use this for initialization
    void Start () {
        newpanels = UniverseSystem.GetInstance().panels;
        turnup = false;
        clearPanels();  //will that be a delay  
        displaytext.text = "Display: Off";
    }

    // Update is called once per frame
    void Update() {
        /*if (Input.GetKeyDown(KeyCode.C))
        {
            if (turnup == false)
            {
                displayPanels();
                turnup = true;
            }
            else
            {
                clearPanels();
                turnup = false;
            }
        }*/
        SetLabel(turnup);
    }

    public void displayPanels()
    {
        //SearchResultsCanvas --> ResultDisplay GetComponentsInChildren<Box Collider>
        //CategoiresCanvas --> CategoryManager
        for (int index = 0; index < newpanels.Length; index++)
        {
            if (newpanels[index].GetComponent<CategoryManager>() != null) //access PanelLeft
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

    public void clearPanels()
    {
        for (int index = 0; index < newpanels.Length; index++)
        {
            if (newpanels[index].GetComponent<CategoryManager>() != null) //access PanelLeft
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
                turnup = true;
            }
            else
            {
                clearPanels();
                turnup = false;
            }
        }
    }


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





}
