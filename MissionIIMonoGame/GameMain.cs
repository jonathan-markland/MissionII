using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace MissionIIMonoGame
{
    public enum ScalingModes
    {
        StretchPreservingAspect,
        SquarePixelsStretch,
        StretchToFillWindow
    }

    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class GameMain : Game
    {
        GraphicsDeviceManager _graphicsDeviceManager;
        SpriteBatch _spriteBatch;
        RenderTarget2D _backingScreen;
        MonoGameDrawingTarget _monoGameDrawingTarget;
        MissionIIClassLibrary.MissionIIKeyStates _cybertronKeyStates;
        ScalingModes _scalingModes;



        private static Color[] TempColourBuffer; // TODO: not threadsafe


        public GameMain()
        {
            base.Window.AllowUserResizing = true;

            TempColourBuffer = new Color[1];

            MissionIIClassLibrary.Business.ReadPixel = 
                (obj, x, y) =>
                {
                    // TODO: efficiency concern.  Re-design:  Get all pixels up-front.
                    var hostImage = (Texture2D)obj;
                    hostImage.GetData<Color>(0, new Rectangle(x, y, 1,1), TempColourBuffer, 0, 1);
                    return (int) TempColourBuffer[0].PackedValue;
                };
            
            _cybertronKeyStates = new MissionIIClassLibrary.MissionIIKeyStates();
            _graphicsDeviceManager = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            _scalingModes = ScalingModes.StretchPreservingAspect;
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

            _backingScreen = new RenderTarget2D(GraphicsDevice, 
                MissionIIClassLibrary.MissionIIGameBoardConstants.ScreenWidth,
                MissionIIClassLibrary.MissionIIGameBoardConstants.ScreenHeight);

            _monoGameDrawingTarget = new MonoGameDrawingTarget(_spriteBatch);

            // TODO: use this.Content to load your game content here

            MissionIIClassLibrary.MissionIISpriteTraits.Load((spriteName) => 
            {
                var texture2d = Content.Load<Texture2D>(spriteName);
                return new MissionIIClassLibrary.HostSuppliedSprite
                {
                    BoardHeight = texture2d.Height, // By design, the client doesn't know how to obtain this itself.
                    BoardWidth = texture2d.Width,   // By design, the client doesn't know how to obtain this itself.
                    HostObject = texture2d
                };
            });

            MissionIIClassLibrary.MissionIISounds.Load((soundName) =>
            {
                return Content.Load<SoundEffect>(soundName);
            });

            MissionIIClassLibrary.MissionIISounds.Init((soundTraits) =>
            {
                ((SoundEffect)soundTraits.HostObject).Play();
            });

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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
                || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            // TODO: Add your update logic here
            ReadAndStorePlayerInputs();
            // TODO: remove function called:  var elapsedTime = GetGameTimeElapsed(gameTime);

            MissionIIClassLibrary.MissionIIGameModeSelector.ModeSelector.CurrentMode.AdvanceOneCycle(_cybertronKeyStates);

            base.Update(gameTime);
        }

        private void ReadAndStorePlayerInputs()
        {
            var theKeyboard = Keyboard.GetState();

            if (theKeyboard.IsKeyDown(Keys.F2))
            {
                _scalingModes = ScalingModes.StretchPreservingAspect;
            }
            if (theKeyboard.IsKeyDown(Keys.F3))
            {
                _scalingModes = ScalingModes.StretchToFillWindow;
            }
            if (theKeyboard.IsKeyDown(Keys.F4))
            {
                _scalingModes = ScalingModes.SquarePixelsStretch;
            }

            _cybertronKeyStates.Down = theKeyboard.IsKeyDown(Keys.Down);
            _cybertronKeyStates.Up = theKeyboard.IsKeyDown(Keys.Up);
            _cybertronKeyStates.Left = theKeyboard.IsKeyDown(Keys.Left);
            _cybertronKeyStates.Right = theKeyboard.IsKeyDown(Keys.Right);
            _cybertronKeyStates.Fire = theKeyboard.IsKeyDown(Keys.Z);
            _cybertronKeyStates.Quit = theKeyboard.IsKeyDown(Keys.Escape);
            _cybertronKeyStates.Pause = theKeyboard.IsKeyDown(Keys.P);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(_backingScreen);
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();

            MissionIIClassLibrary.MissionIIGameModeSelector.ModeSelector.CurrentMode.Draw(_monoGameDrawingTarget);

            _spriteBatch.End();

            base.Draw(gameTime);

            GraphicsDevice.SetRenderTarget(null);
            _spriteBatch.Begin();

            switch(_scalingModes)
            {
                case ScalingModes.SquarePixelsStretch:
                    {
                        _spriteBatch.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;

                        var targetRect = MissionIIClassLibrary.Math.MakeRectangle.GetSquarePixelsProjectionArea(
                            Window.ClientBounds.Width,
                            Window.ClientBounds.Height,
                            _backingScreen.Width,
                            _backingScreen.Height);

                        _spriteBatch.Draw(_backingScreen, new Rectangle(
                            targetRect.Left, targetRect.Top, targetRect.Width, targetRect.Height), Color.White);
                    }
                    break;

                case ScalingModes.StretchPreservingAspect:
                    {
                        var targetRect = MissionIIClassLibrary.Math.MakeRectangle.GetBestFitProjectionArea(
                          Window.ClientBounds.Width,
                          Window.ClientBounds.Height,
                          _backingScreen.Width,
                          _backingScreen.Height);

                        _spriteBatch.Draw(_backingScreen, new Rectangle(
                            targetRect.Left, targetRect.Top, targetRect.Width, targetRect.Height), Color.White);
                    }
                    break;

                case ScalingModes.StretchToFillWindow:
                    {
                        _spriteBatch.Draw(_backingScreen, new Rectangle(0, 0,
                          Window.ClientBounds.Width,
                          Window.ClientBounds.Height), Color.White);
                    }
                    break;

            }

            _spriteBatch.End();
        }
    }
}
