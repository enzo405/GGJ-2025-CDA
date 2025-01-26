using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bloup.Entity
{
    public class Wave : AnimatedSprite
    {
        public Wave(Texture2D texture, Vector2 position, int frameWidth = 32, int frameHeight = 32, int frameCount = 2, float frameTime = 0.2f)
            : base(texture, position, frameWidth, frameHeight, frameCount)
        {
        }
    }
}