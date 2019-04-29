using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Item : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    public GameObject BuildingPrefab;

    public Color Constructible;
    public Color NoConstructible;

    private Building mBuilding;
    private RectTransform mRectTransform;

    private Image mImage;


    public void OnPointerClick(PointerEventData eventData)
    {
        if (mImage.color == Constructible)
            EventManager.TriggerEvent("building_build_request", new object[] { BuildingPrefab });
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        EventManager.TriggerEvent("building_tooltip_show", new object[]{ mBuilding, mRectTransform });
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        EventManager.TriggerEvent("building_tooltip_hide", new object[]{ });
    }

    // Start is called before the first frame update
    void Start()
    {
        mImage = GetComponent<Image>();

        mBuilding = BuildingPrefab.GetComponent<Building>();
        mRectTransform = GetComponent<RectTransform>();

        EventManager.StartListening("population_update", PopulationUpdate);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void PopulationUpdate(object[] args)
    {
        if ((int)args[0] > mBuilding.menCount && (int)args[1] > mBuilding.womenCount)
        {
            mImage.color = Constructible;
        } else
        {
            mImage.color = NoConstructible;
        }
    }
}
