using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using ImGuiNET;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Vec2 = System.Numerics.Vector2;
using Vec4 = System.Numerics.Vector4;

namespace Ember.Vfx.Particles;

public class ParticleSystemSettings
{
    public string Name;
    
    public Color InitialColor  = Color.White;
    public Color FinalColor  = Color.White;
    public ChangesWithTime AlphaChangeWithTime  = ChangesWithTime.Decrease;

    public float MaxSpeed  = 500;
    public float MinSpeed  = 300;
    public ChangesWithTime SpeedChangeWithTime  = ChangesWithTime.Decrease;

    public float MaxAngularSpeed  = 1000;
    public float MinAngularSpeed  = 1000;
    public ChangesWithTime AngularSpeedChangeWithTime  = ChangesWithTime.Decrease;

    public float Gravity;

    public float MaxSize  = 1;
    public float MinSize  = 1;
    public ChangesWithTime SizeChangeWithTime  = ChangesWithTime.Decrease;

    public int MaxParticles  = 128;
    public int ParticlesPerSpawn  = 16;
    public float SpawnCooldown  = 1;
    public SpawnDirections SpawnDirection  = SpawnDirections.Outward;
    public Rectangle Bounds  = new Rectangle(0, 0, 10, 10);

    public float MaxLife  = 1;
    public float MinLife  = 0.5f;
    
    public string TextureName  = "";

    //==== ImGui Buffers
    private List<string> _availableTextures = Array.Empty<string>().ToList();
    private int _availableTextureIndex;
    private string _newTextureBuffer = "";
    
    private string[] _spawnDirections = Array.Empty<string>();
    private int _spawnDirectionIndex;

    private string[] _changesWithTime = Array.Empty<string>();
    private int _speedChangesWithTimeIndex;
    private int _angularSpeedChangesWithTimeIndex;
    private int _sizeChangesWithTimeIndex;
    private int _alphaChangesWithTimeIndex;

    private Vec4 _initialColorBuffer = Vec4.Zero;
    private Vec4 _finalColorBuffer = Vec4.Zero;
    
    private ContentManager _contentManager;

    public static ParticleSystemSettings ParseFromJson(string json, ContentManager contentManager)
    {
        ParticleSystemSettings? settings = JsonSerializer.Deserialize<ParticleSystemSettings>(json,
            new JsonSerializerOptions()
            {
                IncludeFields = true
            });
        if(settings == null)
            throw new Exception("Cannot parse particle system settings");
        
        settings._contentManager = contentManager;
        settings.SetupBuffers();
        
        return settings;
    }

    public static ParticleSystemSettings ParseFromJsonFile(string path, ContentManager contentManager)
    {
        return ParseFromJson(File.ReadAllText(path), contentManager);
    }

    // ReSharper disable once NotNullOrRequiredMemberIsNotInitialized
    // Buffers are all setup in SetupBuffers() Methods
    public ParticleSystemSettings(string name, ContentManager contentManager)
    {
        Name = name;
        _contentManager = contentManager;
        SetupBuffers();
    }

    
    private void SetupBuffers()
    {
        string textureName = TextureName;
        
        _availableTextures = GetAvailableTextureNames();
        if(!_availableTextures.Contains(textureName) && !string.IsNullOrEmpty(TextureName))
            _availableTextures.Add(textureName);
        if(string.IsNullOrEmpty(textureName))
            TextureName = _availableTextures[0];
        _availableTextureIndex = _availableTextures.IndexOf(textureName);

        _spawnDirections = Enum.GetNames<SpawnDirections>();
        _spawnDirectionIndex = (int)SpawnDirection;

        _changesWithTime = Enum.GetNames<ChangesWithTime>();
        _speedChangesWithTimeIndex = (int)SpeedChangeWithTime;
        _angularSpeedChangesWithTimeIndex = (int)AngularSpeedChangeWithTime;
        _sizeChangesWithTimeIndex = (int)SizeChangeWithTime;
        _alphaChangesWithTimeIndex = (int)AlphaChangeWithTime;

        _initialColorBuffer = new Vec4(InitialColor.R, InitialColor.G, InitialColor.B, InitialColor.A)/255f;
        _finalColorBuffer = new Vec4(FinalColor.R, FinalColor.G, FinalColor.B, FinalColor.A)/255f;
        _newTextureBuffer = "";
    }

    private List<string> GetAvailableTextureNames()
    {
        return new List<string>(Enum.GetNames<ParticleShapes>());
    }

    public void DrawImGui()
    {
        ImGui.PushItemWidth(200f);

        // ── Identity ──────────────────────────────────────────────────────────
        if (ImGui.CollapsingHeader("General", ImGuiTreeNodeFlags.DefaultOpen))
        {
            ImGui.InputText("Name", ref Name, 128);
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
                
                InitialColor = new Color(
                    (_initialColorBuffer.X),
                    (_initialColorBuffer.Y),
                    (_initialColorBuffer.Z),
                    (_initialColorBuffer.W)
                );
                
                ImGui.EndPopup();
            }
            ImGui.Text(InitialColor.ToString());
            ImGui.Text(_initialColorBuffer.ToString());
            if (ImGui.ColorButton("##final_color", _finalColorBuffer, ImGuiColorEditFlags.None, new Vec2(120, 24)))
                ImGui.OpenPopup("final_color_picker");
            ImGui.SameLine();
            ImGui.Text("Final Color");

            if (ImGui.BeginPopup("final_color_picker"))
            {
                ImGui.ColorPicker4("##picker", ref _finalColorBuffer);
                
                FinalColor = new Color(
                    (_finalColorBuffer.X),
                    (_finalColorBuffer.Y),
                    (_finalColorBuffer.Z),
                    (_finalColorBuffer.W)
                );
                
                ImGui.EndPopup();
            }
            ImGui.Text(FinalColor.ToString());
            ImGui.Text(_finalColorBuffer.ToString());
            
            if (ImGui.Combo("Alpha Over Time", ref _alphaChangesWithTimeIndex, _changesWithTime,
                    _changesWithTime.Length))
                AlphaChangeWithTime = (ChangesWithTime)_alphaChangesWithTimeIndex;
        }

