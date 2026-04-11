using Ember.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ember.World.MapRenderers;

public class EmberMapRenderer(Map map) : IMapRenderer
{
    public void DrawBackground(SpriteBatch spriteBatch)
    {
        foreach (TileLayer tileLayer in map.TileLayers)
        {
            if (tileLayer.TileLayerTypes == TileLayerTypes.Background)
            {
                DrawTileLayer(spriteBatch, tileLayer);
            }
        }
    }
    private void DrawTileLayer(SpriteBatch spriteBatch, TileLayer tileLayer)
    {
        for (int tileIndex = 0; tileIndex < tileLayer.Tiles.Length; tileIndex++)
        {
            if (tileLayer.Tiles[tileIndex] == -1)
                continue;
            Vector2 tilePosition = new Vector2(tileIndex % map.Width, tileIndex / map.Width);
            spriteBatch.Draw(map.Tileset.Atlas,
                tilePosition * map.TileSize * map.Scale,
                map.Tileset.Atlas?.GetSource(map.TileSize, tileLayer.Tiles[tileIndex]),
                Color.White);
        }
    }
    public void DrawForeground(SpriteBatch spriteBatch)
    {
        foreach (TileLayer tileLayer in map.TileLayers)
        {
            if (tileLayer.TileLayerTypes == TileLayerTypes.Foreground)
            {
                DrawTileLayer(spriteBatch, tileLayer);
            }
        }
    }

    public void DrawGround(SpriteBatch spriteBatch)
    {
        foreach (TileLayer tileLayer in map.TileLayers)
        {
            if (tileLayer.TileLayerTypes == TileLayerTypes.Ground)
            {
                DrawTileLayer(spriteBatch, tileLayer);
            }
        }
    }

    public void DrawObjects(SpriteBatch spriteBatch)
    {
        foreach (ObjectLayer objectLayer in map.ObjectLayers)
        {
            foreach (var obj in objectLayer.Objects)
            {
                if (obj.Gid.HasValue && obj.Gid.Value != -1)
                {
                    spriteBatch.Draw(map.Tileset.Atlas,
                        new Vector2(obj.X, obj.Y) * map.Scale,
                        map.Tileset.Atlas?.GetSource(map.TileSize, obj.Gid.Value),
                        Color.White);
                }
                else if (obj.Width.HasValue && obj.Height.HasValue && obj.Width != 0 && obj.Height != 0)
                {
                    spriteBatch.DrawRectangle(
                        new Rectangle(obj.X, obj.Y, obj.Width.Value, obj.Height.Value),
                        Color.Red * 0.5f);
                }
                else
                {
                    spriteBatch.DrawRectangle(
                        new Rectangle(obj.X, obj.Y, 10, 10),
                        Color.Red * 0.5f);
                }
            }
        }
    }

    public void DrawRoutes(SpriteBatch spriteBatch, Color pointColor, Color lineColor)
    {
        foreach (var routeLayer in map.RoutesLayers)
        {
            for (int j = 0; j < routeLayer.Routes.Length; j++)
            {
                var route = routeLayer.Routes[j];
                for (int k = 0; k < route.Coordinates.Length; k++)
                {
                    var point = route.Coordinates[k];

                    spriteBatch.DrawRectangle(new Rectangle(point - new Point(2), new Point(4)), pointColor);
                    if (k < route.Coordinates.Length - 1)
                    {
                        var nextPoint = route.Coordinates[k + 1];
                        spriteBatch.DrawLine(point.ToVector2(), nextPoint.ToVector2(), lineColor);
                    }
                }
            }
        }
    }
}