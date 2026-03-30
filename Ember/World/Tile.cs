namespace Ember.Tiled;

public struct Tile(int gid)
{
    //Index of Cell starting from 1
    public int GId { get; set; } = gid;
}