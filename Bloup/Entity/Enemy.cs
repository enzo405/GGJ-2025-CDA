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
