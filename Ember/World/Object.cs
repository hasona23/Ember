using System.Numerics;

namespace Ember.Tiled;

public struct Object
{
    /// <summary>
    ///     GId is the texture id in grid
    /// </summary>
    public int GId { get; set; }

    public string? Name { get; set; }
    public string? Class { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
    public Vector2 Position => new(X, Y);
}