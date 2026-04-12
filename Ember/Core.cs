using Ember.Input;
using Ember.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Ember;

public abstract class Core : Game
{
    public static Texture2D Circle { get; private set; } = null!;
    public static Texture2D Pixel { get; private set; } = null!;
    public static SpriteFont DefaultFont {  get; private set; } = null!;
    public Camera2D Camera = new Camera2D();
    
    protected readonly GraphicsDeviceManager Graphics;
    protected SpriteBatch SpriteBatch = null!;
    public ImGuiRenderer ImGuiRenderer { get; private set; }= null!;
    public ScreenManager ScreenManager { get; set; } = null!;
    protected readonly WindowSettings WindowSettings;
    protected Core(WindowSettings windowSettings)
    {
        Graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        WindowSettings = windowSettings;
    }

    protected override void Initialize()
    {
        ScreenManager = new ScreenManager(WindowSettings,Window,Graphics);
        base.Initialize();
        SpriteBatch = new SpriteBatch(GraphicsDevice);
        CreateShapeTextures();
        ImGuiRenderer = new ImGuiRenderer(this);
        ImGuiRenderer.RebuildFontAtlas();
        try
        {
            DefaultFont = Content.Load<SpriteFont>("Font");
        }catch(ContentLoadException ex)
        {
            Console.Error.WriteLine($"Error Reading Default font: {ex.Message}");
        }
        Init();
    }

    protected override void UnloadContent()
    {
        base.UnloadContent();
        Destroy();
        Pixel.Dispose();
        Circle.Dispose();
    }

    private void CreateShapeTextures()
    {
        Pixel = new Texture2D(GraphicsDevice, 1, 1);
        Pixel.Name = "Pixel";
        Pixel.SetData(new[] { Color.White });

        int diameter = 16;
        Color[] data = new Color[diameter*diameter];
        Vector2 center = new Vector2(diameter / 2f)-Vector2.One/2;
        for (int y = 0; y < diameter; y++)
        {
            for (int x = 0; x < diameter; x++)
            {
                
                data[y*diameter + x] = (center-new Vector2(x,y)).LengthSquared() <= diameter*diameter/4f ? Color.White : Color.Transparent;
            }
        }
        Circle = new Texture2D(GraphicsDevice, diameter, diameter);
        Circle.SetData(data);
        Circle.Name = "Circle";
    }
  
    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();
        
        InputManager.Update(gameTime,ScreenManager);
        Time.UpdateUps(gameTime);
        UpdateCore(gameTime); 
    }

    protected override void Draw(GameTime gameTime)
    {
        DrawCore(gameTime);
        Time.UpdateFps(gameTime);
    }


    protected abstract void Init();
    protected abstract void Destroy();
    protected abstract void UpdateCore(GameTime gameTime);
    protected abstract void DrawCore(GameTime gameTime);
}
