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
}