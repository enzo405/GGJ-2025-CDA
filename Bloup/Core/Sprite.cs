using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bloup.Core
{
    public class Sprite
    {
        public Texture2D _texture;
        public Vector2 _position;

        public Sprite(Texture2D texture, Vector2 position)
        {
            _texture = texture;
            _position = position;
        }

        public virtual void Update(GameTime gameTime, GameWindow window)
        {
            Debug.WriteLine("Sprite Update");
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _position, Color.White);
        }
    }
}