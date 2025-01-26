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
using MonoGame.Extended;
using Microsoft.Xna.Framework.Media;

namespace Bloup.Scenes;

public class LevelScene(ContentManager content, GraphicsDeviceManager graphics, GameStart game) : SceneBase(content, graphics, game)
{
    private readonly GraphicsDeviceManager _graphics = graphics;
    private readonly ContentManager _content = content;
    private readonly Random random = new();
    protected override string Name { get; set; } = "LevelScene";
    protected TimeSpan Timer { get; set; } = TimeSpan.Zero;
    public Player player;
    public List<Rat> rats = [];
    public List<Screw> screws = [];
    public List<Wave> waves = [];
    public List<Shit> shits = [];

    // Cooldown settings
    private TimeSpan ratSpawnCooldown = TimeSpan.FromSeconds(3);
    private TimeSpan screwSpawnCooldown = TimeSpan.FromSeconds(3);
    private TimeSpan waveSpawnCooldown = TimeSpan.FromSeconds(1);
    private TimeSpan shitSpawnCooldown = TimeSpan.FromSeconds(3);
    private TimeSpan elapsedRatTime = TimeSpan.Zero;
    private TimeSpan elapsedScrewTime = TimeSpan.Zero;
    private TimeSpan eslapsedWaveTime = TimeSpan.Zero;
    private TimeSpan elapsedShitTime = TimeSpan.Zero;

    // Spawn limits
    private const int MaxRats = 5;
    private const int MaxScrews = 3;
    private const int MaxWaves = 12;
    private const int MaxShits = 3;

    // Add all resources
    private Texture2D tile;
    private Song music;

    protected float MaxHeight = game.ScreenHeight / 2 - 100;
    protected float MinHeight = game.ScreenHeight / 2 + 100;
    protected int MaxWidth = game.ScreenWidth;
    protected int SpawnXPositionsEntity = (int)(game.ScreenWidth * 1.05f); // Spawn off screen

    protected int pointeur = 0;
    protected float elapsedFrameTime;
    private SpriteFont font;

    public override void LoadContent()
    {
        tile = _content.Load<Texture2D>("asset_tuyeau");
        music = _content.Load<Song>("bubble");
        font = Content.Load<SpriteFont>("fonts/Font");
        MediaPlayer.Play(music);
        MediaPlayer.IsRepeating = true;

        // Player setup
        Texture2D playerTexture = _content.Load<Texture2D>("sprites/bubble-animation");
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
        AddWave();
        AddShit();
    }

