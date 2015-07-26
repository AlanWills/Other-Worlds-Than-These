using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ToGalaxy.Gameplay_Objects;
using ToGalaxyCustomData;
using ToGalaxyGameLibrary;
using ToGalaxyGameLibrary.Screens_and_ScreenManager;
using ToGalaxyGameLibrary.UI;

namespace ToGalaxy.Screens.Menu_Screens
{
    public class ShipUpgradeScreen : Screen
    {
        private ExtendedScreenManager ExtendedScreenManager
        {
            get;
            set;
        }

        private PlayerShip PlayerShip
        {
            get;
            set;
        }

        private Dictionary<string, Panel> UpgradePanels
        {
            get;
            set;
        }

        private Panel CurrentObjectInfoPanel
        {
            get;
            set;
        }

        private Panel CurrentShipInfoPanel
        {
            get;
            set;
        }

        public ShipUpgradeScreen(ExtendedScreenManager screenManager, string screenDataAsset)
            : base(screenManager, screenDataAsset)
        {
            ExtendedScreenManager = screenManager;
            PlayerShip = ExtendedScreenManager.Session.PlayerShip;

            UpgradePanels = new Dictionary<string, Panel>();
        }

        private void SetUpUI()
        {
            Vector2 shipPanelButtonPosition = new Vector2(120, ScreenManager.Viewport.Height / 20);
            Vector2 buttonSpacing = new Vector2(ScreenManager.Viewport.Width / 8, 0);

            CreateAndPopulatePanel("Ships", shipPanelButtonPosition);
            CreateAndPopulatePanel("Weapons", shipPanelButtonPosition + buttonSpacing);
            CreateAndPopulatePanel("Engines", shipPanelButtonPosition + 2 * buttonSpacing);
            CreateAndPopulatePanel("Shields", shipPanelButtonPosition + 3 * buttonSpacing);
            CreateAndPopulatePanel("Sensors", shipPanelButtonPosition + 4 * buttonSpacing);
            CreateAndPopulateShipModPanel("Ship Mods", shipPanelButtonPosition + 5 * buttonSpacing);

            CurrentObjectInfoPanel = new Panel(
                "Sprites/UI/Panels/MenuPanelBackground",
                new Vector2(82 * ScreenManager.Viewport.Width / 100, ScreenManager.Viewport.Height / 3),
                new Vector2(7 * ScreenManager.Viewport.Width / 20, 2 * ScreenManager.Viewport.Height / 5),
                Color.White,
                "Current Object Info",
                0.5f);
            AddScreenUIElement(CurrentObjectInfoPanel);

            CurrentShipInfoPanel = new Panel(
                "Sprites/UI/Panels/MenuPanelBackground",
                new Vector2(82 * ScreenManager.Viewport.Width / 100, 3 * ScreenManager.Viewport.Height / 4),
                new Vector2(7 * ScreenManager.Viewport.Width / 20, 2 * ScreenManager.Viewport.Height / 5),
                Color.White,
                "Current Ship Info",
                0.5f);
            AddScreenUIElement(CurrentShipInfoPanel);

            Text money = new Text(
                ExtendedScreenManager.Session.Money.ToString(),
                shipPanelButtonPosition + 6 * buttonSpacing,
                Color.White,
                "Current Money");
            AddScreenUIElement(money);

            Image moneyImage = new Image(
                "Sprites/UI/Thumbnails/MoneyThumbnail",
                new Vector2(money.Position.X - money.TextOrigin.X - 20, money.Position.Y),
                new Vector2(1, 1),
                Color.Cyan,
                "Current Money Thumbnail");
            AddScreenUIElement(moneyImage);
            moneyImage.SetHoverInfoText("Current Money");

            CreateNextMissionButton();
            ActivatePanel("Ships");
            SetUpCurrentShipInformation();
            DisplayCurrentShipInformation();
        }

        private void CreateAndPopulatePanel(string panelName, Vector2 buttonPosition, bool createSwitchButton = true)
        {
            Panel panel = new Panel(
                "Sprites/UI/Panels/MenuPanelBackground",
                new Vector2(ScreenManager.Viewport.Width / 3, 21 * ScreenManager.Viewport.Height / 40),
                new Vector2(3 * ScreenManager.Viewport.Width / 5, 17 * ScreenManager.Viewport.Height / 20),
                Color.White,
                panelName + " Panel");
            UpgradePanels.Add(panelName, panel);

            // Populate panel here
            DirectoryInfo directory = new DirectoryInfo(ScreenManager.Content.RootDirectory + "/XML/" + panelName);
            if (directory.Exists)
            {
                FileInfo[] files = directory.GetFiles("*.xnb*");

                int yDimensions = 4;
                int xDimensions = 8;
                for (int y = 0; y < yDimensions; y++)
                {
                    for (int x = 0; x < xDimensions; x++)
                    {
                        if (y * xDimensions + x < files.Length)
                        {
                            string key = Path.GetFileNameWithoutExtension(files[y * xDimensions + x].Name);
                            if (panelName != "Shields")
                            {
                                GameObjectData gameObjectData = ScreenManager.Content.Load<GameObjectData>("XML/" + panelName + "/" + key);
                                Image objectImage = new Image(
                                    gameObjectData.TextureAsset,
                                    new Vector2(-2 * panel.Dimensions.X / 5 + x * panel.Dimensions.X / (xDimensions + 1), -2 * panel.Dimensions.Y / 5 + y * panel.Dimensions.Y / yDimensions),
                                    panel.Dimensions.X / (2 * xDimensions + 1),
                                    panel.Dimensions.Y / (2 * yDimensions),
                                    panelName + "/" + key);
                                objectImage.InteractEvent += DisplayShopObjectInformation;
                                panel.LoadAndAddUIElement(objectImage);
                                objectImage.SetHoverInfoText(gameObjectData.Name);
                            }
                            else
                            {
                                ShieldData shieldData = ScreenManager.Content.Load<ShieldData>("XML/" + panelName + "/" + key);
                                Image objectImage = new Image(
                                    shieldData.TextureAsset,
                                    new Vector2(-2 * panel.Dimensions.X / 5 + x * panel.Dimensions.X / xDimensions, -2 * panel.Dimensions.Y / 5 + y * panel.Dimensions.Y / yDimensions),
                                    panel.Dimensions.X / (2 * xDimensions),
                                    panel.Dimensions.Y / (2 * yDimensions),
                                    new Color(shieldData.Colour),
                                    panelName + "/" + key);
                                objectImage.InteractEvent += DisplayShopObjectInformation;
                                panel.LoadAndAddUIElement(objectImage);
                                objectImage.SetHoverInfoText(shieldData.Name);
                            }
                        }
                    }
                }
            }

            if (createSwitchButton)
            {
                CreatePanelSwitchButton(panelName, buttonPosition);
            }

            AddScreenUIElement(panel);
        }

        private void CreateAndPopulateShipModPanel(string panelName, Vector2 buttonPosition)
        {
            Panel panel = new Panel(
                "Sprites/UI/Panels/MenuPanelBackground",
                new Vector2(ScreenManager.Viewport.Width / 3, 21 * ScreenManager.Viewport.Height / 40),
                new Vector2(3 * ScreenManager.Viewport.Width / 5, 17 * ScreenManager.Viewport.Height / 20),
                Color.White,
                panelName + " Panel",
                0);
            UpgradePanels.Add(panelName, panel);

            Button modTypeSwitchButton = new Button(
                "XML/UI/Buttons/MenuButton",
                new Vector2(-panel.Dimensions.X / 3, -panel.Dimensions.Y / 2),
                Button.defaultColour,
                Button.highlightedColour,
                "Ship Mods/Offensive",
                "Offensive");
            modTypeSwitchButton.InteractEvent += ChangePanelEvent;
            panel.LoadAndAddUIElement(modTypeSwitchButton, modTypeSwitchButton.Position);

            modTypeSwitchButton = new Button(
                "XML/UI/Buttons/MenuButton",
                new Vector2(0, -panel.Dimensions.Y / 2),
                Button.defaultColour,
                Button.highlightedColour,
                "Ship Mods/Defensive",
                "Defensive");
            modTypeSwitchButton.InteractEvent += ChangePanelEvent;
            panel.LoadAndAddUIElement(modTypeSwitchButton, modTypeSwitchButton.Position);
            AddScreenUIElement(modTypeSwitchButton);

            modTypeSwitchButton = new Button(
                "XML/UI/Buttons/MenuButton",
                new Vector2(panel.Dimensions.X / 3, -panel.Dimensions.Y / 2),
                Button.defaultColour,
                Button.highlightedColour,
                "Ship Mods/Utility",
                "Utility");
            modTypeSwitchButton.InteractEvent += ChangePanelEvent;
            panel.LoadAndAddUIElement(modTypeSwitchButton, modTypeSwitchButton.Position);
            AddScreenUIElement(modTypeSwitchButton);

            CreateAndPopulatePanel("Ship Mods/Defensive", buttonPosition, false);
            CreateAndPopulatePanel("Ship Mods/Offensive", buttonPosition, false);
            CreateAndPopulatePanel("Ship Mods/Utility", buttonPosition, false);

            CreatePanelSwitchButton(panelName, buttonPosition);
            AddScreenUIElement(panel);
        }

