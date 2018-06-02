using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// this is a pass-through class designed to permit easy switching between the rotation lever and the slider lever
public class LeverScript {

    private static LeverScript instance; //singleton reference

    private LeverVariant lever; //reference to the active lever

    private LeverScript()
    {
        lever = null;
    }

    //Sets the current speed of the ship
    //Parameter should be a value from -1 (full reverse) to 1 (full forward), with 0 being stopped
    public void SetThrottle(float speed)
    {
        if (lever != null) lever.SetThrottle(speed);
        else Debug.LogWarning("No lever registered, can't set throttle!");
    }

    //returns the default speed of the ship as a value from -1 (full reverse) to 1 (full forward), with 0 being stopped
    public float GetDefaultThrottle()
    {
        if (lever != null) return lever.GetDefaultThrottle();
        else return 0.0f;
    }

    //returns a reference to the LeverScript object in the scene
    public static LeverScript GetInstance()
    {

        // if this is the first time this was called, create an instance
        if(instance == null)
        {
            instance = new LeverScript();
        }

        return instance;
    }

    // this is the function that levers use to let the LeverScript know they exist
    // only the first lever can register itself, all subsequent levers are rejected
    public bool RegisterLever(LeverVariant lever)
    {
        if (this.lever == null)
        {
            this.lever = lever;
            return true;
        }
        else
        {
            return false;
        }
    }
    
    
}

public interface LeverVariant
{
    void SetThrottle(float speed);

    float GetDefaultThrottle();
}
