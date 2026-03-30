using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Ember.Editor;

public interface IEditor
{
    public string Name { get; }
    
    public void Init(Core core);
    public void Destroy();
    
    public void Update(GameTime gameTime);
    public void Draw(GraphicsDevice graphicsDevice,SpriteBatch spriteBatch);
    public void DrawImGui();
}