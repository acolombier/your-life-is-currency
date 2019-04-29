
using System;

public class AdultPopulation
{
    
    public int Total { get { return TotalMales + TotalFemales; } }
    public int TotalMales;
    public int TotalFemales;

    public AdultPopulation(int initialMalePopulation, int initialFemalePopulation, int maximumPopulation)
    {
        TotalMales = initialMalePopulation;
        TotalFemales = initialFemalePopulation;

        EventManager.TriggerEvent("population_update", new object[] {
            TotalMales, TotalFemales, 0, maximumPopulation
        });
    }

    public void AddMales(int amount)
    {
        TotalMales += amount;
    }

    public void AddFemales(int amount)
    {
        TotalFemales += amount;
    }

    public void ApplyMortalityRate(float mortalityRate)
    {
        for (int f = 0; f < TotalFemales; f++)
        {
            if (UnityEngine.Random.value <= mortalityRate)
                TotalFemales--;
        }
        for (int m = 0; m < TotalMales; m++)
        {
            if (UnityEngine.Random.value <= mortalityRate)
                TotalMales--;
        }
    }

    public void KillMales(int menCount)
    {
        TotalMales -= menCount;
    }

    public void KillFemales(int womenCount)
    {
        TotalFemales -= womenCount;
    }

    public void Feed(ref float foodAmount, float normalizationTick)
    {
        for (int f = 0; f < TotalFemales; f++)
        {
            foodAmount -= normalizationTick;
            if (foodAmount <= 0)
            {
                if (UnityEngine.Random.value <= 0.4f)
                    TotalFemales--;
            }
        }
        for (int m = 0; m < TotalMales; m++)
        {
            foodAmount -= normalizationTick;
            if (foodAmount <= 0)
            {
                if (UnityEngine.Random.value <= 0.4f)
                    TotalMales--;
            }
        }
        foodAmount = UnityEngine.Mathf.Max(0f, foodAmount);
    }
}
