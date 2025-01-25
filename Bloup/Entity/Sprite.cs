using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bloup.Entity
{
    public abstract class Sprite(Texture2D texture, Vector2 position)
    {
        public Texture2D _texture = texture;
        public Vector2 _position = position;

        public virtual void Update(GameTime gameTime, int screenHeight)
        {
            // Debug.WriteLine("Sprite Update");
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _position, Color.White);
        }
    }
}