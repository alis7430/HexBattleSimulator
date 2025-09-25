using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StatCalc
{
    public static float Apply(float baseValue, float flat, float percentAdd, float percentMult)
    {
        return (baseValue * (1 + percentAdd)) * percentMult + flat;
    }
}
