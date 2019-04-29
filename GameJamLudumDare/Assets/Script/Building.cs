using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{

    public enum BuildingType { House, School, Farm, Hospital };

    public BuildingType buildingType;

    public float childDeathRate = 0.5f;

    public float adultDeathRate = 0.1f;

    public float foodProductionRate = 10f;

    public float knowledgeRate = 0.1f;

    public float foodProductionModifier = 1.1f;

    public int occupanyLimit = 10;

    [Tooltip("Price")]
    public int menCount = 1;

    public int womenCount = 1;

    [Tooltip("This is the time to build in Seconds")]
    public float buildTime = 10f;


}
