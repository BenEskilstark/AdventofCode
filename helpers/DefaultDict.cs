public class DefaultDict<TKey, TValue>(TValue _default) where TKey : notnull
{
    public Dictionary<TKey, TValue> Dict { get; set; } = [];
    public TValue Default { get; set; } = _default;


    public int Count { get => Dict.Count; }
    public IEnumerable<TKey> Keys { get => Dict.Keys; }
    public IEnumerable<TValue> Values { get => Dict.Values; }


    public TValue this[TKey key]
    {
        get => Dict.GetValueOrDefault(key, Default);
        set => Dict[key] = value;
    }


    public bool ContainsKey(TKey key)
    {
        return Dict.ContainsKey(key);
    }


    public IEnumerable<TResult> Select<TResult>(
        Func<KeyValuePair<TKey, TValue>, int, TResult> f
    )
    {
        return Dict.Select(f);
    }


    public IEnumerable<KeyValuePair<TKey, TValue>> Where(
        Func<KeyValuePair<TKey, TValue>, bool> f
    )
    {
        return Dict.Where(f);
    }


    public override string ToString()
    {
        string res = "";
        Dict.Keys.ToList().ForEach(key =>
        {
            res += $"{key}: {Dict[key]}\n";
        });
        return res;
    }
}