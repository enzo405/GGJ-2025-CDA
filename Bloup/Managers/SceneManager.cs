using System.Collections.Generic;
using System.Diagnostics;
using Bloup.Core;

namespace Bloup.Managers;

public class SceneManager(GameStart game)
{
    private static SceneManager? _instance;

    private readonly GameStart _game = game;

    private Dictionary<string, SceneBase> scenes = [];

    public static SceneManager Create(GameStart game)
    {
        _instance ??= new SceneManager(game);
        return _instance;
    }

    public void Register(SceneBase scene)
    {
        scenes.Add(scene.GetName(), scene);
    }

    public void ChangeScene(string sceneName)
    {
        _game.ChangeCurrentScene(scenes[sceneName]);
        Debug.WriteLine($"Changing scene to {sceneName}");
    }
}