using System;
using System.Diagnostics;
using Bloup.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Bloup.Scenes;

public class MenuScene(ContentManager content, GraphicsDeviceManager graphics) : SceneBase(content, graphics)
{
    protected override string Name { get; set; } = "MenuScene";

    // Add all ressource

    private Texture2D background;

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();
        spriteBatch.Draw(background, new Vector2(0, 0), Color.Aqua);

        spriteBatch.End();
    }

    public override void LoadContent()
    {
        background = Content.Load<Texture2D>("backgrounds/Menu");
    }

    public override void Update(GameTime gameTime)
    {
        if (Keyboard.GetState().IsKeyDown(Keys.A))
        {
            // rediger des log
            Debug.WriteLine("Message de test");
        }
    }
}