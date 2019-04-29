using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Population : MonoBehaviour
{ 

    private Dictionary<string, RectTransform> mBars;

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

        mTransformRect = GetComponent<RectTransform>();

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

        Debug.Log(mMaleRatio + "(" + male + "), " + mFemaleRatio + "(" + female + "), " + mChildrenRatio + "(" + children + "), " + total + "(" + total + "), " + mRoomRemaining + "(" + max + ")");

        Vector2 size = mTransformRect.rect.size;

        // float offset = mBars["stats"].GetComponent<RectTransform>().sizeDelta.y

        float offset = 0;

        if (mRoomRemaining > 0)
        {
            mBars["Empty"].offsetMax = new Vector2(mBars["Empty"].offsetMax.x, -size.y * offset);
            mBars["Empty"].offsetMin = new Vector2(mBars["Empty"].offsetMax.x, size.y * (1.0f - offset - mRoomRemaining));
        }
        else
        {
            mBars["Empty"].gameObject.SetActive(false);
        }
        offset += mRoomRemaining;
        mBars["Male"].offsetMax = new Vector2(mBars["Male"].offsetMax.x, -size.y * offset + (mRoomRemaining > 0 ? size.x : 0));
        mBars["Male"].offsetMin = new Vector2(mBars["Male"].offsetMax.x, size.y * (1.0f - offset - mMaleRatio));
        //mBars["Male"].transform.Find("Value").gameObject.GetComponent<TextMeshProUGUI>().text = male.ToString();
        offset += mMaleRatio;
        mBars["Female"].offsetMax = new Vector2(mBars["Female"].offsetMax.x, -size.y * offset + size.x);
        mBars["Female"].offsetMin = new Vector2(mBars["Female"].offsetMax.x, size.y * (1.0f - offset - mFemaleRatio));
        offset += mFemaleRatio;
        if (mChildrenRatio > 0)
        {
            mBars["Children"].offsetMax = new Vector2(mBars["Children"].offsetMax.x, Mathf.Max(-size.y * offset + size.x, -size.y + 2 * size.x));
            mBars["Children"].offsetMin = new Vector2(mBars["Children"].offsetMax.x, size.y * (1.0f - offset - mChildrenRatio));
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
