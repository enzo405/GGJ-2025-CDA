using System.Collections.Generic;
using System.Linq;
using Bloup.Core;
using Bloup.Entity;
using System;
using Bloup.Core;
using Bloup.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace Bloup.Scenes;

public class LevelScene(ContentManager content, GraphicsDeviceManager graphics, GameStart game) : SceneBase(content, graphics, game)
{
    private readonly GraphicsDeviceManager _graphics = graphics;
    private readonly ContentManager _content = content;
    private readonly Random random = new();
    protected override string Name { get; set; } = "LevelScene";
    public Player player;
    public List<Rat> rats = new();
    public List<Screw> screws = new();

    // Cooldown settings
    private TimeSpan ratSpawnCooldown = TimeSpan.FromSeconds(1);
    private TimeSpan screwSpawnCooldown = TimeSpan.FromSeconds(2);
    private TimeSpan elapsedRatTime = TimeSpan.Zero;
    private TimeSpan elapsedScrewTime = TimeSpan.Zero;

    // Spawn limits
    private const int MaxRats = 5;
    private const int MaxScrews = 3;

    // Add all resources
    private Texture2D background;
    private Texture2D square_yellow;
    private Texture2D square_red;
    private Texture2D tile;

    private int tilePoint = 0;

    protected int MaxHeight;
    protected int MinHeight;
    protected int SpawnXPositionsEntity = (int)(game.ScreenWidth * 1.05f); // Spawn off screen

    public override void LoadContent()
    {
        tile = _content.Load<Texture2D>("asset_tuyeau");
        square_yellow = _content.Load<Texture2D>("backgrounds/yellow_square");
        square_red = _content.Load<Texture2D>("backgrounds/red_square");
        background = _content.Load<Texture2D>("backgrounds/Menu");

        // Player setup
        Texture2D playerTexture = _content.Load<Texture2D>("sprites/bubble");
        int spawnX = (int)(_graphics.PreferredBackBufferWidth / 5f - playerTexture.Width / 2);
        int spawnY = _graphics.PreferredBackBufferHeight / 2 - playerTexture.Height / 2;
        float scale = 2f;

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

        // Update existing rats
        foreach (Rat rat in rats.ToList())
        {
            rat.Update(gameTime, MaxHeight, MinHeight);
            if (rat._isDestroyed)
            {
                rats.Remove(rat);
            }
        }

        // Update existing screws
        foreach (Screw screw in screws.ToList())
        {
            screw.Update(gameTime, MaxHeight, MinHeight);
            if (screw._isDestroyed)
            {
                screws.Remove(screw);
            }
        }

        // Handle rat spawning
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
        MapLoader mapLoader = new();

        Debug.WriteLine(game.Content.RootDirectory);
        mapLoader.LoadMap("/tiles/tuyeau_set_bg.csv");

        // mapLoader

        int width;
        int wMax = (int)Math.Floor((double)game.ScreenWidth / numberOfTilesX);
        int hMax = (int)Math.Floor((double)game.ScreenHeight / numberOfTilesY);
        width = wMax > hMax ? hMax : wMax;

        int xPosStart = (game.ScreenWidth - (width * numberOfTilesX)) / 2;
        int yPosStart = (game.ScreenHeight - (width * numberOfTilesY)) / 2;

        MaxHeight = yPosStart;
        MinHeight = yPosStart + (numberOfTilesY * tileSize);

        for (int y = 0; y < numberOfTilesY; y++)
        {
            int ypos = (width * y) + yPosStart;
            for (int x = 0; x < numberOfTilesX; x++)
            {
                int xpos = (width * x) + xPosStart;

                float scaleX = width / tileSize;
                Debug.WriteLine($"xpos : {xpos} ypos : {ypos} scaleX : {scaleX} size : {width}");
                spriteBatch.Draw(texture: tiles[4],
                    position: new Vector2(xpos, ypos),
                    sourceRectangle: new Rectangle(0, 0, tileSize, tileSize),
                    color: Color.Gray,
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
        rats.Add(new Rat(
            enemyRatTexture,
            new Vector2(SpawnXPositionsEntity, GetRandomHeight()),
            new Rectangle(100, 100, enemyRatTexture.Width, enemyRatTexture.Height),
            scale
        ));
    }

    public void AddScrew()
    {
        Texture2D screwTexture = _content.Load<Texture2D>("sprites/screw");
        float scale = (float)random.NextDouble() * 1.5f + 1;
        screws.Add(new Screw(
            screwTexture,
            new Vector2(SpawnXPositionsEntity, GetRandomHeight()),
            new Rectangle(100, 100, screwTexture.Width, screwTexture.Height),
            scale
        ));
    }

    public int GetRandomHeight()
    {
        int randomHeight = random.Next(MaxHeight, MinHeight);
        return randomHeight;
    }
}