        private void SetUpCurrentShipInformation()
        {
            Image shipImage = new Image(
                    "",
                    new Vector2(-CurrentObjectInfoPanel.Dimensions.X / 4, -CurrentObjectInfoPanel.Dimensions.Y / 10),
                    CurrentObjectInfoPanel.Dimensions.X / 3,
                    2 * CurrentObjectInfoPanel.Dimensions.Y / 5,
                    "Ship Image");
            shipImage.EnableAndDisableEvent += PlayerShipDependentActivationEvent;
            CurrentShipInfoPanel.LoadAndAddUIElement(shipImage, shipImage.Position);

            Button sellAllButton = new Button(
                "XML/UI/Buttons/MenuButton",
                new Vector2(0, 2 * CurrentObjectInfoPanel.Dimensions.Y / 5),
                Button.defaultColour,
                Button.highlightedColour,
                "Sell All Button",
                "Sell All: ");
            sellAllButton.InteractEvent += SellAllEvent;
            sellAllButton.EnableAndDisableEvent += PlayerShipDependentActivationEvent;
            CurrentShipInfoPanel.LoadAndAddUIElementRelativeTo(sellAllButton, shipImage);

            Text loadoutText = new Text(
                "Ship Loadout",
                new Vector2(CurrentShipInfoPanel.Dimensions.X / 8, -CurrentObjectInfoPanel.Dimensions.Y / 3),
                Color.PaleVioletRed,
                "Ship Loadout Text");
            loadoutText.EnableAndDisableEvent += PlayerShipDependentActivationEvent;
            CurrentShipInfoPanel.LoadAndAddNewTextEntryBelowPrevious(loadoutText);

            /*for (int i = 0; i < PlayerShip.ShipData.WeaponHardPoints.Count; i++)
            {
                if (i < PlayerShip.Turrets.Count)
                {
                    Image weaponImage = new Image(
                        PlayerShip.Turrets[i].TurretData.TextureAsset,
                        PlayerShip.ShipData.WeaponHardPoints[i] * shipImage.Scale,
                        15 * shipImage.Scale.X,
                        15 * shipImage.Scale.Y,
                        "Weapon Image " + i);
                    weaponImage.LoadContent(ScreenManager.Content);
                    CurrentShipInfoPanel.AddUIElementRelativeTo(weaponImage, shipImage);

                    Text weaponText = new Text(
                        PlayerShip.Turrets[i].TurretData.Name,
                        new Vector2(CurrentShipInfoPanel.Dimensions.X / 8, -CurrentObjectInfoPanel.Dimensions.Y / 3),
                        Color.White,
                        "Weapon " + i + " Text");
                    CurrentShipInfoPanel.LoadAndAddNewTextEntryBelowPrevious(weaponText);

                    Button sellButton = new Button(
                        "XML/UI/Buttons/MenuButton",
                        new Vector2(weaponText.TextOrigin.X, 0),
                        Button.defaultColour,
                        Button.highlightedColour,
                        "Weapon " + PlayerShip.Turrets[i].TurretData.Name,
                        "Sell: " + (PlayerShip.Turrets[i].TurretData.Cost / 2).ToString());
                    sellAllButton.InteractEvent += SellItemEvent;
                    CurrentShipInfoPanel.LoadAndAddUIElementRelativeTo(sellButton, weaponText);
                }
                else
                {
                    Image weaponFreeImage = new Image(
                        "Sprites/UI/Panels/Panel",
                        PlayerShip.ShipData.WeaponHardPoints[i] * shipImage.Scale,
                        5 * shipImage.Scale.X,
                        5 * shipImage.Scale.Y,
                        Color.Cyan,
                        "Weapon Free Image " + i);
                    weaponFreeImage.SetOpacity(0.3f);
                    CurrentShipInfoPanel.LoadAndAddUIElementRelativeTo(weaponFreeImage, shipImage);
                    weaponFreeImage.SetHoverInfoText("Free Weapon Slot");
                }
            }*/

            /*for (int i = 0; i < PlayerShip.ShipData.EngineHardPoints.Count; i++)
            {
                if (i < PlayerShip.Engines.Count)
                {
                    Image engineImage = new Image(
                        PlayerShip.Engines[i].EngineData.TextureAsset,
                        PlayerShip.ShipData.EngineHardPoints[i] * shipImage.Scale,
                        12 * shipImage.Scale.X,
                        12 * shipImage.Scale.Y,
                        "Engine Image " + i);
                    engineImage.LoadContent(ScreenManager.Content);
                    CurrentShipInfoPanel.AddUIElementRelativeTo(engineImage, shipImage);

                    Text engineText = new Text(
                            PlayerShip.Engines[i].EngineData.Name,
                            new Vector2(CurrentShipInfoPanel.Dimensions.X / 8, -CurrentObjectInfoPanel.Dimensions.Y / 3),
                            Color.White,
                            "Engine " + i + " Text");
                    CurrentShipInfoPanel.LoadAndAddNewTextEntryBelowPrevious(engineText);

                    Button sellButton = new Button(
                        "XML/UI/Buttons/MenuButton",
                        new Vector2(engineText.TextOrigin.X, 0),
                        Button.defaultColour,
                        Button.highlightedColour,
                        "Engine " + PlayerShip.Engines[i].EngineData.Name,
                        "Sell: " + (PlayerShip.Engines[i].EngineData.Cost / 2).ToString());
                    sellAllButton.InteractEvent += SellItemEvent;
                    CurrentShipInfoPanel.LoadAndAddUIElementRelativeTo(sellButton, engineText);
                }
                else
                {
                    Image engineFreeImage = new Image(
                        "Sprites/UI/Panels/Panel",
                        PlayerShip.ShipData.EngineHardPoints[i] * shipImage.Scale,
                        7 * shipImage.Scale.X,
                        7 * shipImage.Scale.Y,
                        Color.LightGreen,
                        "Engine Free Image " + i);
                    engineFreeImage.SetOpacity(0.3f);
                    CurrentShipInfoPanel.LoadAndAddUIElementRelativeTo(engineFreeImage, shipImage);
                    engineFreeImage.SetHoverInfoText("Free Engine Slot");
                }
            }*/

            Image shieldImage = new Image(
                "",
                Vector2.Zero,
                shipImage.Bounds.Width * 2,
                shipImage.Bounds.Height * 2,
                Color.White,
                "Shield Image");
            shieldImage.SetOpacity(0.1f);
            shieldImage.EnableAndDisableEvent += PlayerShipDependentActivationEvent;
            CurrentShipInfoPanel.LoadAndAddUIElementRelativeTo(shieldImage, shipImage);

            Text shieldText = new Text(
                "",
                new Vector2(CurrentShipInfoPanel.Dimensions.X / 8, -CurrentObjectInfoPanel.Dimensions.Y / 3),
                CurrentShipInfoPanel.Dimensions.X / 4,
                Color.White,
                "Shield Text");
            shieldText.EnableAndDisableEvent += PlayerShipDependentActivationEvent;
            CurrentShipInfoPanel.LoadAndAddNewTextEntryBelowPrevious(shieldText);

            Button sellShieldButton = new Button(
                    "XML/UI/Buttons/MenuButton",
                    new Vector2(shieldText.TextOrigin.X, 0),
                    Button.defaultColour,
                    Button.highlightedColour,
                    "Sell Shield Button",
                    "Sell: ");
            sellShieldButton.InteractEvent += SellItemEvent;
            sellShieldButton.EnableAndDisableEvent += PlayerShipDependentActivationEvent;
            // CurrentShipInfoPanel.LoadAndAddUIElementRelativeTo(sellShieldButton, shieldText);
            
            Image sensorImage = new Image(
                "",
                Vector2.Zero,
                12 * shipImage.Scale.X,
                12 * shipImage.Scale.Y,
                "Sensor Image");
            sensorImage.EnableAndDisableEvent += PlayerShipDependentActivationEvent;
            CurrentShipInfoPanel.LoadAndAddUIElementRelativeTo(sensorImage, shipImage);

            Text sensorText = new Text(
                "",
                new Vector2(CurrentShipInfoPanel.Dimensions.X / 8, -CurrentObjectInfoPanel.Dimensions.Y / 3),
                CurrentShipInfoPanel.Dimensions.X / 4,
                Color.White,
                "Sensor Text");
            sensorText.EnableAndDisableEvent += PlayerShipDependentActivationEvent;
            CurrentShipInfoPanel.LoadAndAddNewTextEntryBelowPrevious(sensorText);

            Button sellSensorButton = new Button(
                    "XML/UI/Buttons/MenuButton",
                    new Vector2(sensorText.TextOrigin.X, 0),
                    Button.defaultColour,
                    Button.highlightedColour,
                    "Sell Sensor Button",
                    "Sell: ");
            sellSensorButton.InteractEvent += SellItemEvent;
            sellSensorButton.EnableAndDisableEvent += PlayerShipDependentActivationEvent;
            // CurrentShipInfoPanel.LoadAndAddUIElementRelativeTo(sellSensorButton, sensorText);
        }

