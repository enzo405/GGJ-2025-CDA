using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Bloup.Core
{
    public class Ennemy : Sprite
    {
        public Rectangle _rectangle;
        protected float _scale; // Scale factor for the texture
        protected float _speedX = 2f;      // Current horizontal velocity
        protected float _speedY = 0f;       // Current vertical velocity

        private bool _movingRight = false;  // Direction du mouvement
        private GraphicsDeviceManager _graphics;

        public Ennemy(Texture2D texture, Vector2 position, Rectangle rectangle, float scale, GraphicsDeviceManager graphics) : base(texture, position)
        {
            _position = position;
            _rectangle = rectangle;
            _scale = scale;
            _graphics = graphics;
        }

        public override void Update(GameTime gameTime, int screenHeight)
        {
            Debug.WriteLine($"{_position.X} le rat");

            base.Update(gameTime, screenHeight);

            // Mouvement horizontal
            if (_movingRight)
            {
                _position.X += _speedX;
                Debug.WriteLine($"{_position.X} deplacement rat");
                // Vérifie si le rat atteint le bord droit
                if (_position.X + _rectangle.Width * _scale >= _graphics.PreferredBackBufferWidth)
                {
                    _movingRight = false;
                }
            }
            else
            {
                _position.X -= _speedX;
                Debug.WriteLine($"{_position.X} deplacement rat");

                // Vérifie si le rat atteint le bord gauche
                if (_position.X <= 0)
                {
                    _movingRight = true;
                }
            }

            // Met à jour le rectangle de collision
            _rectangle.X = (int)_position.X;
            _rectangle.Y = (int)_position.Y;

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // Draw the texture at the current position using the scale
            spriteBatch.Draw(_texture, _position, null, Color.White, 0f, Vector2.Zero, _scale, SpriteEffects.None, 0f);
        }
    }
}
