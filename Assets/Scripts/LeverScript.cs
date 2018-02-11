using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverScript : MonoBehaviour {

    //[SerializeField] private GameObject lever;
    //[SerializeField] private GameObject handle;

    [SerializeField] private float MAX_ROTATION = 75.0f;
    [SerializeField] private float MIN_ROTATION = -75.0f;

    [SerializeField] private float STOP_MAX = 10.0f;
    [SerializeField] private float STOP_MIN = -10.0f;

    [SerializeField] private float MAX_SPEED = 60.0f;

    [SerializeField] private OrbitManager orbitManager;

    private const float FP_TOLERANCE = 0.001f;

    private List<Transform> touchingControllers;

	// Use this for initialization
	void Start () {
        touchingControllers = new List<Transform>();
        //lever = gameObject.GetComponentInChildren<GameObject>().;
	}
    
    // Update is called once per frame
    void Update()
    {

        int numActive = 0;
        Vector3 direction = Vector3.zero;
        foreach (Transform t in touchingControllers)
        {
            SteamVR_TrackedController controller = t.GetComponent<SteamVR_TrackedController>();
            if (controller == null) Debug.LogWarning("No TrackedController script found!");
            if (controller != null && controller.triggerPressed)
            {
                Vector3 contDir = transform.parent.InverseTransformPoint(t.position) - transform.localPosition;
                contDir.x = 0;
                direction += contDir.normalized;
                numActive++;
            }
        }

        if (numActive > 0)
        {
            direction.Normalize();
            float x_rotation = Mathf.Clamp(-Mathf.Atan2(direction.y, direction.z) * Mathf.Rad2Deg, MIN_ROTATION, MAX_ROTATION);
            //Debug.Log("X_Rotation: " + x_rotation);
            transform.localEulerAngles = new Vector3(x_rotation, transform.localEulerAngles.y, transform.localEulerAngles.z);
        }

        if(orbitManager != null)
        {
            float x_rot = transform.localEulerAngles.x;
            //Debug.Log("X_Rotation: " + x_rot);
            if (x_rot > STOP_MAX && x_rot <= MAX_ROTATION + FP_TOLERANCE)
            {
                orbitManager.LinearSpeed = (x_rot - STOP_MAX) / (MAX_ROTATION - STOP_MAX) * MAX_SPEED;
            }
            else if(x_rot < 360.0f + STOP_MIN && x_rot >= 360.0f + MIN_ROTATION - FP_TOLERANCE)
            {
                orbitManager.LinearSpeed = (x_rot - (360.0f + STOP_MIN)) / (STOP_MIN - MIN_ROTATION) * MAX_SPEED;
            }
            else
            {
                orbitManager.LinearSpeed = 0.0f;
            }
        }

    }

    void OnTriggerEnter(Collider other) {
        if (other.tag.Equals("GameController") && !touchingControllers.Contains(other.transform))
        {
            //Debug.Log("Collided with controller: " + other.name);
            touchingControllers.Add(other.transform);
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (touchingControllers.Contains(other.transform))
        {
            //Debug.Log("Uncollided with controller");
            touchingControllers.Remove(other.transform);
        }
    }
}
