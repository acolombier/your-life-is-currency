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
        buildings.Add(building);

        if (building.childDeathRate != 0.0f)
        {
            EventManager.TriggerEvent("update_childdeathrate", new object[] { building.childDeathRate });
        }
        if (building.adultDeathRate != 0.0f)
        {
            EventManager.TriggerEvent("update_adultdeathrate", new object[] { building.adultDeathRate });
        }
        if (building.mortalityRate != 0.0f)
        {
            EventManager.TriggerEvent("update_mortalityrate", new object[] { building.mortalityRate });
        }
        if (building.foodProductionRate != 0.0f)
        {
            EventManager.TriggerEvent("update_foodproductionrate", new object[] { building.foodProductionRate });
        }
        if (building.foodProductionModifier != 0.0f)
        {
            EventManager.TriggerEvent("update_foodproductionmodifer", new object[] { building.foodProductionModifier });
        }
        if (building.occupanyLimit != 0)
        {
            EventManager.TriggerEvent("update_occupanylimit", new object[] { building.occupanyLimit });
        }
    }
}
