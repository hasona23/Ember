using System.IO;
using System.Text.Json;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Ember.World;

public struct Tileset
{
    public string Name { get; set; }
    public string AtlasName { get; set; }
    public Texture2D? Atlas { get; private set; } = null;

    public Tileset(string json)
    {
        JsonDocument jsonDocument = JsonDocument.Parse(json);
        Name = jsonDocument.RootElement.GetProperty("name").GetString() ?? throw new JsonException("name is null");
        AtlasName = Path.GetFileNameWithoutExtension(jsonDocument.RootElement.GetProperty("image").GetString()??throw new JsonException("atlasName is null"));
    }
    public void Load(string atlasPath,ContentManager content)
    {
        Atlas = content.Load<Texture2D>(atlasPath);
    }
}