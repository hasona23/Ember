using System.Text.Json.Serialization;
using Microsoft.Xna.Framework;

namespace Ember.Tiled;

public struct Object(int gid,int x,int y,string name="",string? className=null)
{
    /// <summary>
    ///     GId is the texture id in grid
    /// </summary>
    public int GId { get; set; } = gid;

    public string? Name { get; set; } = name;
    public string? Class { get; set; } = className;
    public int X { get; set; } = x;
    public int Y { get; set; } = y;
    
    [JsonIgnore]
    public Vector2 Pos => new(X, Y);
}