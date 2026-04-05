using Ember.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Ember.Editor.Tiles;

public class TileEditor:IEditor
{
    public string Name => "Tile Editor";
    private Core _core;
    public void Init(Core core)
    {
        _core = core;
    }

    public void Destroy()
    {
        
    }

    public void Update(GameTime gameTime)
    {
        
    }

    public void Draw(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch)
    {
        
    }

    public void DrawImGui()
    {
        
    }
}