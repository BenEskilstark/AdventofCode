public class CountSet<TItem> where TItem : notnull
{
    private DefaultDict<TItem, int> Counts { get; set; }


    public int Count { get => Counts.Count; }
    public IEnumerable<TItem> Items { get => Counts.Keys; }


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
    }

    public int Add(TItem item)
    {
        Counts[item]++;
        return Counts[item];
    }
}