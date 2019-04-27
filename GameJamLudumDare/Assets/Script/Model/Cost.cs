using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Cost : IEnumerable<KeyValuePair<Cost.Type, int>>
{
    private Dictionary<Type, int> Costs;

    public Cost(int defaultValue = 0, Type defaultType = Type.MEN)
    {
        Costs = new Dictionary<Type, int>();

        if (defaultValue > 0)
            Costs[defaultType] = defaultValue;
    }

    public IEnumerator<KeyValuePair<Type, int>> GetEnumerator()
    {
        return Costs.GetEnumerator();
    }

    void Add(int defaultValue, Type defaultType)
    {
        Costs[defaultType] = defaultValue;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return Costs.GetEnumerator();
    }

    public enum Type
    {
        MEN,
        WOMEN
    }
}