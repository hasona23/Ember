namespace Ember;

public record WindowSettings(
    string Title,
    int Width,
    int Height,
    bool AllowResizing = true,
    bool FullScreen = false,
    bool Borderless = false);