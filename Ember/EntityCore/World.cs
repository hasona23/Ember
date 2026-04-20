using Microsoft.Xna.Framework.Graphics;

namespace Ember.EntityCore;

/// <summary>
/// The Container of all entities in the Game
/// </summary>
public class World 
{
    public EntityManager<Entity> EntityManager { get; private set; }
    public World()
    {
        EntityManager = new EntityManager<Entity>("WorldEntityManager", this);
    }
    public void Init()
    {
        EntityManager.Init();
    }
    public void Update()
    {
        EntityManager.Update();
    }
    public void Draw(SpriteBatch spriteBatch)
    {
        EntityManager.Draw(spriteBatch);
    }
    public void DrawImGui()
    {
        EntityManager.DrawImGui();
    }

    public TEntity? GetEntity<TEntity>(string name) where TEntity : Entity
    {
        foreach (var entity in EntityManager.Entities)
        {
            if (entity.Name == name && entity is TEntity typedEntity)
            {
                return typedEntity;
            }
        }
        return null;
    }

    public bool TryGetEntity<TEntity>(string name, out TEntity? entity) where TEntity : Entity
    {
        entity = GetEntity<TEntity>(name);
        return entity != null;
    }

}
