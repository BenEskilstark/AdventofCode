
// Convenient set for telling whether an element occurs in the set.
// So instead of having to do `if (myHashSet.Contains(3))` you can do
// `if (mySet[3])`
public class Set<TItem> where TItem : notnull
{
    private Dict<TItem, bool> BoolSet { get; set; }


    public int Count { get => BoolSet.Count; }
    public IEnumerable<TItem> Items { get => BoolSet.Keys; }


    public Set()
    {
        BoolSet = new(false);
    }
    public Set(List<TItem> items)
    {
        BoolSet = new(false);
        items.ForEach(item => BoolSet[item] = true);
    }
    public Set(HashSet<TItem> set)
    {
        BoolSet = new(false);
        set.ToList().ForEach(item => BoolSet[item] = true);
    }


    public bool this[TItem item]
    {
        get => BoolSet[item];
    }

    public bool Add(TItem item)
    {
        BoolSet[item] = true;
        return true;
    }
    public bool Remove(TItem item)
    {
        return BoolSet.Remove(item);
    }


    public List<TItem> ToList()
    {
        return Items.ToList();
    }

    // Escape hatch into the regular HashSet
    public HashSet<TItem> ToHashSet()
    {
        HashSet<TItem> set = [];
        Items.ToList().ForEach(item => set.Add(item));
        return set;
    }
}