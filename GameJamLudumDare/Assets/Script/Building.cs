using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{

    public enum BuildingType { House };

    public BuildingType buildingType;

    public float childDeathRate = 0.5f;

    public float adultDeathRate = 0.1f;

    public float mortalityRate = 0.1f;

    public float foodProductionRate = 0.1f;

    public float foodProductionModifier = 0.1f;

    public float occupanyLimit = 0.1f;

    public float manCount = 1f;

    public float womenCoun = 1f;

    public float childCount = 1f;




}
