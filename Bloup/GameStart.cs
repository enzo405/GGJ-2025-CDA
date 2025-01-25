using System.Diagnostics;
using Bloup.Core;
using Bloup.Managers;
using Bloup.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Bloup
{
    public class GameStart : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        // Add custom path
        private SceneBase currentScene;
        private SpriteFont font;

        public int ScreenWidth = 1920;
        public int ScreenHeight = 1080;

        public void changeCurrentScene(SceneBase scene)
        {
            this.currentScene = scene;
            scene.LoadContent();
        }

        public GameStart()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            this.currentScene = new MenuScene(Content, _graphics, this);
            SceneManager.Create(this);
            SceneManager.Create(this).Register(new MenuScene(Content, _graphics, this));
            SceneManager.Create(this).Register(new LevelScene(Content, _graphics, this));
        }

        protected override void Initialize()
        {
            _graphics.IsFullScreen = true;

            this.ScreenWidth = GraphicsDevice.DisplayMode.Width;
            this.ScreenHeight = GraphicsDevice.DisplayMode.Height;

            _graphics.PreferredBackBufferWidth = ScreenWidth;
            _graphics.PreferredBackBufferHeight = ScreenHeight;

            _graphics.ApplyChanges();

            Debug.WriteLine($"Résolution en plein écran : {ScreenWidth}x{ScreenHeight}");
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            this.currentScene.LoadContent();

            font = Content.Load<SpriteFont>("fonts/Font");
            
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            // For use with SceneManager
            this.currentScene.Update(gameTime);

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                // rediger des log
                System.Console.WriteLine("cc");
                SceneManager.Create(this).ChangeScene("LevelScene");
            }

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // For use with SceneManager
            this.currentScene.Draw(gameTime, _spriteBatch);


            _spriteBatch.Begin();
            _spriteBatch.DrawString(font, "cc", new Vector2(100, 100), Color.Aqua);

            _spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}