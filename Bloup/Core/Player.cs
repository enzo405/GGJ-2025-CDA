using System;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Bloup.Core
{
    public class Player : Sprite
    {
        protected Rectangle _rectangle;
        protected float _speed = 0.05f;
        protected float _gravity = 0.1f;
        protected float _propulsionForce = -4f; // Upward force when propelling
        protected float _downwardForce = 3f;   // Downward force when moving down
        protected float _velocityY = 0f;       // Current vertical velocity
        protected bool _isPropelling = false;  // Whether the player is propelling up
        protected bool _isDescending = false;  // Whether the player is moving down

        public Player(Texture2D texture, Vector2 position, Rectangle rectangle) : base(texture, position)
        {
            _rectangle = rectangle;
        }

        public override void Update(GameTime gameTime, GameWindow window)
        {
            base.Update(gameTime, window);

            KeyboardState state = Keyboard.GetState();

            // Check for upward or downward propulsion
            _isPropelling = state.IsKeyDown(Keys.Up);
            _isDescending = state.IsKeyDown(Keys.Down);

            StringBuilder stringBuilder = new();
            stringBuilder.Append("Player position: ");
            if (_isPropelling)
            {
                // Go up
                stringBuilder.Append("Propelling");
                Propel();
            }
            else if (_isDescending)
            {
                // Go down
                stringBuilder.Append("Descending");
                Descend();
            }
            else
            {
                // Apply gravity
                stringBuilder.Append("Applying gravity");
                ApplyGravity();
            }

            // Else gravity takes over
            _position.Y += _velocityY;
            stringBuilder.Append($", velocity: {_velocityY}");
            // Console.WriteLine(stringBuilder.ToString());

            // Prevent player from going off-screen
            if (_position.Y < 0)
            {
                _position.Y = 0;
                _velocityY = 0;
            }
            int groundPos = window.ClientBounds.Height - _rectangle.Height;
            if (_position.Y >= groundPos)
            {
                _velocityY = 0;
                _position.Y = groundPos;
            }
        }

        public void Propel()
        {
            _velocityY = Math.Max(_propulsionForce, _velocityY + _propulsionForce);
            System.Console.WriteLine($"Propelling: {_velocityY}");
        }

        public void Descend()
        {
            _velocityY = Math.Min(_downwardForce, _velocityY + _downwardForce);
            System.Console.WriteLine($"Descending: {_velocityY}");
        }

        public void ApplyGravity()
        {
            _velocityY += _gravity;
        }
    }
}
