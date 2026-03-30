using Microsoft.Xna.Framework;

namespace Ember.Input;

public static class InputManager
{
    /// <summary>
    /// Gets the state information of keyboard input.
    /// </summary>
    public static KeyboardInfo Keyboard { get; private set; }

    /// <summary>
    /// Gets the state information of mouse input.
    /// </summary>
    public static MouseInfo Mouse { get; private set; }

    /// <summary>
    /// Gets the state information of a gamepad.
    /// </summary>
    public static GamePadInfo[] GamePads { get; private set; }

    static InputManager()
    {
        Keyboard = new KeyboardInfo();
        Mouse = new MouseInfo();

        GamePads = new GamePadInfo[4];
        for (var i = 0; i < 4; i++) GamePads[i] = new GamePadInfo((PlayerIndex)i);
    }

    /// <summary>
    /// Updates the state information for the keyboard, mouse, and gamepad inputs.
    /// </summary>
    /// <param name="gameTime">A snapshot of the timing values for the current frame.</param>
    public static void Update(GameTime gameTime,ScreenManager screenManager)
    {
        Keyboard.Update();
        Mouse.Update(screenManager);

        for (var i = 0; i < 4; i++) GamePads[i].Update(gameTime);
    }
}