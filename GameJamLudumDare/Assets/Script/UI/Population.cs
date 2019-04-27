﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Population : MonoBehaviour
{ 

    private Dictionary<string, RectTransform> mBars;

    private float mMaleRatio;
    private float mFemaleRatio;
    private float mChildrenRatio;
    private float mRoomRemaining;
    private float mTotal;

    // Start is called before the first frame update
    void Start()
    {
        mBars = new Dictionary<string, RectTransform>();

        foreach (Transform child in transform)
        {
            mBars.Add(child.name, child.gameObject.GetComponent<RectTransform>());
        }

        EventManager.StartListening("population_update", UpdatePopulation);
    }

    void UpdatePopulation(object[] args)
    {
        int male = (int)args[0], female = (int)args[1], children = (int)args[2], max = (int)args[3];

        int total = male + female + children;
        mMaleRatio = (float)male / (float)total;
        mFemaleRatio = (float)female / (float)total;
        mChildrenRatio = (float)children / (float)total;

        mRoomRemaining = 1.0f - (float)total / (float)max;
        mRoomRemaining *= 0.4f;

        mMaleRatio *= 1.0f - mRoomRemaining;
        mFemaleRatio *= 1.0f - mRoomRemaining;
        mChildrenRatio *= 1.0f - mRoomRemaining;

        Debug.Log(mMaleRatio + ", " + mFemaleRatio + ", " + mChildrenRatio + ", " + total + ", " + mRoomRemaining);

        Vector2 size = GetComponent<RectTransform>().rect.size;

        // float offset = mBars["stats"].GetComponent<RectTransform>().sizeDelta.y

        float offset = 0f;
        Debug.Log(offset);

        mBars["Empty"].offsetMax = new Vector2(mBars["Empty"].offsetMax.x, -size.y * offset);
        mBars["Empty"].offsetMin = new Vector2(mBars["Empty"].offsetMax.x, size.y * (1.0f - offset - mRoomRemaining));
        offset += mRoomRemaining;
        mBars["Male"].offsetMax = new Vector2(mBars["Male"].offsetMax.x, -size.y * offset);
        mBars["Male"].offsetMin = new Vector2(mBars["Male"].offsetMax.x, size.y * (1.0f - offset - mMaleRatio));
        offset += mMaleRatio;
        mBars["Female"].offsetMax = new Vector2(mBars["Female"].offsetMax.x, -size.y * offset);
        mBars["Female"].offsetMin = new Vector2(mBars["Female"].offsetMax.x, size.y * (1.0f - offset - mFemaleRatio));
        offset += mFemaleRatio;
        mBars["Children"].offsetMax = new Vector2(mBars["Children"].offsetMax.x, -size.y * offset);
        mBars["Children"].offsetMin = new Vector2(mBars["Children"].offsetMax.x, size.y * (1.0f - offset - mChildrenRatio));
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
