using Ember.Entities;
using Ember.Utils;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Ember.Scenes;

public abstract class Scene(Core core)
{
    private List<Entity> _entities = new List<Entity>();
    private List<Entity> _entityAddBuffer = new List<Entity>();
    private List<Entity> _entityRemoveBuffer = new List<Entity>();
    protected ContentManager SceneContent = new ContentManager(core.Services, core.Content.RootDirectory);
    protected Core Core { get; } = core;
    protected SceneManager SceneManager => Core.SceneManager;
    protected GraphicsDevice GraphicsDevice => Core.GraphicsDevice;
    protected SpriteBatch SpriteBatch => Core.SpriteBatch;
    protected Camera2D Camera => Core.Camera;

    public Entity CreateEntity()
    {
        var entity = new Entity("Entity", Transform2D.Empty, this);
        _entityAddBuffer.Add(entity);
        return entity;
    }
    public Entity CreateEntity(string name)
    {
        var entity = new Entity(name, Transform2D.Empty, this);
        _entityAddBuffer.Add(entity);
        return entity;
    }
    public Entity CreateEntity(string name, Transform2D transform)
    {
        var entity = new Entity(name, transform, this);
        _entityAddBuffer.Add(entity);
        return entity;
    }
    public void AddEntity(Entity entity)
    {
        _entityAddBuffer.Add(entity);

    }
    public void RemoveEntity(Entity entity)
    {
        _entityRemoveBuffer.Add(entity);

    }
    protected void EmptyEntityBuffers()
    {
        foreach (var entity in _entityAddBuffer)
        {
            _entities.Add(entity);
            entity.Initialize();
        }
        _entityAddBuffer.Clear();
        foreach (var entity in _entityRemoveBuffer)
        {
            _entities.Remove(entity);
            entity.Destroy();
        }
        _entityRemoveBuffer.Clear();
    }
    protected void UpdateEntities()
    {
        EmptyEntityBuffers();
        foreach (var entity in _entities)
        {
            if (entity.IsActive)
                entity.Update();
        }
    }
    protected void DrawEntities(SpriteBatch spriteBatch)
    {
        foreach (var entity in _entities)
        {
            if (entity.IsActive)
                entity.Draw(spriteBatch);
        }
    }


    public void Load()
    {
        OnLoad();
    }
    public void Destroy()
    {
        OnDestroy();
        SceneContent.Unload();

    }
    protected abstract void OnLoad();
    protected abstract void OnDestroy();
    public abstract void Update();
    public abstract void Draw();
}