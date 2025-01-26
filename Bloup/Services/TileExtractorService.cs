using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Bloup.Services
{
    public class TileExtractorService(GraphicsDevice graphicsDevice)
    {
        private readonly GraphicsDevice _graphicsDevice = graphicsDevice;

        public List<Texture2D> ExtractTiles(Texture2D texture, int tileWidth, int tileHeight)
        {
            var tiles = new List<Texture2D>();

            int columns = texture.Width / tileWidth;
            int rows = texture.Height / tileHeight;

            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < columns; x++)
                {
                    Rectangle sourceRectangle = new Rectangle(x * tileWidth, y * tileHeight, tileWidth, tileHeight);

                    Texture2D tileTexture = new Texture2D(_graphicsDevice, tileWidth, tileHeight);
                    Color[] tileData = new Color[tileWidth * tileHeight];

                    texture.GetData(0, sourceRectangle, tileData, 0, tileData.Length);

                    tileTexture.SetData(tileData);

                    tiles.Add(tileTexture);
                }
            }

            return tiles;
        }
    }
}
