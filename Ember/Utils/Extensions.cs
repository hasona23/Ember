using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Ember.Utils;

public static class Extensions
{
    public static Rectangle GetSourceRect(this Texture2D atlas, int cellSize, int gid)
    {
        return new Rectangle
        {
            X = ((gid - 1) % (atlas.Width / cellSize)) * cellSize,
            Y = ((gid - 1) / (atlas.Width / cellSize)) * cellSize,
            Width = cellSize,
            Height = cellSize,
        };
    }
}