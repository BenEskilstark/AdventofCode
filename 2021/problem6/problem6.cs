namespace Year2021;

public class Problem6
{
    public static void Solve()
    {
        string file = "2021/problem6/input.txt";
        List<int> fish = File.ReadAllLines(file)[0]
            .Split(',').Select(int.Parse).ToList();

        // part 1
        int numDays = 80;
        for (int i = 0; i < numDays; i++)
        {
            List<int> nextFish = [];
            int numNewFish = 0;
            fish.ForEach(f =>
            {
                if (f == 0)
                {
                    nextFish.Add(6);
                    numNewFish++;
                }
                else
                {
                    nextFish.Add(f - 1);
                }
            });
            for (int j = 0; j < numNewFish; j++)
            {
                nextFish.Add(8);
            }
            fish = nextFish;
            // Console.WriteLine(string.Join(",", fish));
        }
        Console.WriteLine(fish.Count);

        // part 2
        fish = File.ReadAllLines(file)[0]
            .Split(',').Select(int.Parse).ToList();
        numDays = 256;
        List<long> days = [0, 0, 0, 0, 0, 0, 0, 0, 0];
        fish.ForEach(f => days[f]++);
        for (int d = 0; d < numDays; d++)
        {
            List<long> nextDays = [0, 0, 0, 0, 0, 0, 0, 0, 0];
            for (int i = 0; i < days.Count; i++)
            {
                long day = days[i];
                if (i == 0)
                {
                    nextDays[6] += day;
                    nextDays[8] += day;
                }
                else
                {
                    nextDays[i - 1] += day;
                }
            }
            days = nextDays;
        }
        Console.WriteLine(days.Sum());

    }
}