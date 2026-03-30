using System.IO;
using System.Text.Json;
using Ember.Input;
using Ember.Vfx.Particles;
using ImGuiNET;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Ember.Editor.Particles;

public class ParticleEditor:IEditor
{
    public string Name => "ParticleEditor";
    private Core _core;
    private ParticleSystem _particleSystem;
    private ParticleSystemSettings _particleSystemSettings;
    private string _pathBuffer = "";
    private JsonSerializerOptions _options = new JsonSerializerOptions()
    {
        IncludeFields = true,
        PropertyNameCaseInsensitive = true,
        WriteIndented =  true
    };
    
    public void Init(Core core)
    {
        _core = core;
        _particleSystemSettings = new ParticleSystemSettings("Settings1", core.Content);
        _particleSystem = new ParticleSystem(_particleSystemSettings,_core.ScreenManager.Resolution()/2f,core.Content);
    }

    public void Destroy()
    {
        
    }

    public void Update(GameTime gameTime)
    {
        if (ImGui.IsAnyItemHovered())
            return;
        if (InputManager.Keyboard.IsKeyPressed(Keys.E))
            _particleSystem.Emit();
        
        if (InputManager.Keyboard.IsKeyDown(Keys.R))
            _particleSystem.Emit();
        
    }

    public void Draw(GraphicsDevice graphicsDevice,SpriteBatch spriteBatch)
    {
        
    }

    public void Save(string path)
    {
        if(Path.GetExtension(path) != ".ember")
            path = $"{path}.ember";
        string json = JsonSerializer.Serialize(_particleSystemSettings, _options);
        File.WriteAllText(path, json);
    }

    public void Load(string path)
    {
        if(Path.GetExtension(path) != ".ember")
            path = $"{path}.ember";
        string json = File.ReadAllText(path);
        _particleSystemSettings = ParticleSystemSettings.ParseFromJson(json, _core.Content);
    }
    public void DrawImGui()
    {
        ImGui.SetNextWindowPos(System.Numerics.Vector2.Zero);
        float width = _core.GraphicsDevice.PresentationParameters.BackBufferWidth/5f;
        float height = _core.GraphicsDevice.PresentationParameters.BackBufferHeight;
        ImGui.SetNextWindowSize(new System.Numerics.Vector2(width, height));
        ImGui.Begin(Name, ImGuiWindowFlags.NoBackground);
        {
            ImGui.Text("Setting Name: ");
            ImGui.InputText("#SettingsName", ref _particleSystemSettings.Name, 32);
            if (ImGui.Button("Save"))
            {
                ImGui.OpenPopup("SavePopup");
            }
            ImGui.SameLine();
            if (ImGui.Button("Load"))
            {
                ImGui.OpenPopup("LoadPopup");
            }

            if (ImGui.BeginPopupModal("SavePopup"))
            {
                ImGui.Text("Path: ");
                ImGui.InputText("##path", ref _pathBuffer, 256);
                ImGui.SameLine();
                ImGui.Text(".ember");
                if (ImGui.Button("Save"))
                {
                    Save(_pathBuffer);
                    ImGui.CloseCurrentPopup();
                }

                if(ImGui.Button("Cancel"))
                    ImGui.CloseCurrentPopup();
                ImGui.EndPopup();
            }
            if (ImGui.BeginPopupModal("LoadPopup"))
            {
                ImGui.Text("Path: ");
                ImGui.InputText("##path", ref _pathBuffer, 256);
                ImGui.SameLine();
                ImGui.Text(".ember");
                if (ImGui.Button("Load"))
                {
                    Load(_pathBuffer);
                    ImGui.CloseCurrentPopup();
                }

                if(ImGui.Button("Cancel"))
                    ImGui.CloseCurrentPopup();
                ImGui.EndPopup();
            }
            if (ImGui.CollapsingHeader("Settings"))
            {
                _particleSystemSettings.DrawImGui();
            }
        }
        ImGui.End();
        
        
    }
}