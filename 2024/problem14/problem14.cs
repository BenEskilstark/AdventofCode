namespace Year2024;

using Coord = (int X, int Y);
public record Bot(Coord Position, Coord Velocity);

public class Problem14
{
    public static void Solve()
    {
        string file = "2024/problem14/input.txt";
        int width = 101;
        int height = 103;
        // int width = 11;
        // int height = 7;

        List<Bot> bots = [];
        File.ReadLines(file).Select(l => l.Split(' ')).ToList()
            .ForEach(parts =>
            {
                List<int> velocity = [.. parts[1].Split('=')[1].Split(',').Select(int.Parse)];
                bots.Add(new Bot(
                    (parts[0].GetNums()[0], parts[0].GetNums()[1]),
                    (velocity[0], velocity[1])
                ));
            });

        int numIterations = 100;
        List<Bot> endBots = bots.Select(bot =>
        {
            int nextX = (bot.Position.X + bot.Velocity.X * numIterations) % width;
            if (nextX < 0) nextX += width;
            int nextY = (bot.Position.Y + bot.Velocity.Y * numIterations) % height;
            if (nextY < 0) nextY += height;
            return new Bot((nextX, nextY), bot.Velocity);
        }).ToList();

        List<(Coord, Coord)> quadrants = [
            ((0, 0), (width / 2, height / 2)),
            ((width / 2 + 1, 0), (width, height / 2)),
            ((0, height / 2 + 1), (width / 2, height)),
            ((width / 2 + 1, height / 2 + 1), (width, height))
        ];

        int product = 1;
        foreach (var quadrant in quadrants)
        {
            int numInQuadrant = 0;
            foreach (Bot bot in endBots)
            {
                if (
                    bot.Position.X >= quadrant.Item1.X && bot.Position.X < quadrant.Item2.X &&
                    bot.Position.Y >= quadrant.Item1.Y && bot.Position.Y < quadrant.Item2.Y
                )
                {
                    numInQuadrant++;
                }
            }
            product *= numInQuadrant;
        }
        product.WriteLine("Part 1:");


        for (int i = 0; i < numIterations + 12000; i++)
        {
            List<Bot> nextBots = [];
            foreach (Bot bot in bots)
            {
                int nextX = (bot.Position.X + bot.Velocity.X) % width;
                if (nextX < 0) nextX += width;
                int nextY = (bot.Position.Y + bot.Velocity.Y) % height;
                if (nextY < 0) nextY += height;
                nextBots.Add(new Bot((nextX, nextY), bot.Velocity));
            }
            bots = nextBots;
            int numInQuadrant = 0;
            for (int j = 0; j < 2; j++)
            {
                var quadrant = quadrants[j];
                foreach (Bot bot in bots)
                {
                    if (
                        bot.Position.X >= quadrant.Item1.X && bot.Position.X < quadrant.Item2.X &&
                        bot.Position.Y >= quadrant.Item1.Y && bot.Position.Y < quadrant.Item2.Y
                    )
                    {
                        numInQuadrant++;
                    }
                }
            }
            if (numInQuadrant > 150) continue;
            Console.WriteLine("\n---------------------------------------------------------------------------------------------\n" + (i + 1) + ":");
            for (int y = 0; y < height; y++)
            {
                string line = "";
                for (int x = 0; x < width; x++)
                {
                    if (bots.Any(bot => bot.Position.X == x && bot.Position.Y == y))
                    {
                        line += "#";
                    }
                    else
                    {
                        line += " ";
                    }
                }
                Console.WriteLine(line);
            }
        }
    }

}