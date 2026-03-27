using Microsoft.Xna.Framework;

namespace Ember;

public static class Time
{
    public static float DeltaTime { get; private set; }
    public static float UnScaledDeltaTime { get; private set; }
    public static float Fps { get; internal set; }
    public static float TimeScale { get; set; } = 1;

    internal static void Update(GameTime gameTime)
    {
        UnScaledDeltaTime = gameTime.ElapsedGameTime.Milliseconds / 1000f;
        DeltaTime = UnScaledDeltaTime * TimeScale;
        Fps = 1/UnScaledDeltaTime;
    }
}