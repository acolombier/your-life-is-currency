
public class AdultPopulation
{
    
    public int TotalPopulation { get; private set; }
    public int TotalMales { get; private set; }
    public int TotalFemales { get; private set; }
    public float MortalityRate { get; set; }

    public AdultPopulation(int initialMalePopulation, int initialFemalePopulation, float initialMortalityRate)
    {
        TotalMales = initialMalePopulation;
        TotalFemales = initialFemalePopulation;
        TotalPopulation = initialMalePopulation + initialFemalePopulation;
        MortalityRate = initialMortalityRate;
    }

    public void AddMales(int amount)
    {
        TotalMales += amount;
        TotalPopulation += amount;
    }

    public void AddFemales(int amount)
    {
        TotalFemales += amount;
        TotalPopulation += amount;
    }

    public void ApplyMortalityRate()
    {
        TotalMales = (int)((float)TotalMales * (1.0f - MortalityRate));
        TotalFemales = (int)((float)TotalFemales * (1.0f - MortalityRate));
        TotalPopulation = TotalMales + TotalFemales;
    }
}
