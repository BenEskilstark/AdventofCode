
// A more convenient version of Dictionary because instead of throwing 
// when you try to access or set missing keys, instead a given default is used.
// Just make sure to provide a newDefault function if your default value is 
// a reference type like a list or set
public class Dict<TKey, TValue>(TValue _default, Func<TValue>? _newDefault = null
) where TKey : notnull
{
    public Dictionary<TKey, TValue> InnerDict { get; set; } = [];
    // NOTE: when the default value is something like a List or HashSet, then you have
    // to provide a NewDefault function argument to the constructor in order to 
    // prevent all keys from sharing a reference to the same default value
    public TValue Default { get => NewDefault != null ? NewDefault() : _default; }
    public Func<TValue>? NewDefault { get; } = _newDefault;

    public int Count { get => InnerDict.Count; }
    public IEnumerable<TKey> Keys { get => InnerDict.Keys; }
    public IEnumerable<TValue> Values { get => InnerDict.Values; }


    public TValue this[TKey key]
    {
        get
        {
            if (InnerDict.TryGetValue(key, out TValue? value))
            {
                return value;
            }
            else
            {
                InnerDict.Add(key, Default);
                return InnerDict[key];
            }
        }
        set => InnerDict[key] = value;
    }


    public bool ContainsKey(TKey key)
    {
        return InnerDict.ContainsKey(key);
    }


    public bool Remove(TKey key)
    {
        return InnerDict.Remove(key);
    }


    public IEnumerable<TResult> Select<TResult>(
        Func<KeyValuePair<TKey, TValue>, int, TResult> f
    )
    {
        return InnerDict.Select(f);
    }


    public IEnumerable<KeyValuePair<TKey, TValue>> Where(
        Func<KeyValuePair<TKey, TValue>, bool> f
    )
    {
        return InnerDict.Where(f);
    }


    public override string ToString()
    {
        string res = "";
        InnerDict.Keys.ToList().ForEach(key =>
        {
            res += $"{key}: {InnerDict[key]}\n";
        });
        return res;
    }
}