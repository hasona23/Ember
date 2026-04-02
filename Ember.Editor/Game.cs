using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ImGuiNET;
using Microsoft.Xna.Framework;

namespace Ember.Editor;

public class Game() : Core(new WindowSettings(Title,Width,Height))
{
    public const int Width = 480;
    public const int Height = 270;
    public const string Title = "Editor";
    private string _currentEditor = "";
    private readonly Dictionary<string,IEditor> _editors = new();
    private List<string> _editorNames = new();
    protected override void Init()
    {
        GetAllEditors();
        foreach (var (_, editor) in _editors)
        {
            editor.Init(this);
        }

        _currentEditor = _editors.Keys.First();
        _editorNames = _editors.Keys.ToList();
    }

    private void GetAllEditors()
    {
        var editors = Assembly.GetExecutingAssembly().GetTypes().Where(type => typeof(IEditor).IsAssignableFrom(type) && type.IsClass && !type.IsAbstract).ToList();
        foreach (var editor in editors)
        {
            IEditor instance = (IEditor)Activator.CreateInstance(editor);

            if (instance != null) 
                _editors[instance.Name] = instance;
        }
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
        ImGui.BeginMainMenuBar();
        {
            if (ImGui.BeginMenu("Editors"))
            {
                foreach (var editorName in _editorNames)
                {
                    if (ImGui.MenuItem(editorName))
                    {
                        _editors[_currentEditor].Destroy();
                        _currentEditor = editorName;
                        _editors[_currentEditor].Init(this);
                    }
                }
                ImGui.EndMenu();
            }
        }
        ImGui.EndMainMenuBar();
        _editors[_currentEditor].DrawImGui();
    }
}