using System;
using Microsoft.Xna.Framework.Content;

namespace Ember;

public abstract class Scene(string name, Core core) : IDisposable
{
    protected ContentManager LocalContent { get; set; } = new(core.Services,core.Content.RootDirectory);
    public string Name { get; set; } = name;

    public void Dispose()
    {
        Destroy();
        // LocalAssets.Dispose();
    }

    public abstract void Update();
    public abstract void Draw();
    public abstract void Init();
    public abstract void Destroy();
}