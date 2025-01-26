using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Bloup.Entity
{
    public class Player : AnimatedSprite
    {
        public Rectangle _rectangle;
        protected float _speed = 0.05f;
        protected float _gravity = 0.06f; // Reduced gravity for water
        protected float _propulsionForce = -6f; // Upward force when propelling
        protected float _downwardForce = 6f;   // Downward force when descending
        protected float _velocityY = 0f;       // Current vertical velocity
        protected bool _isPropelling = false;  // Whether the player is propelling up
        protected bool _isDescending = false;  // Whether the player is moving down
        protected float _dragFactor = 0.94f;    // Drag to simulate water resistance
        protected bool _isDead = false;        // Whether the player is dead

        public Player(Texture2D texture, Vector2 position, Rectangle rectangle, float scale)
         : base(texture, position, 32, 32, 4, 0.3f, scale, false)
        {
            _position = position;
            _rectangle = rectangle;
            _scale = scale;
        }

        public override void Update(GameTime gameTime, int maxHeight, int minHeight)
        {
            // If the player is dead and the animation has finished, stop updating
            if (_isDead)
            {
                if (IsFinished)
                {
                    return;
                }
                base.Update(gameTime, maxHeight, minHeight); // Update the animation
                return;
            }

            base.Update(gameTime, maxHeight, minHeight);

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

            // Update player position
            _position.Y += _velocityY;

            // Prevent player from going off-screen (top of the screen)
            if (GetTopPosition() <= maxHeight)
            {
                _position.Y = maxHeight;
                _velocityY = 0;
            }
            // Prevent player from falling through the bottom of the screen
            if (GetBottomPosition() > minHeight)
            {
                _position.Y = minHeight - _rectangle.Height;
                _velocityY = 0;
            }
        }

        public void Propel()
        {
            // Apply upward force
            _velocityY = Math.Max(_propulsionForce, _velocityY + _propulsionForce);
        }

        public void Descend()
        {
            // Apply downward force
            _velocityY = Math.Min(_downwardForce, _velocityY + _downwardForce);
        }

        public void ApplyGravity()
        {
            // Gradually pull the player down if not on the ground
            _velocityY += _gravity;
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

        public void Die()
        {
            if (_isDead) return; // Prevent re-triggering death

            _isDead = true;
            _velocityY = 0;
            Play();
        }
    }
}
