using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YearSelectShift : MonoBehaviour, PointableObject {

    public enum YearShift {IncrementYear, DecrementYear};

    [SerializeField] private YearShift ShiftDirection;

    public void PointerStart()
    {
        return;
    }

    public void PointerClick()
    {
        int increment = 0;

        switch (ShiftDirection)
        {
            case YearShift.IncrementYear:
                increment = 1;
                break;
            case YearShift.DecrementYear:
                increment = -1;
                break;
            default:
                increment = 0;
                break;
        }

        YearSelectMain.GetInstance().IncrementYear(increment);
    }

    public void PointerStop()
    {
        return;
    }
}
