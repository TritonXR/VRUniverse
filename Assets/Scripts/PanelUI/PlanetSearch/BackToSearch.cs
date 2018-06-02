using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//script for exit button on detailed search results panel
public class BackToSearch : MonoBehaviour, PointableObject {

	[SerializeField] private Canvas detailedEntryCanvas;
	//[SerializeField] private Image highlight;

	private BoxCollider[] buttonColliders;

	// Use this for initialization
	void Start () {
		buttonColliders = detailedEntryCanvas.GetComponentsInChildren<BoxCollider> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // called when user starts pointing at button
	public void PointerEnter()
	{
		//Enable the highlight to show it is being hovered on
		//highlight.enabled = true;

	}

    // called when user stops pointing at button
	public void PointerExit()
	{
		//Disable the highlight
		//highlight.enabled = false;
	}

    // called when user pulls trigger while pointing at this button
	public void PointerClick()
	{
        //turns off canvas and buttons
		detailedEntryCanvas.enabled = false;
		foreach (BoxCollider col in buttonColliders) {
			col.enabled = false;
		}
		//highlight.enabled = false;

        // enables the Category panel and Search Results panel
		CategoryManager.GetInstance().SetVisible(true);
		ResultDisplay.GetInstance().SetVisible(true);
	}
}
