using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Coreworlds.Core
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Engine : Game
    {
        private const int tileSize = 8;

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private TextureSystem textures;
        private World world;
        private Vector2 cameraWorldPosition;
        private float cameraSpeed = 10f;
        private SpriteFont font;
        private TextRenderHelper textRenderer;

        public Engine()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            this.font = this.Content.Load<SpriteFont>("LucidaConsole10pt");
            this.textures = new TextureSystem(this);
            this.textures.Load("deepwater", "DeepWater");
            this.textures.Load("water", "Water");
            this.textures.Load("sand", "Sand");
            this.textures.Load("grass", "Grass");
            this.world = new World();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            var keyboard = Keyboard.GetState();
            if (keyboard.IsKeyDown(Keys.W)) this.cameraWorldPosition += new Vector2(0, -this.cameraSpeed);
            if (keyboard.IsKeyDown(Keys.D)) this.cameraWorldPosition += new Vector2(this.cameraSpeed, 0);
            if (keyboard.IsKeyDown(Keys.S)) this.cameraWorldPosition += new Vector2(0, this.cameraSpeed);
            if (keyboard.IsKeyDown(Keys.A)) this.cameraWorldPosition += new Vector2(-this.cameraSpeed, 0);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            if (this.textRenderer == null) this.textRenderer = new TextRenderHelper(this.spriteBatch, this.font, new Vector2(10, 10));
            this.textRenderer.Reset();

            this.GraphicsDevice.Clear(Color.Black);
            this.spriteBatch.Begin();

            var estimatedVisibleTiles = new Vector2((this.Window.ClientBounds.Width / Engine.tileSize) + 1, (this.Window.ClientBounds.Height / Engine.tileSize) + 1);
            var cameraTileMargin = new Position((int)Math.Ceiling(estimatedVisibleTiles.X / 2f), (int)Math.Ceiling(estimatedVisibleTiles.Y / 2f));
            var renderTiles = new Position((cameraTileMargin.X * 2) + 1, (cameraTileMargin.Y * 2) + 1);
            var windowCenter = new Vector2(this.Window.ClientBounds.Width / 2, this.Window.ClientBounds.Height / 2);
            var cameraTilePosition = new Position((int)Math.Floor(this.cameraWorldPosition.X / (float)Engine.tileSize), (int)Math.Floor(this.cameraWorldPosition.Y / (float)Engine.tileSize));
            var renderTilePosition = cameraTilePosition - cameraTileMargin;
            var renderOffset = new Vector2(this.cameraWorldPosition.X - (float)(cameraTilePosition.X * Engine.tileSize), this.cameraWorldPosition.Y - (float)(cameraTilePosition.Y * Engine.tileSize));

            for (int x = 0; x < renderTiles.X; x++)
            {
                for (int y = 0; y < renderTiles.Y; y++)
                {
                    var tilePosition = new Position(renderTilePosition.X + x, renderTilePosition.Y + y);
                    var renderPosition = new Vector2((x * Engine.tileSize) - renderOffset.X, (y * Engine.tileSize) - renderOffset.Y);
                    var tile = this.world.GetTile(tilePosition);
                    if (tile.Terrain == TerrainTypes.DeepWater) this.spriteBatch.Draw(this.textures.Get("deepwater"), new Rectangle((int)renderPosition.X, (int)renderPosition.Y, Engine.tileSize, Engine.tileSize), Color.White);
                    else if (tile.Terrain == TerrainTypes.Water) this.spriteBatch.Draw(this.textures.Get("water"), new Rectangle((int)renderPosition.X, (int)renderPosition.Y, Engine.tileSize, Engine.tileSize), Color.White);
                    else if (tile.Terrain == TerrainTypes.Sand) this.spriteBatch.Draw(this.textures.Get("sand"), new Rectangle((int)renderPosition.X, (int)renderPosition.Y, Engine.tileSize, Engine.tileSize), Color.White);
                    else if (tile.Terrain == TerrainTypes.Grass) this.spriteBatch.Draw(this.textures.Get("grass"), new Rectangle((int)renderPosition.X, (int)renderPosition.Y, Engine.tileSize, Engine.tileSize), Color.White);
                }
            }

            this.textRenderer.RenderText("estimatedVisibleTiles: " + estimatedVisibleTiles.ToString());
            this.textRenderer.RenderText("cameraTileMargin: " + cameraTileMargin.ToString());
            this.textRenderer.RenderText("renderTiles: " + renderTiles.ToString());
            this.textRenderer.RenderText("windowCenter: " + windowCenter.ToString());
            this.textRenderer.RenderText("cameraTilePosition: " + cameraTilePosition.ToString());
            this.textRenderer.RenderText("renderTilePosition: " + renderTilePosition.ToString());
            this.textRenderer.RenderText("renderOffset: " + renderOffset.ToString());
            this.textRenderer.RenderText("TotalChunksLoaded: " + world.TotalChunksLoaded.ToString());

            this.spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
