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
        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private SceneBase currentScene;
        public int ScreenWidth = 1920;
        public int ScreenHeight = 1080;

        public GameStart()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            currentScene = new MenuScene(Content, _graphics, this);
            SceneManager.Create(this);
            SceneManager.Create(this).Register(new MenuScene(Content, _graphics, this));
            SceneManager.Create(this).Register(new LevelScene(Content, _graphics, this));
            SceneManager.Create(this).Register(new GameOverScene(Content, _graphics, this));
        }

        protected override void Initialize()
        {
            _graphics.IsFullScreen = true;

            ScreenWidth = GraphicsDevice.DisplayMode.Width;
            ScreenHeight = GraphicsDevice.DisplayMode.Height;

            _graphics.PreferredBackBufferWidth = ScreenWidth;
            _graphics.PreferredBackBufferHeight = ScreenHeight;

            _graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            currentScene.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            // For use with SceneManager
            currentScene.Update(gameTime);

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // For use with SceneManager
            if (_spriteBatch is not null)
            {
                currentScene.Draw(gameTime, _spriteBatch);
                _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
                _spriteBatch.End();
            }

            base.Draw(gameTime);
        }

        public void ChangeCurrentScene(SceneBase scene)
        {
            if (currentScene.GetName() == scene.GetName())
            {
                return;
            }
            currentScene = scene;
            scene.LoadContent();
        }
    }
}