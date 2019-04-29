using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mechanics
{
    public static void ProceedChildren(ref ChildrenPool[] pools, float nomalizedInfantileRate)
    {
        int death = 0;
        foreach (ChildrenPool pool in pools)
        {
            // @Todo: Use the updated child mortality rate format
            if (pool != null)
            {
                death += pool.kill(nomalizedInfantileRate / (float)pools.Length);
            }
        }
        EventManager.TriggerEvent("children_death", new object[] {
            death
        });
    }

    public static void ProceedFood(ref AdultPopulation adultPop, ref float foodAmount, float nomalizedFoodRate, float FoodModifier)
    {
        foodAmount += (1f + FoodModifier) * nomalizedFoodRate;
    }

    public static void ProceedAdult(ref AdultPopulation adultPop, float nomalizedMaleRate, float nomalizedFemaleRate, float nomalizedMaleModifier, float nomalizedFemaleModifier, float normalizationTick, ref float foodAmount)
    {
        int beforePopMale = adultPop.TotalMales;
        int beforePopFemale = adultPop.TotalFemales;

        adultPop.ApplyMortalityRate(nomalizedMaleRate, nomalizedFemaleRate, nomalizedMaleModifier, nomalizedFemaleModifier);

        // @TODO: Maybe introduce enum to detail what kind of death occured or pass two args ?
        EventManager.TriggerEvent("adult_death", new object[] {
            beforePopMale - adultPop.TotalMales,
            beforePopFemale - adultPop.TotalFemales
        });
        beforePopMale = adultPop.Total;

        adultPop.Feed(ref foodAmount, normalizationTick);

        if (foodAmount == 0)
        {
            EventManager.TriggerEvent("starvation", new object[] { beforePopMale - adultPop.Total });
        }
    }

    public static void ProceedNewYear(int year, ref ChildrenPool[] pools, int poolSize, ref AdultPopulation adultPop, int maximumPopulation, int childrenPopulation, float foodAmount)
    {
        if (year > 0)
        {
            childrenPopulation -= pools[(year - 1) % poolSize].children;

            pools[(year - 1) % poolSize].finish();

            adultPop.AddFemales(pools[(year - 1) % poolSize].female);
            adultPop.AddMales(pools[(year - 1) % poolSize].male);

            EventManager.TriggerEvent("gender_commit", new object[] {
                pools[(year - 1) % poolSize].female,
                pools[(year - 1) % poolSize].male
            });
        }

        int numberOfChildren = adultPop.TotalFemales / poolSize;

        if (adultPop.TotalFemales > 0 && (int)Time.fixedTime % (int)((float)poolSize / (float)(adultPop.TotalFemales % poolSize)) == 0)
        {
            numberOfChildren++;
        }

        pools[year % poolSize] = new ChildrenPool(Mathf.Min(numberOfChildren, maximumPopulation - adultPop.Total - childrenPopulation));

        if (foodAmount < adultPop.Total)
        {
            EventManager.TriggerEvent("starvation_comming", new object[] { });
        }

        EventManager.TriggerEvent("children_birth", new object[] {
            pools[year % poolSize].children
        });
    }

    public static void ProceedFood(ref float foodAmount, float foodProductionRate, float foodProductionModifier)
    {
        foodAmount = foodAmount + (foodProductionRate * foodAmount) + foodProductionModifier;
        
        EventManager.TriggerEvent("food_amount_update", new object[] {
            foodAmount
        });
    }
}
