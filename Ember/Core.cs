using Ember.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Ember;

public abstract class Core : Game
{
    public static Texture2D Circle { get; private set; } = null!;
    public static Texture2D Pixel { get; private set; } = null!;
    
    protected readonly GraphicsDeviceManager Graphics;
    protected SpriteBatch SpriteBatch = null!;
    private ImGuiRenderer _imGuiRenderer = null!;
    protected ScreenManager ScreenManager { get; set; } = null!;
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
        _imGuiRenderer = new ImGuiRenderer(this);
        _imGuiRenderer.RebuildFontAtlas();
        Init();
    }

    protected override void UnloadContent()
    {
        base.UnloadContent();
        Destroy();
    }

    private void CreateShapeTextures()
    {
        Pixel = new Texture2D(GraphicsDevice, 1, 1);
        Pixel.SetData(new[] { Color.White });

        int diameter = 16;
        Color[] data = new Color[diameter*diameter];
        Vector2 center = new Vector2(diameter / 2f);
        for (int y = 0; y < diameter; y++)
        {
            for (int x = 0; x < diameter; x++)
            {
                
                data[y*diameter + x] = (center-new Vector2(x,y)).LengthSquared() <= diameter*diameter/4f ? Color.White : Color.Transparent;
            }
        }
        Circle = new Texture2D(GraphicsDevice, diameter, diameter);
        Circle.SetData(data);
    }
  
    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();
        
        Input.Update(gameTime,ScreenManager);
        Time.Update(gameTime);
        UpdateCore(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        
        ScreenManager.AttachScreenBuffer();
        DrawCore(gameTime);
        ScreenManager.DetachScreenBuffer();
        
        ScreenManager.DrawScreen(SpriteBatch);
        
        _imGuiRenderer.BeforeLayout(gameTime);
        DrawImGui();
        _imGuiRenderer.AfterLayout();
    }

    protected abstract void Init();
    protected abstract void Destroy();
    protected abstract void UpdateCore(GameTime gameTime);
    protected abstract void DrawCore(GameTime gameTime);
    protected virtual void DrawImGui(){}
}
