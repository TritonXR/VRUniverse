using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultShift : MonoBehaviour, PointableObject {

	public enum ResultShiftAmount {ShiftUpOne, ShiftDownOne};

    [SerializeField] private bool IsButtonEnabled;
	[SerializeField] private ResultShiftAmount shiftDir;

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

    public void SetButtonEnabled(bool enable)
    {
		buttonCollider.enabled = buttonImage.enabled = IsButtonEnabled = enable;
    }

    public void PointerEnter()
    {
        return;
    }

    public void PointerClick()
    {
		switch (shiftDir) {
		case ResultShiftAmount.ShiftUpOne:
			ResultDisplay.GetInstance().ShiftDisplayedResults(-1);
			break;
		case ResultShiftAmount.ShiftDownOne:
			ResultDisplay.GetInstance().ShiftDisplayedResults(1);
			break;
		}
    }

    public void PointerExit()
    {
        return;
    }
}
