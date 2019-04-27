using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Modifer : IEnumerable<KeyValuePair<Modifer.Type, int>>
{
    private Dictionary<Type, int> Modifiers;

    public Modifer(int defaultValue = 0, Type defaultType = Type.CAPACITY)
    {
        Modifiers = new Dictionary<Type, int>();

        if (defaultValue > 0)
            Modifiers[defaultType] = defaultValue;
    }

    void Add(int defaultValue, Type defaultType)
    {
        Modifiers[defaultType] = defaultValue;
    }

    public IEnumerator<KeyValuePair<Type, int>> GetEnumerator()
    {
        return Modifiers.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return Modifiers.GetEnumerator();
    }

    public enum Type
    {
        CAPACITY
    }
}