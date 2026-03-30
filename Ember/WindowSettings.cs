namespace Ember;

public struct WindowSettings(
    string title,
    int width,
    int height,
    bool allowResizing = true,
    bool fullScreen = false,
    bool borderless = false)
{
    public readonly string Title = title;
    public int Width { get; set; }= width;
    public int Height { get; set; }= height;
    public readonly bool AllowResizing = allowResizing;
    public readonly bool FullScreen = fullScreen;
    public readonly bool Borderless = borderless;
}