        // ── Motion ────────────────────────────────────────────────────────────
        if (ImGui.CollapsingHeader("Motion"))
        {
            if (ImGui.TreeNode("Speed"))
            {
                ImGui.DragFloat("Min Speed", ref MinSpeed, 1f, 0f, MaxSpeed);
                ImGui.DragFloat("Max Speed", ref MaxSpeed, 1f, MinSpeed, 5000f);
                if (ImGui.Combo("Speed Over Time", ref _speedChangesWithTimeIndex, _changesWithTime,
                        _changesWithTime.Length))
                    SpeedChangeWithTime = (ChangesWithTime)_speedChangesWithTimeIndex;
                ImGui.TreePop();
            }

            if (ImGui.TreeNode("Angular Speed"))
            {
                ImGui.DragFloat("Min Angular Speed", ref MinAngularSpeed, 1f, MathF.PI * -10, MaxAngularSpeed);
                ImGui.DragFloat("Max Angular Speed", ref MaxAngularSpeed, 1f, MinAngularSpeed, 5000f);
                if (ImGui.Combo("Angular Speed Over Time", ref _angularSpeedChangesWithTimeIndex, _changesWithTime,
                        _changesWithTime.Length))
                    AngularSpeedChangeWithTime = (ChangesWithTime)_angularSpeedChangesWithTimeIndex;
                ImGui.TreePop();
            }

            ImGui.DragFloat("Gravity", ref Gravity, 0.5f);
        }

        // ── Size ──────────────────────────────────────────────────────────────
        if (ImGui.CollapsingHeader("Size"))
        {
            ImGui.DragFloat("Min Size", ref MinSize, 0.01f, 0f, MaxSize);
            ImGui.DragFloat("Max Size", ref MaxSize, 0.01f, MinSize, 100f);
            if (ImGui.Combo("Size Over Time", ref _sizeChangesWithTimeIndex, _changesWithTime, _changesWithTime.Length))
                SizeChangeWithTime = (ChangesWithTime)_sizeChangesWithTimeIndex;
        }

        // ── Emission ──────────────────────────────────────────────────────────
        if (ImGui.CollapsingHeader("Emission"))
        {
            ImGui.DragInt("Max Particles", ref MaxParticles, 1, 1, 10000);
            ImGui.DragInt("Particles Per Spawn", ref ParticlesPerSpawn, 1, 1, MaxParticles);
            ImGui.DragFloat("Spawn Cooldown", ref SpawnCooldown, 0.01f, 0.001f, 1000f);

            if (ImGui.Combo("Spawn Direction", ref _spawnDirectionIndex, _spawnDirections, _spawnDirections.Length))
                SpawnDirection = (SpawnDirections)_spawnDirectionIndex;

            if (ImGui.TreeNode("Bounds"))
            {
              ImGui.DragInt("X", ref Bounds.X, 0.5f);
                ImGui.DragInt("Y", ref Bounds.Y, 0.5f);
                ImGui.DragInt("Width", ref Bounds.Width, 1, 0, int.MaxValue);
                 ImGui.DragInt("Height", ref Bounds.Height, 1, 0, int.MaxValue);
                ImGui.TreePop();
            }
        }

        // ── Lifetime ──────────────────────────────────────────────────────────
        if (ImGui.CollapsingHeader("Lifetime"))
        {
            ImGui.DragFloat("Min Life", ref MinLife, 1f, 0f, MaxLife);
            ImGui.DragFloat("Max Life", ref MaxLife, 1f, MinLife, 100000f);
        }

        // ── Texture ───────────────────────────────────────────────────────────
        if (ImGui.CollapsingHeader("Texture"))
        {
            ImGui.Text("Texture Path in Content Manager: ");
            ImGui.InputText("##texture_input",ref _newTextureBuffer, 255);
            if (ImGui.Button("Add Texture"))
            {
                try
                {
                    _contentManager.Load<Texture2D>(_newTextureBuffer);
                    _availableTextures.Add(_newTextureBuffer);
                    _newTextureBuffer = "";
                }
                catch
                {
                    ImGui.OpenPopup("TextureErrorPopup");
                }
            }

            if (ImGui.BeginPopupModal("TextureErrorPopup"))
            {
                ImGui.TextColored(new(1,0,0,1),"Couldn't load texture:"+_newTextureBuffer);
                ImGui.EndPopup();
            }

            if (ImGui.Combo("##Texture", ref _availableTextureIndex, _availableTextures.ToArray(), _availableTextures.Count))
                TextureName = _availableTextures[_availableTextureIndex];
        }

        ImGui.PopItemWidth();
    }
    
}

public enum SpawnDirections
{
    Outward,
    Inward,
    RotateAround,
    Right,
    Left,
    Up,
    Down,
}

public enum ParticleShapes
{
    Circle,
    Square,
}
public enum ChangesWithTime
{
    None,
    Increase,
    Decrease,
}