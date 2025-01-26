using System;
using Bloup.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bloup.Entity
{
    public class Shit : Enemy
    {
        public Shit(Texture2D texture, Vector2 postion, Rectangle rectangle, float scale)
            : base(texture, postion, rectangle, scale)
        {
            _speedX = 3.5f; // Ajust shit speed
        }
    }
}
