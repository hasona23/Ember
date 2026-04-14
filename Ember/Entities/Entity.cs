using System;
using System.Collections.Generic;
using Ember.Scenes;
using Microsoft.Xna.Framework.Graphics;

namespace Ember.Entities;

public class Entity(string name, Transform2D transform, Scene scene)
{
    public bool IsActive = true;
    public Scene? Scene = scene;
    public string Name = name;
    public Transform2D Transform = transform;
    private readonly List<Component> _components = new List<Component>(8);
    private bool _initialized = false;
    public void Initialize()
    {
        foreach (var component in _components)
        {
            component.Initialize();
        }
        _initialized = true;
    }

    public Entity WithComponent<T>() where T : Component, new()
    {
        T component = new T();
        _components.Add(component);
        component.Attach(this);
        if (_initialized)
            component.Initialize();
        return this;
    }
    public Entity WithComponent(Component component)
    {
        _components.Add(component);
        component.Attach(this);
        if (_initialized)
            component.Initialize();
        return this;
    }

    public void RemoveComponent(Component component)
    {
        _components.Remove(component);
        component?.Detach();
    }

    public void RemoveComponent<T>() where T : Component
    {
        int index = _components.FindIndex(c => c.GetType() == typeof(T));
        if (index != -1)
        {
            _components[index].Detach();
            _components.RemoveAt(index);
        }
    }

    public T? GetComponent<T>() where T : Component
    {
        foreach (var component in _components)
        {
            if (component is T tComponent)
            {
                return tComponent;
            }
        }

        return null;
    }

    public bool HasComponent<T>() where T : Component
    {
        return GetComponent<T>() != null;
    }

    public void Update()
    {
        foreach (var component in _components)
        {
            if (component.IsActive)
            {
                component.Update();
            }
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        foreach (var component in _components)
        {
            if (component.IsActive)
            {
                component.Draw(spriteBatch);
            }
        }
    }

    public void Destroy()
    {
        foreach (var component in _components)
        {
            component.OnDestroy();
            component.IsActive = false;
            component.Detach();
        }

        Scene = null;
        _components.Clear();
        IsActive = false;
    }
}