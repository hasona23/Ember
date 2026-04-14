using System;
using System.Collections.Generic;
using System.Text;

namespace Ember.Scenes;

public class SceneManager
{
    private Scene? _currentScene;
    private Scene? _nextScene;

    public Scene CurrentScene => _currentScene;

    public void ChangeScene(Scene newScene)
    {
        _nextScene = newScene;
    }
    public void Update()
    {
        if (_nextScene != null)
        {
            _currentScene?.Destroy();
            _currentScene = _nextScene;
            _nextScene = null;
            _currentScene?.Load();
        }
        _currentScene?.Update();
    }
    public void Draw()
    {
        _currentScene?.Draw();
    }
    public void Destroy()
    {
        _currentScene?.Destroy();
        _nextScene?.Destroy();
    }
}
