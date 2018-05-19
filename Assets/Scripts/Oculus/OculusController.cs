using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OculusController : MonoBehaviour {
         
    public bool triggerPressed;
   

    // Use this for initialization
    void Start () {       
        
        triggerPressed = false;
    }
	
	// Update is called once per frame
	void Update () {
        if(this.gameObject.tag == "Left Controller")
        {
            if (OVRInput.GetDown(OVRInput.RawButton.LHandTrigger)) triggerPressed = true;
            if (OVRInput.GetUp(OVRInput.RawButton.LHandTrigger)) triggerPressed = false;
        }
        else if(this.gameObject.tag == "Right Controller")
        {
            if (OVRInput.GetDown(OVRInput.RawButton.RHandTrigger)) triggerPressed = true;
            if (OVRInput.GetUp(OVRInput.RawButton.RHandTrigger)) triggerPressed = false;
        }

        
    }
}
