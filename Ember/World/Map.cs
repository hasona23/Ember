using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Xml.Linq;
using Microsoft.Xna.Framework.Graphics;

namespace Ember.Tiled;

public class Map
{
    public Dictionary<string, ObjectLayer> ObjectLayers { get; set; } = new();
    public Dictionary<string, TileLayer> TileLayers { get; set; } = new();
    public int TileSize { get; set; }
    public List<string> CollisionLayers { get; } = new();
    public TileSet TileSet { get; set; }
    public int Scale { get; set; }

    public static Map FromTmx(string path, string tilesetDir, int scale = 1)
    {
        Map map = new();
        var root = XDocument.Load(path).Root ?? throw new Exception("Invalid tmx file");
        map.TileSet = new TileSet(Path.Combine(tilesetDir,
            root.Element("tileset")?.Attribute("source")?.Value.Replace(".png",".xnb") ??
            throw new Exception("Invalid .tmx . No Tileset Source")));
        map.TileSize = int.Parse(root.Attribute("tilewidth")?.Value ?? throw new Exception("Invalid .tmx . No width"));
        ImmutableArray<XElement> tileLayers = [..root.Elements("layer")];
        foreach (var layer in tileLayers)
        {
            var tileLayer = new TileLayer(layer, map.TileSize);
            map.TileLayers[tileLayer.Name] = tileLayer;
            if (tileLayer.Class == "Collision")
                map.CollisionLayers.Add(tileLayer.Name);
        }

        ImmutableArray<XElement> objectLayers = [..root.Elements("objectgroup")];
        foreach (var layer in objectLayers)
        {
            var objectLayer = new ObjectLayer(layer);
            map.ObjectLayers[objectLayer.Name] = objectLayer;
        }

        map.Scale = scale;
        return map;
    }

    public void DrawLayers(Texture2D tileSet,SpriteBatch spriteBatch, params string[] layers)
    {
        if (layers.Length == 0)
        {
            foreach (var (_, layer) in TileLayers) layer.DrawTiles(tileSet,spriteBatch);

            return;
        }

        foreach (var layer in layers) TileLayers[layer].DrawTiles(tileSet,spriteBatch);
    }
}