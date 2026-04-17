using System;
using System.IO;
using System.Text.Json;
using Ember.Editor.Particles.Gui;
using Ember.Inputs;
using Ember.Vfx.Particles;
using ImGuiNET;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Ember.Editor.Particles;

public class ParticleEditor : IEditor
{
    public string Name => "ParticleEditor";
    private Core _core;
    private ParticleSystem _particleSystem;
    private ParticleSystemSettings _particleSystemSettings;
    private string _pathBuffer = "";
    private int _gridCellSize = 16;
    private const string ParticleFileExtension = ".ember";
    private readonly JsonSerializerOptions _options = new JsonSerializerOptions()
    {
        IncludeFields = true,
        PropertyNameCaseInsensitive = true,
        WriteIndented = true
    };
    
    private readonly Color _backgroundColor = Color.Black;

    public void Init(Core core)
    {
        _core = core;
        _particleSystemSettings ??= new ParticleSystemSettings("Settings1");
        _particleSystem =
            new ParticleSystem(_particleSystemSettings, _core.ScreenManager.Resolution() / 2f, core.Content);
        _systemGui = new ParticleSystemSettingsGui(_particleSystemSettings);
    }

    public void Destroy()
    {
        _particleSystem.Dispose();
        _core.Content.Unload();
    }

    public void Update(GameTime gameTime)
    {
        _particleSystem.Update();
        _particleSystemSettings.Bounds.Location = (_core.ScreenManager.Resolution() / 2).ToPoint();
        _particleSystemSettings.Bounds.Location -= (_particleSystemSettings.Bounds.Size.ToVector2()/2).ToPoint();
        if (ImGui.IsAnyItemHovered())
            return;
        if (Input.Keyboard.IsKeyPressed(Keys.E))
            _particleSystem.Emit();

        if (Input.Keyboard.IsKeyDown(Keys.R))
            _particleSystem.Emit();
        
        if(Input.Keyboard.IsKeyPressed(Keys.C))
            Array.Clear(_particleSystem.Particles.Data);
    }

    public void Draw(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch)
    {
        graphicsDevice.Clear(_backgroundColor);
        spriteBatch.Begin(samplerState: SamplerState.PointClamp, blendState: BlendState.Opaque);
        Vector2 resolution = _core.ScreenManager.Resolution();
        for (int i = 0; i <= (int)(resolution.X/_gridCellSize); i++)
        {
            spriteBatch.Draw(Core.Pixel,new Rectangle(i*_gridCellSize, 0, 2, (int)resolution.Y),Color.DarkGray);
        }

        for (int i = 0; i <= (int)(resolution.Y / _gridCellSize); i++)
        {
            spriteBatch.Draw(Core.Pixel, new Rectangle(0, i * _gridCellSize, (int)resolution.X, 2), Color.DarkGray);
        }
        spriteBatch.End();
        spriteBatch.Begin(samplerState:SamplerState.PointClamp);
        {
            _particleSystem.Draw(spriteBatch);
        }
        spriteBatch.End();
    }

    public void Save(string path)
    {
        if (Path.GetExtension(path) != ParticleFileExtension)
            path = $"{path}.{ParticleFileExtension}";
        string json = JsonSerializer.Serialize(_particleSystemSettings, _options);
        File.WriteAllText(path, json);
    }

    private ParticleSystemSettingsGui _systemGui; 
    public void Load(string path)
    {
        if (Path.GetExtension(path) != ParticleFileExtension)
            path = $"{path}.{ParticleFileExtension}";
        string json = File.ReadAllText(path);
        if(_particleSystemSettings.TextureName != Core.Circle.Name && _particleSystemSettings.TextureName != Core.Pixel.Name)
            _particleSystemSettings.ParticleTexture.Dispose();
        _particleSystemSettings = ParticleSystemSettings.ParseFromJson(json, _core.Content);
        _particleSystem.Settings = _particleSystemSettings;
        _systemGui = new ParticleSystemSettingsGui(_particleSystemSettings);
    }

    public void DrawImGui()
    {
        ImGui.SetNextWindowPos(System.Numerics.Vector2.Zero + new System.Numerics.Vector2(0,20));
        float width = _core.GraphicsDevice.PresentationParameters.BackBufferWidth / 5f;
        float height = _core.GraphicsDevice.PresentationParameters.BackBufferHeight;
        ImGui.SetNextWindowSize(new System.Numerics.Vector2(width, height));
        ImGui.SetNextWindowBgAlpha(0.5f);
       
        ImGui.Begin(Name);
        {
            
            ImGui.Text($"FPS: {Time.Fps}");
            ImGui.Text($"Particles: {_particleSystem.Particles.Data.Length}");
            ImGui.Text($"Index: {_particleSystem.Particles.Index}");
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
                ImGui.Text(ParticleFileExtension);
                if (ImGui.Button("Save"))
                {
                    Save(_pathBuffer);
                    ImGui.CloseCurrentPopup();
                }

                if (ImGui.Button("Cancel"))
                    ImGui.CloseCurrentPopup();
                ImGui.EndPopup();
            }

            if (ImGui.BeginPopupModal("LoadPopup"))
            {
                ImGui.Text("Path: ");
                ImGui.InputText("##path", ref _pathBuffer, 256);
                ImGui.SameLine();
                ImGui.Text(ParticleFileExtension);
                if (ImGui.Button("Load"))
                {
                    Load(_pathBuffer);
                    ImGui.CloseCurrentPopup();
                }

                if (ImGui.Button("Cancel"))
                    ImGui.CloseCurrentPopup();
                ImGui.EndPopup();
            }

            if (ImGui.CollapsingHeader("Screen"))
            {
                ImGui.Text("Grid Cell Size: ");
                ImGui.InputInt("##gridSize", ref _gridCellSize, 1, 1);
            }

            if (ImGui.CollapsingHeader("Settings"))
            {
                _systemGui.DrawImGui(_core.GraphicsDevice,_core.Content);
            }
        }
        ImGui.End();
    }
}