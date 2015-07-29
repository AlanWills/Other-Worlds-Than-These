using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ToGalaxyCustomData;
using ToGalaxyGameLibrary;
using ToGalaxyGameLibrary.Screens_and_ScreenManager;
using ToGalaxyGameLibrary.UI;

namespace ToGalaxy.Screens.Menu_Screens
{
    public class LevelSelectScreen : Screen
    {
        private ExtendedScreenManager ExtendedScreenManager
        {
            get;
            set;
        }

        private Image MissionImage
        {
            get;
            set;
        }

        private Text MissionDescription
        {
            get;
            set;
        }

        private List<Button> MissionButtons
        {
            get;
            set;
        }

        private List<SpaceScreenData> MissionData
        {
            get;
            set;
        }

        private int CurrentLevel
        {
            get;
            set;
        }

        private int maxLevel;

        public LevelSelectScreen(ExtendedScreenManager screenManager, string dataAsset)
            : base(screenManager, dataAsset)
        {
            MissionButtons = new List<Button>();
            MissionData = new List<SpaceScreenData>();

            ExtendedScreenManager = screenManager;
            maxLevel = screenManager.Session.CurrentLevel;
            CurrentLevel = maxLevel;
        }

        public override void LoadContent()
        {
            LoadMissionData(ScreenManager.Content);
            SetUpUI();

            base.LoadContent();
        }

        private void LoadMissionData(ContentManager content)
        {
            DirectoryInfo directory = new DirectoryInfo(content.RootDirectory + "/XML/Space Data/Space Data/");
            if (!directory.Exists)
                throw new DirectoryNotFoundException();

            FileInfo[] files = directory.GetFiles("*.xnb*");
            foreach (FileInfo file in files)
            {
                string key = Path.GetFileNameWithoutExtension(file.Name);
                SpaceScreenData screenData = content.Load<SpaceScreenData>("XML/Space Data/Space Data/" + key);
                MissionData.Add(screenData);
            }
        }

        private void SetUpUI()
        {
            MissionImage = new Image(
                MissionData[CurrentLevel - 1].LevelThumbnailAsset,
                new Vector2(ScreenManager.Viewport.Width / 3, 2 * ScreenManager.Viewport.Height / 5),
                2 * ScreenManager.Viewport.Width / 3,
                2 * ScreenManager.Viewport.Height / 3,
                "Mission Image");
            AddScreenUIElement(MissionImage);

            MissionDescription = new Text(
                MissionData[CurrentLevel - 1].Description,
                new Vector2(ScreenManager.Viewport.Width / 3, 5 * ScreenManager.Viewport.Height / 6),
                3 * ScreenManager.Viewport.Width / 5,
                Color.AliceBlue,
                "Mission Description Text");
            AddScreenUIElement(MissionDescription);

            for (int i = 0; i < maxLevel; i++)
            {
                Button button = new Button(
                    "XML/UI/Buttons/MenuButton",
                    new Vector2(5 * ScreenManager.Viewport.Width / 6, ScreenManager.Viewport.Height / 10 + i * 60),
                    Button.defaultColour,
                    Button.highlightedColour,
                    i.ToString(),
                    MissionData[i].Name);
                button.InteractEvent += LevelInformationEvent;
                MissionButtons.Add(button);
                AddScreenUIElement(button);
            }

            Button acceptMissionButton = new Button(
                "XML/UI/Buttons/MenuButton",
                new Vector2(5 * ScreenManager.Viewport.Width / 6, 9 * ScreenManager.Viewport.Height / 10),
                Button.defaultColour,
                Button.highlightedColour,
                "Accept Mission Button",
                "Start");
            acceptMissionButton.InteractEvent += AcceptMissionEvent;
            AddScreenUIElement(acceptMissionButton);

            Button backButton = new Button(
                "XML/UI/Buttons/MenuButton",
                new Vector2(ScreenManager.Viewport.Width / 6, MissionDescription.Position.Y + MissionDescription.TextOrigin.Y + ScreenManager.Viewport.Height / 20),
                new Color(0.588f, 0, 0),
                new Color(1f, 0, 0),
                "Back Button",
                "Back");
            backButton.InteractEvent += BackEvent;
            AddScreenUIElement(backButton);
        }

        private void LevelInformationEvent(object sender, EventArgs e)
        {
            Button button = sender as Button;

            if (button != null)
            {
                CurrentLevel = Convert.ToInt32(button.Name) + 1;
                ExtendedScreenManager.Session.SetCurrentLevel(CurrentLevel);
                UpdateUI();
            }
        }

        private void AcceptMissionEvent(object sender, EventArgs e)
        {
            ExtendedScreenManager.LoadAndAddScreen(new LoadingScreen(ExtendedScreenManager, "XML/Menu Screens/LoadingScreen"));
            Die();
        }

        private void BackEvent(object sender, EventArgs e)
        {
            ExtendedScreenManager.LoadAndAddScreen(new ShipUpgradeScreen(ExtendedScreenManager, "XML/Menu Screens/ShipUpgradeScreen"));
            Die();
        }

        private void UpdateUI()
        {
            MissionImage.SetTextureFromString(MissionData[CurrentLevel - 1].LevelThumbnailAsset, ScreenManager.Content);
            MissionDescription.ChangeText(MissionData[CurrentLevel - 1].Description);
        }
    }
}