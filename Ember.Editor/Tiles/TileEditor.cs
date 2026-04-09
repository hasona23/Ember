using Ember.Utils;
using Ember.World;
using ImGuiNET;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Ember.Editor.Tiles;

public class TileEditor:IEditor
{
    public string Name => "TileEditor";
    private Core _core;
    private Map _map;
    
    //Map Data Buffers
    private string _mapNameBuffer = "Map1";
    private string _atlasPathBuffer;
    private int _mapWidthBuffer = 50;
    private int _mapHeightBuffer = 30;
    private int _mapTileSizeBuffer = 16;
    
    public void Init(Core core)
    {
        _core = core;
    }

    public void Destroy()
    {
        
    }

    public void Update(GameTime gameTime)
    {
        if (ImGui.IsAnyItemHovered())
        {
            return;
        }

        if (_map == null)
            return;
    }

    public void Draw(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch)
    {
        if (_map == null)
            return;
        spriteBatch.Begin(blendState:BlendState.Opaque);
        {
            spriteBatch.DrawGrid(Point.Zero,_map.Width,_map.Height,_map.TileSize,Color.White);
        }
        spriteBatch.End();
        spriteBatch.Begin(transformMatrix:_core.Camera.GetViewMatrix());
        {
            
        }
        spriteBatch.End();
    }

    public void DrawImGui()
    {
        ImGui.SetNextWindowPos(System.Numerics.Vector2.Zero + new System.Numerics.Vector2(0,20));
        float width = _core.GraphicsDevice.PresentationParameters.BackBufferWidth / 5f;
        float height = _core.GraphicsDevice.PresentationParameters.BackBufferHeight;
        ImGui.SetNextWindowSize(new System.Numerics.Vector2(width, height));
        ImGui.SetNextWindowBgAlpha(0.5f);
        if (ImGui.Begin(Name))
        {
            
            ImGui.End();
        }
    }

    private void NewMapPanel()
    {
        if (ImGui.BeginMainMenuBar())
        {
            if (ImGui.BeginMenu("File"))
            {
                if (ImGui.MenuItem("New"))
                {
                    
                }

                if (ImGui.MenuItem("Load"))
                {
                    
                }

                if (ImGui.MenuItem("Resize"))
                {
                    
                }
                ImGui.EndMenu();
            }
            ImGui.EndMainMenuBar();
        }
    }
}