        public override void LoadContent()
        {
            SetUpUI();

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (ScreenManager.Input.IsKeyDown(Keys.Escape))
            {
                ExtendedScreenManager.AddNewMainMenuScreen();
                Die();
            }
        }

        #region Information Panels

        private void DisplayShopObjectInformation(object sender, EventArgs e)
        {
            Image objectImage = sender as Image;

            if (objectImage != null)
            {
                string[] splitString = objectImage.Name.Split('/');
                switch (splitString[0])
                {
                    case "Ships":
                        DisplayShipInformation(splitString[1]);
                        break;

                    case "Weapons":
                        DisplayWeaponInformation(splitString[1]);
                        break;

                    case "Engines":
                        DisplayEngineInformation(splitString[1]);
                        break;

                    case "Shields":
                        DisplayShieldInformation(splitString[1]);
                        break;

                    case "Sensors":
                        DisplaySensorInformation(splitString[1]);
                        break;

                    case "Ship Mods":
                        DisplayShipModInformation(splitString[1] + "/" + splitString[2]);
                        break;

                    default:
                        CurrentObjectInfoPanel.Clear();
                        break;
                }
            }
        }

        private void DisplayCurrentShipInformation()
        {
            if (PlayerShip != null)
            {
                Image shipImage = (Image)CurrentShipInfoPanel.GetScreenUIElement("Ship Image");
                if (shipImage != null)
                {
                    shipImage.SetTexture(PlayerShip.Texture);
                }

                Button sellAllButton = (Button)CurrentShipInfoPanel.GetScreenUIElement("Sell All Button");
                if (sellAllButton != null)
                {
                    sellAllButton.ChangeText("Sell All: " + (PlayerShip.TotalWorth / 2).ToString());
                }

                /*
                for (int i = 0; i < PlayerShip.ShipData.WeaponHardPoints.Count; i++)
                {
                    if (i < PlayerShip.Turrets.Count)
                    {
                        Image weaponImage = new Image(
                            PlayerShip.Turrets[i].TurretData.TextureAsset,
                            PlayerShip.ShipData.WeaponHardPoints[i] * shipImage.Scale,
                            15 * shipImage.Scale.X,
                            15 * shipImage.Scale.Y,
                            "Weapon Image " + i);
                        weaponImage.LoadContent(ScreenManager.Content);
                        CurrentShipInfoPanel.AddUIElementRelativeTo(weaponImage, shipImage);

                        Text weaponText = new Text(
                            PlayerShip.Turrets[i].TurretData.Name,
                            new Vector2(CurrentShipInfoPanel.Dimensions.X / 8, -CurrentObjectInfoPanel.Dimensions.Y / 3),
                            Color.White,
                            "Weapon " + i + " Text");
                        CurrentShipInfoPanel.LoadAndAddNewTextEntryBelowPrevious(weaponText);

                        Button sellButton = new Button(
                            "XML/UI/Buttons/MenuButton",
                            new Vector2(weaponText.TextOrigin.X, 0),
                            Button.defaultColour,
                            Button.highlightedColour,
                            "Weapon " + PlayerShip.Turrets[i].TurretData.Name,
                            "Sell: " + (PlayerShip.Turrets[i].TurretData.Cost / 2).ToString());
                        sellAllButton.InteractEvent += SellItemEvent;
                        CurrentShipInfoPanel.LoadAndAddUIElementRelativeTo(sellButton, weaponText);
                    }
                    else
                    {
                        Image weaponFreeImage = new Image(
                            "Sprites/UI/Panels/Panel",
                            PlayerShip.ShipData.WeaponHardPoints[i] * shipImage.Scale,
                            5 * shipImage.Scale.X,
                            5 * shipImage.Scale.Y,
                            Color.Cyan,
                            "Weapon Free Image " + i);
                        weaponFreeImage.SetOpacity(0.3f);
                        CurrentShipInfoPanel.LoadAndAddUIElementRelativeTo(weaponFreeImage, shipImage);
                        weaponFreeImage.SetHoverInfoText("Free Weapon Slot");
                    }
                }

                for (int i = 0; i < PlayerShip.ShipData.EngineHardPoints.Count; i++)
                {
                    if (i < PlayerShip.Engines.Count)
                    {
                        Image engineImage = new Image(
                            PlayerShip.Engines[i].EngineData.TextureAsset,
                            PlayerShip.ShipData.EngineHardPoints[i] * shipImage.Scale,
                            12 * shipImage.Scale.X,
                            12 * shipImage.Scale.Y,
                            "Engine Image " + i);
                        engineImage.LoadContent(ScreenManager.Content);
                        CurrentShipInfoPanel.AddUIElementRelativeTo(engineImage, shipImage);

                        Text engineText = new Text(
                                PlayerShip.Engines[i].EngineData.Name,
                                new Vector2(CurrentShipInfoPanel.Dimensions.X / 8, -CurrentObjectInfoPanel.Dimensions.Y / 3),
                                Color.White,
                                "Engine " + i + " Text");
                        CurrentShipInfoPanel.LoadAndAddNewTextEntryBelowPrevious(engineText);

                        Button sellButton = new Button(
                            "XML/UI/Buttons/MenuButton",
                            new Vector2(engineText.TextOrigin.X, 0),
                            Button.defaultColour,
                            Button.highlightedColour,
                            "Engine " + PlayerShip.Engines[i].EngineData.Name,
                            "Sell: " + (PlayerShip.Engines[i].EngineData.Cost / 2).ToString());
                        sellAllButton.InteractEvent += SellItemEvent;
                        CurrentShipInfoPanel.LoadAndAddUIElementRelativeTo(sellButton, engineText);
                    }
                    else
                    {
                        Image engineFreeImage = new Image(
                            "Sprites/UI/Panels/Panel",
                            PlayerShip.ShipData.EngineHardPoints[i] * shipImage.Scale,
                            7 * shipImage.Scale.X,
                            7 * shipImage.Scale.Y,
                            Color.LightGreen,
                            "Engine Free Image " + i);
                        engineFreeImage.SetOpacity(0.3f);
                        CurrentShipInfoPanel.LoadAndAddUIElementRelativeTo(engineFreeImage, shipImage);
                        engineFreeImage.SetHoverInfoText("Free Engine Slot");
                    }
                }*/

                if (PlayerShip.Shield != null)
                {
                    Image shieldImage = (Image)CurrentShipInfoPanel.GetScreenUIElement("Shield Image");
                    if (shieldImage != null)
                    {
                        shieldImage.SetTexture(PlayerShip.Shield.Texture);
                        shieldImage.SetColour(new Color(PlayerShip.Shield.ShieldData.Colour));
                    }

                    Text shieldText = (Text)CurrentShipInfoPanel.GetScreenUIElement("Shield Text");
                    if (shieldText != null)
                    {
                        //shieldText.ChangeText(PlayerShip.Shield.ShieldData.Name);
                    }
                }

                if (PlayerShip.Sensors != null)
                {
                    Image sensorImage = (Image)CurrentShipInfoPanel.GetScreenUIElement("Sensor Image");
                    if (sensorImage != null)
                    {
                        sensorImage.SetTexture(PlayerShip.Sensors.Texture);
                    }

                    Text sensorText = (Text)CurrentShipInfoPanel.GetScreenUIElement("Sensor Text");
                    if (sensorText != null)
                    {
                        //sensorText.ChangeText(PlayerShip.Sensors.SensorData.Name);
                    }
                }
            }
        }

