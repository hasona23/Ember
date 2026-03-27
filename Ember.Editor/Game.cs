using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;



namespace Ember.Editor;

public class Game(WindowSettings windowSettings) : Core(windowSettings)
{
    protected override void Init()
    {
    }

    protected override void Destroy()
    {
        
    }

    protected override void UpdateCore(GameTime gameTime)
    {
        
        
    }

    protected override void DrawCore(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        SpriteBatch.Begin(samplerState:SamplerState.PointClamp);

        SpriteBatch.End();
    }

   
}