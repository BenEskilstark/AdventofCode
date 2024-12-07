
// Convenient set for telling whether an element occurs in the set.
// So instead of having to do `if (myHashSet.Contains(3))` you can do
// `if (mySet[3])`
public class Set<TItem> where TItem : notnull
{
    // private Dict<TItem, bool> BoolSet { get; set; }
    private HashSet<TItem> BackingSet { get; set; }

    public int Count { get => BackingSet.Count; }
    public List<TItem> Items { get => BackingSet.ToList(); }


    public Set()
    {
        BackingSet = [];
    }
    public Set(List<TItem> items)
    {
        BackingSet = new(items);
    }
    public Set(HashSet<TItem> set)
    {
        BackingSet = new(set);
    }


    public bool this[TItem item]
    {
        get => BackingSet.Contains(item);
    }

    public bool Add(TItem item)
    {
        return BackingSet.Add(item);
    }
    public bool Remove(TItem item)
    {
        return BackingSet.Remove(item);
    }


    public List<TItem> ToList()
    {
        return Items;
    }

    // Escape hatch into the regular HashSet

    public Set<TItem> WriteLine()
    {
        Console.WriteLine(string.Join(", ", Items));
        return this;
    }
}