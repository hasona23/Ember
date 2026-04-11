using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Ember.Vfx.Particles;

public class ParticleSystemSettings
{
    public string Name;
    public string TextureName = Core.Circle.Name;
    [JsonIgnore]
    public Texture2D ParticleTexture = Core.Circle;
    public Color InitialColor = Color.White;
    public Color FinalColor = Color.White;
    public ChangesWithTime AlphaChangeWithTime = ChangesWithTime.Decrease;

    public float MaxSpeed = 500;
    public float MinSpeed = 300;
    public ChangesWithTime SpeedChangeWithTime = ChangesWithTime.Decrease;

    public float MaxAngularSpeed = 1000;
    public float MinAngularSpeed = 1000;
    public ChangesWithTime AngularSpeedChangeWithTime = ChangesWithTime.Decrease;

    public float Gravity;

    public float MaxSize = 1;
    public float MinSize = 1;
    public ChangesWithTime SizeChangeWithTime = ChangesWithTime.Decrease;

    public int MaxParticles = 128;
    public int ParticlesPerSpawn = 16;
    public float SpawnCooldown = 1;
    public SpawnDirections SpawnDirection = SpawnDirections.Outward;
    public Rectangle Bounds = new Rectangle(0, 0, 10, 10);

    public float MaxLife = 1;
    public float MinLife = 0.5f;
    
    
    public static ParticleSystemSettings ParseFromJson(string json, ContentManager contentManager)
    {
        ParticleSystemSettings? settings = JsonSerializer.Deserialize<ParticleSystemSettings>(json,
            new JsonSerializerOptions()
            {
                IncludeFields = true
            });
        if (settings == null)
            throw new Exception("Cannot parse particle system settings");
        
        if(settings.TextureName == Core.Circle.Name)
            settings.ParticleTexture = Core.Circle;
        else if(settings.TextureName == Core.Pixel.Name)
            settings.ParticleTexture = Core.Pixel;
        else
            settings.ParticleTexture = contentManager.Load<Texture2D>(settings.TextureName);
        
        return settings;
    }

    public static ParticleSystemSettings ParseFromJsonFile(string path, ContentManager contentManager)
    {
        return ParseFromJson(File.ReadAllText(path), contentManager);
    }
    [JsonConstructor]
    public ParticleSystemSettings()
    { }
    
    // ReSharper disable once NotNullOrRequiredMemberIsNotInitialized
    // Buffers are all setup in SetupBuffers() Methods
    public ParticleSystemSettings(string name)
    {
        Name = name;
        ParticleTexture = Core.Circle;
        TextureName = Core.Circle.Name;
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
    Pixel,
}

public enum ChangesWithTime
{
    None,
    Increase,
    Decrease,
}