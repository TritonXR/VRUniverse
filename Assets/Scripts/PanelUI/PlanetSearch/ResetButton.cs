﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResetButton : MonoBehaviour, PointableObject {

	[SerializeField] private Image resetIcon;
	[SerializeField] private Color defaultColor= new Color(182, 252, 255);
	[SerializeField] private Color highlightColor1 = Color.green;

	// Use this for initialization
	void Start () {
		resetIcon.color = defaultColor;

	}

	// Update is called once per frame
	void Update () {

	}

	public void PointerEnter()
	{
        resetIcon.color = highlightColor1;
	}

	public void PointerClick()
	{

		CategoryManager manager = CategoryManager.GetInstance();
		manager.ResetAll();
	
	}

	public void PointerExit()
	{
		resetIcon.color = defaultColor;
	}
}
