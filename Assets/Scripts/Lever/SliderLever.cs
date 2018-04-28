using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderLever : MonoBehaviour, LeverVariant
{
    [SerializeField] private float MAX_OFFSET = 0.15f;
    [SerializeField] private float MIN_OFFSET = -0.15f;

    [SerializeField] private float STOP_MAX = 0.025f;
    [SerializeField] private float STOP_MIN = -0.025f;

    [SerializeField] private float MAX_SPEED = 120.0f;
    [SerializeField] private float DEFAULT_SPEED = 60.0f;

    [SerializeField] private float THROTTLE_SET_TIME = 0.5f;

    [SerializeField] private float NUM_VIB = 3;
    [SerializeField] private ushort VIB_INTENSITY = 1500;

    [SerializeField] private float DISPLAYED_SPEED_CORRECTION = -1.0f;

    [SerializeField] private OrbitManager orbitManager;
    [SerializeField] private Transform slideAnchor;
    [SerializeField] private Text speedNumber;

    private const float FP_TOLERANCE = 0.001f;

    float currentOffset;

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
    }

    // Update is called once per frame
    void Update()
    {

        int numActive = 0;
        Vector3 controllerPos = Vector3.zero;
        foreach (Transform t in touchingControllers)
        {
            SteamVR_TrackedController controller = t.GetComponent<SteamVR_TrackedController>();
            if (controller == null) Debug.LogWarning("No TrackedController script found!");
            if (controller != null && controller.triggerPressed)
            {
                Vector3 contDir = transform.parent.InverseTransformPoint(t.position) - transform.localPosition;
                contDir.x = 0;
                controllerPos += contDir.normalized;
                numActive++;
            }
        }
        //Debug.Log("Controller Count: " + touchingControllers.Count);

        if (numActive > 0 && AcceptingInput)
        {
            controllerPos /= numActive;
            float nextOffset = Mathf.Clamp(Vector3.Dot(controllerPos - slideAnchor.position, slideAnchor.forward), MIN_OFFSET, MAX_OFFSET);

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

            transform.position = slideAnchor.position + slideAnchor.forward * nextOffset;
            currentOffset = nextOffset;

            if (vibrateController)
            {
                //Debug.Log("Should vibrate here");
                foreach (Transform controller in touchingControllers)
                {
                    SteamVR_Controller.Input((int)controller.GetComponent<SteamVR_TrackedController>().controllerIndex).TriggerHapticPulse(VIB_INTENSITY);
                }

            }
        }

        if (orbitManager != null)
        {
            if (currentOffset > STOP_MAX && currentOffset <= MAX_OFFSET + FP_TOLERANCE)
            {
                orbitManager.LinearSpeed = (currentOffset - STOP_MAX) / (MAX_OFFSET - STOP_MAX) * MAX_SPEED;
            }
            else if (currentOffset < 360.0f + STOP_MIN && currentOffset >= 360.0f + MIN_OFFSET - FP_TOLERANCE)
            {
                orbitManager.LinearSpeed = (currentOffset - (360.0f + STOP_MIN)) / (STOP_MIN - MIN_OFFSET) * MAX_SPEED;
            }
            else
            {
                orbitManager.LinearSpeed = 0.0f;
                if (numActive <= 0 && AcceptingInput) SetThrottle(0.0f);
            }

        }

        SetLabel(orbitManager.LinearSpeed);

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("GameController") && !touchingControllers.Contains(other.transform))
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
            transform.position = slideAnchor.position + slideAnchor.forward * currentOffset;
            yield return null;
        }
        AcceptingInput = true;
    }
}

