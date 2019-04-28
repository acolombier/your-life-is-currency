﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    public GameObject Title;
    public GameObject Label;
    public GameObject Content;
    public float Offset;

    // Start is called before the first frame update
    void Start()
    {
        EventManager.StartListening("building_tooltip_show", UpdateAndShow);
        EventManager.StartListening("building_tooltip_hide", Hide);
    }

    // Update is called once per frame
    void UpdateAndShow(object[] args)
    {
        Item building = (Item)args[0];

        // building.GetComponent<RectTransform>().rect.position;

        Title.GetComponent<Text>().text = building.Name;
        Label.GetComponent<Text>().text = building.BuildingTime + "s";

        Content.GetComponent<Text>().text = "";

        foreach (KeyValuePair<Cost.Type, int> cost in building.Cost)
            Content.GetComponent<Text>().text += "<color=red>- " + cost.Value + " " + cost.Key.ToString().ToLower() + "</color>\n";

        foreach (KeyValuePair<Modifer.Type, int> cost in building.Modifer)
            Content.GetComponent<Text>().text += "<color=green>+ " + cost.Value + " " + cost.Key.ToString().ToLower() + "</color>\n";
        
        transform.gameObject.SetActive(true);

        RectTransform rectPosition = building.GetComponent<RectTransform>();

        transform.position = new Vector2(rectPosition.position.x, rectPosition.position.y + rectPosition.rect.size.y + Offset);
    }

    void Hide(object[] args)
    {
        transform.gameObject.SetActive(false);
    }
}