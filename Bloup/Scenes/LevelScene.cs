using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using Bloup.Core;
using Bloup.Entity;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Bloup.Scenes;

public class LevelScene(ContentManager content, GraphicsDeviceManager graphics) : SceneBase(content, graphics)
{
    private readonly GraphicsDeviceManager _graphics = graphics;
    private readonly ContentManager _content = content;
    protected override string Name { get; set; } = "LevelScene";
    public Player? player;
    public Rat? ennemyRat;
    public Screw? ennemyScrew;

    public List<Rat> rats = [];

    // Add all ressource

    private Texture2D? background;

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();
        spriteBatch.Draw(background, new Vector2(0, 0), Color.Aqua);
        player?.Draw(spriteBatch);
        foreach (var rat in rats)
        {
            rat.Draw(spriteBatch);
        }
        ennemyScrew?.Draw(spriteBatch);
        spriteBatch.End();
    }

    public override void LoadContent()
    {
        background = _content.Load<Texture2D>("backgrounds/Menu");
        // Player
        Texture2D playerTexture = _content.Load<Texture2D>("sprites/bubble");
        Texture2D ennemyRatTexture = _content.Load<Texture2D>("sprites/swimming_rat");
        Texture2D ennemyScrewTexture = _content.Load<Texture2D>("sprites/screw");
        int spawnX = (int)(_graphics.PreferredBackBufferWidth / 5f - playerTexture.Width / 2);
        int spawnY = _graphics.PreferredBackBufferHeight / 2 - playerTexture.Height / 2;
        float scale = 2f; // Change this value to scale your texture

        player = new Player(
            playerTexture,
            new Vector2(spawnX, spawnY), // Position
            new Rectangle(spawnX, spawnY, playerTexture.Width, playerTexture.Height), // Hitbox using original size
            scale // Pass the scale factor
        );


        AddRat();

        ennemyScrew = new Screw(
            ennemyScrewTexture,
            new Vector2(700, 300), //SpawnPosition
            new Rectangle(100, 100, ennemyScrewTexture.Width, ennemyScrewTexture.Height),// Hitbox using original size
            scale, // Pass the scale factor
            _graphics
        );
    }

    public override void Update(GameTime gameTime)
    {
        if (Keyboard.GetState().IsKeyDown(Keys.A))
        {
            // rediger des log
            // Debug.WriteLine("t nul");
        }

        int screenHeight = Graphics.PreferredBackBufferHeight;
        player?.Update(gameTime, screenHeight);
        foreach (var rat in rats.ToList())
        {
            rat.Update(gameTime, screenHeight);
            if (rat._isDestroyed)
            {
                rats.Remove(rat);
            }
        }
    }

    public void AddRat()
    {
        Texture2D ennemyRatTexture = _content.Load<Texture2D>("sprites/swimming_rat");
        float scale = 2f; // Change this value to scale your texture
        rats.Add(new Rat(
            ennemyRatTexture,
            new Vector2(700, 100), //SpawnPosition
            new Rectangle(100, 100, ennemyRatTexture.Width, ennemyRatTexture.Height),// Hitbox using original size
            scale, // Pass the scale factor
            _graphics
        ));
    }
}