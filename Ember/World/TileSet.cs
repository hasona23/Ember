using System;
using System.Xml.Linq;

namespace Ember.Tiled;

public struct TileSet
{
    public string Name { get; set; }
    public string Source { get; set; }
    public int TileWidth { get; set; }
    public int TileHeight { get; set; }

    public TileSet(string filePath)
    {
        var root = XDocument.Load(filePath).Root ?? throw new Exception("Invalid tsx file");
        Name = root.Attribute("name")?.Value ?? throw new Exception("Invalid tsx file. No name");
        TileWidth = int.Parse(root.Attribute("tilewidth")?.Value ?? throw new Exception("Invalid tsx file. No width"));
        TileHeight =
            int.Parse(root.Attribute("tileheight")?.Value ?? throw new Exception("Invalid tsx file. No height"));
        Source = root.Element("image")?.Attribute("source")?.Value ??
                 throw new Exception("Invalid tsx file. No source");
    }
}