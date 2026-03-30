namespace Ember.Utils;

using Microsoft.Xna.Framework;

public struct Camera2D()
{
    public Vector2 Position = Vector2.Zero;
    public float Rotation = 0f;
    public float Zoom = 1f;
    public Vector2 Offset = new Vector2(0f, 0f);
    public Matrix GetViewMatrix()
    {
        return
            Matrix.CreateTranslation(new Vector3(-Position, 0f)) *
            Matrix.CreateRotationZ(Rotation) *
            Matrix.CreateScale(Zoom, Zoom, 1f) *
            Matrix.CreateTranslation(Offset.X, Offset.Y, 0f);
    }
}