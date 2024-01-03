public class Dequeue<T>(List<T>? values = null)
{
    private LinkedList<T> Values { get; set; } = new LinkedList<T>(values ?? []);
    public int Count { get; private set; } = values != null ? values.Count : 0;

    // Stack methods:
    public void Push(T value)
    {
        this.Values.AddFirst(value);
        this.Count++;
    }
    public T Pop()
    {
        T value = this.Values.First();
        this.Values.RemoveFirst();
        this.Count--;
        return value;
    }


    // Queue methods:
    public void Enqueue(T value)
    {
        this.Values.AddLast(value);
        this.Count++;
    }


    // Custom methods:
    /**
    *   How Pop/PushMany works: X = A, B, C, D, E, F, G and Y = 1, 2, 3, 4
    *   X.PopMany(3) => [A, B, C], (D, E, F, G)
    *   Y.PushMany([A, B, C]) => A, B, C, 1, 2, 3, 4
    */
    public List<T> PopMany(int count)
    {
        List<T> res = [];
        for (int i = 0; i < count; i++)
        {
            res.Add(this.Pop());
        }
        this.Count -= count;
        return res;
    }
    public void PushMany(List<T> values)
    {
        for (int i = values.Count - 1; i >= 0; i--)
        {
            this.Push(values[i]);
        }
        this.Count += values.Count;
    }


    // Getters:
    public T First()
    {
        return this.Values.First();
    }
    public T Last()
    {
        return this.Values.Last();
    }
    public override string ToString()
    {
        return string.Join(", ", this.Values.ToList());
    }
}