        #endregion

        #region Display Information Creation Functions

        private void DisplayShipInformation(string shipName)
        {
            CurrentObjectInfoPanel.Clear();

            ShipData shipData = ScreenManager.Content.Load<ShipData>("XML/Ships/" + shipName);
            if (shipData != null)
            {
                Image shipImage = new Image(
                    shipData.TextureAsset,
                    new Vector2(-CurrentObjectInfoPanel.Dimensions.X / 3, -CurrentObjectInfoPanel.Dimensions.Y / 4),
                    CurrentObjectInfoPanel.Dimensions.X / 3,
                    2 * CurrentObjectInfoPanel.Dimensions.Y / 5,
                    shipName + " Image");
                CurrentObjectInfoPanel.LoadAndAddUIElement(shipImage);

                Text name = new Text(
                    shipData.Name,
                    new Vector2(0, -2 * CurrentObjectInfoPanel.Dimensions.Y / 5),
                    Color.LightGreen,
                    "Name");
                CurrentObjectInfoPanel.LoadAndAddUIElement(name);
                name.SetHoverInfoText("Ship Name");

                Text armourText = new Text(
                    shipData.Armour.ToString(),
                    new Vector2(0, 4 * name.TextOrigin.Y),
                    Color.White,
                    shipName + " Armour");
                CurrentObjectInfoPanel.LoadAndAddUIElementRelativeTo(armourText, name);

                Image armourImage = new Image(
                    "Sprites/UI/Thumbnails/ArmourThumbnail",
                    new Vector2(-5 * armourText.TextOrigin.X / 2, 0),
                    "Armour Thumbnail");
                CurrentObjectInfoPanel.LoadAndAddUIElementRelativeTo(armourImage, armourText);
                armourImage.SetHoverInfoText("Armour");

                Text hullText = new Text(
                    shipData.Hull.ToString(),
                    new Vector2(0, 4 * armourText.TextOrigin.Y),
                    Color.White,
                    shipName + " Hull");
                CurrentObjectInfoPanel.LoadAndAddUIElementRelativeTo(hullText, armourText);

                Image hullImage = new Image(
                    "Sprites/UI/Thumbnails/HullThumbnail",
                    new Vector2(-5 * hullText.TextOrigin.X / 2, 0),
                    new Vector2(1, 1),
                    Color.White,
                    "Hull Thumbnail");
                CurrentObjectInfoPanel.LoadAndAddUIElementRelativeTo(hullImage, hullText);
                hullImage.SetHoverInfoText("Hull");

                Text massText = new Text(
                    shipData.Mass.ToString(),
                    new Vector2(0, 4 * hullText.TextOrigin.Y),
                    Color.White,
                    shipName + " Mass");
                CurrentObjectInfoPanel.LoadAndAddUIElementRelativeTo(massText, hullText);

                Image massImage = new Image(
                    "Sprites/UI/Thumbnails/MassThumbnail",
                    new Vector2(-hullText.TextOrigin.X - 35, 0),
                    new Vector2(1, 1),
                    Color.White,
                    "Mass Thumbnail");
                CurrentObjectInfoPanel.LoadAndAddUIElementRelativeTo(massImage, massText);
                massImage.SetHoverInfoText("Mass");

                Text weaponSlotsText = new Text(
                    shipData.WeaponHardPoints.Count.ToString(),
                    new Vector2(0, 4 * massText.TextOrigin.Y),
                    Color.White,
                    shipName + " Weapon Slots");
                CurrentObjectInfoPanel.LoadAndAddUIElementRelativeTo(weaponSlotsText, massText);

                Image weaponSlotsImage = new Image(
                    "Sprites/UI/Thumbnails/WeaponSlotsThumbnail",
                    new Vector2(-weaponSlotsText.TextOrigin.X - 25, 0),
                    "Weapons Slots Thumbnail");
                CurrentObjectInfoPanel.LoadAndAddUIElementRelativeTo(weaponSlotsImage, weaponSlotsText);
                weaponSlotsImage.SetHoverInfoText("Weapon Slots");

                Text shipModSlotsText = new Text(
                    shipData.ShipModSlots.ToString(),
                    new Vector2(0, 4 * massText.TextOrigin.Y),
                    Color.White,
                    shipName + " Ship Mods Slot");
                CurrentObjectInfoPanel.LoadAndAddUIElementRelativeTo(shipModSlotsText, weaponSlotsText);
                shipModSlotsText.SetHoverInfoText("Ship Mod Slots");

                Text descriptionText = new Text(
                    shipData.Description,
                    new Vector2(0, CurrentObjectInfoPanel.Dimensions.Y / 7),
                    4 * CurrentObjectInfoPanel.Dimensions.X / 5,
                    Color.White,
                    shipName + " Description");
                CurrentObjectInfoPanel.LoadAndAddUIElementRelativeTo(descriptionText, shipModSlotsText);

                int cost = shipData.Cost;
                if (PlayerShip != null)
                {
                    cost -= PlayerShip.TotalWorth / 2;
                }
                string costString = cost.ToString();

                Text costText = new Text(
                    costString,
                    new Vector2(-CurrentObjectInfoPanel.Dimensions.X / 8, CurrentObjectInfoPanel.Dimensions.Y / 6),
                    Color.White,
                    "Cost");
                CurrentObjectInfoPanel.LoadAndAddUIElementRelativeTo(costText, descriptionText);

                Image moneyImage = new Image(
                    "Sprites/UI/Thumbnails/MoneyThumbnail",
                    new Vector2(-costText.TextOrigin.X - 20, 0),
                    new Vector2(1, 1),
                    Color.Cyan,
                    "Cost Thumbnail");
                CurrentObjectInfoPanel.LoadAndAddUIElementRelativeTo(moneyImage, costText);

                Button buyButton = new Button(
                    "XML/UI/Buttons/MenuButton",
                    new Vector2(costText.TextOrigin.X + CurrentObjectInfoPanel.Dimensions.X / 3, 0),
                    Button.defaultColour,
                    Button.highlightedColour,
                    shipName + " " + costString,
                    "Purchase");
                buyButton.InteractEvent += BuyShipEvent;
                buyButton.EnableAndDisableEvent += SameShipPurchaseActivationEvent;
                CurrentObjectInfoPanel.LoadAndAddUIElementRelativeTo(buyButton, costText);
            }
        }

