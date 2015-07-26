using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using ToGalaxyCustomData;
using ToGalaxyGameLibrary.Screens_and_ScreenManager.Managers;

namespace ToGalaxyGameLibrary.Screens_and_ScreenManager
{
    public class ScreenManager
    {
        #region List of Screens

        private List<Screen> ActiveScreens
        {
            get;
            set;
        }

        private List<Screen> ScreensToAdd
        {
            get;
            set;
        }

        private List<Screen> ScreensToRemove
        {
            get;
            set;
        }

        private Screen PreviousScreen
        {
            get;
            set;
        }

        #endregion

        private Game Game
        {
            get;
            set;
        }

        private GraphicsDeviceManager GraphicsDeviceManager
        {
            get;
            set;
        }

        public static ContentManager Content
        {
            get;
            private set;
        }

        public Viewport Viewport
        {
            get;
            private set;
        }

        public static Vector2 ScreenCentre
        {
            get;
            set;
        }

        public Camera2D Camera
        {
            get;
            private set;
        }

        public static InGameMouse Mouse
        {
            get;
            private set;
        }

        public static InputManager Input
        {
            get;
            private set;
        }

        public MusicManager Music
        {
            get;
            private set;
        }

        public static SoundEffectManager SoundEffects
        {
            get;
            private set;
        }

        public static OptionsManager Settings
        {
            get;
            set;
        }

        public ScreenManager(Game game, GraphicsDeviceManager graphicsDeviceManager, ContentManager content, Viewport viewport)
        {
            Game = game;
            GraphicsDeviceManager = graphicsDeviceManager;
            Content = content;
            Viewport = viewport;
            ScreenCentre = new Vector2(Viewport.Width * 0.5f, Viewport.Height * 0.5f);

            ActiveScreens = new List<Screen>();
            ScreensToAdd = new List<Screen>();
            ScreensToRemove = new List<Screen>();
            Camera = new Camera2D(this);
            Mouse = new InGameMouse(this);
            Input = new InputManager();
            Music = new MusicManager("Music");
            SoundEffects = new SoundEffectManager("Sounds");
        }

        public void LoadContent(ContentManager content)
        {
            Music.LoadContent(content);
            SoundEffects.LoadContent(content);
            Mouse.LoadContent(content);
        }

        public virtual void Update(GameTime gameTime)
        {
            Music.Update();
            Camera.Update();
            Mouse.Update(gameTime);
            Input.Update();

            foreach (Screen screen in ScreensToAdd)
            {
                ActiveScreens.Add(screen);
                screen.ScreenState = ScreenState.Active;
            }

            ScreensToAdd.Clear();

            foreach (Screen screen in ScreensToRemove)
            {
                PreviousScreen = screen;
                // screen.Dispose();
                ActiveScreens.Remove(screen);
            }

            ScreensToRemove.Clear();

            foreach (Screen screen in ActiveScreens)
            {
                screen.Update(gameTime);

                if (screen.ScreenState == ScreenState.Dead)
                {
                    ScreensToRemove.Add(screen);
                }
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            foreach (Screen screen in ActiveScreens)
            {
                screen.Draw(spriteBatch);
            }
        }

        public virtual void DrawBackgrounds(SpriteBatch spriteBatch)
        {
            foreach (Screen screen in ActiveScreens)
            {
                screen.DrawBackground(spriteBatch);
            }

            foreach (Screen screen in ScreensToRemove)
            {
                screen.DrawBackground(spriteBatch);
            }
        }

        public virtual void DrawCameraIndependentObjects(SpriteBatch spriteBatch)
        {
            foreach (Screen screen in ActiveScreens)
            {
                screen.DrawCameraIndependentObjects(spriteBatch);
            }

            Mouse.Draw(spriteBatch);
        }

        #region Add and Remove Functions for Screens

        public void LoadAndAddScreen(Screen screen)
        {
            screen.LoadContent();
            ScreensToAdd.Add(screen);
        }

        public void AddScreen(Screen screen)
        {
            ScreensToAdd.Add(screen);
        }

        #endregion

        public void BackToPreviousScreen()
        {
            if (PreviousScreen != null)
            {
                LoadAndAddScreen(PreviousScreen);
            }
        }

        public void Quit()
        {
            SaveOptions();
            Game.Exit();
        }

        public void ApplyGraphicsDeviceChange()
        {
            GraphicsDeviceManager.IsFullScreen = Settings.OptionsData.IsFullScreen;
            GraphicsDeviceManager.ApplyChanges();
        }

        public void SaveOptions()
        {
            // Get the path of the save game
            DirectoryInfo contentDirectory = new DirectoryInfo(Content.RootDirectory);
            string fullpath = contentDirectory.FullName + "/" + Settings.OptionsAsset;

            // Open the file, creating it if necessary
            FileStream stream = File.Open(fullpath, FileMode.OpenOrCreate, FileAccess.Write);
            try
            {
                // Convert the object to XML data and put it in the stream
                XmlSerializer serializer = new XmlSerializer(typeof(OptionsData));
                serializer.Serialize(stream, Settings.OptionsData);
            }
            finally
            {
                // Close the file
                stream.Close();
            }
        }
    }
}
