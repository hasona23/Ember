using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Ember.World;

public struct Tileset(string contentPath)
{
    public string Name { get; set; } = System.IO.Path.GetFileNameWithoutExtension(contentPath);
    public string Path { get; set; } =  contentPath;
    [JsonIgnore]
    public Texture2D Atlas { get; set; }
    public void Load(ContentManager content)
    {
        Atlas = content.Load<Texture2D>(Path);
    }

    public static Tileset FromJsonFile(string contentPath,ContentManager content)
    {
        return FromJson(File.ReadAllText(contentPath),content);
    }
    public static Tileset FromJson(string json,ContentManager content)
    {
        Tileset tileset = JsonSerializer.Deserialize(json,MapJsonContext.Default.Tileset);
        if (string.IsNullOrEmpty(tileset.Name) || string.IsNullOrEmpty(tileset.Path))
            throw new JsonException("Failed to parse tileset");
        tileset.Load(content);
        return tileset;
    }
}