        private void DisplayWeaponInformation(string weaponName)
        {
            CurrentObjectInfoPanel.Clear();

            TurretData turretData = ScreenManager.Content.Load<TurretData>("XML/Weapons/" + weaponName);
            if (turretData != null)
            {
                Image weaponImage = new Image(
                    turretData.TextureAsset,
                    new Vector2(-CurrentObjectInfoPanel.Dimensions.X / 3, -CurrentObjectInfoPanel.Dimensions.Y / 4),
                    CurrentObjectInfoPanel.Dimensions.X / 4,
                    CurrentObjectInfoPanel.Dimensions.Y / 5,
                    weaponName + " Image");
                CurrentObjectInfoPanel.LoadAndAddUIElement(weaponImage);

                Text name = new Text(
                    turretData.Name,
                    new Vector2(0, -2 * CurrentObjectInfoPanel.Dimensions.Y / 5),
                    new Color(1, 0, 0.17f, 1),
                    "Name");
                CurrentObjectInfoPanel.LoadAndAddUIElement(name);
                name.SetHoverInfoText("Weapon Name");

                Text weaponType = new Text(
                    turretData.Type,
                    new Vector2(0, 4 * name.TextOrigin.Y),
                    Color.White,
                    weaponName + " Type");
                CurrentObjectInfoPanel.LoadAndAddUIElementRelativeTo(weaponType, name);

                Image weaponTypeImage = new Image(
                    "Sprites/UI/Thumbnails/WeaponType" + turretData.Type,
                    new Vector2(-3 * weaponType.TextOrigin.X / 2, 0),
                    "Weapon Type Thumbnail");
                CurrentObjectInfoPanel.LoadAndAddUIElementRelativeTo(weaponTypeImage, weaponType);
                weaponTypeImage.SetHoverInfoText("Weapon Type");

                int lengthOfString = (1 / turretData.FireTimer).ToString().Length > 4 ? 4 : (1 / turretData.FireTimer).ToString().Length;
                Text weaponFireSpeed = new Text(
                    (1 / turretData.FireTimer).ToString().Substring(0, lengthOfString),
                    new Vector2(0, 4 * weaponType.TextOrigin.Y),
                    Color.White,
                    weaponName + " Fire Speed");
                CurrentObjectInfoPanel.LoadAndAddUIElementRelativeTo(weaponFireSpeed, weaponType);
                weaponFireSpeed.SetHoverInfoText("Shot Per Second");

                Text weaponRange = new Text(
                    turretData.Range.ToString(),
                    new Vector2(0, 4 * weaponFireSpeed.TextOrigin.Y),
                    Color.White,
                    weaponName + " Range");
                CurrentObjectInfoPanel.LoadAndAddUIElementRelativeTo(weaponRange, weaponFireSpeed);
                weaponRange.SetHoverInfoText("Range");

                Text weaponDamage = new Text(
                    turretData.BulletDamage.ToString(),
                    new Vector2(0, 4 * weaponRange.TextOrigin.Y),
                    Color.White,
                    weaponName + " Damage");
                CurrentObjectInfoPanel.LoadAndAddUIElementRelativeTo(weaponDamage, weaponRange);

                Image weaponDamageImage = new Image(
                    "Sprites/UI/Thumbnails/DamageThumbnail",
                    new Vector2(-weaponDamage.TextOrigin.X - 25, 0),
                    new Vector2(1, 1),
                    Color.OrangeRed,
                    "Weapon Damage Thumbnail");
                CurrentObjectInfoPanel.LoadAndAddUIElementRelativeTo(weaponDamageImage, weaponDamage);
                weaponDamageImage.SetHoverInfoText("Damage");

                Text descriptionText = new Text(
                    turretData.Description,
                    new Vector2(0, CurrentObjectInfoPanel.Dimensions.Y / 6),
                    4 * CurrentObjectInfoPanel.Dimensions.X / 5,
                    Color.White,
                    weaponName + " Description");
                CurrentObjectInfoPanel.LoadAndAddUIElementRelativeTo(descriptionText, weaponDamage);

                Text costText = new Text(
                    turretData.Cost.ToString(),
                    new Vector2(-CurrentObjectInfoPanel.Dimensions.X / 8, CurrentObjectInfoPanel.Dimensions.Y / 5),
                    Color.White,
                    "Cost");
                CurrentObjectInfoPanel.LoadAndAddUIElementRelativeTo(costText, descriptionText);

                Image moneyImage = new Image(
                    "Sprites/UI/Thumbnails/MoneyThumbnail",
                    new Vector2(-costText.TextOrigin.X - 20, 0),
                    new Vector2(1, 1),
                    Color.Cyan,
                    "Cost Thumbnail");
                CurrentObjectInfoPanel.LoadAndAddUIElementRelativeTo(moneyImage, costText);

                Button buyButton = new Button(
                    "XML/UI/Buttons/MenuButton",
                    new Vector2(costText.TextOrigin.X + CurrentObjectInfoPanel.Dimensions.X / 3, 0),
                    Button.defaultColour,
                    Button.highlightedColour,
                    weaponName + " " + turretData.Cost,
                    "Purchase");
                buyButton.InteractEvent += BuyWeaponEvent;
                buyButton.EnableAndDisableEvent += WeaponPurchaseActivationEvent;
                CurrentObjectInfoPanel.LoadAndAddUIElementRelativeTo(buyButton, costText);
            }
        }

        private void DisplayEngineInformation(string engineName)
        {
            CurrentObjectInfoPanel.Clear();

            EngineData engineData = ScreenManager.Content.Load<EngineData>("XML/Engines/" + engineName);
            if (engineData != null)
            {
                Image shieldImage = new Image(
                    engineData.TextureAsset,
                    new Vector2(-CurrentObjectInfoPanel.Dimensions.X / 3, -CurrentObjectInfoPanel.Dimensions.Y / 7),
                    CurrentObjectInfoPanel.Dimensions.X / 3,
                    2 * CurrentObjectInfoPanel.Dimensions.Y / 5,
                    engineName + " Image");
                CurrentObjectInfoPanel.LoadAndAddUIElement(shieldImage);

                Text name = new Text(
                    engineData.Name,
                    new Vector2(0, -2 * CurrentObjectInfoPanel.Dimensions.Y / 5),
                    Color.Yellow,
                    "Name");
                CurrentObjectInfoPanel.LoadAndAddUIElement(name);
                name.SetHoverInfoText("Engine Name");

                Text speedText = new Text(
                    engineData.EngineSpeed.ToString(),
                    new Vector2(0, 4 * name.TextOrigin.Y),
                    Color.White,
                    engineName + " Speed");
                CurrentObjectInfoPanel.LoadAndAddUIElementRelativeTo(speedText, name);
                speedText.SetHoverInfoText("Thrust");

                string rotateValueString = MathHelper.ToDegrees(engineData.EngineRotateSpeed).ToString();
                string trimmedRotateValue = rotateValueString.Length > 3 ? rotateValueString.Substring(0, 4) : rotateValueString;

                Text rotateSpeedText = new Text(
                    trimmedRotateValue,
                    new Vector2(0, 4 * speedText.TextOrigin.Y),
                    Color.White,
                    engineData + " RotateSpeed");
                CurrentObjectInfoPanel.LoadAndAddUIElementRelativeTo(rotateSpeedText, speedText);

                Image rotateSpeedImage = new Image(
                    "Sprites/UI/Thumbnails/RotateSpeedThumbnail",
                    new Vector2(-rotateSpeedText.TextOrigin.X - 25, 0),
                    "Rotate Speed Image");
                CurrentObjectInfoPanel.LoadAndAddUIElementRelativeTo(rotateSpeedImage, rotateSpeedText);
                rotateSpeedImage.SetHoverInfoText("Rotate Degrees Per Second");

                Text descriptionText = new Text(
                    engineData.Description,
                    new Vector2(0, 2 * CurrentObjectInfoPanel.Dimensions.Y / 5),
                    4 * CurrentObjectInfoPanel.Dimensions.X / 5,
                    Color.White,
                    engineName + " Description");
                CurrentObjectInfoPanel.LoadAndAddUIElementRelativeTo(descriptionText, rotateSpeedText);

                Text costText = new Text(
                    engineData.Cost.ToString(),
                    new Vector2(-CurrentObjectInfoPanel.Dimensions.X / 8, CurrentObjectInfoPanel.Dimensions.Y / 5),
                    Color.White,
                    "Cost");
                CurrentObjectInfoPanel.LoadAndAddUIElementRelativeTo(costText, descriptionText);

                Image moneyImage = new Image(
                    "Sprites/UI/Thumbnails/MoneyThumbnail",
                    new Vector2(-costText.TextOrigin.X - 20, 0),
                    new Vector2(1, 1),
                    Color.Cyan,
                    "Cost Thumbnail");
                CurrentObjectInfoPanel.LoadAndAddUIElementRelativeTo(moneyImage, costText);

                Button buyButton = new Button(
                    "XML/UI/Buttons/MenuButton",
                    new Vector2(costText.TextOrigin.X + CurrentObjectInfoPanel.Dimensions.X / 3, 0),
                    Button.defaultColour,
                    Button.highlightedColour,
                    engineName + " " + engineData.Cost,
                    "Purchase");
                buyButton.InteractEvent += BuyEngineEvent;
                buyButton.EnableAndDisableEvent += EnginePurchaseActivationEvent;
                CurrentObjectInfoPanel.LoadAndAddUIElementRelativeTo(buyButton, costText);
            }
        }