    public override void Update(GameTime gameTime)
    {
        if (player.IsFinished)
        {
            GameOver();
        }

        Timer += gameTime.ElapsedGameTime;
        player.Update(gameTime, (int)MaxHeight, (int)MinHeight);
        foreach (Rat rat in rats.ToList())
        {
            rat.Update(gameTime, (int)MaxHeight, (int)MinHeight);
            player.CheckCollision(rat);
            if (rat._isDestroyed)
            {
                rats.Remove(rat);
            }
        }
        foreach (Screw screw in screws.ToList())
        {
            screw.Update(gameTime, (int)MaxHeight, (int)MinHeight);
            player.CheckCollision(screw);
            if (screw._isDestroyed)
            {
                screws.Remove(screw);
            }
        }
        foreach (Wave wave in waves.ToList())
        {
            wave.Update(gameTime, (int)MaxHeight, (int)MinHeight);
        }

        foreach (Shit shit in shits.ToList())
        {
            shit.Update(gameTime, (int)MaxHeight, (int)MinHeight);
            player.CheckCollision(shit);
            if (shit._isDestroyed)
            {
                shits.Remove(shit);
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

        // Handle wave spawning
        eslapsedWaveTime += gameTime.ElapsedGameTime;
        if (eslapsedWaveTime >= waveSpawnCooldown && waves.Count < MaxWaves)
        {
            AddWave();
            eslapsedWaveTime = TimeSpan.Zero;
        }

        elapsedShitTime += gameTime.ElapsedGameTime;
        if (elapsedShitTime >= shitSpawnCooldown && shits.Count < MaxShits)
        {
            AddShit();
            elapsedShitTime = TimeSpan.Zero;
        }

        elapsedFrameTime += gameTime.GetElapsedSeconds(); // Ajout du temps �coul� � chaque frame
        if (elapsedFrameTime >= 0.5f) // V�rifie si une seconde s'est �coul�e
        {
            elapsedFrameTime -= 0.5f; // R�initialise le compteur d'une seconde

            // Met � jour le pointeur
            if (pointeur > 9)
            {
                pointeur = 0; // R�initialise le pointeur
            }
            else
            {
                pointeur++; // Incr�mente le pointeur
            }
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

        float scaleX = width / tileSize;
        float xPosStart = (game.ScreenWidth - (tileSize * scaleX * numberOfTilesX)) / 2;
        float yPosStart = (game.ScreenHeight - (tileSize * scaleX * numberOfTilesY)) / 2;

        MaxHeight = yPosStart;
        MinHeight = yPosStart + (tileSize * scaleX * numberOfTilesY);

        spriteBatch.Begin();

        for (int y = 0; y < numberOfTilesY; y++)
        {
            float ypos = (y * tileSize * scaleX) + yPosStart;

            for (int x = 0; x < numberOfTilesX; x++)
            {
                int rx = x + pointeur;

                if (rx >= numberOfTilesX)
                {
                    rx -= numberOfTilesX;
                }

                float xpos = (x * tileSize * scaleX) + xPosStart;
                int idTitle = mapLoader.TileMap[new Vector2(rx, y)];
                spriteBatch.Draw(texture: tiles[idTitle],
                    position: new Vector2(xpos, ypos),
                    sourceRectangle: new Rectangle(0, 0, tileSize, tileSize),
                    color: Color.White,
                    rotation: 0,
                    origin: Vector2.Zero,
                    scale: new Vector2(scaleX, scaleX),
                    effects: SpriteEffects.None,
                    layerDepth: 0f
                    );
            }
        }

        player.Draw(spriteBatch);
        spriteBatch.DrawString(font, $"Timer: {Timer.Minutes}:{Timer.Seconds}", new Vector2(10, 10), Color.White);

        foreach (Rat rat in rats)
        {
            rat.Draw(spriteBatch);
        }
        foreach (Screw screw in screws)
        {
            screw.Draw(spriteBatch);
        }
        foreach (Wave wave in waves)
        {
            wave.Draw(spriteBatch);
        }
        foreach (Shit shit in shits)
        {
            shit.Draw(spriteBatch);
        }

        spriteBatch.End();
    }

    public void AddRat()
    {
        Texture2D enemyRatTexture = _content.Load<Texture2D>("sprites/swimming_rat");
        float scale = (float)random.NextDouble() * 5 + 1;
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

    public void AddWave()
    {
        Texture2D waveTexture = _content.Load<Texture2D>("sprites/vagounette");
        int height = GetRandomHeight();
        waves.Add(new Wave(
            waveTexture,
            new Vector2(SpawnXPositionsEntity, height),
            new Rectangle(SpawnXPositionsEntity, height, waveTexture.Width, waveTexture.Height),
            2f
        ));
    }

    public void AddShit()
    {
        Texture2D shitTexture = _content.Load<Texture2D>("sprites/shit");
        float scale = (float)random.NextDouble() * 2f + 1;
        int height = GetRandomHeight();
        shits.Add(new Shit(
            shitTexture,
            new Vector2(SpawnXPositionsEntity, height),
            new Rectangle(SpawnXPositionsEntity, height, shitTexture.Width, shitTexture.Height),
            scale
        ));
    }

    public int GetRandomHeight()
    {
        int randomHeight = random.Next((int)MaxHeight + 60, (int)MinHeight - 60);
        return randomHeight;
    }

    public void GameOver()
    {
        Timer = TimeSpan.Zero;
        MediaPlayer.Stop();
        MediaPlayer.Pause();
        rats.Clear();
        screws.Clear();
        shits.Clear();
        SceneManager.Create(game).ChangeScene("GameOverScene");
    }
}