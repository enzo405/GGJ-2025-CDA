using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Bloup.Core;

public abstract class SceneBase(GraphicsDeviceManager graphics)
{
    protected abstract string name { get; set; }
    protected GraphicsDeviceManager Graphics = graphics;

    public string GetName()
    {
        return this.name;
    }

    public abstract void LoadContent(ContentManager content);

    public abstract void Update(GameTime gameTime);

    public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);

}