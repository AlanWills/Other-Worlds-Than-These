using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToGalaxyCustomData;
using ToGalaxyGameLibrary.UI;

namespace ToGalaxyGameLibrary.Screens_and_ScreenManager
{
    public class OptionsScreen : Screen
    {
        public OptionsScreen(ScreenManager screenManager, string screenDataAsset)
            : base(screenManager, screenDataAsset)
        {
            
        }

        public override void LoadContent()
        {
            base.LoadContent();

            SetUpOptionsUI();
        }

        private void SetUpOptionsUI()
        {
            // Full Screen UI
            Text isFullScreenText = new Text(
                "Full Screen",
                new Vector2(ScreenManager.Viewport.Width / 4, ScreenManager.Viewport.Height / 3),
                Color.Cyan,
                "Full Screen Text");
            AddScreenUIElement(isFullScreenText);

            Button isFullScreenButton = new Button(
                "XML/UI/Buttons/MenuButton",
                isFullScreenText.Position + new Vector2(2 * ScreenManager.Viewport.Width / 5, 0),
                Button.defaultColour,
                Button.highlightedColour,
                "Change Full Screen Button",
                ScreenManager.Settings.OptionsData.IsFullScreen.ToString());
            isFullScreenButton.InteractEvent += ChangeFullScreenEvent;
            AddScreenUIElement(isFullScreenButton);

            // Music Volume UI
            Text musicVolumeText = new Text(
                "Music Volume",
                isFullScreenText.Position + new Vector2(0, ScreenManager.Viewport.Height / 10),
                Color.Cyan,
                "Music Volume Text");
            AddScreenUIElement(musicVolumeText);

            Slider musicVolumeSlider = new Slider(
                ScreenManager.Settings.OptionsData.MusicVolume,
                "Sprites/UI/Sliders/VolumeSlider",
                musicVolumeText.Position + new Vector2(2 * ScreenManager.Viewport.Width / 5, 0),
                new Vector2(1, 1),
                "Music Volume Slider");
            musicVolumeSlider.InteractEvent += ChangeMusicVolumeEvent;
            AddScreenUIElement(musicVolumeSlider);

            // Sound Effects UI
            Text soundEffectsVolumeText = new Text(
                "Sound Effects Volume",
                musicVolumeText.Position + new Vector2(0, ScreenManager.Viewport.Height / 10),
                Color.Cyan,
                "Sound Effects Volume Text");
            AddScreenUIElement(soundEffectsVolumeText);

            Slider soundEffectsVolumeSlider = new Slider(
                ScreenManager.Settings.OptionsData.SoundEffectsVolume,
                "Sprites/UI/Sliders/VolumeSlider",
                soundEffectsVolumeText.Position + new Vector2(2 * ScreenManager.Viewport.Width / 5, 0),
                new Vector2(1, 1),
                "Sound Effects Volume Slider");
            soundEffectsVolumeSlider.InteractEvent += ChangeSoundEffectsVolume;
            AddScreenUIElement(soundEffectsVolumeSlider);
        }

        private void ChangeFullScreenEvent(object sender, EventArgs e)
        {
            ScreenManager.Settings.OptionsData.IsFullScreen = !ScreenManager.Settings.OptionsData.IsFullScreen;

            ScreenManager.ApplyGraphicsDeviceChange();
            ConfirmationDialogBox confirmFullScreen = new ConfirmationDialogBox(
                                        "Sprites/UI/Panels/MenuPanelBackground",
                                        "Are you sure?",
                                        new Vector2(ScreenManager.Viewport.Width / 2, ScreenManager.Viewport.Height / 2),
                                        new Vector2(500, 185),
                                        Color.White,
                                        "Confirm Full Screen Dialog Box");
            confirmFullScreen.CancelButton.InteractEvent += CancelFullScreenEvent;
            confirmFullScreen.ConfirmButton.InteractEvent += ConfirmFullScreenEvent;
            AddScreenUIElement(confirmFullScreen);
        }

        private void CancelFullScreenEvent(object sender, EventArgs e)
        {
            RemoveScreenUIElement(GetScreenUIElement("Confirm Full Screen Dialog Box"));
            ScreenManager.Settings.OptionsData.IsFullScreen = !ScreenManager.Settings.OptionsData.IsFullScreen;

            ScreenManager.ApplyGraphicsDeviceChange();
        }

        private void ConfirmFullScreenEvent(object sender, EventArgs e)
        {
            RemoveScreenUIElement(GetScreenUIElement("Confirm Full Screen Dialog Box"));

            Button fullScreenButton = (Button)GetScreenUIElement("Change Full Screen Button");
            if (fullScreenButton != null)
            {
                fullScreenButton.ChangeText(ScreenManager.Settings.OptionsData.IsFullScreen.ToString());
            }
        }

        private void ChangeMusicVolumeEvent(object sender, EventArgs e)
        {
            Slider musicSlider = (Slider)sender;
            ScreenManager.Settings.OptionsData.MusicVolume = (float)musicSlider.PercentageFilled / 100f;
        }

        private void ChangeSoundEffectsVolume(object sender, EventArgs e)
        {
            Slider soundEffectsSlider = (Slider)sender;
            ScreenManager.Settings.OptionsData.SoundEffectsVolume = (float)soundEffectsSlider.PercentageFilled / 100f;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                ScreenManager.BackToPreviousScreen();
                Die();
            }
        }

        public override void Die()
        {
            ScreenManager.SaveOptions();
            base.Die();
        }
    }
}
