namespace Ember.World.MapImporters;

public interface IMapImporter
{
    public Map Import(string filePath,string tilesetDir);
}