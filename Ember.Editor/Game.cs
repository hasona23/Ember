using System.Collections.Generic;
using System.Linq;
using Ember.Editor.Particles;
using Microsoft.Xna.Framework;

namespace Ember.Editor;

public class Game() : Core(new WindowSettings(Title,Width,Height))
{
    public const int Width = 480;
    public const int Height = 270;
    public const string Title = "Editor";
    private string _currentEditor = "";
    private Dictionary<string,IEditor> _editors = new();
    protected override void Init()
    {
        ParticleEditor particleEditor = new ParticleEditor();
        _editors.Add(particleEditor.Name, particleEditor);
        foreach (var (_, editor) in _editors)
        {
            editor.Init(this);
        }

        _currentEditor = _editors.Keys.First();
    }

    protected override void Destroy()
    {
        foreach (var (_,editor) in _editors)
        {
            editor.Destroy();
        }
    }

    protected override void UpdateCore(GameTime gameTime)
    {
        _editors[_currentEditor].Update(gameTime);
    }

    protected override void DrawCore(GameTime gameTime)
    {
        ScreenManager.AttachScreenBuffer();
        _editors[_currentEditor].Draw(GraphicsDevice,SpriteBatch);
        ScreenManager.DetachScreenBuffer();
        ScreenManager.DrawScreen(SpriteBatch);
    }

    protected override void DrawImGui()
    {
        _editors[_currentEditor].DrawImGui();
    }
}