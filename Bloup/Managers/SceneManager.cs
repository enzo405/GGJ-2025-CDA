using System.Collections.Generic;
using System.Diagnostics;
using Bloup.Core;
using Microsoft.Xna.Framework;

namespace Bloup.Managers;

public class SceneManager(GameStart game)
{
    private static SceneManager _instance;

    private readonly GameStart _game = game;

    private Dictionary<string, SceneBase> scenes = new Dictionary<string, SceneBase>();

    public static SceneManager Create(GameStart game)
    {
        if (_instance == null)
        {
            _instance = new SceneManager(game);
        }
        return _instance;
    }

    public void Register(SceneBase scene)
    {
        scenes.Add(scene.GetName(), scene);
    }

    public void ChangeScene(string sceneName)
    {
        this._game.changeCurrentScene(scenes[sceneName]);
        Debug.WriteLine("Changing scene to ");
    }
}