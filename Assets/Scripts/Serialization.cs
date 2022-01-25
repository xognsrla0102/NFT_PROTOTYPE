using UnityEngine;
using System.Collections.Generic;
public class Serialization<TKey, TValue> : ISerializationCallbackReceiver
{
    [SerializeField] private List<TKey> keys;
    [SerializeField] private List<TValue> values;

    Dictionary<TKey, TValue> target;
    public Dictionary<TKey, TValue> ToDictionary() => target;

    public Serialization(Dictionary<TKey, TValue> target) => this.target = target;

    public void OnBeforeSerialize()
    {
        keys = new List<TKey>(target.Keys);
        values = new List<TValue>(target.Values);
    }

    public void OnAfterDeserialize()
    {
        var count = Mathf.Min(keys.Count, values.Count);
        target = new Dictionary<TKey, TValue>(count);
        for (int i = 0; i < count; i++)
        {
            target.Add(keys[i], values[i]);
        }
    }
}