using System.Numerics;

namespace Ember.EntitySystem;

public class Transform2D(Vector2 pos, Vector2 origin = default, float rotation = 0.0f, float scale = 1.0f)
{
    public Vector2 Origin = origin;
    public Vector2 Pos = pos;
    public float Rotation = rotation;
    public float Scale = scale;

    //Returns (Pos - Origin)
    public Vector2 BasePosition => Pos - Origin;
}