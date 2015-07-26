using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToGalaxy.Gameplay_Objects;
using ToGalaxy.Gameplay_Objects.Space_Screen;
using ToGalaxy.Screens.Cut_Scene_Screens;
using ToGalaxy.Screens.Cut_Scene_Screens.Cut_Scenes;
using ToGalaxy.Screens.Cut_Scene_Screens.Text_Cut_Scenes;
using ToGalaxyGameLibrary.Game_Objects;
using ToGalaxyGameLibrary.Screens_and_ScreenManager;
using ToGalaxyGameLibrary.UI;

namespace ToGalaxy.Screens.Menu_Screens
{
    public class MainMenuScreen : Screen
    {
        private ExtendedScreenManager ExtendedScreenManager
        {
            get;
            set;
        }

        public MainMenuScreen(ExtendedScreenManager screenManager, string screenDataAsset)
            : base(screenManager, screenDataAsset)
        {
            ExtendedScreenManager = screenManager;
        }

        public override void LoadContent()
        {
            base.LoadContent();

            SetUpTitle();
            SetUpButtons(ScreenManager.Content);

            ExtendedScreenManager.Camera.SetFixedScreenCamera(true);
        }

        private void SetUpTitle()
        {
            Image title = new Image("Sprites/UI/Titles/MainMenuTitleBlue",
                                    new Vector2(ScreenManager.Viewport.Width / 3, ScreenManager.Viewport.Height / 7),
                                    "Main Menu Title");
            AddScreenUIElement(title);
        }

        private void SetUpButtons(ContentManager content)
        {
            int buttonPadding = 20;
            int panelPadding = 10;
            int numberOfButtons = 4;

            Panel panel = new Panel(
                "Sprites/UI/Panels/MenuPanelBackground",
                new Vector2(ScreenManager.Viewport.Width / 3, ScreenManager.Viewport.Height / 3 + panelPadding + buttonPadding + 60),
                new Vector2(200 + panelPadding, numberOfButtons * 40 + (numberOfButtons - 1) * buttonPadding + 2 * panelPadding),
                new Color(0, 0, 0.7f, 1f),
                "Menu Buttons Panel",
                0.6f);
            AddScreenUIElement(panel);

            Button newGameButton = new Button(
                "XML/UI/Buttons/MenuButton", 
                new Vector2(ScreenManager.Viewport.Width / 3, ScreenManager.Viewport.Height / 3), 
                Button.defaultColour,
                Button.highlightedColour,
                "New Game Button",
                "New Game");
            newGameButton.InteractEvent += NewGameEvent;
            AddScreenUIElement(newGameButton);
            // newGameButton.LoadContent(content);
            // AddUIElement(newGameButton, new Vector2(panel.Dimensions.X / 2, panel.Dimensions.Y / 2));

            Button continueButton = new Button(
                "XML/UI/Buttons/MenuButton",
                newGameButton.Position + new Vector2(0, newGameButton.Bounds.Height + buttonPadding),
                Button.defaultColour,
                Button.highlightedColour,
                "Continue Button",
                "Continue");
            continueButton.InteractEvent += ContinueEvent;
            AddScreenUIElement(continueButton);
            // continueButton.LoadContent(content);
            // panel.AddUIElementRelativeTo(continueButton, newGameButton, new Vector2(0, newGameButton.Bounds.Height + buttonPadding));

            Button settingsButton = new Button(
                "XML/UI/Buttons/MenuButton",
                continueButton.Position + new Vector2(0, continueButton.Bounds.Height + buttonPadding),
                Button.defaultColour,
                Button.highlightedColour,
                "Settings Button",
                "Settings");
            settingsButton.InteractEvent += SettingsEvent;
            AddScreenUIElement(settingsButton);

            Button exitButton = new Button(
                "XML/UI/Buttons/MenuButton",
                settingsButton.Position + new Vector2(0, settingsButton.Bounds.Height + buttonPadding),
                new Color(0.588f, 0, 0),
                new Color(1f, 0, 0),
                "Exit Button", 
                "Exit");
            exitButton.InteractEvent += ExitEvent;
            AddScreenUIElement(exitButton);
        }

        private void NewGameEvent(object sender, EventArgs e)
        {
            ExtendedScreenManager.LoadAndAddScreen(new ShipUpgradeScreen(ExtendedScreenManager, "XML/Menu Screens/ShipUpgradeScreen"));
            // ExtendedScreenManager.LoadAndAddScreen(new IntroCutScene4(ExtendedScreenManager));
            Die();
        }

        private void ContinueEvent(object sender, EventArgs e)
        {
            ExtendedScreenManager.LoadAndAddScreen(new LevelSelectScreen(ExtendedScreenManager, "XML/Menu Screens/LevelSelectScreen"));
            Die();
        }

        private void SettingsEvent(object sender, EventArgs e)
        {
            ExtendedScreenManager.LoadAndAddScreen(new OptionsScreen(ExtendedScreenManager, "XML/Menu Screens/OptionsScreen"));
            Die();
        }

        private void ExitEvent(object sender, EventArgs e)
        {
            ExtendedScreenManager.Quit();
        }
    }
}
