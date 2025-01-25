﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Bloup.Core;

public abstract class SceneBase(ContentManager content, GraphicsDeviceManager graphics)
{
    protected abstract string name { get; set; }
    protected GraphicsDeviceManager Graphics = graphics;
    protected ContentManager Content = content;

    public string GetName()
    {
        return this.name;
    }

    public abstract void LoadContent();

    public abstract void Update(GameTime gameTime);

    public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);

}