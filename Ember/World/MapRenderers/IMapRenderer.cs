using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ember.World.MapRenderers;

public interface IMapRenderer
{
    void DrawBackground(SpriteBatch spriteBatch);
    void DrawGround(SpriteBatch spriteBatch);
    void DrawForeground(SpriteBatch spriteBatch);
    void DrawObjects(SpriteBatch spriteBatch);
    void DrawRoutes(SpriteBatch spriteBatch,Color pointColor,Color lineColor);
    public void DrawAll(SpriteBatch spriteBatch,Color pointColor, Color lineColor)
    {
        DrawBackground(spriteBatch);
        DrawGround(spriteBatch);
        DrawForeground(spriteBatch);
        DrawObjects(spriteBatch);
        DrawRoutes(spriteBatch,pointColor,lineColor);
    }
}
