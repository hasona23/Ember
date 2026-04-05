using Microsoft.Xna.Framework;

namespace Ember.World;

public struct Tile(int gid,Rectangle bounds)
{
    public int Gid = gid;
    public Rectangle Bounds = bounds;

    public static Tile Empty { get; private set; } = new Tile(-1, Rectangle.Empty);
}