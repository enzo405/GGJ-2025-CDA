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
        protected float _gravity = 0.06f; // Reduced gravity for water
        protected float _propulsionForce = -6f; // Upward force when propelling
        protected float _downwardForce = 6f;   // Downward force when descending
        protected float _velocityY = 0f;       // Current vertical velocity
        protected bool _isPropelling = false;  // Whether the player is propelling up
        protected bool _isDescending = false;  // Whether the player is moving down
        protected float _dragFactor = 0.95f;    // Drag to simulate water resistance

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

            // Handle propulsion or descent
            if (_isPropelling)
            {
                Propel();
            }
            else if (_isDescending)
            {
                Descend();
            }
            else
            {
                ApplyGravity();
            }

            // Simulate water resistance to gradually stabilize the velocity
            ApplyDrag();

            // Update the player's position
            _position.Y += _velocityY;

            // Prevent player from going off-screen (top of the screen)
            if (_position.Y < 0)
            {
                _position.Y = 0;
                _velocityY = 0;
            }

            // Prevent player from falling through the bottom of the screen
            int groundPos = window.ClientBounds.Height - _rectangle.Height;
            if (_position.Y >= groundPos)
            {
                _velocityY = 0;
                _position.Y = groundPos;
            }

            // Debug output
            Console.WriteLine($"Position: {_position.Y}, Velocity: {_velocityY}");
        }

        public void Propel()
        {
            // Apply upward force
            _velocityY = Math.Max(_propulsionForce, _velocityY + _propulsionForce);
            Console.WriteLine($"Propelling: {_velocityY}");
        }

        public void Descend()
        {
            // Apply downward force
            _velocityY = Math.Min(_downwardForce, _velocityY + _downwardForce);
            Console.WriteLine($"Descending: {_velocityY}");
        }

        public void ApplyGravity()
        {
            // Gradually pull the player down if not on the ground
            _velocityY += _gravity;
            Console.WriteLine($"Applying Gravity: {_velocityY}");
        }

        public void ApplyDrag()
        {
            // Reduce the velocity gradually to simulate water resistance
            _velocityY *= _dragFactor;

            // Stop tiny oscillations caused by drag
            if (Math.Abs(_velocityY) < 0.01f)
            {
                _velocityY = 0f;
            }
        }
    }
}
