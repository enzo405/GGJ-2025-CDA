using System;
using Bloup.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bloup.Entity
{
    public class Screw : Ennemy
    {
        public Screw(Texture2D texture, Vector2 position, Rectangle rectangle, float scale, GraphicsDeviceManager graphics)
            : base(texture, position, rectangle, scale, graphics)
        {
            _speedX = 3f; // Adjust screw speed
        }
    }
}