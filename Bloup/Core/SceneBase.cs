using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bloup.Core;

public abstract class SceneBase(SpriteBatch spriteBatch, GraphicsDeviceManager graphics)
{
    protected abstract string name { get; set; }
    protected SpriteBatch _spriteBatch = spriteBatch;
    protected GraphicsDeviceManager _graphics = graphics;

    public abstract void LoadContent(Microsoft.Xna.Framework.Content.ContentManager content);

    public abstract void Update(GameTime gameTime);

    public abstract void Draw(GameTime gameTime);

}