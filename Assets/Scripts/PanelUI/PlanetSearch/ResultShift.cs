using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultShift : MonoBehaviour, PointableObject {

    [SerializeField] private bool IsButtonEnabled;

    private Image buttonImage;

	// Use this for initialization
	void Start () {
        buttonImage = GetComponent<Image>();
        buttonImage.enabled = IsButtonEnabled;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetButtonEnabled(bool enable)
    {
        buttonImage.enabled = IsButtonEnabled = enable;
    }

    public void PointerEnter()
    {
        return;
    }

    public void PointerClick()
    {
        return;
    }

    public void PointerExit()
    {
        return;
    }
}
