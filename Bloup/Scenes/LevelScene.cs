using System;
using System.Diagnostics;
using Bloup.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using static System.Net.Mime.MediaTypeNames;

namespace Bloup.Scenes;

public class LevelScene(SpriteBatch spriteBatch, GraphicsDeviceManager graphics) : SceneBase(spriteBatch, graphics)
{
    protected override string name { get; set; } = "LevelScene";

    // Add all ressource

    private Texture2D background;

    public override void Draw(GameTime gameTime)
    {
        spriteBatch.Begin();
        spriteBatch.Draw(background, new Vector2(0, 0), Color.Aqua);

        spriteBatch.End();
    }

    public override void LoadContent(ContentManager content)
    {
        background = content.Load<Texture2D>("backgrounds/Menu");
    }

    public override void Update(GameTime gameTime)
    {
        if (Keyboard.GetState().IsKeyDown(Keys.A))
        {
            // rediger des log
            Debug.WriteLine("t nul");
        }
    }
}