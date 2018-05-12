using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OculusPointer : MonoBehaviour {
    //Contains references to the color when the user is not pointing at a specific gameobject versus when they are
    [SerializeField] private Color inactiveColor;
    [SerializeField] private Color activeColor;

    public float distance = 2000f;
    public GameObject holder;
    public float thickness = 0.002f;

    PointableObject currContact = null;
    GameObject prevGO;
    GameObject pointer;
    bool isActive;

    /*
     * Start: Initializes controller, laser, and handles first pointer delegates to the laser
     * Parameters: None
     */
    void Start()
    {
        isActive = false;
        pointer = GameObject.CreatePrimitive(PrimitiveType.Cube);
        pointer.transform.parent = holder.transform;
        pointer.transform.localScale = new Vector3(thickness, thickness, distance);
        pointer.transform.localPosition = new Vector3(0f, 0f, distance/2);
        pointer.transform.localRotation = Quaternion.identity;
        pointer.layer = 2;
        BoxCollider collider = pointer.GetComponent<BoxCollider>();

        Material newMaterial = new Material(Shader.Find("Unlit/Color"));
        newMaterial.SetColor("_Color", inactiveColor);
        pointer.GetComponent<MeshRenderer>().material = newMaterial;
        SetPointerColor(isActive);
    }

    // Update is called once per frame
    void Update()
    {
        float dist = distance;


        Ray raycast = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        bool bHit = Physics.Raycast(raycast, out hit);

        if (bHit && hit.distance < distance)
        {
            dist = hit.distance;

            if (prevGO != null && prevGO != hit.transform.gameObject)
            {
                if (prevGO.GetComponent<PointableObject>() != null) prevGO.GetComponent<PointableObject>().PointerExit();
            }

            currContact = hit.transform.gameObject.GetComponent<PointableObject>();
            if (currContact != null)
            {
                isActive = true;
                SetPointerColor(isActive);
                currContact.PointerEnter();
            }

            prevGO = hit.transform.gameObject;
        }
        else
        {
            if (isActive)
            {
                isActive = false;
                SetPointerColor(isActive);
            }
            if (currContact != null)
            {
                currContact.PointerExit();
                currContact = null;
            }
        }
        if ((currContact != null) && OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger))
        {
            currContact.PointerClick();
        }
        if (OVRInput.Get(OVRInput.RawButton.RIndexTrigger))
        {
            pointer.transform.localScale = new Vector3(thickness * 5f, thickness * 5f, dist);

        }
        else
        {
            pointer.transform.localScale = new Vector3(thickness, thickness, dist);
        }
        pointer.transform.localPosition = new Vector3(0f, 0f, dist / 2f);
    }

    /*
     * SetPointerColor: Set the color of the laser
     * Parameters: bool active - true means the laser is active so should be activeColor
	 *			                 false means the laser is not active so should be inactiveColor
     */
    private void SetPointerColor(bool active)
    {
        if (active)
        {
            //Only sets the color in the component. Need to directly access the pointer gameobject and set the material color
            pointer.GetComponent<MeshRenderer>().material.color = activeColor;
        }
        else
        {
            pointer.GetComponent<MeshRenderer>().material.color = inactiveColor;
        }
    }
}
