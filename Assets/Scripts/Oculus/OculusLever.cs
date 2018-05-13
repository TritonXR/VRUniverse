using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OculusLever : MonoBehaviour, LeverVariant
{

    private static OculusLever instance;

    [SerializeField] private float MAX_OFFSET = 1;
    [SerializeField] private float MIN_OFFSET = -1;

    [SerializeField] private float STOP_MAX = 0.1f;
    [SerializeField] private float STOP_MIN = -0.1f;

    [SerializeField] private float MAX_SPEED = 120.0f;
    [SerializeField] private float DEFAULT_SPEED = 60.0f;

    [SerializeField] private float THROTTLE_SET_TIME = 0.5f;

    [SerializeField] private float NUM_VIB = 3;
    [SerializeField] private ushort VIB_INTENSITY = 1500;

    [SerializeField] private float DISPLAYED_SPEED_CORRECTION = -1.0f;

    [SerializeField] private OrbitManager orbitManager;
    [SerializeField] private Transform ForwardAnchor;
    [SerializeField] private Transform RearAnchor;
    [SerializeField] private Text speedNumber;
 

    private const float FP_TOLERANCE = 0.001f;

    private float currentOffset;
    private float maxOffsetDistance;

    private List<Transform> touchingControllers;

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

        if (DEFAULT_SPEED * MAX_SPEED > 0.0f)
        {
            currentOffset = STOP_MAX + DEFAULT_SPEED / MAX_SPEED * (MAX_OFFSET - STOP_MAX);
        }
        else
        {
            currentOffset = STOP_MIN + DEFAULT_SPEED / MAX_SPEED * (MIN_OFFSET - STOP_MIN);
        }
        transform.position = Vector3.Lerp(RearAnchor.position, ForwardAnchor.position, currentOffset / 2.0f + 0.5f);

        maxOffsetDistance = Vector3.Distance(ForwardAnchor.position, RearAnchor.position) / 2.0f;
    }

    // Update is called once per frame
    void Update()
    {
        int numActive = 0;
        Vector3 controllerPos = Vector3.zero;

        foreach (Transform t in touchingControllers)
        {             
            if (t.gameObject.GetComponent<OculusController>().triggerPressed)
            {
                controllerPos += t.gameObject.GetComponent<OculusController>().transform.position;
                numActive++;
            }
        }
        //Debug.Log("Controller Count: " + touchingControllers.Count);

        if (numActive > 0 && AcceptingInput)
        {
            controllerPos /= numActive;
            Vector3 centralAnchor = (ForwardAnchor.position + RearAnchor.position) / 2;
            Vector3 forwardDirection = (ForwardAnchor.position - RearAnchor.position).normalized;

            float nextOffset = Mathf.Clamp(Vector3.Dot(controllerPos - centralAnchor, forwardDirection) / maxOffsetDistance, MIN_OFFSET, MAX_OFFSET);

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

            transform.position = Vector3.Lerp(RearAnchor.position, ForwardAnchor.position, nextOffset / 2.0f + 0.5f);
            currentOffset = nextOffset;

            if (vibrateController)
            {
                // Debug.Log("Should vibrate here");
                foreach (Transform controller in touchingControllers)
                {
                    //SteamVR_Controller.Input((int)controller.GetComponent<SteamVR_TrackedController>().controllerIndex).TriggerHapticPulse(VIB_INTENSITY);
                    //ovr_SetControllerVibration(Hmd, ovrControllerType_LTouch, VIB_INTENSITY, trigger);
                    //VIBRATE
                }

            }
        }

        if (orbitManager != null)
        {
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
                if (numActive <= 0 && AcceptingInput && Mathf.Abs(currentOffset - (STOP_MAX + STOP_MIN) / 2.0f) > FP_TOLERANCE) SetThrottle(0.0f);
            }

        }

        SetLabel(orbitManager.LinearSpeed);
    }

    void OnTriggerEnter(Collider other)
    {
        if ((other.tag.Equals("Right Controller") || other.tag.Equals("Left Controller")) && !touchingControllers.Contains(other.transform))
        {
            touchingControllers.Add(other.transform);
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (touchingControllers.Contains(other.transform))
        {
            touchingControllers.Remove(other.transform);
        }
    }

    public void SetThrottle(float speed)
    {
        speed = Mathf.Clamp(speed, -1.0f, 1.0f);
        StartingThrottle = currentOffset;
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

        CurrentInterpolation = 0.0f;
        AcceptingInput = false;
        StartCoroutine(SmoothAcceleration());
    }

    private void SetLabel(float target)
    {
        target = target * DISPLAYED_SPEED_CORRECTION;
        speedNumber.text = "Speed: " + target.ToString("N0");
    }

    public float GetDefaultThrottle()
    {
        return DEFAULT_SPEED / MAX_SPEED;
    }

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