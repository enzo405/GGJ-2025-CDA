using System.Collections.Generic;
using System.Linq;
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
    protected int MaxHeight;
    protected int MinHeight;
    protected int SpawnXPositionsEntity = (int)(game.ScreenWidth * 1.05f); // Spawn off screen

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

        AddRat();
        AddScrew();
    }


    public override void Update(GameTime gameTime)
    {
        player.Update(gameTime, MaxHeight, MinHeight);
        foreach (Rat rat in rats.ToList())
        {
            rat.Update(gameTime, MaxHeight, MinHeight);
            if (rat._isDestroyed)
            {
                rats.Remove(rat);
            }
        }
        foreach (Screw screw in screws.ToList())
        {
            screw.Update(gameTime, MaxHeight, MinHeight);
            if (screw._isDestroyed)
            {
                screws.Remove(screw);
            }
        }
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        int numberOfTilesX = 24;
        int numberOfTilesY = 10;

        spriteBatch.Begin();
        int tileSize;
        int wMax = (int)Math.Floor((double)game.ScreenWidth / numberOfTilesX);
        int hMax = (int)Math.Floor((double)game.ScreenHeight / numberOfTilesY);
        if (wMax > hMax)
        {
            tileSize = hMax;
        }
        else
        {
            tileSize = wMax;
        }
        int xPosStart = (game.ScreenWidth - (tileSize * numberOfTilesX)) / 2;
        int yPosStart = (game.ScreenHeight - (tileSize * numberOfTilesY)) / 2;

        MaxHeight = yPosStart;
        MinHeight = yPosStart + (numberOfTilesY * tileSize);

        for (int y = 0; y < numberOfTilesY; y++)
        {
            int ypos = (tileSize * y) + yPosStart;
            for (int x = 0; x < numberOfTilesX; x++)
            {
                int xpos = (tileSize * x) + xPosStart;
                if (y % 2 == 0 && x % 2 == 0)
                {
                    spriteBatch.Draw(square_red, new Vector2(xpos, ypos), new Rectangle(0, 0, tileSize, tileSize), Color.Black);
                }
                else if (y % 2 != 0 && x % 2 != 0)
                {
                    spriteBatch.Draw(square_yellow, new Vector2(xpos, ypos), new Rectangle(0, 0, tileSize, tileSize), Color.Black);
                }
                else
                {
                    spriteBatch.Draw(square_yellow, new Vector2(xpos, ypos), new Rectangle(0, 0, tileSize, tileSize), Color.Azure);
                }
            }
        }

        player.Draw(spriteBatch);
        foreach (Rat rat in rats)
        {
            rat.Draw(spriteBatch);
        }
        foreach (Screw screw in screws)
        {
            screw.Draw(spriteBatch);
        }

        spriteBatch.End();
    }

    public void AddRat()
    {
        Texture2D enemyRatTexture = _content.Load<Texture2D>("sprites/swimming_rat");
        float scale = 2f; // Change this value to scale your texture
        rats.Add(new Rat(
            enemyRatTexture,
            new Vector2(SpawnXPositionsEntity, GetRandomHeight()), // SpawnPosition
            new Rectangle(100, 100, enemyRatTexture.Width, enemyRatTexture.Height),// Hitbox using original size
            scale // Pass the scale factor
        ));
    }

    public void AddScrew()
    {
        Texture2D screwTexture = _content.Load<Texture2D>("sprites/screw");
        float scale = 2f; // Change this value to scale your texture
        screws.Add(new Screw(
            screwTexture,
            new Vector2(SpawnXPositionsEntity, GetRandomHeight()), //SpawnPosition
            new Rectangle(100, 100, screwTexture.Width, screwTexture.Height),// Hitbox using original size
            scale // Pass the scale factor
        ));
    }

    public int GetRandomHeight()
    {
        Random random = new();
        return random.Next(MaxHeight, MinHeight);
    }
}