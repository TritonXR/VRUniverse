using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// use to switch which search results are visible
public class ResultShift : MonoBehaviour, PointableObject {

	public enum ResultShiftAmount {ShiftUpOne, ShiftDownOne};

    [SerializeField] private bool IsButtonEnabled; // used to set whether button is enabled by default
	[SerializeField] private ResultShiftAmount shiftDir; // whether button shifts results up or down

    private Image buttonImage;
	private BoxCollider buttonCollider;

	// Use this for initialization
	void Start () {
        buttonImage = GetComponent<Image>();
        buttonImage.enabled = IsButtonEnabled;
		buttonCollider = GetComponent<BoxCollider>();
		buttonCollider.enabled = IsButtonEnabled;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // sets whether button is visible or not
    public void SetButtonEnabled(bool enable)
    {
		buttonCollider.enabled = buttonImage.enabled = IsButtonEnabled = enable;
    }


    // called when user starts pointing at button
    public void PointerEnter()
    {
        return;
    }

    // called when user pulls trigger while pointing at this button
    public void PointerClick()
    {
        //shifts the list of search results up or down one
		switch (shiftDir) {
		case ResultShiftAmount.ShiftUpOne:
			ResultDisplay.GetInstance().ShiftDisplayedResults(-1);
			break;
		case ResultShiftAmount.ShiftDownOne:
			ResultDisplay.GetInstance().ShiftDisplayedResults(1);
			break;
		}
    }

    // called when user stops pointing at button
    public void PointerExit()
    {
        return;
    }
}
