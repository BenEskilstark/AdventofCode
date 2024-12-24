namespace Year2024;

using KeyPair = (char Source, char Dest);
using Coord = (int X, int Y);

public class Problem21
{
    public static void Solve()
    {
        List<string> goals = [.. File.ReadAllLines("2024/problem21/testinput.txt")];

        Grid<char> keypad = new([
            ['7', '8', '9'],
            ['4', '5', '6'],
            ['1', '2', '3'],
            [' ', '0', 'A']
        ], ' ');

        Dict<KeyPair, List<char>> paths = new([], () => []);
        List<char> keys = [.. keypad.Collect((p, v) => v != ' ').Select(keypad.At)];
        for (int i = 0; i < keys.Count - 1; i++)
        {
            for (int j = i + 1; j < keys.Count; j++)
            {
                paths[(keys[i], keys[j])] = ShortestPath(keypad, keys[i], keys[j]);
            }
        }

        Grid<char> botpad = new([
            [' ', '^', 'A'],
            ['<', 'v', '>']
        ], ' ');
    }

    public static List<char> ShortestPath(Grid<char> grid, char source, char dest)
    {
        Coord start = grid.Collect((pos, val) => val == source)[0];
        Coord end = grid.Collect((pos, val) => val == dest)[0];
        Stack < List
    }

}