using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    [Header("Initial rate")]
    public float InfantMortalityRate = 0.2f;
    public float AdultMortalityRate = 0.1f;
    public float FoodProductionRate = 0.0f;
    public float CostRate = 1.0f;
    [Header("Initial amount")]
    public float FoodAmount = 40;
    public int StartMale = 100;
    public int StartFemale = 50;
    [Header("Initial modifier")]
    public float FoodProductionModifier = 0.0f;
    [Header("Event ticks")]
    [Tooltip("The number of time the rate is applied. This property does not impact the rate itself")]
    public int InfantileDeathTick = 6;
    public int FoodProductionTick = 6;
    public int AdultDeathTick = 6;

    [Header("Global settings")]
    [Tooltip("The time a year takes in second")]
    public int LengthOfAYear = 60;
    public int MaximumPopulation = 1000000;
    public int NumberOfChildrenPool = 12;
    public int SacrificeCount = 0;

    private AdultPopulation mAdultPool;
    private ChildrenPool[] mChildrenPool;
    private bool mIsOver = false;
    private int mYear;
    private int mLastProceeded = -1;

    public float InfantMortality { get; private set; }

    public int ChildrenPopulation { get {
        int children = 0;

        // Modern languages have reduce functions, C# has... Microsoft support
        foreach (var c in mChildrenPool)
            if (c != null && c.female == 0)
                children += c.children;
        return children;
    } }

    public BuildingModifier buildingModifier;

    // Start is called before the first frame update
    void Start()
    {
        mChildrenPool = new ChildrenPool[NumberOfChildrenPool];

        mAdultPool = new AdultPopulation(StartMale, StartFemale, MaximumPopulation);
        EventManager.StartListening("update_childdeathrate", HandleChildDeathRateUpdate);
        EventManager.StartListening("update_adultdeathrate", HandleAdultDeathRateUpdate);
        EventManager.StartListening("update_foodproductionrate", HandleFoodProductionRateUpdate);
        EventManager.StartListening("update_foodproductionmodifier", HandleFoodProductionModifierUpdate);
        EventManager.StartListening("update_occupanylimit", HandleOccupanyLimitUpdate);
        EventManager.StartListening("update_knowledgerate", HandleKnowledgeRateUpdate);
    }

    // Update is called once per frame
    void Update()
    {
        if (mLastProceeded == (int)Time.fixedTime || mIsOver)
            return;

        if (mAdultPool.TotalMales == 0 || mAdultPool.TotalFemales == 0)
        {
            GameOver();
            return;
        }


        mYear = (int)Time.fixedTime / LengthOfAYear;
        mLastProceeded = (int)Time.fixedTime;

        // Proceed ticked updated
        if (ShouldTick(InfantileDeathTick))
        {
            Mechanics.ProceedChildren(ref mChildrenPool, NormalizedRate(InfantMortalityRate, InfantileDeathTick));
        }

        if (ShouldTick(AdultDeathTick))
        {
            
            Mechanics.ProceedAdult(ref mAdultPool, NormalizedRate(AdultMortalityRate, AdultDeathTick));
        }

        EventManager.TriggerEvent("population_update", new object[] {
            mAdultPool.TotalMales, mAdultPool.TotalFemales, ChildrenPopulation, MaximumPopulation
        });


        // Update food amount
        if (ShouldTick(FoodProductionTick))
        {
            Mechanics.ProceedFood(ref FoodAmount, NormalizedRate(FoodProductionRate, FoodProductionTick), FoodProductionModifier);
        }

        // Proceed game loop
        if (IsNewYear())
        {
            Mechanics.ProceedNewYear(mYear, ref mChildrenPool, NumberOfChildrenPool, ref mAdultPool, MaximumPopulation, ChildrenPopulation);


            if (mChildrenPool[mYear % NumberOfChildrenPool].children > 0)
            {
                EventManager.TriggerEvent("population_update", new object[] {
                    mAdultPool.TotalMales, mAdultPool.TotalFemales, ChildrenPopulation, MaximumPopulation
                });
            }
        }
    }

    public void Buy(Building building)
    {
        mAdultPool.KillMales(building.menCount);
        mAdultPool.KillFemales(building.womenCount);
        SacrificeCount += building.menCount + building.womenCount;
        EventManager.TriggerEvent("update_sacrifice_count", new object[] { SacrificeCount } );
        EventManager.TriggerEvent("population_update", new object[] {
            mAdultPool.TotalMales, mAdultPool.TotalFemales, ChildrenPopulation, MaximumPopulation
        });
    }

    public bool CanBuy(Building building)
    {
        return mAdultPool.TotalMales >= building.menCount && mAdultPool.TotalFemales >= building.womenCount;
    }

    private void GameOver()
    {
        mIsOver = true;
        EventManager.TriggerEvent("game_over", new object[] { });
    }

    private bool IsNewYear()
    {
        return (int)Time.fixedTime % LengthOfAYear == 0;
    }

    private float NormalizedRate(float rate, int tick)
    {
        return rate / (float)tick / (float)LengthOfAYear;
    }

    private bool ShouldTick(int tick)
    {
        return (int)Time.fixedTime % (int)Mathf.Max(1.0f, ((float)LengthOfAYear / (float)tick)) == 0;
    }

    private void HandleChildDeathRateUpdate(object[] args)
    {
        // takes a percentage decrease and applies to the child death rate
        InfantMortalityRate = InfantMortalityRate * (1.0f - (float)args[0]);
    }

    private void HandleAdultDeathRateUpdate(object[] args)
    {
        AdultMortalityRate = AdultMortalityRate * (1.0f - (float)args[0]);
    }

    private void HandleKnowledgeRateUpdate(object[] args)
    {
        CostRate *= 1.0f - (float)args[0];
        EventManager.TriggerEvent("costrate_update", new object[] { CostRate });
    }

    private void HandleFoodProductionRateUpdate(object[] args)
    {
        FoodProductionRate = FoodProductionRate * (1.0f + (float)args[0]);
    }
    private void HandleFoodProductionModifierUpdate(object[] args)
    {
        FoodProductionModifier = FoodProductionModifier + (int)args[0];
    }
    private void HandleOccupanyLimitUpdate(object[] args)
    {
        MaximumPopulation = MaximumPopulation + (int)args[0];
    }
}
