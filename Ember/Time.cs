using Microsoft.Xna.Framework;

namespace Ember;

public static class Time
{
    public static float DeltaTime { get; private set; }
    public static float UnScaledDeltaTime { get; private set; }
    public static float Fps { get; private set; }
    public static float Ups { get; private set; }
    public static float TimeScale { get; set; } = 1;

    internal static void UpdateUps(GameTime gameTime)
    {
        UnScaledDeltaTime = gameTime.ElapsedGameTime.Milliseconds / 1000f;
        DeltaTime = UnScaledDeltaTime * TimeScale;
        Ups = 1/UnScaledDeltaTime;
    }

    internal static void UpdateFps(GameTime gameTime)
    {
        Fps = gameTime.ElapsedGameTime.Milliseconds / 1000f;
        Fps = 1 / Fps;
    }
}