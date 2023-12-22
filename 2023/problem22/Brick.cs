namespace Year2023;

using Coord = (int X, int Y, int Z);

public class Brick(Coord p, Coord p2, string name)
{
    public string Name { get; } = name;
    public List<Coord> Ends { get; private set; } = [p, p2];

    public override string ToString()
    {
        return this.Name + ": " + this.Ends[0] + " ~ " + this.Ends[1];
    }

    public Brick Translate(Coord vec)
    {
        return new Brick(
            (this.Ends[0].X + vec.X, this.Ends[0].Y + vec.Y, this.Ends[0].Z + vec.Z),
            (this.Ends[1].X + vec.X, this.Ends[1].Y + vec.Y, this.Ends[1].Z + vec.Z),
            this.Name
        );
    }

    public bool Contains(Coord point)
    {
        return (
            (point.X >= this.Ends[0].X && point.X <= this.Ends[1].X &&
            point.Y >= this.Ends[0].Y && point.Y <= this.Ends[1].Y &&
            point.Z >= this.Ends[0].Z && point.Z <= this.Ends[1].Z) ||
            (point.X >= this.Ends[1].X && point.X <= this.Ends[0].X &&
            point.Y >= this.Ends[1].Y && point.Y <= this.Ends[0].Y &&
            point.Z >= this.Ends[1].Z && point.Z <= this.Ends[0].Z)
        );
    }

    public Brick Flip()
    {
        return new Brick(this.Ends[1], this.Ends[0], this.Name);
    }
    public Brick Copy()
    {
        return new Brick(this.Ends[0], this.Ends[1], this.Name);
    }

    // assumes that this brick is being dropped and so oriented with lowest-Z
    // End first and will only collide along that (x,y, lowest-Z) plane
    public List<Brick> Collides(List<Brick> bs)
    {
        int z = this.Ends[0].Z;
        // only consider bricks that cross this Z plane
        List<Brick> bricks = bs.Where(b =>
        {
            return (b.Ends[0].Z >= z && b.Ends[1].Z <= z) || (b.Ends[1].Z >= z && b.Ends[0].Z <= z);
        }).ToList();
        List<Brick> collisions = [];

        int width = Math.Abs(this.Ends[0].X - this.Ends[1].X);
        int xIndex = this.Ends[0].X < this.Ends[1].X ? 0 : 1;
        int depth = Math.Abs(this.Ends[0].Y - this.Ends[1].Y);
        int yIndex = this.Ends[0].Y < this.Ends[1].Y ? 0 : 1;

        for (int x = 0; x <= width; x++)
        {
            for (int y = 0; y <= depth; y++)
            {
                foreach (Brick brick in bricks)
                {
                    if (brick.Contains((this.Ends[xIndex].X + x, this.Ends[yIndex].Y + y, z)))
                    {
                        if (!collisions.Contains(brick))
                        {
                            collisions.Add(brick);
                            break;
                        }

                    }
                }
            }
        }

        return collisions;
    }

}