using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parser
{
    public static string Unit(int value)
    {
        if (value >= 1000000000)
        {
            return $"{((float)value / 1000000000f).ToString("0.00")}B";
        }
        else if (value >= 1000000)
        {
            return $"{((float)value / 1000000f).ToString("0.00")}M";
        }
        else if (value >= 1000)
        {
            return $"{((float)value / 1000f).ToString("0.00")}K";
        }
        else
        {
            return value.ToString();
        }
    }
}
