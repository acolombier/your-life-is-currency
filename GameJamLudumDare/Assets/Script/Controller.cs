using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    [Header("Property")]
    public float InitialInfantMortality = 0.2f;
    public float InitialAdultMortality = 0.1f;
    public int NumberOfChildrenPool = 12;
    public int MaximumPopulation = 1000000;
    public int InitialFoodProductionAmount = 10;
    public float FoodAmount = 40;
    [Header("Event ticks")]
    [Tooltip("The number of time a child may die. This property does not impact the infant mortality")]
    public int NumberOfInfantileDeathEventPerYear = 6;
    public int NumberOfFoodProductionEventsPerYear = 6;
    public int NumberOfAdultDeathEventsPerYear = 6;
    [Tooltip("The time a year takes in second")]
    public int LenghtOfAYear = 60;

    public int StartMale = 100;
    public int StartFemale = 50;

    private AdultPopulation adultPool;
    private ChildrenPool[] mChildrenPool;
    private int mYear;
    private int mLastProceeded = -1;
    private int mMale;
    private int mFemale;

    public float InfantMortality { get; private set; }
    public int population { get {
        int children = 0;

        // Modern languages have reduce functions, C# has... Microsoft support
        foreach (var c in mChildrenPool)
            if (c != null && c.female == 0)
                children += c.children;
        return mMale + mFemale + children;
    } }

    public BuildingModifier buildingModifier;

    // Start is called before the first frame update
    void Start()
    {
        mChildrenPool = new ChildrenPool[NumberOfChildrenPool];

        mMale = StartMale;
        mFemale = StartFemale;

        adultPool = new AdultPopulation(mMale, mFemale, InitialAdultMortality);

        EventManager.StartListening("publish_modifier_update", HandleModifierUpdate);
    }

    // Update is called once per frame
    void Update()
    {
        if (mLastProceeded == (int)Time.fixedTime)
            return;

        mYear = (int)Time.fixedTime / LenghtOfAYear;

        if ((int)Time.fixedTime % (LenghtOfAYear / NumberOfInfantileDeathEventPerYear) == 0)
        {
            for (int c = 0; c < NumberOfChildrenPool; c++)
            {
                if (mChildrenPool[c] != null)
                    EventManager.TriggerEvent("children_death", new object[] { mChildrenPool[c].kill(InfantMortality / NumberOfInfantileDeathEventPerYear) });
            }
        }

        if ((int)Time.fixedTime % (LenghtOfAYear / NumberOfAdultDeathEventsPerYear) == 0)
        {
            int beforePop = adultPool.TotalPopulation;
            // this represents the number of times a year adults will die
            if (buildingModifier != null)
            {
                adultPool.MortalityRate = buildingModifier.adultDeathRate;
            }

            adultPool.ApplyMortalityRate();
            // @TODO: Maybe introduce enum to detail what kind of death occured
            EventManager.TriggerEvent("adult_death", new object[] { beforePop - adultPool.TotalPopulation });
        }

        if ((int)Time.fixedTime % LenghtOfAYear == 0)
        {
            if (mYear > 0)
            {
                mChildrenPool[(mYear - 1) % NumberOfChildrenPool].finish();

                mFemale += mChildrenPool[(mYear - 1) % NumberOfChildrenPool].female;
                mMale += mChildrenPool[(mYear - 1) % NumberOfChildrenPool].male;
            }

            mChildrenPool[mYear % NumberOfChildrenPool] = computeBirth();
            EventManager.TriggerEvent("children_birth", new object[] { mChildrenPool[mYear % NumberOfChildrenPool].children });

            int children = 0;

            // Modern languages have reduce functions, C# has... Microsoft support
            foreach (var c in mChildrenPool)
                if (c != null)
                    children += c.children;

            EventManager.TriggerEvent("population_update", new object[] { adultPool.TotalMales, adultPool.TotalFemales, children, MaximumPopulation });
        }


        // Update food amount
        if ((int)Time.fixedTime % (LenghtOfAYear / NumberOfFoodProductionEventsPerYear) == 0)
        {
            float newFoodAmount = FoodAmount;
            if (buildingModifier != null)
            {
                newFoodAmount = FoodAmount + (buildingModifier.foodProductionRate * buildingModifier.foodProductionModifier);
            }

            FoodAmount = newFoodAmount;
            EventManager.TriggerEvent("food_amount_update", new object[] { newFoodAmount });
        }
        
        if (buildingModifier != null)
        {
            // Update max occupancy based on housing capacity
            MaximumPopulation = buildingModifier.occupanyLimit;
            // Update infant mortality
            InfantMortality = buildingModifier.childDeathRate;
        }
        

        mLastProceeded = (int)Time.fixedTime;
    }

    private ChildrenPool computeBirth()
    {
        return new ChildrenPool(Mathf.Min(mFemale / NumberOfChildrenPool, MaximumPopulation - population));
    }

    private void HandleModifierUpdate(object[] args) 
    {
        // set the modifiers
        buildingModifier = (BuildingModifier)args[0];
    }
}
