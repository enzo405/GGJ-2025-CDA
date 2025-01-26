using Bloup.Core;
using Bloup.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Bloup.Scenes;

public class GameOverScene(ContentManager content, GraphicsDeviceManager graphics, GameStart game) : SceneBase(content, graphics, game)
{
    protected override string Name { get; set; } = "GameOverScene";

    // Add all ressource

    private Texture2D background;
    protected GameStart game;
    protected SpriteFont font;

    public override void LoadContent()
    {
        background = Content.Load<Texture2D>("backgrounds/Menu");
        font = Content.Load<SpriteFont>("fonts/Font");
    }

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();
        spriteBatch.DrawString(font, "Game Over", new Vector2(game?.ScreenHeight ?? 100, game?.ScreenWidth ?? 100), Color.White, 0, Vector2.Zero, 2, SpriteEffects.None, 0);
        spriteBatch.DrawString(font, "Press any key to restart", new Vector2(game?.ScreenHeight ?? 200, game?.ScreenWidth ?? 200), Color.Purple, 0, Vector2.Zero, 2, SpriteEffects.None, 0);

        spriteBatch.End();
    }

    public override void Update(GameTime gameTime)
    {
        if (Keyboard.GetState().GetPressedKeys().Length > 0)
        {
            SceneManager.Create(game).ChangeScene("LevelScene");
        }
    }
}