using System;
using System.Diagnostics;
using Bloup.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Bloup.Scenes;

public class LevelScene(ContentManager content, GraphicsDeviceManager graphics) : SceneBase(content, graphics)
{
    protected override string Name { get; set; } = "LevelScene";
    public Player player;

    // Add all ressource

    private Texture2D? background;

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();
        spriteBatch.Draw(background, new Vector2(0, 0), Color.Aqua);
        player.Draw(spriteBatch);
        spriteBatch.End();
    }

    public override void LoadContent()
    {
        background = content.Load<Texture2D>("backgrounds/Menu");
        // Player
        Texture2D playerTexture = content.Load<Texture2D>("bubble");
        int spawnX = (int)(graphics.PreferredBackBufferWidth / 5f - playerTexture.Width / 2);
        int spawnY = graphics.PreferredBackBufferHeight / 2 - playerTexture.Height / 2;
        float scale = 2f; // Change this value to scale your texture

        player = new Player(
            playerTexture,
            new Vector2(spawnX, spawnY), // Position
            new Rectangle(spawnX, spawnY, playerTexture.Width, playerTexture.Height), // Hitbox using original size
            scale // Pass the scale factor
        );
    }

    public override void Update(GameTime gameTime)
    {
        if (Keyboard.GetState().IsKeyDown(Keys.A))
        {
            // rediger des log
            Debug.WriteLine("t nul");
        }

        int screenHeight = Graphics.PreferredBackBufferHeight;
        player.Update(gameTime, screenHeight);
    }
}