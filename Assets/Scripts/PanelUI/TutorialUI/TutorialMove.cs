using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialMove : MonoBehaviour
{
    [SerializeField] private float moveTime = 1.0f;
    [SerializeField] private Transform followAnchor;

    private bool followEnabled;
    private Vector3 defaultPosition;
    private Quaternion defaultRotation;

    void Awake()
    {
        defaultPosition = transform.localPosition;
        defaultRotation = transform.localRotation;
    }

    // Use this for initialization
    void Start()
    {
        followEnabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (followEnabled)
        {
            Vector3 anchorPos = followAnchor.position;
            Quaternion anchorRot = followAnchor.rotation;

            if (transform.parent != null)
            {
                anchorPos = transform.parent.InverseTransformPoint(anchorPos);
                anchorRot = Quaternion.Inverse(transform.parent.rotation) * anchorRot;
            }

            transform.localPosition = anchorPos;
            transform.localRotation = anchorRot;
        }
    }

    public void StartFollowing()
    {
        followEnabled = true;
    }

    public bool IsFollowing()
    {
        return followEnabled;
    }

    public void StopFollowing()
    {
        followEnabled = false;
        StartCoroutine(MoveBackToDefault());
    }

    private IEnumerator MoveBackToDefault()
    {
        float currInterpolation = 0.0f;
        Vector3 startPosition = transform.localPosition;
        Quaternion startRotation = transform.localRotation;

        while (currInterpolation < 1.0f)
        {
            yield return null;
            currInterpolation += Time.deltaTime / moveTime;
            transform.localPosition = Vector3.Lerp(startPosition, defaultPosition, currInterpolation);
            transform.localRotation = Quaternion.Slerp(startRotation, defaultRotation, currInterpolation);
        }
    }
}
