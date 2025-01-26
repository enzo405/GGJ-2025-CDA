using System;
using System.Diagnostics;
using System.Threading.Tasks;
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
        protected float _rightForce = 6f;      // Horizontal force when moving right
        protected float _leftForce = -6f;      // Horizontal force when moving left
        protected float _velocityY = 0f;       // Current vertical velocity
        protected float _velocityX = 0f;       // Current horizontal velocity
        protected bool _isPropelling = false;  // Whether the player is propelling up
        protected bool _isDescending = false;  // Whether the player is moving down
        protected bool _isMovingLeft = false;  // Whether the player is moving left
        protected bool _isMovingRight = false; // Whether the player is moving right
        protected float _dragFactor = 0.94f;    // Drag to simulate water resistance
        protected float _waterFlowFactor = 0.2f; // Water flow factor
        protected int _maxWidth = 1200;
        protected bool _isDead = false;        // Whether the player is dead
        private Texture2D _fishTexture;    // Texture pour le poisson
        private int _fishFrameHeight;      // Hauteur d'une frame (calcule à partir de la texture)
        private int _fishFrameIndex = 0;   // Frame actuelle de l'animation du poisson
        private float _fishAnimationSpeed = 0.2f; // Vitesse d'animation (en secondes)
        private float _fishElapsedTime = 0f;      // Temps écoulé depuis la dernière frame

        public Player(Texture2D texture, Vector2 position, Rectangle rectangle, float scale, int maxWidth)
         : base(texture, position, 32, 0.2f, scale, false)
        {
            _position = position;
            _rectangle = rectangle;
            _scale = scale;
            _maxWidth = maxWidth;
        }

        public void SetFishTexture(Texture2D fishTexture)
        {
            _fishTexture = fishTexture;
            _fishFrameHeight = fishTexture.Height / 4; // Divise la hauteur de la texture par le nombre de frames (4 frames)
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            if (_fishTexture != null)
            {
                // Calculer la position du poisson au centre de la bulle
                Vector2 fishPosition = new Vector2(
                    _position.X ,
                    _position.Y
                );

                // Définir le rectangle source pour afficher la bonne frame du poisson
                Rectangle fishSourceRect = new Rectangle(
                    0,
                    _fishFrameIndex * _fishFrameHeight, // Frame actuelle
                    _fishTexture.Width,
                    _fishFrameHeight
                );

                // Dessiner le poisson
                spriteBatch.Draw(
                    _fishTexture,
                    fishPosition,
                    fishSourceRect,
                    Color.White,
                    0f,
                    Vector2.Zero,
                    _scale, // Utiliser le même facteur de mise à l'échelle que la bulle
                    SpriteEffects.None,
                    0f
                );
            }
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

            if (_fishTexture != null)
            {
                _fishElapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (_fishElapsedTime >= _fishAnimationSpeed)
                {
                    _fishFrameIndex = (_fishFrameIndex + 1) % 4; // Passe à la frame suivante (4 frames au total)
                    _fishElapsedTime = 0f;
                }
            }

            base.Update(gameTime, maxHeight, minHeight);

            KeyboardState state = Keyboard.GetState();

            // Check for upward or downward propulsion
            _isPropelling = state.IsKeyDown(Keys.Up);
            _isDescending = state.IsKeyDown(Keys.Down);
            _isMovingLeft = state.IsKeyDown(Keys.Left);
            _isMovingRight = state.IsKeyDown(Keys.Right);

            // Handle propulsion or descent
            if (_isPropelling)
            {
                Propel();
            }
            else if (_isDescending)
            {
                Descend();
            }

            // Handle horizontal movement
            if (_isMovingLeft)
            {
                MoveLeft();
            }
            else if (_isMovingRight)
            {
                MoveRight();
            }

            // Apply gravity
            ApplyGravity();

            // Simulate water flow
            ApplyWaterFlow();

            // Simulate water resistance to gradually stabilize the velocity
            ApplyDrag();

            // Update player position
            _position.Y += _velocityY;
            _position.X += _velocityX;

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

            if (_position.X < 0)
            {
                _position.X = 0;
                _velocityX = 0;
            }

            if (_position.X + _rectangle.Width > _maxWidth)
            {
                _position.X = _maxWidth - _rectangle.Width;
                _velocityX = 0;
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

        public void MoveLeft()
        {
            _velocityX = Math.Max(_leftForce, _velocityX + _leftForce);
        }

        public void MoveRight()
        {
            _velocityX = Math.Min(_rightForce, _velocityX + _rightForce);
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

        public void ApplyWaterFlow()
        {
            // Simulate water flow to the left
            _velocityX -= _waterFlowFactor;

            // Apply drag to simulate water resistance
            _velocityX *= _dragFactor;

            if (Math.Abs(_velocityX) < 0.01f)
            {
                _velocityX = 0f;
            }
        }

        public void Die()
        {
            if (_isDead) return; // Prevent re-triggering death

            _isDead = true;
            _velocityY = 0;
            Play();
        }

        public void CheckCollision(Enemy entity)
        {
            if (_position.X < entity._position.X + entity._rectangle.Width &&
                _position.X + _rectangle.Width > entity._position.X &&
                _position.Y < entity._position.Y + entity._rectangle.Height &&
                _position.Y + _rectangle.Height > entity._position.Y)
            {
                Die();
            }
        }
    }
}