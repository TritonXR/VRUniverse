using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverScript : MonoBehaviour {

    //[SerializeField] private GameObject lever;
    //[SerializeField] private GameObject handle;
    private Transform controllerTransform;

    private const float FAST = 70.0f;
    private const float DEFAULT = 35.0f;
    private const float STOP = 0.0f;
    private const float REVERSE = 70.0f;

	// Use this for initialization
	void Start () {
        controllerTransform = null;
        //lever = gameObject.GetComponentInChildren<GameObject>().;
	}
	
	// Update is called once per frame
	void Update () {
        if (controllerTransform != null)
        {
            //controllerTransform = other.transform;
            Quaternion rotation = Quaternion.FromToRotation(transform.forward, controllerTransform.position - transform.position);
            transform.localEulerAngles = new Vector3(rotation.x, 0, 0);
        }

    }

    void OnTriggerEnter(Collider other) {
        if (other.tag.Equals("GameController"))
        {
            controllerTransform = other.transform;
            //Quaternion rotation = Quaternion.FromToRotation(transform.forward, controllerTransform.position - transform.position);
            //transform.localEulerAngles = new Vector3(rotation.x, 0, 0);
        }

    }

    private void OnTriggerExit(Collider other)
    {
        //should rest at nearest position
    }
}
