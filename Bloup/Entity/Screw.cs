using System;
using Bloup.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bloup.Entity
{
    public class Screw : Enemy
    {
        public Screw(Texture2D texture, Vector2 position, Rectangle rectangle, float scale)
            : base(texture, position, rectangle, scale)
        {
            _speedX = 3f; // Adjust screw speed
        }
    }
}