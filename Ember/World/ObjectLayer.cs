using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Xml.Linq;

namespace Ember.Tiled;

public class ObjectLayer
{
    public ObjectLayer(XElement objectLayerElement)
    {
        Id = int.Parse(objectLayerElement.Attribute("id")?.Value ?? throw new Exception("Object group without ID"));
        Name = objectLayerElement.Attribute("name")?.Value ?? throw new Exception("Object group without Name");
        Class = objectLayerElement.Attribute("class")?.Value;

        var objectGroupElements = objectLayerElement.Elements("object").ToImmutableArray();
        Objects = new List<Object>(objectGroupElements.Length);
        foreach (var objElement in objectGroupElements)
        {
            var obj = new Object();
            obj.GId = int.Parse(objElement.Attribute("gid")?.Value ?? "-1");
            obj.Name = objElement.Attribute("name")?.Value;
            obj.Class = objElement.Attribute("class")?.Value;
            obj.X = int.Parse(objElement.Attribute("x")?.Value ??
                              throw new InvalidOperationException("Object group without X"));
            obj.Y = int.Parse(objElement.Attribute("y")?.Value ??
                              throw new InvalidOperationException("Object group without Y"));
            Objects.Add(obj);
        }
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public string? Class { get; set; }
    public List<Object> Objects { get; set; }
}