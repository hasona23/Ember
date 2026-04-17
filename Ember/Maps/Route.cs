using Microsoft.Xna.Framework;
using System.Text;

namespace Ember.Maps;

public struct Route(string name,params Point[] points)
{
    public string Name = name;
    public Point[] Coordinates { get; private set; } = points;
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine($"Route: {Name} - {Coordinates.Length} points");
        foreach (var point in Coordinates)
        {
            sb.Append($"({point.X},{point.Y})");   
        }
        sb.AppendLine();
        return sb.ToString();
    }
}