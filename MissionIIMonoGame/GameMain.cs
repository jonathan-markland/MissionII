﻿
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using GameClassLibrary.Sound;
using GameClassLibrary.Input;

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
        private GraphicsDeviceManager _graphicsDeviceManager;
        private SpriteBatch _spriteBatch;
        private RenderTarget2D _backingScreen;
        private MonoGameDrawingTarget _monoGameDrawingTarget;
        private KeyStates _keyStates;
        private ScalingModes _scalingModes;
        private static object _musicLock = new object();
        private static SoundEffectInstance _soundEffectInstance;



        public GameMain()
        {
            base.Window.AllowUserResizing = true;

            GameClassLibrary.Graphics.HostSuppliedSprite.InitialisationByHost(
                (theArray, theWidth, theHeight) =>
                {
                    // The library will call out to this.
                    // This separates the engine from MonoGame.
                    var colorData = theArray.Select(x => new Color(x));
                    var hostImage = new Texture2D(GraphicsDevice, theWidth, theHeight);
                    hostImage.SetData(theArray);
                    return hostImage;
                },
                (obj) =>
                {
                    // The game engine will call out to this.  This separates
                    // the engine from MonoGame.
                    var hostImage = (Texture2D)obj;
                    var n = hostImage.Width * hostImage.Height;
                    var resultColorData = new Color[n];
                    hostImage.GetData(resultColorData);
                    return resultColorData.Select(x => x.PackedValue).ToArray();
                });

            _keyStates = new KeyStates();
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

            // Screen dimensions:

            GameClassLibrary.Graphics.Screen.Width = MissionIIClassLibrary.Constants.ScreenWidth;
            GameClassLibrary.Graphics.Screen.Height = MissionIIClassLibrary.Constants.ScreenHeight;

            // Connect the library to the host routines to load sprites.

            GameClassLibrary.Graphics.SpriteTraits.InitSpriteSupplier((spriteName) => 
            {
                var texture2d = Content.Load<Texture2D>(spriteName);
                return new GameClassLibrary.Graphics.HostSuppliedSprite(
                    texture2d,
                    texture2d.Width,     // By design, the client doesn't know how to obtain this itself.
                    texture2d.Height     // By design, the client doesn't know how to obtain this itself.
                );
            });

            // Connect the library to the host routines that load and play sound:

            GameClassLibrary.Sound.SoundTraits.InitSoundSupplier((soundName) =>
            {
                return new GameClassLibrary.Sound.HostSuppliedSound(
                    Content.Load<SoundEffect>(soundName));
            });

            GameClassLibrary.Sound.SoundTraits.InitSoundPlay((soundTraits) =>
            {
                ((SoundEffect)soundTraits.HostSoundObject).Play();
            });


            GameClassLibrary.Sound.SoundTraits.InitMusicPlay(PlayMusic);
            GameClassLibrary.Sound.SoundTraits.InitMusicStop(StopMusic);

            // Now that the above is done, we can load everything:

            MissionIIClassLibrary.Modes.HiScoreEntry.StaticInit(
                MissionIIClassLibrary.Constants.InitialLowestHiScore,
                MissionIIClassLibrary.Constants.InitialHiScoresIncrement);

            MissionIIClassLibrary.MissionIISprites.Load();
            MissionIIClassLibrary.MissionIIFonts.Load();
            MissionIIClassLibrary.MissionIISounds.Load();

            GameClassLibrary.Modes.GameMode.ActiveMode = new MissionIIClassLibrary.Modes.TitleScreen();
        }



        private void PlayMusic(HostSuppliedSound obj)
        {
            lock (_musicLock)
            {
                if (_soundEffectInstance != null)
                {
                    if (_soundEffectInstance.State == SoundState.Playing)
                    {
                        return; // Something already playing.
                    }
                }
            }

            StopMusic();

            lock (_musicLock)
            {
                _soundEffectInstance = ((SoundEffect)obj.HostSoundObject).CreateInstance();
                _soundEffectInstance.Play();
            }
        }



        private void StopMusic()
        {
            lock (_musicLock)
            {
                if (_soundEffectInstance != null)
                {
                    _soundEffectInstance.Stop();
                    _soundEffectInstance.Dispose();
                    _soundEffectInstance = null;
                }
            }
        }


        
        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            StopMusic();
        }



        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            /* TODO: Maybe re-instate this somehow.  Needs thinking about.
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
                || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }
            */
            ReadAndStorePlayerInputs();
            GameClassLibrary.Modes.GameMode.AdvanceActiveModeOneCycle(_keyStates);
            base.Update(gameTime);
        }



        private void ReadAndStorePlayerInputs()
        {
            var theKeyboard = Keyboard.GetState();

            if (theKeyboard.IsKeyDown(Keys.F2))
            {
                _scalingModes = ScalingModes.StretchPreservingAspect;
            }
            else if (theKeyboard.IsKeyDown(Keys.F3))
            {
                _scalingModes = ScalingModes.StretchToFillWindow;
            }
            else if (theKeyboard.IsKeyDown(Keys.F4))
            {
                _scalingModes = ScalingModes.SquarePixelsStretch;
            }
            else if (theKeyboard.IsKeyDown(Keys.F11))
            {
                ToggleFullScreen(true);
            }
            else if (theKeyboard.IsKeyDown(Keys.F12))
            {
                ToggleFullScreen(false);
            }

            _keyStates.Down = theKeyboard.IsKeyDown(Keys.Down);
            _keyStates.Up = theKeyboard.IsKeyDown(Keys.Up);
            _keyStates.Left = theKeyboard.IsKeyDown(Keys.Left);
            _keyStates.Right = theKeyboard.IsKeyDown(Keys.Right);
            _keyStates.Fire = theKeyboard.IsKeyDown(Keys.Z);
            _keyStates.Quit = theKeyboard.IsKeyDown(Keys.Escape);
            _keyStates.Pause = theKeyboard.IsKeyDown(Keys.P);

            var gamepadCapabilities = GamePad.GetCapabilities(PlayerIndex.One);
            if (gamepadCapabilities.IsConnected)
            {
                var gamePadState = GamePad.GetState(PlayerIndex.One);

                if (gamepadCapabilities.HasLeftXThumbStick)
                {
                    if (gamePadState.ThumbSticks.Left.X < -0.5f) _keyStates.Left = true;
                    if (gamePadState.ThumbSticks.Left.X > 0.5f) _keyStates.Right = true;
                    if (gamePadState.ThumbSticks.Left.Y < -0.5f) _keyStates.Down = true;
                    if (gamePadState.ThumbSticks.Left.Y > 0.5f) _keyStates.Up = true;
                }

                if (gamepadCapabilities.HasRightXThumbStick)
                {
                    if (gamePadState.ThumbSticks.Right.X < -0.5f) _keyStates.Left = true;
                    if (gamePadState.ThumbSticks.Right.X > 0.5f) _keyStates.Right = true;
                    if (gamePadState.ThumbSticks.Right.Y < -0.5f) _keyStates.Down = true;
                    if (gamePadState.ThumbSticks.Right.Y > 0.5f) _keyStates.Up = true;
                }

                if (gamePadState.IsButtonDown(Buttons.LeftTrigger)
                    || gamePadState.IsButtonDown(Buttons.RightTrigger)
                    || gamePadState.IsButtonDown(Buttons.A)
                    || gamePadState.IsButtonDown(Buttons.B)
                    || gamePadState.IsButtonDown(Buttons.X)
                    || gamePadState.IsButtonDown(Buttons.Y)
                    )
                {
                    _keyStates.Fire = true;
                }

                if (gamePadState.IsButtonDown(Buttons.Start))
                {
                    _keyStates.Pause = true;
                }
            }
        }



        private void ToggleFullScreen(bool stateToSet)
        {
            _graphicsDeviceManager.IsFullScreen = stateToSet;
            _graphicsDeviceManager.ApplyChanges();
        }



        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(_backingScreen);
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin(
                SpriteSortMode.Deferred,
                BlendState.AlphaBlend,
                SamplerState.PointClamp,
                DepthStencilState.None,
                RasterizerState.CullCounterClockwise);

            GameClassLibrary.Modes.GameMode.ActiveMode.Draw(_monoGameDrawingTarget);

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
