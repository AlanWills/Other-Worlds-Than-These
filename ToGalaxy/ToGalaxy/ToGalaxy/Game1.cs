using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using ToGalaxyGameLibrary.Screens_and_ScreenManager;
using ToGalaxy.Screens.Gameplay_Screens;
using ToGalaxy.Screens;
using ToGalaxy.Player_and_Session;

namespace ToGalaxy
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
    //test commit please ignore
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        ExtendedScreenManager screenManager;
        OptionsManager options;

        public Game1()
        {
            Content.RootDirectory = "Content";

            options = new OptionsManager("XML/Settings/Options.xml");
            options.LoadContent(Content);
            ScreenManager.Settings = options;

            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            graphics.IsFullScreen = options.OptionsData.IsFullScreen;

            Mouse.WindowHandle = Window.Handle;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            screenManager = new ExtendedScreenManager(this, graphics, Content, GraphicsDevice.Viewport, new Session());
            screenManager.LoadContent(Content);
            screenManager.AddNewMainMenuScreen();
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            screenManager.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque);
            screenManager.DrawBackgrounds(spriteBatch);
            spriteBatch.End();   

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, screenManager.Camera.viewMatrix);
            screenManager.Draw(spriteBatch);
            spriteBatch.End();

            spriteBatch.Begin();
            screenManager.DrawCameraIndependentObjects(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        public void Restart()
        {
            GraphicsDevice.Reset();
        }
    }
}
