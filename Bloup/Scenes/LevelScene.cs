using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Bloup.Core;
using Bloup.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Bloup.Scenes;

public class LevelScene(ContentManager content, GraphicsDeviceManager graphics, GameStart game) : SceneBase(content, graphics, game)
{
    protected override string name { get; set; } = "LevelScene";

    // Add all ressource

    private Texture2D background;
    private Texture2D square_yellow;
    private Texture2D square_red;
    private Texture2D tile;

    private int tilePoint = 0;

    public override void LoadContent()
    {
        background = this.Content.Load<Texture2D>("backgrounds/Menu");
        square_yellow = this.Content.Load<Texture2D>("backgrounds/yellow_square");
        square_red = this.Content.Load<Texture2D>("backgrounds/red_square");
        tile = this.Content.Load<Texture2D>("asset_tuyeau");
    }

    public override void Update(GameTime gameTime)
    {

    }
    
    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();

        TileExtractorService tileExtractorService = new TileExtractorService(this.game.GraphicsDevice);

        List<Texture2D> tiles = tileExtractorService.ExtractTiles(tile, 32, 32);

        MapLoader mapLoader = new MapLoader();
        
        Debug.WriteLine(this.game.Content.RootDirectory);
        mapLoader.LoadMap("/tiles/tuyeau_set_bg.csv");

        // mapLoader

        int width = 0;
        int wMax = (int)Math.Floor((double)this.game.ScreenWidth / 24);
        int hMax = (int)Math.Floor((double)this.game.ScreenHeight / 10);
        if (wMax > hMax)
        {
            width = hMax;
        }
        else
        {
            width = wMax;
        }

        int xPosStart = (this.game.ScreenWidth - (width * 24)) / 2;
        int yPosStart = (this.game.ScreenHeight - (width * 10)) / 2;
        for (int y = 0; y < 10; y++)
        {
            int ypos = (width * y) + yPosStart;
            for (int x = 0; x < 24; x++)
            {
                int xpos = (width * x) + xPosStart;

                float scaleX = width / 32f;
                Debug.WriteLine($"xpos : {xpos} ypos : {ypos} scaleX : {scaleX} size : {width}");
                spriteBatch.Draw(texture: tiles[4],
                    position: new Vector2(xpos, ypos),
                    sourceRectangle: new Rectangle(0, 0, 32, 32),
                    color: Color.Gray,
                    rotation: 0,
                    origin: Vector2.One,
                    scale: new Vector2(scaleX, scaleX),
                    effects: SpriteEffects.None,
                    layerDepth: 0f);
            }
        }

        spriteBatch.End();
    }
}