using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;

namespace Ember.World;

public class ObjectLayer
{
    public List<MapObject> Objects { get; set; } = new List<MapObject>(8);
    public LayerData Data { get; set; }
    
    
    [JsonConstructor]
    public ObjectLayer()
    {}
    public ObjectLayer(string name, string tag, Map map)
    {
        Data = new LayerData(name, tag, map);
    }
}