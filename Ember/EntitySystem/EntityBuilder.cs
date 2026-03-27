namespace Ember.EntitySystem;

public class EntityBuilder
{
    private readonly Entity _entity;


    private EntityBuilder(string name)
    {
        _entity = new Entity(name);
    }

    public static EntityBuilder Create(string name)
    {
        return new EntityBuilder(name);
    }

    public EntityBuilder WithTransform(Transform2D transform)
    {
        _entity.Transform = transform;
        return this;
    }

    public EntityBuilder WithComponent(string name, Component component)
    {
        _entity.AddComponent(name, component);
        return this;
    }
    public EntityBuilder WithComponent<T>(T component) where T: Component
    {
        _entity.AddComponent(component);
        return this;
    }

    public Entity Build()
    {
        _entity.Init();
        return _entity;
    }
}