using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Ember.Input;

public class KeyboardInfo
{
    public KeyboardState PreviousState;
    public KeyboardState CurrentState = Keyboard.GetState();

    public void Update()
    {
        PreviousState = CurrentState;
        CurrentState = Keyboard.GetState();
    }

    public Vector2 GetMovementWasd()
    {
        var result = Vector2.Zero;
        if (CurrentState.IsKeyDown(Keys.D))
            result.X = 1;
        if (CurrentState.IsKeyDown(Keys.A))
            result.X = -1;
        if (CurrentState.IsKeyDown(Keys.W))
            result.Y = -1;
        if (CurrentState.IsKeyDown(Keys.S))
            result.Y = 1;
        return result;
    }



    /// <summary>
    /// Checks if the key was just pressed
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public bool IsKeyPressed(Keys key)
    {
        return CurrentState.IsKeyDown(key) && PreviousState.IsKeyUp(key);
    }

    /// <summary>
    /// Checks if the key is pressed
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public bool IsKeyDown(Keys key)
    {
        return CurrentState.IsKeyDown(key);
    }

    /// <summary>
    /// Checks if the Key Is not pressed
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public bool IsKeyUp(Keys key)
    {
        return CurrentState.IsKeyUp(key);
    }

    /// <summary>
    /// Checks if the key was just released
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public bool IsKeyReleased(Keys key)
    {
        return CurrentState.IsKeyUp(key) && PreviousState.IsKeyDown(key);
    }
}