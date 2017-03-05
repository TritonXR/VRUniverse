using UnityEngine;
using System.Collections;

public class Lever : MonoBehaviour {

	bool leverOn = false;
	private Animator anima;

	void OnTriggerStay(Collider other) {
		if (other.tag == "Player" )
		{
			Debug.Log("is player");
		    if (Input.GetMouseButtonDown(0))
			{
				Debug.Log("left clicked");
				anima.SetBool("switch", true);
				anima.SetBool("switch 2", false);

				leverOn = true;
			}
			else if (Input.GetMouseButtonDown(1))
			{
				Debug.Log("right clicked");
				anima.SetBool("switch", false);
				anima.SetBool("switch 2", true);
				leverOn = false;
			}	
		}
	}

	// Use this for initialization
	void Start () { 
	
			anima = GetComponent<Animator>();
			

		
	}
	
	// Update is called once per frame
	void Update () {
		
		
	}
}
