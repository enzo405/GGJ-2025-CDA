using Bloup.Core;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Bloup
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public Player player;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            // Set fullscreen and resolution
            _graphics.IsFullScreen = true;
            _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;

            Window.AllowUserResizing = false; // Resizing is unnecessary in fullscreen
            Window.AllowAltF4 = true;
            Window.Title = "Bloup";
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            Texture2D playerTexture = Content.Load<Texture2D>("bubble");
            int spawnX = (int)(_graphics.PreferredBackBufferWidth / 5f - playerTexture.Width / 2);
            int spawnY = _graphics.PreferredBackBufferHeight / 2 - playerTexture.Height / 2;

            player = new Player(
                playerTexture,
                new Vector2(spawnX, spawnY), // Position
                new Rectangle(spawnX, spawnY, playerTexture.Width, playerTexture.Height) // Rectangle based on texture size
            );
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            player.Update(gameTime, Window);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            _spriteBatch.Draw(player._texture, player._position, Color.Red);

            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