        private void DisplayShieldInformation(string shieldName)
        {
            CurrentObjectInfoPanel.Clear();

            ShieldData shieldData = ScreenManager.Content.Load<ShieldData>("XML/Shields/" + shieldName);
            if (shieldData != null)
            {
                Image shieldImage = new Image(
                    shieldData.TextureAsset,
                    new Vector2(-CurrentObjectInfoPanel.Dimensions.X / 3, -CurrentObjectInfoPanel.Dimensions.Y / 7),
                    CurrentObjectInfoPanel.Dimensions.X / 3,
                    2 * CurrentObjectInfoPanel.Dimensions.Y / 5,
                    new Color(shieldData.Colour),
                    shieldName + " Image");
                CurrentObjectInfoPanel.LoadAndAddUIElement(shieldImage);

                Text name = new Text(
                    shieldData.Name,
                    new Vector2(0, -2 * CurrentObjectInfoPanel.Dimensions.Y / 5),
                    Color.Cyan,
                    "Name");
                CurrentObjectInfoPanel.LoadAndAddUIElement(name);
                name.SetHoverInfoText("Shield Name");

                Text strengthText = new Text(
                    shieldData.Strength.ToString(),
                    new Vector2(0, 4 * name.TextOrigin.Y),
                    Color.White,
                    shieldName + " Strength");
                CurrentObjectInfoPanel.LoadAndAddUIElementRelativeTo(strengthText, name);
                strengthText.SetHoverInfoText("Strength");

                Text rechargePerSecondText = new Text(
                    shieldData.RechargePerSecond.ToString(),
                    new Vector2(0, 4 * strengthText.TextOrigin.Y),
                    Color.White,
                    shieldName + " Recharge Per Second");
                CurrentObjectInfoPanel.LoadAndAddUIElementRelativeTo(rechargePerSecondText, strengthText);

                Image rechargePerSecondImage = new Image(
                    "Sprites/UI/Thumbnails/RepairRateThumbnail",
                    new Vector2(-rechargePerSecondText.TextOrigin.X - 25, 0),
                    new Vector2(1, 1),
                    Color.LightGreen,
                    "Recharge Per Second Thumbnail");
                CurrentObjectInfoPanel.LoadAndAddUIElementRelativeTo(rechargePerSecondImage, rechargePerSecondText);
                rechargePerSecondImage.SetHoverInfoText("Recharge Per Second");

                Text rechargeDelayText = new Text(
                    shieldData.RechargeDelay.ToString(),
                    new Vector2(0, 4 * rechargePerSecondText.TextOrigin.Y),
                    Color.White,
                    shieldName + " Recharge Delay");
                CurrentObjectInfoPanel.LoadAndAddUIElementRelativeTo(rechargeDelayText, rechargePerSecondText);
                rechargeDelayText.SetHoverInfoText("Recharge Delay");

                Text depletionDelayText = new Text(
                    shieldData.DepletionDelay.ToString(),
                    new Vector2(0, 4 * rechargeDelayText.TextOrigin.Y),
                    Color.White,
                    shieldName + " Depletion Delay");
                CurrentObjectInfoPanel.LoadAndAddUIElementRelativeTo(depletionDelayText, rechargeDelayText);
                depletionDelayText.SetHoverInfoText("Depletion Delay");

                Text descriptionText = new Text(
                    shieldData.Description,
                    new Vector2(0, CurrentObjectInfoPanel.Dimensions.Y / 6),
                    4 * CurrentObjectInfoPanel.Dimensions.X / 5,
                    Color.White,
                    shieldName + " Description");
                CurrentObjectInfoPanel.LoadAndAddUIElementRelativeTo(descriptionText, depletionDelayText);

                int cost = shieldData.Cost;
                if (PlayerShip.Shield != null)
                {
                    cost -= PlayerShip.Shield.ShieldData.Cost / 2;
                }
                string costString = cost.ToString();

                Text costText = new Text(
                    costString,
                    new Vector2(-CurrentObjectInfoPanel.Dimensions.X / 8, CurrentObjectInfoPanel.Dimensions.Y / 5),
                    Color.White,
                    "Cost");
                CurrentObjectInfoPanel.LoadAndAddUIElementRelativeTo(costText, descriptionText);

                Image moneyImage = new Image(
                    "Sprites/UI/Thumbnails/MoneyThumbnail",
                    new Vector2(-costText.TextOrigin.X - 20, 0),
                    new Vector2(1, 1),
                    Color.Cyan,
                    "Cost Thumbnail");
                CurrentObjectInfoPanel.LoadAndAddUIElementRelativeTo(moneyImage, costText);

                Button buyButton = new Button(
                    "XML/UI/Buttons/MenuButton",
                    new Vector2(costText.TextOrigin.X + CurrentObjectInfoPanel.Dimensions.X / 3, 0),
                    Button.defaultColour,
                    Button.highlightedColour,
                    shieldName + " " + costString,
                    "Purchase");
                buyButton.InteractEvent += BuyShieldEvent;
                buyButton.EnableAndDisableEvent += SameShieldPurchaseActivationEvent;
                CurrentObjectInfoPanel.LoadAndAddUIElementRelativeTo(buyButton, costText);
            }
        }

        private void DisplaySensorInformation(string sensorName)
        {
            CurrentObjectInfoPanel.Clear();

            SensorData sensorData = ScreenManager.Content.Load<SensorData>("XML/Sensors/" + sensorName);
            if (sensorData != null)
            {
                Image shieldImage = new Image(
                    sensorData.TextureAsset,
                    new Vector2(-CurrentObjectInfoPanel.Dimensions.X / 3, -CurrentObjectInfoPanel.Dimensions.Y / 7),
                    CurrentObjectInfoPanel.Dimensions.X / 3,
                    2 * CurrentObjectInfoPanel.Dimensions.Y / 5,
                    sensorName + " Image");
                CurrentObjectInfoPanel.LoadAndAddUIElement(shieldImage);

                Text name = new Text(
                    sensorData.Name,
                    new Vector2(0, -2 * CurrentObjectInfoPanel.Dimensions.Y / 5),
                    Color.Yellow,
                    "Name");
                CurrentObjectInfoPanel.LoadAndAddUIElement(name);
                name.SetHoverInfoText("Sensor Name");

                Text sensorRangeText = new Text(
                    sensorData.Range.ToString(),
                    new Vector2(0, 4 * name.TextOrigin.Y),
                    Color.White,
                    sensorName + " Range");
                CurrentObjectInfoPanel.LoadAndAddUIElementRelativeTo(sensorRangeText, name);

                Image sensorRangeImage = new Image(
                    "Sprites/UI/Thumbnails/SensorRangeThumbnail",
                    new Vector2(-2 * sensorRangeText.TextOrigin.X, 0),
                    new Vector2(1, 1),
                    Color.LightGreen,
                    "Range Image");
                CurrentObjectInfoPanel.LoadAndAddUIElementRelativeTo(sensorRangeImage, sensorRangeText);
                sensorRangeImage.SetHoverInfoText("Range");

                Text descriptionText = new Text(
                    sensorData.Description,
                    new Vector2(0, CurrentObjectInfoPanel.Dimensions.Y / 2),
                    4 * CurrentObjectInfoPanel.Dimensions.X / 5,
                    Color.White,
                    sensorName + " Description");
                CurrentObjectInfoPanel.LoadAndAddUIElementRelativeTo(descriptionText, sensorRangeText);

                int cost = sensorData.Cost;
                if (PlayerShip.Sensors != null)
                {
                    cost -= PlayerShip.Sensors.SensorData.Cost / 2;
                }
                string costString = cost.ToString();

                Text costText = new Text(
                    costString,
                    new Vector2(-CurrentObjectInfoPanel.Dimensions.X / 8, CurrentObjectInfoPanel.Dimensions.Y / 5),
                    Color.White,
                    "Cost");
                CurrentObjectInfoPanel.LoadAndAddUIElementRelativeTo(costText, descriptionText);

                Image moneyImage = new Image(
                    "Sprites/UI/Thumbnails/MoneyThumbnail",
                    new Vector2(-costText.TextOrigin.X - 20, 0),
                    new Vector2(1, 1),
                    Color.Cyan,
                    "Cost Thumbnail");
                CurrentObjectInfoPanel.LoadAndAddUIElementRelativeTo(moneyImage, costText);

                Button buyButton = new Button(
                    "XML/UI/Buttons/MenuButton",
                    new Vector2(costText.TextOrigin.X + CurrentObjectInfoPanel.Dimensions.X / 3, 0),
                    Button.defaultColour,
                    Button.highlightedColour,
                    sensorName + " " + costString,
                    "Purchase");
                buyButton.InteractEvent += BuySensorEvent;
                buyButton.EnableAndDisableEvent += SameSensorPurchaseActivationEvent;
                CurrentObjectInfoPanel.LoadAndAddUIElementRelativeTo(buyButton, costText);
            }
        }

