using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HelperMethods
{
    public static float ProjectOnRange(float value, float baseMin, float baseMax, float nextMin, float nextMax)
    {
        return nextMin + ((nextMax - nextMin) / (baseMax - baseMin)) * (value - baseMin);
    }
}
