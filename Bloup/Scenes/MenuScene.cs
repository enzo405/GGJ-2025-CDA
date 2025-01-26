using System;
using System.Diagnostics;
using Bloup.Core;
using Bloup.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Bloup.Scenes;

public class MenuScene(ContentManager content, GraphicsDeviceManager graphics, GameStart game) : SceneBase(content, graphics, game)
{
    protected override string Name { get; set; } = "MenuScene";

    // Add all ressource
    private Texture2D background;
    private Texture2D startButton;
    private Rectangle startButtonRect;
    private Rectangle exitButtonRect;
    private Texture2D exitButton;
    private Color startButtonColor = Color.White;
    private Color exitButtonColor = Color.White;
    private int rectScaleX;
    private int rectScaleY;
    private MouseState currentMouseState;
    private MouseState previousMouseState;


    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        rectScaleX = game.ScreenWidth / 1 / 5;
        rectScaleY = game.ScreenHeight / 2 / 3;

        spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        spriteBatch.Draw(background, new Rectangle(0, 0, game.ScreenWidth, game.ScreenHeight), Color.White);
        spriteBatch.Draw(startButton, new Rectangle(rectScaleX / 2, rectScaleY / 2, startButton.Width * 10, 10 * startButton.Height), startButtonColor);
        spriteBatch.Draw(exitButton, new Rectangle(rectScaleX / 2, rectScaleY / 2 + 250, exitButton.Width * 10, 10 * exitButton.Height), exitButtonColor);

        spriteBatch.End();
    }
    public override void LoadContent()
    {
        currentMouseState = Mouse.GetState();
        background = Content.Load<Texture2D>("backgrounds/menu_bloup");
        startButton = Content.Load<Texture2D>("backgrounds/start");
        exitButton = Content.Load<Texture2D>("backgrounds/exit");
    }

    public override void Update(GameTime gameTime)
    {

        previousMouseState = currentMouseState;
        currentMouseState = Mouse.GetState();

        startButtonRect = new Rectangle(
            rectScaleX / 2,
            rectScaleY / 2,
            startButton.Width * 10,
            startButton.Height * 10
        );

        exitButtonRect = new Rectangle(
            rectScaleX / 2,
            rectScaleY / 2 + 250,
            exitButton.Width * 10,
            exitButton.Height * 10
        );

        if (startButtonRect.Contains(currentMouseState.Position))
        {
            startButtonColor = Color.Yellow;
        }
        else
        {
            startButtonColor = Color.White;
        }

        if (exitButtonRect.Contains(currentMouseState.Position))
        {
            exitButtonColor = Color.Yellow;
        }
        else
        {
            exitButtonColor = Color.White;
        }

        if (IsMouseClicked() && startButtonRect.Contains(currentMouseState.Position))
        {
            SceneManager.Create(game).ChangeScene("LevelScene");
        }
        Console.WriteLine(IsMouseClicked());
        if (IsMouseClicked() && exitButtonRect.Contains(currentMouseState.Position))
        {
            game.Exit();
        }
    }

    private bool IsMouseClicked()
    {
        return previousMouseState.LeftButton == ButtonState.Pressed &&
               currentMouseState.LeftButton == ButtonState.Released;
    }
}