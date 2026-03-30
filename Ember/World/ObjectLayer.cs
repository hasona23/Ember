using System.Collections.Generic;
using System.Text.Json.Serialization;
using Ember.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Ember.Tiled;

public class ObjectLayer
{
    public string Name { get; set; }
    public List<Object> Objects { get; set; } = new(8);
    public int TileSize { get; set; }
    public float Alpha { get; set; } = 1.0f;
    public Color Tint { get; set; } = Color.White;
    [JsonConstructor]
    public ObjectLayer()
    { }
    public ObjectLayer(string name, int tileSize)
    {
        Name = name;
        TileSize = tileSize;
    }
    public void Draw(Texture2D atlas, SpriteBatch spriteBatch)
    {
        foreach (var obj in Objects)
        {
            spriteBatch.Draw(atlas, obj.Pos, atlas.GetSourceRect(TileSize,obj.GId), Tint * Alpha);
        }
    }
}