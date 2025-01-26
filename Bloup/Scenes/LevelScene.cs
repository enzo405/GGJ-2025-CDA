using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using Bloup.Core;
using Bloup.Entity;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Bloup.Scenes;

public class LevelScene(ContentManager content, GraphicsDeviceManager graphics, GameStart game) : SceneBase(content, graphics, game)
{
    private readonly GraphicsDeviceManager _graphics = graphics;
    private readonly ContentManager _content = content;
    protected override string Name { get; set; } = "LevelScene";
    public Player player;
    public List<Rat> rats = [];
    public List<Screw> screws = [];

    // Add all ressource
    private Texture2D background;

    private Texture2D square_yellow;
    private Texture2D square_red;


    public override void LoadContent()
    {
        square_yellow = _content.Load<Texture2D>("backgrounds/yellow_square");
        square_red = _content.Load<Texture2D>("backgrounds/red_square");
        background = _content.Load<Texture2D>("backgrounds/Menu");
        // Player
        Texture2D playerTexture = _content.Load<Texture2D>("sprites/bubble");
        int spawnX = (int)(_graphics.PreferredBackBufferWidth / 5f - playerTexture.Width / 2);
        int spawnY = _graphics.PreferredBackBufferHeight / 2 - playerTexture.Height / 2;
        float scale = 2f; // Change this value to scale your texture

        player = new Player(
            playerTexture,
            new Vector2(spawnX, spawnY), // SpawnPosition
            new Rectangle(spawnX, spawnY, playerTexture.Width, playerTexture.Height), // Hitbox using original size
            scale // Pass the scale factor
        );

        AddRats();
        AddScrews();
    }


    public override void Update(GameTime gameTime)
    {
        int screenHeight = Graphics.PreferredBackBufferHeight;
        player.Update(gameTime, screenHeight);
        foreach (Rat rat in rats.ToList())
        {
            rat.Update(gameTime, screenHeight);
            if (rat._isDestroyed)
            {
                rats.Remove(rat);
            }
        }
        foreach (Screw screw in screws.ToList())
        {
            screw.Update(gameTime, screenHeight);
            if (screw._isDestroyed)
            {
                screws.Remove(screw);
            }
        }
    }


    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();
        spriteBatch.Draw(background, new Vector2(0, 0), Color.Aqua);
        player.Draw(spriteBatch);
        foreach (Rat rat in rats)
        {
            rat.Draw(spriteBatch);
        }
        foreach (Screw screw in screws)
        {
            screw.Draw(spriteBatch);
        }

        int width = 0;
        int wMax = (int)Math.Floor((double)game.ScreenWidth / 24);
        int hMax = (int)Math.Floor((double)game.ScreenHeight / 10);
        if (wMax > hMax)
        {
            width = hMax;
        }
        else
        {
            width = wMax;
        }
        int xPosStart = (game.ScreenWidth - (width * 24)) / 2;
        int yPosStart = (game.ScreenHeight - (width * 10)) / 2; ;
        Debug.WriteLine($"width : {width} xPosStart : {xPosStart} yPosStart : {yPosStart}");
        for (int y = 0; y < 10; y++)
        {
            int ypos = (width * y) + yPosStart;
            for (int x = 0; x < 24; x++)
            {
                int xpos = (width * x) + xPosStart;
                if (y % 2 == 0 && x % 2 == 0)
                {
                    spriteBatch.Draw(square_red, new Vector2(xpos, ypos), new Rectangle(0, 0, width, width), Color.Black);
                }
                else if (y % 2 != 0 && x % 2 != 0)
                {
                    spriteBatch.Draw(square_yellow, new Vector2(xpos, ypos), new Rectangle(0, 0, width, width), Color.Black);
                }
                else
                {
                    spriteBatch.Draw(square_yellow, new Vector2(xpos, ypos), new Rectangle(0, 0, width, width), Color.Azure);
                }
            }
        }

        spriteBatch.End();
    }

    public void AddRats()
    {
        Texture2D EnemyRatTexture = _content.Load<Texture2D>("sprites/swimming_rat");
        float scale = 2f; // Change this value to scale your texture
        rats.Add(new Rat(
            EnemyRatTexture,
            new Vector2(700, 100), // SpawnPosition
            new Rectangle(100, 100, EnemyRatTexture.Width, EnemyRatTexture.Height),// Hitbox using original size
            scale // Pass the scale factor
        ));
    }

    public void AddScrews()
    {
        Texture2D screwTexture = _content.Load<Texture2D>("sprites/screw");
        float scale = 2f; // Change this value to scale your texture
        screws.Add(new Screw(
            screwTexture,
            new Vector2(700, 100), //SpawnPosition
            new Rectangle(100, 100, screwTexture.Width, screwTexture.Height),// Hitbox using original size
            scale // Pass the scale factor
        ));
    }
}