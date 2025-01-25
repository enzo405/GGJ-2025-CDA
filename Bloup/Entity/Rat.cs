using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Bloup.Core
{
    public class Rat : Ennemy
    {
        public Rat(Texture2D texture, Vector2 position, Rectangle rectangle, float scale, GraphicsDeviceManager graphics) : base(texture, position, rectangle, scale, graphics)
        {
        }

        public override void Update(GameTime gameTime, int screenHeight)
        {
            base.Update(gameTime, screenHeight);
            // Debug.WriteLine("Sprite Update");
        }
    }
}