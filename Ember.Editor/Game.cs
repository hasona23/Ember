using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ImGuiNET;
using Microsoft.Xna.Framework;

namespace Ember.Editor;

public class Game() : Core(new WindowSettings(Title, Width, Height))
{
    public const int Width = 480;
    public const int Height = 270;
    public const string Title = "Editor";
    private string _currentEditor = "";
    private readonly Dictionary<string, IEditor> _editors = new();
    private List<string> _editorNames = new();
    private int _resolutionXBuffer, _resolutionYBuffer;
    private bool _isResizingWindowOpen = false;

    protected override void Init()
    {
        GetAllEditors();
        foreach (var (_, editor) in _editors)
        {
            editor.Init(this);
        }

        _editorNames = _editors.Keys.ToList();
        _currentEditor = "ParticleEditor";
        (_resolutionXBuffer, _resolutionYBuffer) = ScreenManager.Resolution().ToPoint();
    }

    private void GetAllEditors()
    {
        var editors = Assembly.GetExecutingAssembly().GetTypes()
            .Where(type => typeof(IEditor).IsAssignableFrom(type) && type.IsClass && !type.IsAbstract).ToList();
        foreach (var editor in editors)
        {
            IEditor instance = (IEditor)Activator.CreateInstance(editor);

            if (instance != null)
                _editors[instance.Name] = instance;
        }
    }

    protected override void Destroy()
    {
        foreach (var (_, editor) in _editors)
        {
            editor.Destroy();
        }
    }

    protected override void UpdateCore(GameTime gameTime)
    {
        if (_editors.TryGetValue(_currentEditor, out var editor))
            editor.Update(gameTime);
    }

    protected override void DrawCore(GameTime gameTime)
    {
        ScreenManager.AttachScreenBuffer();
        if (_editors.TryGetValue(_currentEditor, out var editor))
            editor.Draw(GraphicsDevice, SpriteBatch);
        ScreenManager.DetachScreenBuffer();
        ScreenManager.DrawScreen(SpriteBatch);

        ImGuiRenderer.BeforeLayout(gameTime);
        DrawImGui();
        ImGuiRenderer.AfterLayout();
    }

    protected void DrawImGui()
    {
        if (!_editors.ContainsKey(_currentEditor))
        {
            ImGui.Begin("Choose Editor");
            foreach (var editorName in _editorNames)
            {
                if (ImGui.Button(editorName))
                    _currentEditor = editorName;

                ImGui.SameLine();
            }

            ImGui.End();
        }

        ImGui.BeginMainMenuBar();
        {
            if (ImGui.BeginMenu("Editors"))
            {
                foreach (var editorName in _editorNames)
                {
                    if (ImGui.MenuItem(editorName))
                    {
                        _editors[_currentEditor]?.Destroy();
                        _currentEditor = editorName;
                        _editors[_currentEditor]?.Init(this);
                    }
                }

                ImGui.EndMenu();
            }

            if (ImGui.BeginMenu("Settings"))
            {
                if (ImGui.MenuItem("Change Resolution"))
                {
                    _isResizingWindowOpen = true;
                }

                ImGui.EndMenu();
            }
        }
        ImGui.EndMainMenuBar();
        if (_isResizingWindowOpen)
        {
            ImGui.OpenPopup("ResizeWindow");
            _isResizingWindowOpen = false;
        }

        if (ImGui.BeginPopupModal("ResizeWindow"))
        {
            ImGui.Text("Resolution: ");
            ImGui.InputInt("X", ref _resolutionXBuffer, 1);
            ImGui.InputInt("Y", ref _resolutionYBuffer, 1);
            if (ImGui.Button("Apply"))
            {
                ScreenManager.ChangeResolution(new Point(_resolutionXBuffer, _resolutionYBuffer));
                ImGui.CloseCurrentPopup();
            }

            ImGui.SameLine();
            if (ImGui.Button("Toggle Full Screen"))
            {
                ScreenManager.ToggleFullScreen();
                ImGui.CloseCurrentPopup();
            }

            ImGui.SameLine();
            if (ImGui.Button("Cancel"))
            {
                ImGui.CloseCurrentPopup();
            }

            ImGui.EndPopup();
        }

        if (_editors.TryGetValue(_currentEditor, out var editor))
            editor.DrawImGui();
    }
}