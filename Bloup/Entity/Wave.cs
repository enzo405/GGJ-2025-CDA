using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bloup.Entity
{
    public class Wave(Texture2D texture, Vector2 position, Rectangle rectangle, float scale, int frameWidth = 32, int frameHeight = 32, int frameCount = 2, float frameTime = 0.2f) : AnimatedEnemy(texture, position, rectangle, frameWidth, frameHeight, frameCount, frameTime, scale)
    {
    }
}