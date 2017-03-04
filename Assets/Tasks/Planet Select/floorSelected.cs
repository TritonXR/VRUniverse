using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using VRTK;

public class floorSelected : VRTK_InteractableObject
{

    // Use this for initialization
    void Start()
    {
    }

    public override void StartUsing(GameObject currentUsingObject)
    {
        //Debug.Log("1name of selected project" + gameObject.name);
        currentUsingObject.GetComponent<VRTK_SimplePointer>().enableTeleport = true;
        Debug.Log("setting the : " + currentUsingObject.name + " equal to true");
        base.StartUsing(currentUsingObject);

        //Debug.Log("3name of selected project" + gameObject.name);


    }

    public override void StopUsing(GameObject previousUsingObject)
    {
        Debug.Log("stop");
        previousUsingObject.GetComponent<VRTK_SimplePointer>().enableTeleport = true;
        base.StopUsing(previousUsingObject);
    }
    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}