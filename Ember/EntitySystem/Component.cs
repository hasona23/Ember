using Microsoft.Xna.Framework.Graphics;

namespace Ember.EntitySystem;

public abstract class Component
{
    public Entity Owner { get; set; } = null!;
    public bool IsActive { get; set; } = true;
    public abstract void Init();
    public abstract void Update();
    public abstract void Draw(SpriteBatch spriteBatch);
    public abstract void OnDestroy();
}