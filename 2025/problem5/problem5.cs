namespace Year2025;

public class Problem5
{
    public static void Solve()
    {
        string file = "2025/problem5/input.txt";
        List<string> rangesAndNums = File.ReadAllText(file).Split("\r\n\r\n").ToList();
        List<LongRange> ranges = rangesAndNums[0]
            .Split("\r\n")
            .Select(l =>
            {
                List<long> bounds = l.GetLongs();
                return new LongRange(bounds[0], bounds[1]);
            })
            .ToList();
        List<long> nums = rangesAndNums[1].GetLongs();

        nums.Where(n => ranges.Any(r => r.InRange(n)))
            .Count()
            .WriteLine("Part 1:");

        ranges.Aggregate(new List<LongRange> { }, (merged, range) =>
        {
            List<int> toKeep = [];
            for (int i = 0; i < merged.Count; i++)
            {
                List<LongRange> nextMerge = range.MergeWith(merged[i]);
                if (nextMerge.Count == 1)
                {
                    range = nextMerge[0];
                }
                else
                {
                    toKeep.Add(i);
                }
            }
            return [range, .. toKeep.Select(i => merged[i])];
        })
        .Select(r => r.Count)
        .Sum()
        .WriteLine("Part 2:");


    }

    public class LongRange(long start, long end)
    {
        public long Start { get; set; } = start;
        public long End { get; set; } = end;

        public long Count { get => End - Start + 1; }

        public bool InRange(long value)
        {
            return value >= Start && value <= End;
        }

        public List<LongRange> MergeWith(LongRange range)
        {
            LongRange a = this;
            LongRange b = range;
            if (a.Start > b.Start)
            {
                a = range;
                b = this;
            } // so we know a.Start <= b.Start

            if (a.End < b.End)
            {
                if (a.End < b.Start)
                {
                    return [this, range]; // no overlap, but don't swap them
                }
                else
                {
                    return [new(a.Start, b.End)];
                }
            }
            else // else b is entirely contained within a
            {
                return [a];
            }
        }

        public override String ToString()
        {
            return Start + " - " + End;
        }
    }
}