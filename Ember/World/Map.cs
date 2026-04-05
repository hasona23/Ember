using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using Microsoft.Xna.Framework.Content;

namespace Ember.World;

public class Map
{
    public string Name { get; set; } = string.Empty;
    public List<TileLayer> TileLayers { get; set; } = new List<TileLayer>(8);
    public List<ObjectLayer> ObjectLayers { get; set; } = new List<ObjectLayer>(8);
    public Tileset Tileset { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public int TileSize { get; set; }
    
    [JsonConstructor]
    public Map()
    { }

    public Map(string name,int width, int height, int tileSize,string atlasContentPath,ContentManager contentManager)
    {
        Name = name;
        Width = width;
        Height = height;
        TileSize = tileSize;
        Tileset = new Tileset(atlasContentPath);
        Tileset.Load(contentManager);
    }

    public void AddTileLayer(string name,string tag,bool isSolid,TileLayer.LayerType layerType)
    {
        TileLayers.Add(new TileLayer(name,tag,this)
        {
            IsSolid = isSolid,
            Type = layerType
        });
    }
    public ReadOnlySpan<TileLayer> GetTileLayer()
    {
        return CollectionsMarshal.AsSpan(TileLayers);
    }
    public void RemoveTileLayer(int index)
    {
        TileLayers.RemoveAt(index);
    }
    
    public void AddObjectLayer(string name,string tag)
    {
        ObjectLayers.Add(new ObjectLayer(name,tag,this));
    }
    public ReadOnlySpan<ObjectLayer> GetObjectLayer()
    {
        return CollectionsMarshal.AsSpan(ObjectLayers);
    }
    public void RemoveObjectLayer(int index)
    {
        ObjectLayers.RemoveAt(index);
    }
}