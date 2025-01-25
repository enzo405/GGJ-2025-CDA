using System;
using System.Diagnostics;
using Bloup.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bloup.Entity
{
    public class Rat : AnimatedEnemy
    {
        private const int FRAME_WIDTH = 32;
        private const int FRAME_HEIGHT = 32;
        private const int FRAME_COUNT = 2;
        private const float FRAME_TIME = 0.2f;

        public Rat(Texture2D texture, Vector2 position, Rectangle rectangle, float scale, GraphicsDeviceManager graphics)
            : base(texture, position, rectangle, scale, graphics,
                FRAME_WIDTH, FRAME_HEIGHT, FRAME_COUNT, FRAME_TIME)
        {
        }
    }
}