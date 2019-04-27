using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopulationBar : MonoBehaviour
{ 

    private Dictionary<string, GameObject> mBars;

    private float mMaleRatio;
    private float mFemaleRatio;
    private float mChildrenRatio;
    private float mFullfilmentRatio;
    private float mTotal;

    // Start is called before the first frame update
    void Start()
    {
        mBars = new Dictionary<string, GameObject>();

        foreach (Transform child in transform)
        {
            mBars.Add(child.name, child.gameObject);
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

        mFullfilmentRatio = (float)total / (float)max;

        //Debug.Log(mMaleRatio + ", " + mFemaleRatio + ", " + mChildrenRatio + ", " + total + ", " + mFullfilmentRatio);

        Vector2 size = GetComponent<RectTransform>().rect.size;
        //Debug.Log(size);

        // float offset = mBars["stats"].GetComponent<RectTransform>().sizeDelta.y

        // mBars["Empty"].GetComponent<RectTransform>().offsetMax = new Vector2(mBars["Empty"].GetComponent<RectTransform>().offsetMax.x,  -size.y + size.y * 0.6f);
        mBars["Empty"].GetComponent<RectTransform>().offsetMin = new Vector2(mBars["Empty"].GetComponent<RectTransform>().offsetMax.x,  size.y * 0.6f);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
