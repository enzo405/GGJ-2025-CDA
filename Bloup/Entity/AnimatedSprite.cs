using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bloup.Entity
{
    public class AnimatedSprite : Sprite
    {
        private Rectangle[] _frames;
        private int _currentFrame;
        private float _frameTime;
        private float _timeElapsed;
        private bool _isLooping;
        private bool _isPlaying;
        public bool IsFinished => !_isLooping && _currentFrame == _frames.Length - 1;
        public Rectangle CurrentFrame => _frames[_currentFrame];

        public AnimatedSprite(
            Texture2D texture,
            Vector2 position,
            int frameWidth,
            int frameHeight,
            int frameCount,
            float frameTime,
            float scale,
            bool isLooping = true) : base(texture, position, scale)
        {
            _frameTime = frameTime;
            _isLooping = isLooping;
            _isPlaying = false;
            _frames = new Rectangle[frameCount];

            for (int i = 0; i < frameCount; i++)
            {
                _frames[i] = new Rectangle(0, i * frameHeight, frameWidth, frameHeight);
            }
        }

        public void Play()
        {
            _isPlaying = true;
            _currentFrame = 0;
            _timeElapsed = 0;
        }

        public override void Update(GameTime gameTime, int maxHeight, int minHeight)
        {
            base.Update(gameTime, maxHeight, minHeight);

            if (_isPlaying)
            {
                _timeElapsed += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (_timeElapsed >= _frameTime)
                {
                    _timeElapsed = 0;
                    if (_currentFrame + 1 < _frames.Length || _isLooping)
                    {
                        _currentFrame = (_currentFrame + 1) % _frames.Length;
                    }
                    else
                    {
                        _isPlaying = false; // Stop playing if animation is finished and not looping.
                    }
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