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

    public void AddBuilding(Building buildingController)
    {
        buildings.Add(buildingController);
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Building building in buildings)
        {

        }
    }
}
