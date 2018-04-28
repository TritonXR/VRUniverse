using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverScript {

    private static LeverScript instance;

    private LeverVariant lever;

    private LeverScript()
    {
        lever = null;
    }

    public void SetThrottle(float speed)
    {
        if (lever != null) lever.SetThrottle(speed);
        else Debug.LogWarning("No lever registered, can't set throttle!");
    }

    public float GetDefaultThrottle()
    {
        if (lever != null) return lever.GetDefaultThrottle();
        else return 0.0f;
    }

    public static LeverScript GetInstance()
    {
        if(instance == null)
        {
            instance = new LeverScript();
        }

        return instance;
    }

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
