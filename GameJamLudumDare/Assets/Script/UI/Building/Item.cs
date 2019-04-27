using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Item : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    [Header("Game Properties")]
    public Cost Cost = new Cost(10, Cost.Type.MEN);
    public Modifer Modifer = new Modifer(20, Modifer.Type.CAPACITY);
    public int BuildingTime = 10;


    [Header("Appeareance")]
    public string Name = "Building";
    public GameObject BuildingPrefab;


    public void OnPointerClick(PointerEventData eventData)
    {
        EventManager.TriggerEvent("building_build_request", new object[] { BuildingPrefab });
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        EventManager.TriggerEvent("building_tooltip_show", new object[]{ this });
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        EventManager.TriggerEvent("building_tooltip_hide", new object[]{ });
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
