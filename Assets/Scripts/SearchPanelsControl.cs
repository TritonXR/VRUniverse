using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchPanelsControl : MonoBehaviour {

    // new panels = UniverseSystem.GetInstance().GetComponent<Canvas>();
    private Canvas[] newpanels;
    private bool turnup;
    // private ToggleButton 

    // Use this for initialization
    void Start () {
        newpanels = UniverseSystem.GetInstance().panels;
        turnup = false;
        clearPanels();  //will that be a delay  
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.C))
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

    public void displayPanels()
    {
        for (int index = 0; index < newpanels.Length; index++)
        {
            if (newpanels[index].GetComponent<CategoryManager>() != null || newpanels[index].GetComponentInChildren<ResultDisplay>() != null) //access PanelLeft
            {
                newpanels[index].enabled = true;
            }
        }
    }

    public void clearPanels()
    {
        for (int index = 0; index < newpanels.Length; index++)
        {
            if (newpanels[index].GetComponent<CategoryManager>() != null || newpanels[index].GetComponentInChildren<ResultDisplay>() != null) //access PanelLeft
            {
                newpanels[index].enabled = false;
            }
        }
    }





}
