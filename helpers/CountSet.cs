
// A HashSet that keeps track of the number of times a given
// element was added to the set.
public class CountSet<TItem> where TItem : notnull
{
    private Dict<TItem, int> Counts { get; set; }


    public int Count { get => Items.Count(); }
    public IEnumerable<TItem> Items { get => Counts.Keys.Where(k => this[k] > 0); }


    public CountSet()
    {
        Counts = new(0);
    }
    public CountSet(List<TItem> items)
    {
        Counts = new(0);
        items.ForEach(item =>
        {
            Counts[item]++;
        });
    }


    public int this[TItem item]
    {
        get => Counts[item];
        set => Counts[item] = value;
    }

    public int Add(TItem item)
    {
        Counts[item]++;
        return Counts[item];
    }
    public int AddMany(TItem item, int count)
    {
        Counts[item] += count;
        return Counts[item];
    }


    public TItem Max()
    {
        int bestVal = 0;
        TItem bestKey = default;
        Items.ForEach(i =>
        {
            if (Counts[i] > bestVal)
            {
                bestVal = Counts[i];
                bestKey = i;
            }
        });
        return bestKey!;
    }

    public TItem Min()
    {
        int bestVal = int.MaxValue;
        TItem bestKey = default;
        Items.ForEach(i =>
        {
            if (Counts[i] < bestVal)
            {
                bestVal = Counts[i];
                bestKey = i;
            }
        });
        return bestKey!;
    }


    public HashSet<TItem> ToHashSet()
    {
        HashSet<TItem> set = [];
        Items.ToList().ForEach(item => set.Add(item));
        return set;
    }
}