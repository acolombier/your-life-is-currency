using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Population : MonoBehaviour
{ 

    private Dictionary<string, RectTransform> mBars;
    private Dictionary<string, TextMeshProUGUI> mValues;

    private float mMaleRatio;
    private float mFemaleRatio;
    private float mChildrenRatio;
    private float mRoomRemaining;
    private float mTotal;

    private RectTransform mTransformRect;

    // Start is called before the first frame update
    void Start()
    {
        mBars = new Dictionary<string, RectTransform>();
        mValues = new Dictionary<string, TextMeshProUGUI>();

        mTransformRect = transform.Find("Display").GetComponent<RectTransform>();

        foreach (Transform child in transform.Find("Display"))
        {
            mBars.Add(child.name, child.gameObject.GetComponent<RectTransform>());
            if (child.name == "Empty")
                mValues.Add(child.name, transform.Find("Value").gameObject.GetComponent<TextMeshProUGUI>());
            else
                mValues.Add(child.name, child.gameObject.transform.Find("Value").gameObject.GetComponent<TextMeshProUGUI>());
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

        //Debug.Log(mMaleRatio + "(" + male + "), " + mFemaleRatio + "(" + female + "), " + mChildrenRatio + "(" + children + "), " + total + "(" + total + "), " + mRoomRemaining + "(" + max + ")");

        Vector2 size = mTransformRect.rect.size;

        // float offset = mBars["stats"].GetComponent<RectTransform>().sizeDelta.y

        float offset = 0;

        mBars["Empty"].offsetMax = new Vector2(mBars["Empty"].offsetMax.x, 0);
        mBars["Empty"].offsetMin = new Vector2(mBars["Empty"].offsetMax.x, 0);
        mValues["Empty"].text = "<b>Max</b>\n" + Parser.Unit(max);

        if (mRoomRemaining > 0)
        {
            mBars["Empty"].gameObject.SetActive(true);
            offset += mRoomRemaining;
        }
        else
        {
            mBars["Empty"].gameObject.SetActive(false);
        }

        if (mMaleRatio > 0f)
        {
            mBars["Male"].offsetMax = new Vector2(mBars["Male"].offsetMax.x, Mathf.Clamp(-size.y * offset + (mRoomRemaining > 0f ? size.x : 0), -size.y, 0));
            mBars["Male"].offsetMin = new Vector2(mBars["Male"].offsetMax.x, size.y * (1.0f - offset - mMaleRatio));
            mValues["Male"].text = Parser.Unit(male);
            mBars["Male"].gameObject.SetActive(true);
            offset += mMaleRatio;
        }
        else
        {
            mBars["Male"].gameObject.SetActive(false);
        }

        if (mFemaleRatio > 0f)
        {
            mBars["Female"].offsetMax = new Vector2(mBars["Female"].offsetMax.x, Mathf.Clamp(-size.y * offset + ((mRoomRemaining + mMaleRatio) > 0f ? size.x : 0), -size.y, 0));
            mBars["Female"].offsetMin = new Vector2(mBars["Female"].offsetMax.x, size.y * (1.0f - offset - mFemaleRatio));
            mValues["Female"].text = Parser.Unit(female);
            mBars["Female"].gameObject.SetActive(true);
            offset += mFemaleRatio;
        }
        else
        {
            mBars["Female"].gameObject.SetActive(false);
        }

        if (mChildrenRatio > 0f)
        {
            mBars["Children"].offsetMax = new Vector2(mBars["Children"].offsetMax.x, Mathf.Clamp(-size.y * offset + size.x, -size.y + 2 * size.x, 0));
            mBars["Children"].offsetMin = new Vector2(mBars["Children"].offsetMax.x, size.y * (1.0f - offset - mChildrenRatio));
            mValues["Children"].text = Parser.Unit(children);
            mBars["Children"].gameObject.SetActive(true);
        } else
        {
            mBars["Children"].gameObject.SetActive(false);
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
