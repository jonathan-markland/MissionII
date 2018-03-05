using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System.Linq;
using GameClassLibrary;

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
        MissionIIClassLibrary.MissionIIKeyStates _keyStates;
        ScalingModes _scalingModes;



        public GameMain()
        {
            base.Window.AllowUserResizing = true;

            MissionIIClassLibrary.Business.SpriteToUintArray = 
                (obj) =>
                {
                    // The game engine will call out to this.  This separates
                    // the engine from MonoGame.
                    var hostImage = (Texture2D)obj.HostObject;
                    var n = hostImage.Width * hostImage.Height;
                    var resultColorData = new Color[n];
                    hostImage.GetData(resultColorData);
                    return resultColorData.Select(x => x.PackedValue).ToArray();
                };

            MissionIIClassLibrary.Business.UintArrayToSprite =
                (theArray, theWidth, theHeight) =>
                {
                    // The game engine will call out to this.  This separates
                    // the engine from MonoGame.

                    if(    theWidth  >= 0 
                        && theHeight >= 0 
                        && theWidth  <= 10000 
                        && theHeight <= 10000
                        && (theWidth * theHeight) == theArray.Length)
                    {
                        var colorData = theArray.Select(x => new Color(x));
                        var hostImage = new Texture2D(GraphicsDevice, theWidth, theHeight);
                        hostImage.SetData(theArray);
                        return new GameClassLibrary.Graphics.HostSuppliedSprite { HostObject = hostImage, BoardWidth = theWidth, BoardHeight = theHeight };
                    }
                    throw new System.Exception("Cannot create sprite from array and stated dimensions.");
                };

            _keyStates = new MissionIIClassLibrary.MissionIIKeyStates();
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
                MissionIIClassLibrary.Constants.ScreenWidth,
                MissionIIClassLibrary.Constants.ScreenHeight);

            _monoGameDrawingTarget = new MonoGameDrawingTarget(_spriteBatch);

            // Connect the library to the host routines to load sprites.
            // The load the sprites for this game:

            GameClassLibrary.Graphics.SpriteTraits.InitSpriteSupplier((spriteName) => 
            {
                var texture2d = Content.Load<Texture2D>(spriteName);
                return new GameClassLibrary.Graphics.HostSuppliedSprite
                {
                    BoardHeight = texture2d.Height, // By design, the client doesn't know how to obtain this itself.
                    BoardWidth = texture2d.Width,   // By design, the client doesn't know how to obtain this itself.
                    HostObject = texture2d
                };
            });

            MissionIIClassLibrary.MissionIISprites.Load();

            // Connect the library to the host routines that load and play sound.
            // Then load the sounds for this game:
            
            GameClassLibrary.Sound.SoundTraits.InitSoundSupplier((soundName) =>
            {
                return new GameClassLibrary.Sound.HostSuppliedSound
                {
                    HostSoundObject = Content.Load<SoundEffect>(soundName)
                };
            });

            GameClassLibrary.Sound.SoundTraits.InitSoundPlay((soundTraits) =>
            {
                ((SoundEffect)soundTraits.HostSoundObject).Play();
            });

            MissionIIClassLibrary.MissionIISounds.Load();
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

            MissionIIClassLibrary.MissionIIGameModeSelector.ModeSelector.CurrentMode.AdvanceOneCycle(_keyStates);

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

            _keyStates.Down = theKeyboard.IsKeyDown(Keys.Down);
            _keyStates.Up = theKeyboard.IsKeyDown(Keys.Up);
            _keyStates.Left = theKeyboard.IsKeyDown(Keys.Left);
            _keyStates.Right = theKeyboard.IsKeyDown(Keys.Right);
            _keyStates.Fire = theKeyboard.IsKeyDown(Keys.Z);
            _keyStates.Quit = theKeyboard.IsKeyDown(Keys.Escape);
            _keyStates.Pause = theKeyboard.IsKeyDown(Keys.P);
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

                        var targetRect = GameClassLibrary.Math.MakeRectangle.GetSquarePixelsProjectionArea(
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
                        var targetRect = GameClassLibrary.Math.MakeRectangle.GetBestFitProjectionArea(
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