        private void DisplayShipModInformation(string shipModName)
        {
            CurrentObjectInfoPanel.Clear();

            ShipModData shipModData = ScreenManager.Content.Load<ShipModData>("XML/Ship Mods/" + shipModName);
            if (shipModData != null)
            {
                Image shieldImage = new Image(
                    shipModData.TextureAsset,
                    new Vector2(-CurrentObjectInfoPanel.Dimensions.X / 3, -CurrentObjectInfoPanel.Dimensions.Y / 4),
                    CurrentObjectInfoPanel.Dimensions.X / 6,
                    CurrentObjectInfoPanel.Dimensions.X / 6,
                    shipModName + " Image");
                CurrentObjectInfoPanel.LoadAndAddUIElement(shieldImage);

                Text name = new Text(
                    shipModData.Name,
                    new Vector2(0, -2 * CurrentObjectInfoPanel.Dimensions.Y / 5),
                    Color.Yellow,
                    "Name");
                CurrentObjectInfoPanel.LoadAndAddUIElement(name);
                name.SetHoverInfoText("Ship Mod Name");

                Text activeOrPassiveText;
                if (shipModData.Active)
                {
                    activeOrPassiveText = new Text(
                        "Active",
                        new Vector2(0, 4 * name.TextOrigin.Y),
                        Color.White,
                        "Active Or Passive Text");
                    CurrentObjectInfoPanel.LoadAndAddUIElementRelativeTo(activeOrPassiveText, name);
                    activeOrPassiveText.SetHoverInfoText("Mod Type");

                    Text runTimeText = new Text(
                        shipModData.RunTime.ToString(),
                        new Vector2(0, 4 * name.TextOrigin.Y),
                        Color.White,
                        "Run Time Text");
                    CurrentObjectInfoPanel.LoadAndAddUIElementRelativeTo(runTimeText, activeOrPassiveText);
                    runTimeText.SetHoverInfoText("Mod Run Time");

                    Text coolDownText = new Text(
                        shipModData.Cooldown.ToString(),
                        new Vector2(0, 4 * name.TextOrigin.Y),
                        Color.White,
                        "Cool Down Text");
                    CurrentObjectInfoPanel.LoadAndAddUIElementRelativeTo(coolDownText, runTimeText);
                    coolDownText.SetHoverInfoText("Mod Cool Down");
                }
                else
                {
                    activeOrPassiveText = new Text(
                        "Passive",
                        new Vector2(0, 4 * name.TextOrigin.Y),
                        Color.White,
                        "Active Or Passive Text");
                    CurrentObjectInfoPanel.LoadAndAddUIElementRelativeTo(activeOrPassiveText, name);
                    activeOrPassiveText.SetHoverInfoText("Mod Type");
                }

                Text descriptionText = new Text(
                    shipModData.Description,
                    new Vector2(0, 2 * CurrentObjectInfoPanel.Dimensions.Y / 5),
                    4 * CurrentObjectInfoPanel.Dimensions.X / 5,
                    Color.White,
                    shipModName + " Description");
                CurrentObjectInfoPanel.LoadAndAddUIElementRelativeTo(descriptionText, activeOrPassiveText);

                int cost = shipModData.Cost;
                if (PlayerShip.Sensors != null)
                {
                    cost -= PlayerShip.Sensors.SensorData.Cost / 2;
                }
                string costString = cost.ToString();

                Text costText = new Text(
                    costString,
                    new Vector2(-CurrentObjectInfoPanel.Dimensions.X / 8, CurrentObjectInfoPanel.Dimensions.Y / 4),
                    Color.White,
                    "Cost");
                CurrentObjectInfoPanel.LoadAndAddUIElementRelativeTo(costText, descriptionText);

                Image moneyImage = new Image(
                    "Sprites/UI/Thumbnails/MoneyThumbnail",
                    new Vector2(-costText.TextOrigin.X - 20, 0),
                    new Vector2(1, 1),
                    Color.Cyan,
                    "Cost Thumbnail");
                CurrentObjectInfoPanel.LoadAndAddUIElementRelativeTo(moneyImage, costText);

                Button buyButton = new Button(
                    "XML/UI/Buttons/MenuButton",
                    new Vector2(costText.TextOrigin.X + CurrentObjectInfoPanel.Dimensions.X / 3, 0),
                    Button.defaultColour,
                    Button.highlightedColour,
                    shipModName + " " + costString,
                    "Purchase");
                buyButton.InteractEvent += BuyShipModEvent;
                buyButton.EnableAndDisableEvent += SameShipModPurchaseActivationEvent;
                CurrentObjectInfoPanel.LoadAndAddUIElementRelativeTo(buyButton, costText);
            }
        }

        #endregion

        #region Purchase Events

        private void BuyShipEvent(object sender, EventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                string[] strings = button.Name.Split(' ');
                if (PlayerShip != null)
                {
                    ExtendedScreenManager.Session.AddMoney(-Convert.ToInt32(strings[1]) + PlayerShip.TotalWorth / 2);
                }
                else
                {
                    ExtendedScreenManager.Session.AddMoney(-Convert.ToInt32(strings[1]));
                }

                PlayerShip = new PlayerShip("XML/Ships/" + strings[0], Vector2.Zero);
                PlayerShip.LoadContent(ScreenManager.Content);
                DisplayCurrentShipInformation();

                Text currentMoney = (Text)GetScreenUIElement("Current Money");
                if (currentMoney != null)
                {
                    currentMoney.ChangeText(ExtendedScreenManager.Session.Money.ToString());
                }
            }
        }

