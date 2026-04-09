using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Ember.Utils;

public static class Extensions
{
    /// <summary>
    /// Get Source rectangle based on Cell ID/Order
    /// </summary>
    /// <param name="atlas">Texture to get from</param>
    /// <param name="cellSize">Size of Rectangle</param>
    /// <param name="gid">Order of Cell in Texture Starting from ZERO</param>
    /// <returns></returns>
    public static Rectangle GetSourceRect(this Texture2D atlas, int cellSize, int gid)
    {
        return new Rectangle
        {
            X = ((gid) % (atlas.Width / cellSize)) * cellSize,
            Y = ((gid) / (atlas.Width / cellSize)) * cellSize,
            Width = cellSize,
            Height = cellSize,
        };
    }
    
    public static void DrawGrid(this SpriteBatch spriteBatch,Point pos,int rows, int cols, int cellSize, Color color)
    {
        for (int i = 0; i <= rows; i++)
        {
            spriteBatch.Draw(Core.Pixel,new Rectangle(pos.X,pos.Y+i *cellSize,cols * cellSize,1),color);
        }

        for (int i = 0; i <= cols; i++)
        {
            spriteBatch.Draw(Core.Pixel,new Rectangle(pos.X + i *cellSize,pos.Y,-1,rows * cellSize),color);
        }
    }

    public static void DrawRectangle(this SpriteBatch spriteBatch, Rectangle rectangle, Color color)
    {
        spriteBatch.Draw(Core.Pixel,rectangle,color);
    }

    public static void DrawHollowRectangle(this SpriteBatch spriteBatch, Rectangle rectangle, int thickness,
        Color color)
    {
        //TOP
        spriteBatch.Draw(Core.Pixel,new Rectangle(rectangle.X,rectangle.Y,rectangle.Width,thickness),color);
        //BOTTOM
        spriteBatch.Draw(Core.Pixel,new Rectangle(rectangle.X,rectangle.Y+rectangle.Height-thickness,rectangle.Width,thickness),color);
        //RIGHT
        spriteBatch.Draw(Core.Pixel,new Rectangle(rectangle.X,rectangle.Y,thickness,rectangle.Height),color);
        //LEFT
        spriteBatch.Draw(Core.Pixel,new Rectangle(rectangle.X+rectangle.Width-thickness,rectangle.Y,thickness,rectangle.Height),color);
    }
}