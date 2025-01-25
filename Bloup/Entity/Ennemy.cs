using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Bloup.Entity
{
    public class Ennemy : Sprite
    {
        public Rectangle _rectangle;
        protected float _scale; // Scale factor for the texture
        protected float _speedX = 2f;      // Current horizontal velocity
        protected float _speedY = 0f;       // Current vertical velocity
        private GraphicsDeviceManager _graphics;

        public bool _isDestroyed = false;

        public Ennemy(Texture2D texture, Vector2 position, Rectangle rectangle, float scale, GraphicsDeviceManager graphics) : base(texture, position)
        {
            _position = position;
            _rectangle = rectangle;
            _scale = scale;
            _graphics = graphics;
        }

        public override void Update(GameTime gameTime, int screenHeight)
        {
            if (_isDestroyed) return;

            base.Update(gameTime, screenHeight);

            _position.X -= _speedX;

            _rectangle.X = (int)_position.X;

            if (_position.X < -100)
            {
                _isDestroyed = true;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // Draw the texture at the current position using the scale
            spriteBatch.Draw(_texture, _position, null, Color.White, 0f, Vector2.Zero, _scale, SpriteEffects.None, 0f);
        }
    }
}
