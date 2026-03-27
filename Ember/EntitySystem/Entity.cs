using System.Collections.Generic;
using System.Numerics;

namespace Ember.EntitySystem;

public sealed class Entity
{
    private readonly Dictionary<string, Component> _components;
    private bool _hasInit;

    public Entity(string name)
    {
        Name = name;
        _components = new Dictionary<string, Component>();
        _components.EnsureCapacity(8);
        Transform = new Transform2D(Vector2.Zero);
    }

    public string Name { get; set; }
    public bool IsActive { get; private set; } = true;
    public Transform2D Transform { get; set; }

    public T? GetComponent<T>() where T : Component
    {
        return GetComponent<T>(typeof(T).Name);
    }
    public T? GetComponent<T>(string name) where T : Component
    {
        if (_components.TryGetValue(name, out var component))
            return (T)component;
        return null;
    }

    public bool HasComponent(string name)
    {
        return _components.ContainsKey(name);
    }
    public bool HasComponent<T>()
    {
        return _components.ContainsKey(typeof(T).Name);
    }

    public void AddComponent<T>(string name, T component) where T : Component
    {
        _components.Add(name, component);
        component.Owner = this;
        if (_hasInit)
            component.Init();
    }
    public void AddComponent<T>(T component) where T : Component
    {
        AddComponent(typeof(T).Name, component);
    }
    public bool RemoveComponent(string name)
    {
        if (!_components.TryGetValue(name, out _))
            return false;
        _components[name].OnDestroy();
        _components[name].Owner = null!;
        _components.Remove(name);
        return true;
    }
    public bool RemoveComponent<T>()
    {
        return _components.Remove(typeof(T).Name);
    }

    public void Init()
    {
        foreach (var (_, component) in _components) component.Init();
        _hasInit = true;
    }

    public void Update()
    {
        foreach (var (_, component) in _components)
            if (component.IsActive)
                component.Update();
    }

    public void Draw()
    {
        foreach (var (_, component) in _components)
            if (component.IsActive)
                component.Draw();
    }

    public void Destroy()
    {
        IsActive = false;
        foreach (var (_, component) in _components)
        {
            component.OnDestroy();
            component.IsActive = false;
        }

        _components.Clear();
    }
}