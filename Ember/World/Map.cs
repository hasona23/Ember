using System.Collections.Generic;

namespace Ember.Tiled;

public class Map(int tileSize,int width,int height)
{
    public List<ObjectLayer> ObjectLayers { get; set; } = [new ObjectLayer("Layer 1",tileSize)];
    public List<TileLayer> TileLayers { get; set; } = [new TileLayer("Layer 1",width,height,tileSize)];
    public int TileSize { get; set; }= tileSize;
    public int Width { get; set; }= width;
    public int Height { get; set; }= height;
    public int Scale { get; set; } = 1;
}