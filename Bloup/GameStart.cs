using System.Diagnostics;
using Bloup.Core;
using Bloup.Managers;
using Bloup.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Bloup.Services;
using System.IO;

namespace Bloup
{
    public class GameStart : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        // Add custom path
        private SceneBase currentScene;
        private SpriteFont font;

        private MapLoader _mapLoader;
        private Texture2D _tileAtlas;

        private const int TileSize = 32;
        private const float TileScale = 2.0f;

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
            this.currentScene = new MenuScene(Content, _graphics);
            SceneManager.Create(this);
            SceneManager.Create(this).Register(new MenuScene(Content, _graphics));
            SceneManager.Create(this).Register(new LevelScene(Content, _graphics));
        }

        protected override void Initialize()
        {
            _mapLoader = new MapLoader();
            // TODO: Add your initialization logic here
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            this.currentScene.LoadContent();

            font = Content.Load<SpriteFont>("fonts/Font");

            // TODO: use this.Content to load your game content here

            _tileAtlas = Content.Load<Texture2D>("asset_tuyeau");

            string filePath = Path.Combine(Content.RootDirectory, "map.csv");
            _mapLoader.LoadMap(filePath);
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

            foreach (var tile in _mapLoader.TileMap)
            {
                Vector2 position = tile.Key * TileSize * TileScale; // Ajuster la position avec l'échelle
                Rectangle sourceRectangle = new Rectangle(tile.Value * TileSize, 0, TileSize, TileSize);

                // Dessiner chaque tuile avec le facteur d'échelle
                _spriteBatch.Draw(
                    _tileAtlas,
                    position,
                    sourceRectangle,
                    Color.White,
                    0f,               // Rotation
                    Vector2.Zero,     // Origine
                    TileScale,        // Échelle
                    SpriteEffects.None,
                    0f                // Profondeur
                );
            }

            _spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
