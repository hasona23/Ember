using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
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
    public int TileSize { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public Tile[] Tiles { get; set; } 
    public string Name { get; set; }
    public float Alpha { get; set; } = 1.0f;
    public Color Tint { get; set; } = Color.White;
    [JsonConstructor]
    public TileLayer()
    {}

    public TileLayer(string name, int width, int height, int tileSize)
    {
        Name = name;
        TileSize = tileSize;
        Width = width;
        Height = height;
        Tiles = new Tile[width * height];
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

    public Tile? GetTileAt(int x, int y)
    {
        x /= TileSize;
        y /= TileSize;
        int index = y * Width + x;
        if (index >= 0 && index < Tiles.Length) 
            return Tiles[index];
        return null;
    }

    public void SetTileAt(int x, int y, Tile tile)
    {
        x /= TileSize;
        y /= TileSize;
        int index = y * Width + x;
        if (index >= 0 && index < Tiles.Length)
            Tiles[index] = tile;
    }

    public Vector2 GetTilePos(int index)
    {
        if (index < 0 || index >= Tiles.Length)
            return -Vector2.One;
        Vector2 pos = new Vector2
        {
            X = index % Width ,
            // ReSharper disable once PossibleLossOfFraction
            Y = index / Width
        } * TileSize;
       
        return pos;
    }
    public void SetTileGIdAt(int x, int y, int gid)
    {
        x /= TileSize;
        y /= TileSize;
        int index = y * Width + x;
        if (index >= 0 && index < Tiles.Length)
            Tiles[index].GId = gid;
    }
    
    public void Draw(Texture2D atlas,SpriteBatch spriteBatch)
    {
        for (int tileIndex = 0; tileIndex <Tiles.Length; tileIndex++)
        {
            Tile tile = Tiles[tileIndex];
            if (tile.GId == 0)
            {
                continue;
            }

            spriteBatch.Draw(atlas, GetTilePos(tileIndex), atlas.GetSourceRect(TileSize,tile.GId), Tint * Alpha);
        }
    }
}