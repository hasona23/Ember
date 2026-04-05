using Microsoft.Xna.Framework;

namespace Ember.World;

public struct MapObject
{
    public int? Gid;
    public int X;
    public int Y;
    public int? Width;
    public int? Height;

    public MapObject(int? gid, int x, int y, int width, int height)
    {
        Gid = gid;
        X = x;
        Y = y;
        Width = width;
        Height = height;
    }

    public MapObject(int x, int y)
    {
        X = x;
        Y = y;
    }

    public MapObject(Rectangle rect)
    {
        X = rect.X;
        Y = rect.Y;
    }
}