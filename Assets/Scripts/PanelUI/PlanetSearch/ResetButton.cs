using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResetButton : MonoBehaviour, PointableObject {

	[SerializeField] private Image resetIcon;
	[SerializeField] private Color defaultColor;
	[SerializeField] private Color highlightColor;

	// Use this for initialization
	void Start () {
		resetIcon.color = defaultColor;
	}

	// Update is called once per frame
	void Update () {

	}

	public void PointerEnter()
	{
		resetIcon.color = highlightColor;
	}

	public void PointerClick()
	{

		CategoryManager manager = CategoryManager.GetInstance();
		List<string> selectedCategories = manager.getSelectedCategories ();
		manager.ResetAll (selectedCategories);
	
	}

	public void PointerExit()
	{
		resetIcon.color = highlightColor;
	}
}
