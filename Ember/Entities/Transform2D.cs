using Microsoft.Xna.Framework;

namespace Ember.Entities;

public struct Transform2D(Vector2 position, float rotation, Vector2 scale)
{
    public Vector2 Position = position;
    public float Rotation = rotation;
    public Vector2 Scale = scale;
    public Vector2 Origin = Vector2.Zero;

    public readonly static Transform2D Empty = new Transform2D(Vector2.Zero, 0f, Vector2.One);
}