using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ember.EntityCore;

public struct Transform2D(Vector2 position)
{
    public Vector2 Position = position;
    public float Rotation = 0f;
    public float Scale = 1f;
    public Vector2 Origin = Vector2.Zero;
}
