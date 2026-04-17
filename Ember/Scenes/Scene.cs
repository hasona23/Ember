using Ember.EntityCore;
using Ember.Utils;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Ember.Scenes;

public abstract class Scene(Core core)
{
    protected ContentManager SceneContent { get; set; } = new ContentManager(core.Services, core.Content.RootDirectory);
    protected Core Core { get; } = core;
    protected SceneManager SceneManager => Core.SceneManager;
    protected GraphicsDevice GraphicsDevice => Core.GraphicsDevice;
    protected SpriteBatch SpriteBatch => Core.SpriteBatch;
    protected Camera2D Camera => Core.Camera;

    protected World World { get; set; } = new World();
    
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
    public virtual void DrawImGui() { }
}