using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToGalaxy.Player_and_Session;
using ToGalaxy.Screens.Cut_Scene_Screens;
using ToGalaxy.Screens.Cut_Scene_Screens.Cut_Scenes;
using ToGalaxy.Screens.Cut_Scene_Screens.Text_Cut_Scenes;
using ToGalaxy.Screens.Gameplay_Screens;
using ToGalaxy.Screens.Menu_Screens;
using ToGalaxyGameLibrary.Screens_and_ScreenManager;

namespace ToGalaxy.Screens
{
    public class ExtendedScreenManager : ScreenManager
    {
        #region Three Game Play Screens

        public ShipInteriorScreen ShipInteriorScreen
        {
            get;
            private set;
        }

        public SensorsScreen SensorsScreen
        {
            get;
            private set;
        }

        public SpaceScreen SpaceScreen
        {
            get;
            private set;
        }

        private bool GamePlayScreensInitialized
        {
            get
            {
                return ShipInteriorScreen != null && SpaceScreen != null && SensorsScreen != null;
            }
        }

        #endregion

        public bool DoneLoading
        {
            get;
            private set;
        }

        private bool GamePlayScreensActive
        {
            get
            {
                bool gamePlayScreensActive = false;

                if (GamePlayScreensInitialized && SpaceScreen.ScreenState != ScreenState.Dead && SensorsScreen.ScreenState != ScreenState.Dead)
                {
                    gamePlayScreensActive = true;
                }

                return gamePlayScreensActive && GamePlayScreensInitialized;
            }
        }

        public Session Session
        {
            get;
            private set;
        }

        private float pauseTimer = 0;

        public ExtendedScreenManager(Game game, GraphicsDeviceManager graphicsDeviceManager, ContentManager content, Viewport viewport, Session session)
            : base(game, graphicsDeviceManager, content, viewport)
        {
            Session = session;

            ModFunctionManager.SetUpEvents();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (GamePlayScreensActive)
            {
                CheckForScreenPause(gameTime);
                CheckForScreenSwap();

                // ShipInteriorScreen.Update(gameTime);
                SensorsScreen.Update(gameTime);
                SpaceScreen.Update(gameTime);
            }
        }

        private void CheckForScreenPause(GameTime gameTime)
        {
            pauseTimer += (float)gameTime.ElapsedGameTime.Milliseconds / 1000f;

            if ((ScreenManager.Input.IsKeyDown(Keys.P)) && (ScreenManager.Input.PreviousKeyboardState.IsKeyUp(Keys.P)))
            {
                if (pauseTimer > 0.2f)
                {
                    if (SpaceScreen.ScreenState == ScreenState.Active)
                    {
                        SpaceScreen.Freeze();
                        pauseTimer = 0;
                    }
                    else if (SpaceScreen.ScreenState == ScreenState.Frozen)
                    {
                        SpaceScreen.Activate();
                        pauseTimer = 0;
                    }

                    if (SensorsScreen.ScreenState == ScreenState.Active)
                    {
                        SensorsScreen.Freeze();
                        pauseTimer = 0;
                    }
                    else if (SensorsScreen.ScreenState == ScreenState.Frozen)
                    {
                        SensorsScreen.Activate();
                        pauseTimer = 0;
                    }
                }
            }
        }

        private void CheckForScreenSwap()
        {
            if ((ScreenManager.Input.IsKeyDown(Keys.Space)) && (ScreenManager.Input.PreviousKeyboardState.IsKeyUp(Keys.Space)))
            {
                if (SpaceScreen.ScreenState == ScreenState.Active)
                {
                    SpaceScreen.ScreenState = ScreenState.Hidden;
                    SensorsScreen.ScreenState = ScreenState.Active;
                    Camera.SetFixedScreenCamera(true);
                }
                else if (SpaceScreen.ScreenState == ScreenState.Hidden)
                {
                    SpaceScreen.ScreenState = ScreenState.Active;
                    SensorsScreen.ScreenState = ScreenState.Hidden;
                    Camera.SetCameraToFocusOnObject(SpaceScreen.PlayerShip);
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            if (GamePlayScreensActive)
            {
                // ShipInteriorScreen.Draw(spriteBatch);
                SensorsScreen.Draw(spriteBatch);
                SpaceScreen.Draw(spriteBatch);
            }
        }

        public override void DrawBackgrounds(SpriteBatch spriteBatch)
        {
            base.DrawBackgrounds(spriteBatch);

            if (GamePlayScreensActive)
            {
                // ShipInteriorScreen.DrawBackground(spriteBatch);
                SensorsScreen.DrawBackground(spriteBatch);
                SpaceScreen.DrawBackground(spriteBatch);
            }
        }

        public override void DrawCameraIndependentObjects(SpriteBatch spriteBatch)
        {
            if (GamePlayScreensActive)
            {
                // ShipInteriorScreen.DrawCameraIndependentObjects(spriteBatch);
                SensorsScreen.DrawCameraIndependentObjects(spriteBatch);
                SpaceScreen.DrawCameraIndependentObjects(spriteBatch);
            }

            base.DrawCameraIndependentObjects(spriteBatch);
        }

        public void AddNewMainMenuScreen()
        {
            LoadAndAddScreen(new MainMenuScreen(this, "XML/Menu Screens/MainMenuScreen"));
        }

        public void LoadNewGameplayScreens(int currentLevel)
        {
            Music.PlayAmbientMusic();

            ShipInteriorScreen = new ShipInteriorScreen(this, String.Format("XML/Ship Interior Maps/{0}Interior", "Dragonfly"));
            SpaceScreen = new SpaceScreen(this, String.Format("XML/Space Data/Level{0}Data", currentLevel));

            ShipInteriorScreen.ScreenState = ScreenState.Frozen;
            SpaceScreen.ScreenState = ScreenState.Frozen;

            // ShipInteriorScreen.LoadContent();
            SpaceScreen.LoadContent();
            SensorsScreen = new SensorsScreen(this, SpaceScreen);
            SensorsScreen.LoadContent();

            // Add all the details because this involves adding sensor images which requires the SensorsScreen to load content
            SpaceScreen.SetUpSpaceScreen();

            DoneLoading = true;
        }

        public void StartGameplay()
        {
            LoadNewGameplayScreens(Session.CurrentLevel);

            // ShipInteriorScreen.ScreenState = ScreenState.Hidden;
            SensorsScreen.ScreenState = ScreenState.Hidden;
            SpaceScreen.ScreenState = ScreenState.Active;
        }
    }
}
