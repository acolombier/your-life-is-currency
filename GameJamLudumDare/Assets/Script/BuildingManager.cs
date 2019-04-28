using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public static BuildingManager Instance;

    public List<Building> buildings = new List<Building>();

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }

    public void AddBuilding(Building building)
    {
        building.timeToCompleted = Time.fixedTime + building.buildTime;
        buildings.Add(building);
    }

    // Update is called once per frame
    void Update()
    {

        bool completeBuildings = false;

        if (buildings.Count == 0)
        {
            return;
        }

        BuildingModifier modifiers = new BuildingModifier();  
        foreach (Building building in buildings)
        {
            if (isComplete(building))
            {
                completeBuildings = true;
                // get all modifiers and publish event
                modifiers.childDeathRate = modifiers.childDeathRate + building.childDeathRate;
                modifiers.adultDeathRate = modifiers.adultDeathRate + building.adultDeathRate;
                modifiers.mortalityRate = modifiers.mortalityRate + building.mortalityRate;
                modifiers.foodProductionRate = modifiers.foodProductionRate + building.foodProductionRate;
                modifiers.foodProductionModifier = modifiers.foodProductionModifier + building.foodProductionModifier;
            }
        }

        if (completeBuildings)
        {
            EventManager.TriggerEvent("publish_modifier_update", new object[] { modifiers });
        }
    }

    private bool isComplete(Building building)
    {
        return Time.fixedTime >= building.timeToCompleted;
    }
}
