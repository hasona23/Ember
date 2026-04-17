using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Ember.EntityCore;

public abstract class Entity(string name, Vector2 position, World world)
{
    public string Name { get; set; } = name;
    public Transform2D Transform  = new(position);
    public bool IsActive { get; set;} = true;
    public bool IsDead { get; set; } = false;
    protected World World { get; set; } = world;

    public abstract void Update();
    public abstract void Draw(SpriteBatch spriteBatch);
    public virtual void Destroy()
    {
        IsDead = true;
        IsActive = false;
    }
    public abstract void DrawImGui();

}
