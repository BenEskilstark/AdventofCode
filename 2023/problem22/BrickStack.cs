namespace Year2023;
using Coord = (int X, int Y, int Z);
public class BrickStack(List<Brick> bricks)
{
    public List<Brick> Bricks { get; private set; } = bricks;

    public override string ToString()
    {
        return string.Join("\n", this.Bricks);
    }


    public string Perspective(int axis)
    { // axis is which dimension we're viewing FROM, so 0 means we are viewing 
      // from the x axis and so will see y across and z down
        List<string> view = [];
        Coord bounds = this.GetBounds();
        int horizontalWidth = bounds.Y;
        int depth = bounds.X;
        if (axis == 1)
        {
            horizontalWidth = bounds.X;
            depth = bounds.Y;
        }

        for (int z = bounds.Z; z > 0; z--)
        {
            for (int i = 0; i <= horizontalWidth; i++)
            {
                bool depthContainsBrick = false;
                for (int j = 0; j <= depth; j++)
                {
                    string inName = InsideABrick((axis == 0 ? j : i, axis == 1 ? j : i, z));
                    if (inName != "")
                    {
                        if (depthContainsBrick)
                        {
                            if (view[^1] != inName) view[^1] = "?";
                            break;
                        }
                        depthContainsBrick = true;
                        // view.Add("□");
                        view.Add(inName);
                    }
                }

                if (!depthContainsBrick) view.Add(".");
            }
            view.Add("\n");
        }

        return string.Join("", view);
    }


    public string InsideABrick(Coord point)
    {
        foreach (Brick brick in this.Bricks)
        {
            if (brick.Contains(point)) return brick.Name;
        }
        return "";
    }


    public Coord GetBounds()
    {
        int xBound = 0;
        int yBound = 0;
        int zBound = 0;
        foreach (Brick brick in this.Bricks)
        {
            if (brick.Ends[0].X > xBound) xBound = brick.Ends[0].X;
            if (brick.Ends[1].X > xBound) xBound = brick.Ends[1].X;
            if (brick.Ends[0].Y > yBound) yBound = brick.Ends[0].Y;
            if (brick.Ends[1].Y > yBound) yBound = brick.Ends[1].Y;
            if (brick.Ends[0].Z > zBound) zBound = brick.Ends[0].Z;
            if (brick.Ends[1].Z > zBound) zBound = brick.Ends[1].Z;
        }
        return (xBound, yBound, zBound);
    }

    public (List<Brick>, Dictionary<string, List<string>>) Drop()
    {
        List<Brick> dropped = [];
        Dictionary<string, List<string>> supports = [];
        this.Bricks.OrderBy(b => Math.Min(b.Ends[0].Z, b.Ends[1].Z)).ToList().ForEach(br =>
        {
            // orient the brick on the z axis:
            Brick brick = br.Ends[0].Z <= br.Ends[1].Z ? br.Copy() : br.Flip();
            List<Brick> collisions = [];
            while (collisions.Count == 0 && brick.Ends[0].Z > 1)
            {
                brick = brick.Translate((0, 0, -1));
                collisions = brick.Collides(dropped);
            }
            supports[brick.Name] = collisions.Select(b => b.Name).ToList();
            if (collisions.Count > 0) brick = brick.Translate((0, 0, 1));
            dropped.Add(brick);

        });

        return (dropped, supports);
    }
}