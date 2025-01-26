using Bloup.Entity;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bloup.Entity
{
    public class AnimatedEnemy : Enemy
    {
        private Rectangle[] _frames;
        private int _currentFrame;
        private float _frameTime;
        private float _timeElapsed;
        private bool _isLooping;
        public bool IsFinished => !_isLooping && _currentFrame == _frames.Length - 1;
        public Rectangle CurrentFrame => _frames[_currentFrame];

        public AnimatedEnemy(
            Texture2D texture,
            Vector2 position,
            Rectangle rectangle,
            int frameWidth,
            int frameHeight,
            int frameCount,
            float frameTime,
            float scale,
            bool isLooping = true) : base(texture, position, rectangle, scale)
        {
            _frameTime = frameTime;
            _isLooping = isLooping;
            _frames = new Rectangle[frameCount];

            for (int i = 0; i < frameCount; i++)
            {
                _frames[i] = new Rectangle(0, i * frameHeight, frameWidth, frameHeight);
            }
        }

        public override void Update(GameTime gameTime, int maxHeight, int minHeight)
        {
            base.Update(gameTime, maxHeight, minHeight);

            _timeElapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_timeElapsed >= _frameTime)
            {
                _timeElapsed = 0;
                if (_currentFrame + 1 < _frames.Length || _isLooping)
                {
                    _currentFrame = (_currentFrame + 1) % _frames.Length;
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _position, CurrentFrame, Color.White, 0f, Vector2.Zero, _scale,
                SpriteEffects.None, 0f);
        }
    }
}