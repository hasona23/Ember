using System.Text.Json.Serialization;

namespace Ember.World;

[JsonSourceGenerationOptions]
[JsonSerializable(typeof(Map))]
[JsonSerializable(typeof(TileLayer))]
[JsonSerializable(typeof(ObjectLayer))]
[JsonSerializable(typeof(MapObject))]
[JsonSerializable(typeof(Tileset))]

public partial class MapJsonContext:JsonSerializerContext
{
    
}