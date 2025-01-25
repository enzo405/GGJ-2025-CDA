using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Bloup.Core;
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

    public override void LoadContent()
    {
        background = this.Content.Load<Texture2D>("backgrounds/Menu");
        square_yellow = this.Content.Load<Texture2D>("backgrounds/yellow_square");
        square_red = this.Content.Load<Texture2D>("backgrounds/red_square");

    }

    public override void Update(GameTime gameTime)
    {

    }
    
    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();

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
        int yPosStart = (this.game.ScreenHeight - (width * 10)) / 2; ;
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
}