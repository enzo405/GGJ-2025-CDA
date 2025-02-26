﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Bloup.Core;

public abstract class SceneBase(ContentManager content, GraphicsDeviceManager graphics, GameStart game)
{
    protected abstract string Name { get; set; }
    protected GraphicsDeviceManager Graphics = graphics;
    protected ContentManager Content = content;
    protected GameStart game = game;

    public string GetName()
    {
        return Name;
    }

    public abstract void LoadContent();

    public abstract void Update(GameTime gameTime);

    public abstract void Draw(GameTime gameTime, SpriteBatch spriteBatch);

}