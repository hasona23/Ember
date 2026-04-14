using Ember.Scenes;
using Microsoft.Xna.Framework.Graphics;

namespace Ember.Entities;

public abstract class Component
{
    public Entity? Owner;
    public Scene? Scene => Owner?.Scene;
    public bool IsActive = true;
    
    public void Attach(Entity owner)
    {
        Owner = owner;
        OnAttach();
    }
    public void Detach()
    {
        Owner = null;
        OnDetach();
    }
    
    public abstract void Initialize();
    public abstract void Update();
    public abstract void Draw(SpriteBatch spriteBatch);
    public abstract void OnDestroy();
    protected virtual void OnAttach()
    { }
    protected virtual void OnDetach()
    { }
}