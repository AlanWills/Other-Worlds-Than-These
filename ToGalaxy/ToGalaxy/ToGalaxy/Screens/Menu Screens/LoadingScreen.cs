using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToGalaxyGameLibrary.Screens_and_ScreenManager;
using ToGalaxyGameLibrary.UI;

namespace ToGalaxy.Screens.Menu_Screens
{
    public class LoadingScreen : Screen
    {
        private ExtendedScreenManager ExtendedScreenManager
        {
            get;
            set;
        }

        private Image LoadingImage
        {
            get;
            set;
        }

        private float currentTime = 0;

        public LoadingScreen(ExtendedScreenManager screenManager, string screenDataAsset)
            : base(screenManager, screenDataAsset)
        {
            ExtendedScreenManager = screenManager;
            LoadingImage = new Image(
                "Sprites/UI/Titles/LoadingScreenTitle",
                new Vector2(ExtendedScreenManager.Viewport.Width, ExtendedScreenManager.Viewport.Height) / 2,
                "Loading Image");
            AddScreenUIElement(LoadingImage);
        }

        public override void LoadContent()
        {
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            currentTime += (float)gameTime.ElapsedGameTime.Milliseconds / 1000;

            if (currentTime > 1)
            {
                ExtendedScreenManager.StartGameplay();
                Die();
            }
        }
    }
}
