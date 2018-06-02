using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RotationLever : MonoBehaviour, LeverVariant
{
    // max and minimum rotations the lever can move, with 90 being the 90 degrees forward, -90 being 90 degrees backwards
    [SerializeField] private float MAX_ROTATION = 75.0f;
    [SerializeField] private float MIN_ROTATION = -75.0f;

    // max and minimum rotations that count as 0, with 90 being the 90 degrees forward, -90 being 90 degrees backwards
    [SerializeField] private float STOP_MAX = 10.0f;
    [SerializeField] private float STOP_MIN = -10.0f;

    // the maximum and default speeds the ship can move at
    // the forward and backward speeds cap at the same absolute value
    // due to a bug, a positive speed means moving backwards
    [SerializeField] private float MAX_SPEED = 120.0f;
    [SerializeField] private float DEFAULT_SPEED = 60.0f;

    // how long (in seconds) it takes the throttle to move when moving on its own
    [SerializeField] private float THROTTLE_SET_TIME = 0.5f;

    // num of vibration points on either side of 0, and the intensity of vibrations
    [SerializeField] private float NUM_VIB = 3;
    [SerializeField] private ushort VIB_INTENSITY = 1500;

    // this is multiplied by the stored speed to get the speed displayed on the panel
    [SerializeField] private float DISPLAYED_SPEED_CORRECTION = -1.0f;

    [SerializeField] private OrbitManager orbitManager; // reference to script  that populates planets and moves the ship

    [SerializeField] private Text speedNumber; // text where speed is displayed

    private const float FP_TOLERANCE = 0.001f; //how much floating point difference is considered 'equal'

    private List<Transform> touchingControllers;

    // these are used when the throttle moves on its own
    private bool AcceptingInput;
    private float StartingThrottle;
    private float TargetThrottle;
    private float CurrentInterpolation;

    void Awake()
    {
        if (!LeverScript.GetInstance().RegisterLever(this))
        {
            Destroy(this);
        }
    }

    // Use this for initialization
    void Start()
    {
        touchingControllers = new List<Transform>();
        AcceptingInput = true;
        //Debug.Log("Default speed: " + DEFAULT_SPEED);
        SetLabel(DEFAULT_SPEED);
    }

    // Update is called once per frame
    void Update()
    {

        // finds all of the controllers in contact with the lever that have the trigger pressed
        int numActive = 0;
        Vector3 direction = Vector3.zero; // note: this direction is in the lever's local space
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

            //calculates where the controller should be moved to
            float x_rotation = Mathf.Clamp(-Mathf.Atan2(direction.y, direction.z) * Mathf.Rad2Deg, MIN_ROTATION, MAX_ROTATION);
            //Debug.Log("X_Rotation: " + x_rotation);

            bool vibrateController = false;
            float old_x_rotation = (transform.localEulerAngles.x <= 180) ? transform.localEulerAngles.x : transform.localEulerAngles.x - 360;

            // vibrates controller if you pass a vibration point on the lever
            if (x_rotation > STOP_MAX || old_x_rotation > STOP_MAX)
            {
                float angleInc = (MAX_ROTATION - STOP_MAX) / NUM_VIB;
                float oldInc = Mathf.Floor((old_x_rotation - STOP_MAX) / angleInc);
                float newInc = Mathf.Floor((x_rotation - STOP_MAX) / angleInc);
                vibrateController = (oldInc != newInc);
            }
            else if (x_rotation < STOP_MIN || old_x_rotation < STOP_MIN)
            {
                float angleInc = (STOP_MIN - MIN_ROTATION) / NUM_VIB;
                float oldInc = Mathf.Floor((STOP_MIN - old_x_rotation) / angleInc);
                float newInc = Mathf.Floor((STOP_MIN - x_rotation) / angleInc);
                vibrateController = (oldInc != newInc);
            }

            //updates the lever's rotation
            transform.localEulerAngles = new Vector3(x_rotation, transform.localEulerAngles.y, transform.localEulerAngles.z);

            //vibrates the controller if a vibration point is passed
            if (vibrateController)
            {
                Debug.Log("Should vibrate here");
                foreach (Transform controller in touchingControllers)
                {
                    SteamVR_Controller.Input((int)controller.GetComponent<SteamVR_TrackedController>().controllerIndex).TriggerHapticPulse(VIB_INTENSITY);
                }

            }
        }

        // updates the speed of the ship if there's a valid orbitManager
        if (orbitManager != null)
        {
            float x_rot = transform.localEulerAngles.x;

            //calculates the desired speed from the lever's rotation
            if (x_rot > STOP_MAX && x_rot <= MAX_ROTATION + FP_TOLERANCE)
            {
                orbitManager.LinearSpeed = (x_rot - STOP_MAX) / (MAX_ROTATION - STOP_MAX) * MAX_SPEED;
            }
            else if (x_rot < 360.0f + STOP_MIN && x_rot >= 360.0f + MIN_ROTATION - FP_TOLERANCE)
            {
                orbitManager.LinearSpeed = (x_rot - (360.0f + STOP_MIN)) / (STOP_MIN - MIN_ROTATION) * MAX_SPEED;
            }
            else
            {
                orbitManager.LinearSpeed = 0.0f;

                //moves controller to exact center if you let go of it the zone considered 'stop' or '0 speed'
                if (numActive <= 0 && AcceptingInput) SetThrottle(0.0f);
            }

            //updates the speed label
            SetLabel(orbitManager.LinearSpeed);
        }

    }

    // updates the list of controllers in contact with the lever when a collision occurs
    void OnTriggerEnter(Collider other)
    {
        // only care about controllers
        if (other.tag.Equals("GameController") && !touchingControllers.Contains(other.transform))
        {
            touchingControllers.Add(other.transform);
        }

    }

    // updates the list of controllers in contact with the lever when a collision occurs
    private void OnTriggerExit(Collider other)
    {
        if (touchingControllers.Contains(other.transform))
        {
            touchingControllers.Remove(other.transform);
        }
    }

    // use this to set the speed of the ship
    public void SetThrottle(float speed)
    {
        // only accept speeds from -1 (full reverse) to 1 (full ahead) with 0 being stop
        speed = Mathf.Clamp(speed, -1.0f, 1.0f);
        StartingThrottle = (transform.localEulerAngles.x > 180.0f) ? transform.localEulerAngles.x - 360.0f : transform.localEulerAngles.x;

        //calculates desired throttle offset
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

        // smoothly move the throttle from where it is now to where its supposed to go
        CurrentInterpolation = 0.0f;
        AcceptingInput = false;
        StartCoroutine(SmoothAcceleration());
    }

    //display the target value as the speed on the speed label
    private void SetLabel(float target)
    {
        target = target * DISPLAYED_SPEED_CORRECTION;
        speedNumber.text = "Speed: " + target.ToString("N0");
    }

    // returns the default throttle value
    public float GetDefaultThrottle()
    {
        return DEFAULT_SPEED / MAX_SPEED;
    }

    //interpolates the position of the lever over time to move it
    IEnumerator SmoothAcceleration()
    {
        while (CurrentInterpolation < 1.0f)
        {
            CurrentInterpolation += Time.deltaTime / THROTTLE_SET_TIME;
            float x_rot = Mathf.Lerp(StartingThrottle, TargetThrottle, CurrentInterpolation);
            transform.localEulerAngles = new Vector3(x_rot, transform.localEulerAngles.y, transform.localEulerAngles.z);
            yield return null;
        }
        AcceptingInput = true;
    }
}
