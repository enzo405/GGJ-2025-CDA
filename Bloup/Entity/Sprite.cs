using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Bloup.Entity
{
    public abstract class Sprite(Texture2D texture, Vector2 position, float scale)
    {
        public Texture2D _texture = texture;
        public Vector2 _position = position;
        protected float _scale = scale; // Scale factor for the texture

        public virtual void Update(GameTime gameTime, int screenHeight)
        {
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            Debug.WriteLine("Drawing sprite at position " + _position);
            spriteBatch.Draw(_texture, _position, Color.White);
        }
    }
}