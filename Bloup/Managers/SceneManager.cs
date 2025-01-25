using System.Diagnostics;
using System.Runtime.CompilerServices;
using Bloup.Core;
using Microsoft.Xna.Framework;

namespace Bloup.Managers;

public class SceneManager(Game game)
{
    private static SceneManager instance;

    private Game game = game;

    public static SceneManager Create(Game game)
    {
        if (instance == null)
        {
            instance = new SceneManager(game);
        }
        return instance;
    }

    public void changeScene(SceneBase scene)
    {
        Debug.WriteLine("Changing scene to ");
    }
}