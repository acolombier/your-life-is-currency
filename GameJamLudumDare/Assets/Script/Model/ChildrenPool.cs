using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildrenPool
{
    public int children { get; private set; }

    public int female { get; private set; }
    public int male { get; private set; }

    public ChildrenPool(int c)
    {
        children = c;
    }

    public int kill(float infantMortality)
    {
        int death = 0;
        for (int c = 0; c < children; c++)
        {
            if (UnityEngine.Random.value <= infantMortality)
            {
                death++;
            }
        }
        children -= death;
        return death;
    }

    public int killByPercentage(float percentage)
    {
        int deathCount = (int)((float)children * (1 - percentage));
        children -= deathCount;
        return deathCount;
    }

    public void finish()
    {
        for (int c = 0; c < children; c++)
        {
            if (UnityEngine.Random.value <= 0.5f)
            {
                female++;
            }
        }
        male = children - female;
        children = 0;
    }
}
