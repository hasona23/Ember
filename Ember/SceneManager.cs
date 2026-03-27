using System.Collections.Generic;
using Ember.EntitySystem;

namespace Ember;

public static class SceneManager
{
    private static readonly Dictionary<string, Scene> Scenes = new(4);
    private static string CurrentScene { get; set; } = "";
    public static Scene Current => Scenes[CurrentScene];

    public static void AddScene(Scene scene)
    {
        AddScene(scene.Name, scene);
    }

    public static void AddScene(string name, Scene scene)
    {
        Scenes[name] = scene;
        if (string.IsNullOrEmpty(CurrentScene))
        {
            CurrentScene = name;
            World.Clear();
            scene.Init();
        }
    }

    public static void RemoveScene(string name)
    {
        if (Scenes.TryGetValue(name, out var scene))
        {
            scene.Destroy();
            scene.Dispose();
        }

        Scenes.Remove(name);
    }

    public static void Clear()
    {
        foreach (var (_, scene) in Scenes) scene.Dispose();

        Scenes.Clear();
    }

    public static void UpdateScene()
    {
        Scenes[CurrentScene].Update();
    }

    public static void DrawScene()
    {
        Scenes[CurrentScene].Draw();
    }

    public static void ChangeScene(string name)
    {
        Scenes[CurrentScene].Destroy();
        CurrentScene = name;
        World.Clear();
        Scenes[CurrentScene].Init();
    }
}