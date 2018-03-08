using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackToSearch : MonoBehaviour, PointableObject {

	[SerializeField] private Canvas detailedEntryCanvas;
	[SerializeField] private Image highlight;

	private BoxCollider[] buttonColliders;

	// Use this for initialization
	void Start () {
		buttonColliders = detailedEntryCanvas.GetComponentsInChildren<BoxCollider> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void PointerEnter()
	{
		//Enable the highlight to show it is being hovered on
		highlight.enabled = true;

	}

	/*
     * PointerStop: Called whenever the user stops pointing at the travel menu button.
     */
	public void PointerExit()
	{
		//Disable the highlight
		highlight.enabled = false;
	}

	public void PointerClick()
	{
		detailedEntryCanvas.enabled = false;
		foreach (BoxCollider col in buttonColliders) {
			col.enabled = false;
		}
		highlight.enabled = false;
	}
}
