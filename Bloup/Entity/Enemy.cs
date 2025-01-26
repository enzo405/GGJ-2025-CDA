using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bloup.Entity
{
    public class Enemy : Sprite
    {
        public Rectangle _rectangle;
        protected float _speedX = 2f;      // Current horizontal velocity
        protected float _speedY = 0f;       // Current vertical velocity
        public bool _isDestroyed = false;

        public Enemy(Texture2D texture, Vector2 position, Rectangle rectangle, float scale) : base(texture, position, scale)
        {
            _position = position;
            _rectangle = rectangle;
            _scale = scale;
        }

        public override void Update(GameTime gameTime, int maxHeight, int minHeight)
        {
            if (_isDestroyed) return;

            base.Update(gameTime, maxHeight, minHeight);

            float timeModifiedSpeedX = gameTime.TotalGameTime.TotalSeconds switch
            {
                <= 10 => _speedX * 1f,
                <= 20 => _speedX * 1.2f,
                <= 30 => _speedX * 1.4f,
                <= 40 => _speedX * 1.6f,
                <= 50 => _speedX * 1.8f,
                <= 60 => _speedX * 2f,
                <= 70 => _speedX * 3f,
                <= 80 => _speedX * 5f,
                <= 90 => _speedX * 10f,
                _ => _speedX * 100f // Impossible mode
            };

            _position.X -= timeModifiedSpeedX;

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
