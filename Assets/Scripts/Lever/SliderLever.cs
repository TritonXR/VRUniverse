using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// this is a lever that slides forward and backward
public class SliderLever : MonoBehaviour, LeverVariant
{
    // max and minimum locations the lever can move, with 1 being the forward anchor, -1 being the rear anchor
    [SerializeField] private float MAX_OFFSET = 1;
    [SerializeField] private float MIN_OFFSET = -1;

    // max and minimum locations that count as 0, with 1 being the forward anchor, -1 being the rear anchor
    [SerializeField] private float STOP_MAX = 0.1f;
    [SerializeField] private float STOP_MIN = -0.1f;

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
    [SerializeField] private Transform ForwardAnchor; // reference to object indicating forward limit of movement
    [SerializeField] private Transform RearAnchor;// reference to object indicating rear limit of movement
    [SerializeField] private Text speedNumber; // text where speed is displayed

    private const float FP_TOLERANCE = 0.001f; //how much floating point difference is considered 'equal'

    private float currentOffset;
    private float maxOffsetDistance;

    private List<Transform> touchingControllers; // all controllers touching the lever

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
        SetLabel(0);
        currentOffset = 0;
        transform.position = Vector3.Lerp(RearAnchor.position, ForwardAnchor.position, currentOffset / 2.0f + 0.5f); // starts in center
        maxOffsetDistance = Vector3.Distance(ForwardAnchor.position, RearAnchor.position) / 2.0f;
    }

    // Update is called once per frame
    void Update()
    {

        // finds all of the controllers in contact with the lever that have the trigger pressed
        int numActive = 0;
        Vector3 controllerPos = Vector3.zero;
        foreach (Transform t in touchingControllers)
        {
            SteamVR_TrackedController controller = t.GetComponent<SteamVR_TrackedController>();
            if (controller == null) Debug.LogWarning("No TrackedController script found!");
            if (controller != null && controller.triggerPressed)
            {
                controllerPos += controller.transform.position;
                numActive++;
            }
        }
        //Debug.Log("Controller Count: " + touchingControllers.Count);

        if (numActive > 0 && AcceptingInput)
        {
            controllerPos /= numActive; // gets average position of touching controllers with pulled trigger
            Vector3 centralAnchor = (ForwardAnchor.position + RearAnchor.position) / 2;
            Vector3 forwardDirection = (ForwardAnchor.position - RearAnchor.position).normalized;

            //calculates where the controller should be moved to (nearest point to average controller position on line between forward and rear anchors)
            float nextOffset = Mathf.Clamp(Vector3.Dot(controllerPos - centralAnchor, forwardDirection) / maxOffsetDistance, MIN_OFFSET, MAX_OFFSET);

            // vibrates controller if you pass a vibration point on the lever
            bool vibrateController = false;
            if (nextOffset > STOP_MAX || currentOffset > STOP_MAX)
            {
                float distInc = (MAX_OFFSET - STOP_MAX) / NUM_VIB;
                float oldInc = Mathf.Floor((currentOffset - STOP_MAX) / distInc);
                float newInc = Mathf.Floor((nextOffset - STOP_MAX) / distInc);
                vibrateController = (oldInc != newInc);
            }
            else if (nextOffset < STOP_MIN || currentOffset < STOP_MIN)
            {
                float distInc = (STOP_MIN - MIN_OFFSET) / NUM_VIB;
                float oldInc = Mathf.Floor((STOP_MIN - currentOffset) / distInc);
                float newInc = Mathf.Floor((STOP_MIN - nextOffset) / distInc);
                vibrateController = (oldInc != newInc);
            }

            //updates the lever's positon
            transform.position = Vector3.Lerp(RearAnchor.position, ForwardAnchor.position, nextOffset / 2.0f + 0.5f);
            currentOffset = nextOffset;

            //vibrates the controller if a vibration point is passed
            if (vibrateController)
            {
                //Debug.Log("Should vibrate here");
                foreach (Transform controller in touchingControllers)
                {
                    SteamVR_Controller.Input((int)controller.GetComponent<SteamVR_TrackedController>().controllerIndex).TriggerHapticPulse(VIB_INTENSITY);
                }

            }
        }

        // updates the speed of the ship if there's a valid orbitManager
        if (orbitManager != null)
        {
            //calculates the desired speed from the lever's positon
            if (currentOffset > STOP_MAX && currentOffset <= MAX_OFFSET + FP_TOLERANCE)
            {
                orbitManager.LinearSpeed = (currentOffset - STOP_MAX) / (MAX_OFFSET - STOP_MAX) * MAX_SPEED;
            }
            else if (currentOffset < STOP_MIN && currentOffset >= MIN_OFFSET - FP_TOLERANCE)
            {
                orbitManager.LinearSpeed = (currentOffset - STOP_MIN) / (STOP_MIN - MIN_OFFSET) * MAX_SPEED;
            }
            else
            {
                orbitManager.LinearSpeed = 0.0f;
                
                //moves controller to exact center if you let go of it the zone considered 'stop' or '0 speed'
                if (numActive <= 0 && AcceptingInput && Mathf.Abs(currentOffset - (STOP_MAX + STOP_MIN) / 2.0f) > FP_TOLERANCE) SetThrottle(0.0f);
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
        StartingThrottle = currentOffset;

        //calculates desired throttle offset
        if (speed > 0.0f)
        {
            TargetThrottle = STOP_MAX + speed * (MAX_OFFSET - STOP_MAX);
        }
        else if (speed < 0.0f)
        {
            TargetThrottle = STOP_MIN + speed * (MIN_OFFSET - STOP_MIN);
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
            currentOffset = Mathf.Lerp(StartingThrottle, TargetThrottle, CurrentInterpolation);
            transform.position = Vector3.Lerp(RearAnchor.position, ForwardAnchor.position, currentOffset / 2.0f + 0.5f);
            yield return null;
        }
        AcceptingInput = true;
    }
}

