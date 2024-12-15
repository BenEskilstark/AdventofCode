namespace Year2024;

using Coord = (int X, int Y);

public class Problem15
{
    public static void Solve()
    {
        List<string> file = File.ReadAllText("2024/problem15/input.txt")
            .Split("\n\n").ToList();
        Grid<char> grid = Grid<char>.CharsFromString(file[0].Trim());
        Coord bot = grid.Collect((coord, val) => val == '@')[0];
        List<char> dirs = file[1].Where(c => c != '\n').ToList();

        Dictionary<char, Coord> moves = new()
        {
            ['<'] = (-1, 0),
            ['>'] = (1, 0),
            ['^'] = (0, -1),
            ['v'] = (0, 1)
        };

        foreach (char dir in dirs)
        {
            Coord move = moves[dir];
            int i = 1;
            for (; grid.At((bot.X + move.X * i, bot.Y + move.Y * i)) == 'O'; i++) { }
            if (grid.At((bot.X + move.X * i, bot.Y + move.Y * i)) == '#') continue; // move cannot occur
            grid.Set((bot.X + move.X * i, bot.Y + move.Y * i), 'O');
            grid.Set(bot, '.');
            bot = (bot.X + move.X, bot.Y + move.Y);
            grid.Set(bot, '@');
        }
        grid.Collect((c, v) => v == 'O').Sum(c => c.Y * 100 + c.X).WriteLine("Part 1:");


        string expansion = "";
        foreach (char c in file[0].Trim())
        {
            expansion += c switch
            {
                '\n' => '\n',
                '#' => "##",
                '.' => "..",
                '@' => "@.",
                'O' => "[]",
            };
        }
        grid = Grid<char>.CharsFromString(expansion);
        bot = grid.Collect((coord, val) => val == '@')[0];
        // Console.WriteLine(grid);

        foreach (char dir in dirs)
        {
            Coord move = moves[dir];
            if ("><".Contains(dir))
            {
                int i = 1;
                for (; "[]".Contains(grid.At((bot.X + move.X * i, bot.Y))); i++) { }
                if (grid.At((bot.X + move.X * i, bot.Y)) == '#') continue; // move cannot occur
                Dict<Coord, char> nextChar = new('.');
                nextChar[bot] = '.';
                for (int j = 1; j <= i; j++)
                {
                    Coord next = (bot.X + move.X * j, bot.Y);
                    Coord prev = (bot.X + move.X * (j - 1), bot.Y);
                    nextChar[next] = grid.At(prev);
                }
                bot = (bot.X + move.X, bot.Y + move.Y);
                nextChar.Keys.ToList().ForEach(pos => grid.Set(pos, nextChar[pos]));
            }
            else
            {
                Dict<Coord, char> nextChar = new('.');
                nextChar[bot] = '.';
                Stack<Coord> stack = new([bot]);
                while (stack.TryPop(out Coord next))
                {
                    Coord prev = (next.X, next.Y - move.Y);
                    if (grid.At(next) == '#')
                    {
                        nextChar = new('.');
                        break; // move is impossible
                    }

                    if (grid.At(next) == ']' && !nextChar.ContainsKey((next.X - 1, next.Y)))
                    {
                        stack.Push((next.X - 1, next.Y));
                    }
                    if (grid.At(next) == '[' && !nextChar.ContainsKey((next.X + 1, next.Y)))
                    {
                        stack.Push((next.X + 1, next.Y));
                    }
                    nextChar[next] = nextChar.ContainsKey(prev) ? grid.At(prev) : '.';
                    if (grid.At(next) != '.')
                    {
                        stack.Push((next.X, next.Y + move.Y));
                    }
                }
                if (nextChar.Keys.Any()) bot = (bot.X + move.X, bot.Y + move.Y);
                nextChar.Keys.ToList().ForEach(pos => grid.Set(pos, nextChar[pos]));
            }
            // Console.WriteLine(dir);
            // Console.WriteLine(grid);
        }

        // Console.WriteLine(grid);
        grid.Collect((c, v) => v == '[').Sum(c => c.Y * 100 + c.X).WriteLine("Part 2:");
    }

}