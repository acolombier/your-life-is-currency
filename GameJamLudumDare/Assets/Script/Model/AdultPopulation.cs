
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

    public void ApplyMortalityRate(float maleMortalityRate, float femaleMortalityRate, float maleMortalityModifier, float femaleMortalityModifier)
    {
        for (int f = 0; f < TotalFemales; f++)
        {
            if (UnityEngine.Random.value <= maleMortalityRate)
                TotalFemales--;
        }
        for (int m = 0; m < TotalMales; m++)
        {
            if (UnityEngine.Random.value <= femaleMortalityRate)
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

    public int Feed(ref float foodAmount)
    {
        int deathFemale = 0;
        for (int f = 0; f < TotalFemales; f++)
        {
            foodAmount--;
            if (foodAmount <= 0)
            {
                if (UnityEngine.Random.value <= 0.4f)
                    deathFemale++;
            }
        }
        TotalFemales -= deathFemale;
        int deathMale = 0;
        for (int m = 0; m < TotalMales; m++)
        {
            foodAmount--;
            if (foodAmount <= 0)
            {
                if (UnityEngine.Random.value <= 0.4f)
                    deathMale++; ;
            }
        }
        TotalMales -= deathMale;
        foodAmount = UnityEngine.Mathf.Max(0f, foodAmount);
        return deathFemale + deathMale;
    }
}
