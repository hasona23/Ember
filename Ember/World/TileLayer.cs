using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Microsoft.Xna.Framework;


namespace Ember.World;

public class TileLayer
{
    [JsonInclude]
    public int[] Tiles { get; set; } =  Array.Empty<int>();
    public LayerData Data { get; set; } 
    public bool IsSolid { get; set; } = false;
    public LayerType Type { get; set; } = LayerType.Ground;
    public enum LayerType
    {
        Background,
        Ground,
        Foreground
    }
    

    public TileLayer(string name, string tag, Map map)
    {
        Data = new LayerData(name, tag, map);
        Tiles = new int[map.Width * map.Height];
    }
    [JsonConstructor]
    public TileLayer()
    {}

    private static Vector2[] _offsets =
    [
        new Vector2(1, 1), new Vector2(0, 1), new Vector2(-1, 1),
        new Vector2(1, 0), new Vector2(0, 0), new Vector2(-1, 0),
        new Vector2(1, -1), new Vector2(0, -1), new Vector2(-1, -1)
    ];

    public List<Tile> GetTilesNear(Vector2 position)
    {
        List<Tile> tiles = new List<Tile>(9);
        position = Vector2.Floor(position/Data.Map.TileSize);
        foreach (var offset in _offsets)
        {
            position += offset;
            int index = (int)(position.X + position.Y * Data.Map.Width);
            if(index >= 0 && index < tiles.Count)
                tiles.Add(new Tile(Tiles[index],new Rectangle((position * Data.Map.TileSize).ToPoint(), new Point(Data.Map.TileSize))));
            position -= offset;
        }
        return tiles;
    }

    public void SetTile(Vector2 position, int gid)
    {
        position = Vector2.Floor(position/Data.Map.TileSize);
        int index = (int)(position.X + position.Y * Data.Map.Width);
        if(index >= 0 && index < Tiles.Length)
            Tiles[index] = gid;
    }
    public Tile? GetTileAt(Vector2 position)
    {
        position = Vector2.Floor(position / Data.Map.TileSize);
        int index = (int)(position.X + position.Y * Data.Map.Width);
        if (index >= 0 && index < Tiles.Length)
            return new Tile(Tiles[index],new Rectangle((position * Data.Map.TileSize).ToPoint(), new Point(Data.Map.TileSize)));
        return null;
    }
}