namespace Year2023;

using Coord = (double X, double Y, double Z);
using Linear = (double A, double B, double C);

public class Problem24
{
    public static void Solve()
    {
        string file = "2023/problem24/testinput.txt";

        List<Line> lines = File.ReadAllLines(file).Select(l =>
        {
            List<double> point = [.. l.Split('@')[0].Trim().Split(',').Select(double.Parse)];
            List<double> slope = [.. l.Split('@')[1].Trim().Split(',').Select(double.Parse)];
            return new Line((point[0], point[1], point[2]), (slope[0], slope[1], slope[2]));
        }).ToList();


    }
}

public class Line(Coord point, Coord slope)
{
    public Coord Point { get; } = point;
    Coord Slope { get; } = slope;

    public override string ToString()
    {
        return this.Point + " @ " + this.Slope;
    }
}


// part 1
// public static void Part1()
// {
//     double testMin = 7;
//     double testMax = 27;
//     double testMin = 200000000000000;
//     double testMax = 400000000000000;
//     List<Coord> intercepts = [];
//     for (int i = 0; i < lines.Count - 1; i++)
//     {
//         for (int j = i + 1; j < lines.Count; j++)
//         {
//             Coord? intercept = lines[i].Intersection(lines[j]);
//             if (intercept == null) continue;
//             Coord inter = intercept ?? (testMin - 1, testMin - 1, 0);
//             if (inter.X < testMin || inter.X > testMax || inter.Y < testMin || inter.Y > testMax) continue;
//             if (lines[i].InterceptTime(lines[j]) < 0 || lines[j].InterceptTime(lines[i]) < 0)
//             {
//                 continue;
//             }
//             intercepts.Add(inter);
//         }
//     }

//     // Console.WriteLine(string.Join('\n', intercepts));
//     Console.WriteLine(intercepts.Count);
// }
// public class Line(Coord point, Coord slope)
// {
//     public Coord Point { get; } = point;
//     Coord Slope { get; } = slope;

//     public Coord? Intersection(Line line)
//     {
//         Linear i = this.GetLinearTuple();
//         Linear j = line.GetLinearTuple();

//         Linear res = (i.B * j.C - j.B * i.C, j.A * i.C - i.A * j.C, i.A * j.B - j.A * i.B);
//         if (res.C == 0) return null;

//         return (res.A / res.C, res.B / res.C);
//     }

//     public double InterceptTime(Line line)
//     {
//         double det = this.Slope.X * line.Slope.Y - this.Slope.Y * line.Slope.X;

//         (double x1, double y1) = this.Point;
//         (double x2, double y2) = line.Point;

//         return -1 * ((x1 - x2) * line.Slope.Y - (y1 - y2) * line.Slope.X) / det;
//     }

//     private Linear GetLinearTuple()
//     {
//         double slope = this.Slope.Y / this.Slope.X;
//         // Values for a, b, and c
//         double a = -slope;
//         double b = 1;
//         double c = (slope * this.Point.X) - this.Point.Y;

//         // Returning the standard form coefficients as a tuple
//         return (a, b, c);
//     }
// }