using System;
using Bloup.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bloup.Entity
{
    public class Screw : Ennemy
    {
        public Screw(Texture2D texture, Vector2 position, Rectangle rectangle, float scale, GraphicsDeviceManager graphics)
            : base(texture, position, rectangle, scale, graphics)
        {
            _speedX = 3f; // Adjust screw speed
        }

        public override void Update(GameTime gameTime, int screenHeight)
        {
            Console.WriteLine($"{_position.X} le screw");
            base.Update(gameTime, screenHeight);

            _position.X -= _speedX; // Move right instead of left

            // if (_position.X > _graphics.PreferredBackBufferWidth)
            // {
            //     _position.X = -_texture.Width * _scale; // Reset position
            // }

            _rectangle.X = (int)_position.X;
            _rectangle.Y = (int)_position.Y;
        }
    }

}