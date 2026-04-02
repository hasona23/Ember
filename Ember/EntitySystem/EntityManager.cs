using System.Collections.Generic;

namespace Ember.EntitySystem;

public static class EntityManager
{
    private static readonly Dictionary<string, Entity> Entities = new(32);

    public static void AddEntity(Entity entity)
    {
        Entities.Add(entity.Name, entity);
    }

    public static void RemoveEntity(string name)
    {
        if (Entities.TryGetValue(name, out var entity))
            entity.Destroy();
        Entities.Remove(name);
    }

    public static Entity GetEntity(string name)
    {
        return Entities[name];
    }

    public static bool HasEntity(string name)
    {
        return Entities.ContainsKey(name);
    }

    public static void Update()
    {
        foreach (var (_, entity) in Entities)
            if (entity.IsActive)
                entity.Update();
    }

    public static void Draw()
    {
        foreach (var (_, entity) in Entities) entity.Draw();
    }

    public static void Clear()
    {
        foreach (var (_, entity) in Entities) entity.Destroy();
        Entities.Clear();
    }
}