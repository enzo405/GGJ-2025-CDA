using System.Collections.Generic;
using System.Linq;
using Bloup.Core;
using Bloup.Entity;
using System;
using Bloup.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Bloup.Managers;

namespace Bloup.Scenes;

public class LevelScene(ContentManager content, GraphicsDeviceManager graphics, GameStart game) : SceneBase(content, graphics, game)
{
    private readonly GraphicsDeviceManager _graphics = graphics;
    private readonly ContentManager _content = content;
    private readonly Random random = new();
    protected override string Name { get; set; } = "LevelScene";
    public Player player;
    public List<Rat> rats = [];
    public List<Screw> screws = [];

    // Cooldown settings
    private TimeSpan ratSpawnCooldown = TimeSpan.FromSeconds(1);
    private TimeSpan screwSpawnCooldown = TimeSpan.FromSeconds(2);
    private TimeSpan elapsedRatTime = TimeSpan.Zero;
    private TimeSpan elapsedScrewTime = TimeSpan.Zero;

    // Spawn limits
    private const int MaxRats = 5;
    private const int MaxScrews = 3;

    // Add all resources
    private Texture2D tile;

    protected int MaxHeight = game.ScreenHeight / 2 - 100;
    protected int MinHeight = game.ScreenHeight / 2 + 100;

    protected int MaxWidth = game.ScreenWidth;
    protected int SpawnXPositionsEntity = (int)(game.ScreenWidth * 1.05f); // Spawn off screen

    public override void LoadContent()
    {
        tile = _content.Load<Texture2D>("asset_tuyeau");
        // Player setup
        Texture2D playerTexture = _content.Load<Texture2D>("sprites/bubble");
        int spawnX = (int)(_graphics.PreferredBackBufferWidth / 5f - playerTexture.Width / 2);
        int spawnY = _graphics.PreferredBackBufferHeight / 2 - playerTexture.Height / 2;
        float scale = 2f;

        player = new Player(
            playerTexture,
            new Vector2(spawnX, spawnY), // SpawnPosition
            new Rectangle(spawnX, spawnY, playerTexture.Width, playerTexture.Height), // Hitbox using original size
            scale, // Pass the scale factor
            MaxWidth
        );

        AddRat();
        AddScrew();
    }

    public override void Update(GameTime gameTime)
    {
        if (player.IsFinished)
        {
            GameOver();
        }

        player.Update(gameTime, MaxHeight, MinHeight);
        foreach (Rat rat in rats.ToList())
        {
            rat.Update(gameTime, MaxHeight, MinHeight);
            player.CheckCollision(rat);
            if (rat._isDestroyed)
            {
                rats.Remove(rat);
            }
        }
        foreach (Screw screw in screws.ToList())
        {
            screw.Update(gameTime, MaxHeight, MinHeight);
            player.CheckCollision(screw);
            if (screw._isDestroyed)
            {
                screws.Remove(screw);
            }
        }
        elapsedRatTime += gameTime.ElapsedGameTime;
        if (elapsedRatTime >= ratSpawnCooldown && rats.Count < MaxRats)
        {
            AddRat();
            elapsedRatTime = TimeSpan.Zero;
        }

        // Handle screw spawning
        elapsedScrewTime += gameTime.ElapsedGameTime;
        if (elapsedScrewTime >= screwSpawnCooldown && screws.Count < MaxScrews)
        {
            AddScrew();
            elapsedScrewTime = TimeSpan.Zero;
        }
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        int numberOfTilesX = 24;
        int numberOfTilesY = 10;
        int tileSize = 32;

        TileExtractorService tileExtractorService = new(game.GraphicsDevice);
        List<Texture2D> tiles = tileExtractorService.ExtractTiles(tile, tileSize, tileSize);

        string basePath = AppDomain.CurrentDomain.BaseDirectory;
        string mapPath = Path.Combine(basePath, "Content/tiles/tuyeau_set_bg.csv");

        MapLoader mapLoader = new();
        mapLoader.LoadMap(mapPath);

        int wMax = (int)Math.Floor((double)game.ScreenWidth / numberOfTilesX);
        int hMax = (int)Math.Floor((double)game.ScreenHeight / numberOfTilesY);
        int width = wMax > hMax ? hMax : wMax;

        int xPosStart = (game.ScreenWidth - (width * numberOfTilesX)) / 2;
        int yPosStart = (game.ScreenHeight - (width * numberOfTilesY)) / 2;

        MaxHeight = yPosStart;
        MinHeight = yPosStart + (numberOfTilesY * width);

        spriteBatch.Begin();

        for (int y = 0; y < numberOfTilesY; y++)
        {
            int ypos = (width * y) + yPosStart;
            for (int x = 0; x < numberOfTilesX; x++)
            {
                int xpos = (width * x) + xPosStart;
                float scaleX = width / tileSize;
                int idTitle = mapLoader.TileMap[new Vector2(x, y)];

                spriteBatch.Draw(texture: tiles[idTitle],
                    position: new Vector2(xpos, ypos),
                    sourceRectangle: new Rectangle(0, 0, width, width),
                    color: Color.White,
                    rotation: 0,
                    origin: Vector2.One,
                    scale: new Vector2(scaleX, scaleX),
                    effects: SpriteEffects.None,
                    layerDepth: 0f);
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
        float scale = (float)random.NextDouble() * 3 + 1;
        int height = GetRandomHeight();
        rats.Add(new Rat(
            enemyRatTexture,
            new Vector2(SpawnXPositionsEntity, height),
            new Rectangle(SpawnXPositionsEntity, height, enemyRatTexture.Width, enemyRatTexture.Height),
            scale
        ));
    }

    public void AddScrew()
    {
        Texture2D screwTexture = _content.Load<Texture2D>("sprites/screw");
        float scale = (float)random.NextDouble() * 1.5f + 1;
        int height = GetRandomHeight();
        screws.Add(new Screw(
            screwTexture,
            new Vector2(SpawnXPositionsEntity, height),
            new Rectangle(SpawnXPositionsEntity, height, screwTexture.Width, screwTexture.Height),
            scale
        ));
    }

    public int GetRandomHeight()
    {
        int randomHeight = random.Next(MaxHeight, MinHeight);
        return randomHeight;
    }

    public void GameOver()
    {
        rats.Clear();
        screws.Clear();
        SceneManager.Create(game).ChangeScene("GameOverScene");
    }
}