using Microsoft.Xna.Framework;

namespace Ember.Tiled;

public struct Tile(int gId, int x, int y)
{
    public int X { get; set; } = x;
    public int Y { get; set; } = y;
    public int GId { get; set; } = gId;

    public Rectangle Bounds(int tileSize)
    {
        return new Rectangle(X, Y, tileSize, tileSize);
    }

    public Vector2 Position => new(X, Y);
}