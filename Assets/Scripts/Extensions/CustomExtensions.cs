using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CustomExtensions
{
    /// <summary>
    /// Clampea un valor al valor mínimo. Si es más chico, el valor es igual al mínimo
    /// </summary>
    /// <param name="val"></param>
    /// <param name="floor"></param>
    /// <returns></returns>
    public static float ClampMinValue(this float val, float floor)
    {
        return val >= floor ? val : floor;
    }
    /// <summary>
    /// Clampea un valor al valor mínimo. Si es más chico, el valor es igual al mínimo
    /// </summary>
    /// <param name="val"></param>
    /// <param name="floor"></param>
    /// <returns></returns>
    public static int ClampMinValue(this int val, int floor)
    {
        return val >= floor ? val : floor;
    }
}
