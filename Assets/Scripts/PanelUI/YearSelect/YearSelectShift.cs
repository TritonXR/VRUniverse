using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YearSelectShift : MonoBehaviour, PointableObject {

    public enum YearShiftAmount {IncrementYear, DecrementYear};

    [SerializeField] private YearShiftAmount ShiftDirection;

    public void PointerEnter()
    {
        return;
    }

    public void PointerClick()
    {
        int increment = 0;

        switch (ShiftDirection)
        {
            case YearShiftAmount.IncrementYear:
                increment = 1;
                break;
            case YearShiftAmount.DecrementYear:
                increment = -1;
                break;
            default:
                increment = 0;
                break;
        }

        YearSelectMain.GetInstance().IncrementYear(increment);
    }

    public void PointerExit()
    {
        return;
    }
}
