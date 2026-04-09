using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ember.Vfx.Particles;
using ImGuiNET;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Vec4 = System.Numerics.Vector4;
using Vec2 = System.Numerics.Vector2;
namespace Ember.Editor.Particles.Gui;

public class ParticleSystemSettingsGui
{
    private ParticleSystemSettings _settings;

    public ParticleSystemSettingsGui(ParticleSystemSettings settings)
    {
        _settings = settings;
        
        string textureName = _settings.TextureName;

        _availableTextures = Enum.GetNames<ParticleShapes>().ToList();
        if (!_availableTextures.Contains(textureName) && !string.IsNullOrEmpty(_settings.TextureName))
            _availableTextures.Add(textureName);
        if (string.IsNullOrEmpty(textureName))
            _settings.TextureName = _availableTextures[0];
        _availableTextureIndex = _availableTextures.IndexOf(textureName);
        
        _spawnDirectionIndex = (int)_settings.SpawnDirection;
        
        _speedChangesWithTimeIndex = (int)_settings.SpeedChangeWithTime;
        _angularSpeedChangesWithTimeIndex = (int)_settings.AngularSpeedChangeWithTime;
        _sizeChangesWithTimeIndex = (int)_settings.SizeChangeWithTime;
        _alphaChangesWithTimeIndex = (int)_settings.AlphaChangeWithTime;

        _initialColorBuffer = _settings.InitialColor.ToVector4().ToNumerics();
        _finalColorBuffer = _settings.FinalColor.ToVector4().ToNumerics();
        _newTextureBuffer = "";
    }
    #region Buffers

    private List<string> _availableTextures;
    private int _availableTextureIndex;
    private string _newTextureBuffer;

    private static readonly string[] SpawnDirections = Enum.GetNames<SpawnDirections>();
    private int _spawnDirectionIndex;

    private static readonly string[] ChangesWithTime = Enum.GetNames<ChangesWithTime>();
    private int _speedChangesWithTimeIndex;
    private int _angularSpeedChangesWithTimeIndex;
    private int _sizeChangesWithTimeIndex;
    private int _alphaChangesWithTimeIndex;

    private Vec4 _initialColorBuffer;
    private Vec4 _finalColorBuffer;

    #endregion
    
