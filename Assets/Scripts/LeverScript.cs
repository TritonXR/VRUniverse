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

    [SerializeField] private float MAX_SPEED = 120.0f;
    [SerializeField] private float DEFAULT_SPEED = 60.0f;

    [SerializeField] private float THROTTLE_SET_TIME = 0.5f;

    [SerializeField] private OrbitManager orbitManager;

    private const float FP_TOLERANCE = 0.001f;

    private List<Transform> touchingControllers;

    private bool AcceptingInput;
    private float StartingThrottle;
    private float TargetThrottle;
    private float CurrentInterpolation;

	// Use this for initialization
	void Start () {
        touchingControllers = new List<Transform>();
        AcceptingInput = true;
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
        //Debug.Log("Controller Count: " + touchingControllers.Count);

        if (numActive > 0 && AcceptingInput)
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

    public void SetThrottle(float speed)
    {
        speed = Mathf.Clamp(speed, 0.0f, 1.0f);
        StartingThrottle = (transform.localEulerAngles.x > 180.0f) ? transform.localEulerAngles.x - 360.0f : transform.localEulerAngles.x;
        if (speed > 0.0f)
        {
            TargetThrottle = STOP_MAX + speed * (MAX_ROTATION - STOP_MAX);
        }
        else if (speed < 0.0f)
        {
            TargetThrottle = STOP_MIN + speed * (MIN_ROTATION - STOP_MIN);
        }
        else
        {
            TargetThrottle = (STOP_MAX + STOP_MIN) / 2;
        }
        CurrentInterpolation = 0.0f;
        AcceptingInput = false;
        StartCoroutine(SmoothAcceleration());
    }

    public float GetDefaultThrottle()
    {
        return DEFAULT_SPEED / MAX_SPEED;
    }

    IEnumerator SmoothAcceleration()
    {
        while(CurrentInterpolation < 1.0f)
        {
            CurrentInterpolation += Time.deltaTime / THROTTLE_SET_TIME;
            float x_rot = Mathf.Lerp(StartingThrottle, TargetThrottle, CurrentInterpolation);
            transform.localEulerAngles = new Vector3(x_rot, transform.localEulerAngles.y, transform.localEulerAngles.z);
            yield return null;
        }
        AcceptingInput = true;
    }
}
