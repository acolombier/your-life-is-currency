using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    public GameObject Title;
    public GameObject Label;
    public GameObject Content;

    private Text mTitle;
    private Text mLabel;
    private Text mContent;

    private Building mBuilding;
    private RectTransform mItemPosition;
    private float mCostRate = 1f;

    private RectTransform mRect;

    // Start is called before the first frame update
    void Start()
    {
        EventManager.StartListening("building_tooltip_show", UpdateAndShow);
        EventManager.StartListening("costrate_update", RefreshRate);
        EventManager.StartListening("building_tooltip_hide", Hide);

        mTitle = Title.GetComponent<Text>();
        mLabel = Label.GetComponent<Text>();
        mContent = Content.GetComponent<Text>();

        mRect = transform.gameObject.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void UpdateAndShow(object[] args)
    {
        mBuilding = (Building)args[0];
        mItemPosition = (RectTransform)args[1];
        Refresh();
    }
    
    void RefreshRate(object[] args)
    {
        mCostRate = (float)args[0];
        Refresh();
    }

    void Refresh()
    {
        mTitle.text = mBuilding.buildingType.ToString();
        mLabel.text = mBuilding.buildTime + "s";

        mContent.text = "";

        if (mBuilding.womenCount > 0)
            mContent.text += "<color=red>- " + mBuilding.womenCount + " women</color>\n";

        if (mBuilding.menCount > 0)
            mContent.text += "<color=red>- " + mBuilding.menCount + " men</color>\n";

        switch (mBuilding.buildingType)
        {
            case Building.BuildingType.House:
                if (mBuilding.occupanyLimit > 0f)
                    mContent.text += "<color=green>+ " + (int)(mBuilding.occupanyLimit) + " population capacity</color>\n";
                break;
            case Building.BuildingType.School:
                if (mBuilding.knowledgeRate > 0f)
                    mContent.text += "<color=green>+ " + (int)(mBuilding.knowledgeRate * 100f) + " % knowledge </color>\n";
                break;
            case Building.BuildingType.Farm:
                if (mBuilding.foodProductionModifier > 0f)
                    mContent.text += "<color=green>+ " + (int) mBuilding.foodProductionModifier + " food bonus</color>\n";
                if (mBuilding.foodProductionRate > 0f)
                    mContent.text += "<color=green>+ " + (int) (mBuilding.foodProductionRate * 100f) + " % farming efficiency</color>\n";
                break;
            case Building.BuildingType.Hospital:
                if (mBuilding.childDeathRate > 0f)
                    mContent.text += "<color=green>- " + (int)(mBuilding.childDeathRate * 100f) + " % infantile mortality</color>\n";
                if (mBuilding.adultDeathRate > 0f)
                    mContent.text += "<color=green>- " + (int)(mBuilding.adultDeathRate * 100f) + " % adult mortality</color>\n";
                break;
        }
        
        transform.gameObject.SetActive(true);

        mRect.anchoredPosition = new Vector2(mItemPosition.anchoredPosition.x, mItemPosition.anchoredPosition.y);
    }

    void Hide(object[] args)
    {
        transform.gameObject.SetActive(false);
    }
}
