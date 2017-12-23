using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;
using System.Collections.Generic;

namespace MonogameTest
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager _graphicsDeviceManager;
        SpriteBatch _spriteBatch;
        MonoGameDrawingTarget _monoGameDrawingTarget;
        GameClassLibrary.CybertronGameBoard _cybertronGameBoard;
        GameClassLibrary.CybertronKeyStates _cybertronKeyStates;


        public Game1()
        {
            _cybertronKeyStates = new GameClassLibrary.CybertronKeyStates();
            _graphicsDeviceManager = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            using (var sr = new StreamReader("Resources\\Levels.txt"))
            {
                var loadedWorld = GameClassLibrary.LevelFileParser.Parse(sr);
                GameClassLibrary.LevelFileValidator.ExpectValidPathsInWorld(loadedWorld);
                GameClassLibrary.LevelExpander.ExpandWallsInWorld(loadedWorld);

                _cybertronGameBoard = new GameClassLibrary.CybertronGameBoard() // TODO: HACK
                {
                    TheWorldWallData = loadedWorld,
                    BoardWidth = 320,
                    BoardHeight = 256,
                    LevelNumber = 1,
                    RoomNumber = 15,
                    Score = 0,
                    Lives = 3
                };

                // TODO: PrepareForNewGame() function
                // TODO: PrepareForNewLevel() function
                // TODO: Initialise man position?

            }
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
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _monoGameDrawingTarget = new MonoGameDrawingTarget(_spriteBatch);

            // TODO: use this.Content to load your game content here
            GameClassLibrary.CybertronSpriteTraits.Load((spriteName) => 
            {
                var texture2d = Content.Load<Texture2D>(spriteName);
                return new GameClassLibrary.HostSuppliedSprite
                {
                    BoardHeight = texture2d.Height, // By design, the client doesn't know how to obtain this itself.
                    BoardWidth = texture2d.Width,   // By design, the client doesn't know how to obtain this itself.
                    HostObject = texture2d
                };
            });

            // HACKS
            _cybertronGameBoard.Key = new GameClassLibrary.CybertronKey(100, 100, 1);
            _cybertronGameBoard.Ring = new GameClassLibrary.CybertronRing(100, 100, 4);
            _cybertronGameBoard.Gold = new GameClassLibrary.CybertronGold(100, 100, 16);
            _cybertronGameBoard.Man.Alive(0, 150, 170);

            GameClassLibrary.CybertronGameStateUpdater.PrepareForNewRoom(_cybertronGameBoard);

        }

        private GameClassLibrary.GameTimeSpan GetGameTimeElapsed(GameTime gameTime)
        {
            var elapsedTime =
                new GameClassLibrary.GameTimeSpan
                {
                    Milliseconds = gameTime.TotalGameTime.Milliseconds
                };
            return elapsedTime;
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

            // TODO: Add your update logic here
            ReadAndStorePlayerInputs();
            var elapsedTime = GetGameTimeElapsed(gameTime);
            GameClassLibrary.CybertronGameStateUpdater.UpdateTo(elapsedTime, _cybertronGameBoard, _cybertronKeyStates);

            base.Update(gameTime);
        }

        private void ReadAndStorePlayerInputs()
        {
            var theKeyboard = Keyboard.GetState();
            _cybertronKeyStates.Down = theKeyboard.IsKeyDown(Keys.Down);
            _cybertronKeyStates.Up = theKeyboard.IsKeyDown(Keys.Up);
            _cybertronKeyStates.Left = theKeyboard.IsKeyDown(Keys.Left);
            _cybertronKeyStates.Right = theKeyboard.IsKeyDown(Keys.Right);
            _cybertronKeyStates.Fire = theKeyboard.IsKeyDown(Keys.Space);
            _cybertronKeyStates.Quit = theKeyboard.IsKeyDown(Keys.Escape);
            _cybertronKeyStates.Pause = theKeyboard.IsKeyDown(Keys.P);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();

            var elapsedTime = GetGameTimeElapsed(gameTime);

            GameClassLibrary.CybertronScreenPainter.DrawBoardToTarget(
                _cybertronGameBoard,
                _monoGameDrawingTarget);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
