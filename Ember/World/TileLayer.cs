using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Ember.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Ember.Tiled;

public class TileLayer
{
    private static readonly Vector2[] Offsets =
    [
        new(1, 1), new(0, 1), new(-1, 1),
        new(1, 0), new(0, 0), new(-1, 0),
        new(1, -1), new(0, -1), new(-1, -1)
    ];

    public int Id { get; set; }
    public int TileSize { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public Tile[] Tiles { get; set; }
    public string Name { get; set; }
    public string? Class { get; set; }

    public TileLayer(XElement layerElement, int tileSize)
    {
        using var w = new BenchmarkWatch($"Layer {Name} Loading");

        TileSize = tileSize;
        Id = int.Parse(layerElement.Attribute("id")?.Value ?? throw new Exception("Layer Id not found"));
        Class = layerElement.Attribute("class")?.Value;
        Name = layerElement.Attribute("name")?.Value ?? "TileLayer";
        Width = int.Parse(
            layerElement.Attribute("width")?.Value ?? throw new Exception($"Cant read layer {Name} width"));
        Height = int.Parse(layerElement.Attribute("height")?.Value ?? throw new Exception($"Layer {Name} height"));

        var data = layerElement.Element("data")?.Value.Split(',') ?? throw new Exception($"Layer {Name} has no data");

        Tiles = new Tile[data.Length];
        for (var i = 0; i < data.Length; i++)
        {
            var id = int.Parse(data[i]);
            Tiles[i] = new Tile(id - 1, i % Width * tileSize, i / Width * tileSize);
        }
    }

    public List<Tile> GetNearTiles(Vector2 pos)
    {
        var tiles = new List<Tile>(9);
        //Normalize Position
        pos.X = MathF.Floor(pos.X / TileSize) * TileSize;
        pos.Y = MathF.Floor(pos.Y / TileSize) * TileSize;
        pos /= TileSize;
        foreach (var offset in Offsets)
        {
            int index = (int)((pos.Y + offset.Y) * Width + pos.X + offset.X);
            if(index > 0 && index < Tiles.Length)
                tiles.Add(Tiles[index]);
        }

        return tiles;
    }

    public void DrawTiles(Texture2D tileset, SpriteBatch spriteBatch)
    {
        foreach (Tile tile in Tiles)
        {
            if (tile.GId == -1)
                continue;
            var src = new Rectangle(0, 0, TileSize, TileSize);
            var tileSetWidthTiles = tileset.Width / TileSize;
            src.X = tile.GId % tileSetWidthTiles * TileSize;
            src.Y = (int)MathF.Floor((float)tile.GId / tileSetWidthTiles) * TileSize;
            spriteBatch.Draw(tileset, tile.Position, src, Color.White);
        }
    }
}