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

        public void changeCurrentScene(SceneBase scene)
        {
            this.currentScene = scene;
        }

        public GameStart()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            SceneManager.Create(this);
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            this.currentScene = new MenuScene(_graphics);

            this.currentScene.LoadContent(Content);

            font = Content.Load<SpriteFont>("fonts/Font");
            
            // Scene
            SceneManager.Create(this).Register(new MenuScene(_graphics));
            SceneManager.Create(this).Register(new LevelScene(_graphics));

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
