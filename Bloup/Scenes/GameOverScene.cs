using Bloup.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Bloup.Scenes;

public class GameOverScene(ContentManager content, GraphicsDeviceManager graphics, GameStart game) : SceneBase(content, graphics, game)
{
    protected override string Name { get; set; } = "GameOverScene";

    // Add all ressource

    private Texture2D background;
    protected GameStart game;

    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        spriteBatch.Begin();
        spriteBatch.Draw(background, new Vector2(0, 0), Color.Aqua);
        spriteBatch.End();
    }

    public override void LoadContent()
    {
        background = Content.Load<Texture2D>("backgrounds/Menu");
    }

    public override void Update(GameTime gameTime)
    {
    }
}