        private void BuyWeaponEvent(object sender, EventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                string[] strings = button.Name.Split(' ');

                if (PlayerShip.Turrets.Count < PlayerShip.ShipData.WeaponHardPoints.Count)
                {
                    ExtendedScreenManager.Session.AddMoney(-Convert.ToInt32(strings[1]));

                    PlayerShip.ShipData.TurretNames.Add(strings[0]);
                    DisplayCurrentShipInformation();

                    Text currentMoney = (Text)GetScreenUIElement("Current Money");
                    if (currentMoney != null)
                    {
                        currentMoney.ChangeText(ExtendedScreenManager.Session.Money.ToString());
                    }

                    PlayerShip.LoadTurrets(ScreenManager.Content);
                }
                else
                {
                    // Add dialog box saying all weapon slots are full.
                    // Or maybe deactivate the button beforehand so only appears if a slot free.
                }
            }
        }

        private void BuyEngineEvent(object sender, EventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                string[] strings = button.Name.Split(' ');

                if (PlayerShip.Engines.Count < PlayerShip.ShipData.EngineHardPoints.Count)
                {
                    ExtendedScreenManager.Session.AddMoney(-Convert.ToInt32(strings[1]));

                    PlayerShip.ShipData.EngineNames.Add(strings[0]);
                    PlayerShip.LoadEngines(ScreenManager.Content);
                    DisplayCurrentShipInformation();

                    Text currentMoney = (Text)GetScreenUIElement("Current Money");
                    if (currentMoney != null)
                    {
                        currentMoney.ChangeText(ExtendedScreenManager.Session.Money.ToString());
                    }
                }
                else
                {
                    // Add dialog box saying all engine slots are full.
                    // Or maybe deactivate the button beforehand so only appears if a slot free.
                }
            }
        }

        private void BuyShieldEvent(object sender, EventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                string[] strings = button.Name.Split(' ');

                if (PlayerShip.Shield != null)
                {
                    ExtendedScreenManager.Session.AddMoney(-Convert.ToInt32(strings[1]) + PlayerShip.Shield.ShieldData.Cost / 2);
                }
                else
                {
                    ExtendedScreenManager.Session.AddMoney(-Convert.ToInt32(strings[1]));
                }

                PlayerShip.ShipData.ShieldAsset = strings[0];
                PlayerShip.LoadShield(ScreenManager.Content);
                DisplayCurrentShipInformation();

                Text currentMoney = (Text)GetScreenUIElement("Current Money");
                if (currentMoney != null)
                {
                    currentMoney.ChangeText(ExtendedScreenManager.Session.Money.ToString());
                }
            }
        }

        private void BuySensorEvent(object sender, EventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                string[] strings = button.Name.Split(' ');

                if (PlayerShip.Sensors != null)
                {
                    ExtendedScreenManager.Session.AddMoney(-Convert.ToInt32(strings[1]) + PlayerShip.Sensors.SensorData.Cost / 2);
                }
                else
                {
                    ExtendedScreenManager.Session.AddMoney(-Convert.ToInt32(strings[1]));
                }

                PlayerShip.ShipData.SensorName = strings[0];
                PlayerShip.LoadSensors(ScreenManager.Content);
                DisplayCurrentShipInformation();

                Text currentMoney = (Text)GetScreenUIElement("Current Money");
                if (currentMoney != null)
                {
                    currentMoney.ChangeText(ExtendedScreenManager.Session.Money.ToString());
                }
            }
        }

        private void BuyShipModEvent(object sender, EventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                string[] strings = button.Name.Split(' ');

                ExtendedScreenManager.Session.AddMoney(-Convert.ToInt32(strings[1]));

                PlayerShip.ShipData.ShipModNames.Add(strings[0]);
                PlayerShip.LoadShipMods(ScreenManager.Content);
                DisplayCurrentShipInformation();

                Text currentMoney = (Text)GetScreenUIElement("Current Money");
                if (currentMoney != null)
                {
                    currentMoney.ChangeText(ExtendedScreenManager.Session.Money.ToString());
                }
            }
        }

        #endregion

        #region Sell Events

        private void SellAllEvent(object sender, EventArgs e)
        {
            if (PlayerShip != null)
            {
                ExtendedScreenManager.Session.AddMoney(PlayerShip.TotalWorth / 2);

                PlayerShip = null;

                Text currentMoney = (Text)GetScreenUIElement("Current Money");
                if (currentMoney != null)
                {
                    currentMoney.ChangeText(ExtendedScreenManager.Session.Money.ToString());
                }

                // DisplayCurrentShipInformation();
            }
        }

        private void SellItemEvent(object sender, EventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                string[] strings = button.Name.Split(' ');
                switch (strings[0])
                {
                    case "Weapon":
                        TurretData turretData = ScreenManager.Content.Load<TurretData>("XML/Weapons/" + strings[1]);
                        PlayerShip.ShipData.TurretNames.Remove(strings[1]);
                        ExtendedScreenManager.Session.AddMoney(turretData.Cost / 2);
                        break;

                    case "Engine":
                        EngineData engineData = ScreenManager.Content.Load<EngineData>("XML/Engines/" + strings[1]);
                        PlayerShip.ShipData.EngineNames.Remove(strings[1]);
                        ExtendedScreenManager.Session.AddMoney(engineData.Cost / 2);
                        break;

                    case "Shield":
                        PlayerShip.ShipData.ShieldAsset = "";
                        ExtendedScreenManager.Session.AddMoney(PlayerShip.Shield.ShieldData.Cost / 2);
                        break;

                    case "Sensor":
                        PlayerShip.ShipData.SensorName = "";
                        ExtendedScreenManager.Session.AddMoney(PlayerShip.Sensors.SensorData.Cost / 2);
                        break;

                    default:
                        break;
                }
            }
        }

        #endregion

        #region Panel and Next Mission Button Functions

        private void CreatePanelSwitchButton(string panelName, Vector2 buttonPosition)
        {
            Button panelButton = new Button(
                                    "XML/UI/Buttons/MenuButton",
                                    buttonPosition,
                                    new Color(0, 0.318f, 0.49f),
                                    new Color(0, 0.71f, 0.988f),
                                    panelName,
                                    panelName);
            panelButton.InteractEvent += ChangePanelEvent;
            panelButton.EnableAndDisableEvent += PlayerShipDependentActivationEvent;
            AddScreenUIElement(panelButton);
        }

        private void ChangePanelEvent(object sender, EventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                ActivatePanel(button.Name);
            }
        }

        private void CreateNextMissionButton()
        {
            Button nextMissionButton = new Button(
                "XML/UI/Buttons/MenuButton",
                new Vector2(9 * ScreenManager.Viewport.Width / 10, ScreenManager.Viewport.Height / 20),
                Color.Green,
                Color.LightGreen,
                "Next Mission Button",
                "Next Mission");
            nextMissionButton.InteractEvent += ProceedToNextMissionEvent;
            nextMissionButton.EnableAndDisableEvent += PlayerShipDependentActivationEvent;
            AddScreenUIElement(nextMissionButton);
        }

        private void ProceedToNextMissionEvent(object sender, EventArgs e)
        {
            ExtendedScreenManager.Session.PlayerShip = PlayerShip;
            ExtendedScreenManager.LoadAndAddScreen(new LevelSelectScreen(ExtendedScreenManager, "XML/Menu Screens/LevelSelectScreen"));
            Die();
        }

        #endregion

        #region Disable and Activate Functions

        private void PlayerShipDependentActivationEvent(object sender, EventArgs e)
        {
            UIElement uielement = sender as UIElement;
            if (uielement != null)
            {
                if (PlayerShip != null)
                {
                    uielement.Activate();
                }
                else
                {
                    uielement.DisableAndHide();
                }
            }
            else
            {
                uielement.DisableAndHide();
            }
        }

        private void SameShipPurchaseActivationEvent(object sender, EventArgs e)
        {
            Button buyButton = sender as Button;
            if (buyButton != null)
            {
                Text objectName = (Text)CurrentObjectInfoPanel.GetScreenUIElement("Name");
                if (objectName != null)
                {
                    if (PlayerShip != null)
                    {
                        if (PlayerShip.ShipData.Name != buyButton.Name.Split(' ')[0])
                        {
                            buyButton.Activate();
                            SufficientMoneyActivationEvent(sender, e);
                        }
                        else
                        {
                            buyButton.DisableAndHide();
                        }
                    }
                    else
                    {
                        buyButton.Activate();
                        SufficientMoneyActivationEvent(sender, e);
                    }
                }
            }
        }

        private void WeaponPurchaseActivationEvent(object sender, EventArgs e)
        {
            Button buyButton = sender as Button;
            if (buyButton != null)
            {
                if (PlayerShip.Turrets.Count < PlayerShip.ShipData.WeaponHardPoints.Count)
                {
                    buyButton.Activate();
                    SufficientMoneyActivationEvent(sender, e);
                }
                else
                {
                    buyButton.DisableAndHide();
                }
            }
        }

        private void EnginePurchaseActivationEvent(object sender, EventArgs e)
        {
            Button buyButton = sender as Button;
            if (buyButton != null)
            {
                if (PlayerShip.Engines.Count < PlayerShip.ShipData.EngineHardPoints.Count)
                {
                    buyButton.Activate();
                    SufficientMoneyActivationEvent(sender, e);
                }
                else
                {
                    buyButton.DisableAndHide();
                }
            }
        }

        private void SameShieldPurchaseActivationEvent(object sender, EventArgs e)
        {
            Button buyButton = sender as Button;
            if (buyButton != null)
            {
                Text objectName = (Text)CurrentObjectInfoPanel.GetScreenUIElement("Name");
                if (objectName != null)
                {
                    if (PlayerShip.ShipData.ShieldAsset != buyButton.Name.Split(' ')[0])
                    {
                        buyButton.Activate();
                        SufficientMoneyActivationEvent(sender, e);
                    }
                    else
                    {
                        buyButton.DisableAndHide();
                    }
                }
            }
        }

        private void SameSensorPurchaseActivationEvent(object sender, EventArgs e)
        {
            Button buyButton = sender as Button;
            if (buyButton != null)
            {
                Text objectName = (Text)CurrentObjectInfoPanel.GetScreenUIElement("Name");
                if (objectName != null)
                {
                    if (PlayerShip.ShipData.SensorName != buyButton.Name.Split(' ')[0])
                    {
                        buyButton.Activate();
                        SufficientMoneyActivationEvent(sender, e);
                    }
                    else
                    {
                        buyButton.DisableAndHide();
                    }
                }
            }
        }

        private void SameShipModPurchaseActivationEvent(object sender, EventArgs e)
        {
            Button buyButton = sender as Button;
            if (buyButton != null)
            {
                Text objectName = (Text)CurrentObjectInfoPanel.GetScreenUIElement("Name");
                if (objectName != null)
                {
                    if (PlayerShip.ShipData.ShipModNames.Count < PlayerShip.ShipData.ShipModSlots)
                    {
                        if (!PlayerShip.ShipData.ShipModNames.Contains(buyButton.Name.Split(' ')[0]))
                        {
                            buyButton.Activate();
                            SufficientMoneyActivationEvent(sender, e);
                        }
                        else
                        {
                            buyButton.DisableAndHide();
                        }
                    }
                    else
                    {
                        buyButton.DisableAndHide();
                    }
                }
            }
        }

        private void SufficientMoneyActivationEvent(object sender, EventArgs e)
        {
            Button buyButton = sender as Button;
            if (buyButton != null)
            {
                string[] strings = buyButton.Name.Split(' ');
                if (ExtendedScreenManager.Session.Money >= Convert.ToInt32(strings[1]))
                {
                    buyButton.Activate();
                }
                else
                {
                    buyButton.DisableAndHide();
                }
            }
        }

        private void ActivatePanel(string panelToActivate)
        {
            foreach (KeyValuePair<string, Panel> panel in UpgradePanels)
            {
                if (panel.Key == panelToActivate)
                {
                    panel.Value.Activate();
                    if (panelToActivate.Contains("Defensive") || panelToActivate.Contains("Offensive") || panelToActivate.Contains("Utility"))
                    {
                        UpgradePanels["Ship Mods"].Activate();
                    }
                }
                else
                {
                    panel.Value.DisableAndHide();
                }
            }
        }

        #endregion
    }
}
