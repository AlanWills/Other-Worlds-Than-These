﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToGalaxy.Gameplay_Objects;
using ToGalaxy.Screens.Gameplay_Screens;
using ToGalaxyGameLibrary.UI;

namespace ToGalaxy.UI
{
    public class SpaceScreenHud : Panel
    {
        private ShipInformationPanel ShipInfoPanel
        {
            get;
            set;
        }

        private SensorsUI SensorsUI
        {
            get;
            set;
        }

        private WeaponsCooldownUI WeaponCooldownUI
        {
            get;
            set;
        }

        private ShipModCooldownUI ShipModCooldownUI
        {
            get;
            set;
        }

        private SpaceScreen SpaceScreen
        {
            get;
            set;
        }

        public SpaceScreenHud(string dataAsset, SpaceScreen spaceScreen, Vector2 position, Vector2 dimensions, Color colour, string name)
            : base(dataAsset, position, dimensions, colour, name)
        {
            paddingVector = new Vector2(0, 0);

            SpaceScreen = spaceScreen;
        }

        public override void LoadContent(ContentManager content)
        {
            base.LoadContent(content);

            Vector2 infoPanelDimensions = new Vector2(400, 200);

            ShipInfoPanel = new ShipInformationPanel(
                "Sprites/UI/Panels/MenuPanelBackground",
                new Vector2((infoPanelDimensions.X - Dimensions.X) / 2, (Dimensions.Y - infoPanelDimensions.Y) / 2),
                infoPanelDimensions,
                Color.White,
                "Ship Info Panel");
            AddUIElement(ShipInfoPanel, ShipInfoPanel.Position);
            ShipInfoPanel.LoadContent(content);

            if (SpaceScreen.PlayerShip.Sensors != null)
            {
                SensorsUI = new SensorsUI(
                    "XML/UI/Sensors/SensorsUI",
                    Vector2.Zero,
                    SpaceScreen,
                    (int)(SpaceScreen.PlayerShip.Sensors.SensorData.Range * SpaceScreen.PlayerShip.Sensors.RangeMultiplier),
                    Color.White,
                    "Sensors UI",
                    0.7f);
                LoadAndAddUIElement(SensorsUI, SensorsUI.Position);
                SensorsUI.SetPosition(new Vector2(Dimensions.X - SensorsUI.Bounds.Width / 2, Dimensions.Y - SensorsUI.Bounds.Height / 2));
            }

            WeaponCooldownUI = new WeaponsCooldownUI(
                "",
                SpaceScreen.PlayerShip,
                new Vector2(ShipInfoPanel.Dimensions.X / 2 + SpaceScreen.PlayerShip.Turrets.Count * 35, 0),
                new Vector2(SpaceScreen.PlayerShip.Turrets.Count * 35 + 5, 50),
                Color.White,
                "Weapons Cooldown UI",
                0.8f);
            // Add it first so that we get the proper screen position for the cooldown bar in the load content method of the WeaponsCooldownUI
            AddUIElementRelativeTo(WeaponCooldownUI, ShipInfoPanel);
            WeaponCooldownUI.LoadContent(content);

            ShipModCooldownUI = new ShipModCooldownUI(
                "",
                SpaceScreen.PlayerShip,
                new Vector2(WeaponCooldownUI.Dimensions.X / 2 + SpaceScreen.PlayerShip.ShipMods.Count * 55, 0),
                new Vector2(SpaceScreen.PlayerShip.ShipMods.Count * 55 + 5, 50),
                Color.White,
                "Ship Mods Cooldown UI",
                0.8f);
            AddUIElementRelativeTo(ShipModCooldownUI, WeaponCooldownUI);
            ShipModCooldownUI.LoadContent(content);
        }

        public void ShowShipInfo(Ship ship)
        {
            ShipInfoPanel.SetShip(ship);
            WeaponCooldownUI.Activate();
            ShipModCooldownUI.Activate();
        }

        public void ClearShipInfo()
        {
            ShipInfoPanel.SetShip(null);
            WeaponCooldownUI.DisableAndHide();
            ShipModCooldownUI.DisableAndHide();
        }
    }
}
