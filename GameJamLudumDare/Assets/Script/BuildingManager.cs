using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public static BuildingManager Instance;

    public List<Building> buildings = new List<Building>();

    public enum BuildingType { House, School, Farm, Hospital };

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
        if (building.maleDeathRate != 0.0f)
        {
            EventManager.TriggerEvent("update_maledeathrate", new object[] { building.maleDeathRate });
        }
        if (building.femaleDeathRate != 0.0f)
        {
            EventManager.TriggerEvent("update_femaledeathrate", new object[] { building.femaleDeathRate });
        }
        if (building.foodProductionRate != 0.0f)
        {
            EventManager.TriggerEvent("update_foodproductionrate", new object[] { building.foodProductionRate });
        }
        if (building.foodProductionModifier != 0.0f)
        {
            EventManager.TriggerEvent("update_foodproductionmodifier", new object[] { building.foodProductionModifier });
        }
        if (building.knowledgeRate != 0.0f)
        {
            EventManager.TriggerEvent("update_knowledgerate", new object[] { building.knowledgeRate });
        }
        if (building.occupanyLimit != 0)
        {
            EventManager.TriggerEvent("update_occupanylimit", new object[] { building.occupanyLimit });
        }
    }

    public int GetBuildingCount(BuildingType type)
    {
        // type is defined in two places now, building and in here, this needs to be sorted out
        int count = 0;
        foreach(var building in buildings)
        {
            if (building.buildingType.Equals(type))
            {
                count += 1;
            }
        }

        return count;
    }     
}
