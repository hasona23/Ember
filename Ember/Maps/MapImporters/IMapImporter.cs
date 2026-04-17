namespace Ember.Maps.MapImporters;

public interface IMapImporter
{
    public Map Import(string filePath,string tilesetDir);
}