    public void DrawImGui(GraphicsDevice graphicsDevice,ContentManager content)
    {
        ImGui.PushItemWidth(200f);

        // ── Identity ──────────────────────────────────────────────────────────
        if (ImGui.CollapsingHeader("General", ImGuiTreeNodeFlags.DefaultOpen))
        {
            ImGui.InputText("Name", ref _settings.Name, 128);
        }

        // ── Color ─────────────────────────────────────────────────────────────
        if (ImGui.CollapsingHeader("Color"))
        {
            if (ImGui.ColorButton("##initial_color", _initialColorBuffer, ImGuiColorEditFlags.None, new Vec2(120, 24)))
                ImGui.OpenPopup("initial_color_picker");
            ImGui.SameLine();
            ImGui.Text("Initial Color");

            if (ImGui.BeginPopup("initial_color_picker"))
            {
                ImGui.ColorPicker4("##picker", ref _initialColorBuffer);

                _settings.InitialColor = new Color(
                    (_initialColorBuffer.X),
                    (_initialColorBuffer.Y),
                    (_initialColorBuffer.Z),
                    (_initialColorBuffer.W)
                );

                ImGui.EndPopup();
            }

            ImGui.Text(_settings.InitialColor.ToString());
            ImGui.Text(_initialColorBuffer.ToString());
            if (ImGui.ColorButton("##final_color", _finalColorBuffer, ImGuiColorEditFlags.None, new Vec2(120, 24)))
                ImGui.OpenPopup("final_color_picker");
            ImGui.SameLine();
            ImGui.Text("Final Color");

            if (ImGui.BeginPopup("final_color_picker"))
            {
                ImGui.ColorPicker4("##picker", ref _finalColorBuffer);

                _settings.FinalColor = new Color(
                    (_finalColorBuffer.X),
                    (_finalColorBuffer.Y),
                    (_finalColorBuffer.Z),
                    (_finalColorBuffer.W)
                );

                ImGui.EndPopup();
            }

            ImGui.Text(_settings.FinalColor.ToString());
            ImGui.Text(_finalColorBuffer.ToString());

            if (ImGui.Combo("Alpha Over Time", ref _alphaChangesWithTimeIndex, ChangesWithTime,
                    ChangesWithTime.Length))
                _settings.AlphaChangeWithTime = (ChangesWithTime)_alphaChangesWithTimeIndex;
        }

        // ── Motion ────────────────────────────────────────────────────────────
        if (ImGui.CollapsingHeader("Motion"))
        {
            if (ImGui.TreeNode("Speed"))
            {
                ImGui.DragFloat("Min Speed", ref _settings.MinSpeed, 1f, 0f, _settings.MaxSpeed);
                ImGui.DragFloat("Max Speed", ref _settings.MaxSpeed, 1f, _settings.MinSpeed, 5000f);
                if (ImGui.Combo("Speed Over Time", ref _speedChangesWithTimeIndex, ChangesWithTime,
                        ChangesWithTime.Length))
                    _settings.SpeedChangeWithTime = (ChangesWithTime)_speedChangesWithTimeIndex;
                ImGui.TreePop();
            }

            if (ImGui.TreeNode("Angular Speed"))
            {
                ImGui.DragFloat("Min Angular Speed", ref _settings.MinAngularSpeed, 1f, MathF.PI * -10, _settings.MaxAngularSpeed);
                ImGui.DragFloat("Max Angular Speed", ref _settings.MaxAngularSpeed, 1f, _settings.MinAngularSpeed, 5000f);
                if (ImGui.Combo("Angular Speed Over Time", ref _angularSpeedChangesWithTimeIndex, ChangesWithTime,
                        ChangesWithTime.Length))
                    _settings.AngularSpeedChangeWithTime = (ChangesWithTime)_angularSpeedChangesWithTimeIndex;
                ImGui.TreePop();
            }

            ImGui.DragFloat("Gravity", ref _settings.Gravity, 0.5f);
        }

        // ── Size ──────────────────────────────────────────────────────────────
        if (ImGui.CollapsingHeader("Size"))
        {
            ImGui.DragFloat("Min Size", ref _settings.MinSize, 0.01f, 0f, _settings.MaxSize);
            ImGui.DragFloat("Max Size", ref _settings.MaxSize, 0.01f, _settings.MinSize, 100f);
            if (ImGui.Combo("Size Over Time", ref _sizeChangesWithTimeIndex, ChangesWithTime, ChangesWithTime.Length))
                _settings.SizeChangeWithTime = (ChangesWithTime)_sizeChangesWithTimeIndex;
        }

        // ── Emission ──────────────────────────────────────────────────────────
        if (ImGui.CollapsingHeader("Emission"))
        {
            ImGui.DragInt("Max Particles", ref _settings.MaxParticles, 1, 1, 10000);
            ImGui.DragInt("Particles Per Spawn", ref _settings.ParticlesPerSpawn, 1, 1, _settings.MaxParticles);
            ImGui.DragFloat("Spawn Cooldown", ref _settings.SpawnCooldown, 0.01f, 0.001f, 1000f);

            if (ImGui.Combo("Spawn Direction", ref _spawnDirectionIndex, SpawnDirections, SpawnDirections.Length))
                _settings.SpawnDirection = (SpawnDirections)_spawnDirectionIndex;

            if (ImGui.TreeNode("Bounds"))
            {
                ImGui.DragInt("Width", ref _settings.Bounds.Width, 1, 0, int.MaxValue);
                ImGui.DragInt("Height", ref _settings.Bounds.Height, 1, 0, int.MaxValue);
                ImGui.TreePop();
            }
        }

        // ── Lifetime ──────────────────────────────────────────────────────────
        if (ImGui.CollapsingHeader("Lifetime"))
        {
            ImGui.DragFloat("Min Life", ref _settings.MinLife, 1f, 0f, _settings.MaxLife);
            ImGui.DragFloat("Max Life", ref _settings.MaxLife, 1f, _settings.MinLife, 100000f);
        }

        // ── Texture ───────────────────────────────────────────────────────────
        if (ImGui.CollapsingHeader("Texture"))
        {
            ImGui.Text("Texture Path in Content Manager: ");
            ImGui.InputText("##texture_input", ref _newTextureBuffer, 255);
            if (ImGui.Button("Add Texture"))
            {
                try
                {
                    _settings.ParticleTexture = content.Load<Texture2D>(_newTextureBuffer);
                    _availableTextures.Add(_newTextureBuffer);
                    _settings.TextureName = _newTextureBuffer;
                    _availableTextureIndex = _availableTextures.IndexOf(_settings.TextureName);
                    _newTextureBuffer = "";
                }
                catch
                {
                    ImGui.OpenPopup("TextureErrorPopup");
                }
            }

            if (ImGui.BeginPopupModal("TextureErrorPopup"))
            {
                ImGui.TextColored(new(1, 0, 0, 1), "Couldn't load texture:" + _newTextureBuffer);
                if (ImGui.Button("Close"))
                    ImGui.CloseCurrentPopup();
                ImGui.EndPopup();
            }

            if (ImGui.Combo("##Texture", ref _availableTextureIndex, _availableTextures.ToArray(),
                    _availableTextures.Count))
            {
                _settings.TextureName = _availableTextures[_availableTextureIndex];
                _settings.ParticleTexture = _settings.TextureName switch
                {
                    "Circle" => Core.Circle,
                    "Pixel" => Core.Pixel,
                    _ => content.Load<Texture2D>(_settings.TextureName)
                };
            }
        }

        ImGui.PopItemWidth();
    }
}