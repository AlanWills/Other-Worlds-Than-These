using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToGalaxy.Gameplay_Objects;
using ToGalaxyGameLibrary;
using ToGalaxyGameLibrary.UI;

namespace ToGalaxy.UI
{
    public class ShipModCooldownUI : Panel
    {
        private PlayerShip Ship
        {
            get;
            set;
        }

        private List<RefreshUI> ShipModRefreshUI
        {
            get;
            set;
        }

        public ShipModCooldownUI(string dataAsset, PlayerShip ship, Vector2 position, Vector2 dimensions, Color colour, string name, float opacity = 0.25f)
            : base(dataAsset, position, dimensions, colour, name, opacity)
        {
            Ship = ship;
            ShipModRefreshUI = new List<RefreshUI>();
        }

        public override void LoadContent(ContentManager content)
        {
            base.LoadContent(content);

            if (Ship.ShipMods.Count > 0)
            {
                RefreshUI shipModRefreshUI = new RefreshUI(
                            Ship.ShipMods[0].ShipModData.TextureAsset,
                            new Vector2(5 - Dimensions.X / 2, 25),
                            50,
                            50,
                            Ship.ShipMods[0].ShipModData.Cooldown,
                            Ship.ShipMods[0].ShipModData.Name + " Ship Mod Refresh UI",
                            true);
                // Add it first so that we get the proper screen position for the cooldown bar in the load content method of the RefreshUI
                // ABSOLUTELY MUST LEAVE THE LOAD CONTENT SEPARATELY
                LoadAndAddUIElement(shipModRefreshUI);
                shipModRefreshUI.LoadContent(content);
                if (Ship.ShipMods[0].ShipModData.Active)
                {
                    AddKeyText(Ship.ShipMods[0].ActivationKey.ToString(), shipModRefreshUI);
                }
                ShipModRefreshUI.Add(shipModRefreshUI);

                for (int i = 1; i < Ship.ShipMods.Count; i++)
                {
                    shipModRefreshUI = new RefreshUI(
                        Ship.ShipMods[i].ShipModData.TextureAsset,
                        new Vector2(55, 0),
                        50,
                        50,
                        Ship.ShipMods[i].ShipModData.Cooldown,
                        Ship.ShipMods[i].ShipModData.Name + " Ship Mod Refresh UI",
                        true);
                    // Add it first so that we get the proper screen position for the cooldown bar in the load content method of the RefreshUI
                    // ABSOLUTELY MUST LEAVE THE LOAD CONTENT SEPARATELY
                    AddUIElementRelativeTo(shipModRefreshUI, ShipModRefreshUI[i - 1]);
                    shipModRefreshUI.LoadContent(content);
                    if (Ship.ShipMods[i].ShipModData.Active)
                    {
                        AddKeyText(Ship.ShipMods[i].ActivationKey.ToString(), shipModRefreshUI);
                    }
                    ShipModRefreshUI.Add(shipModRefreshUI);
                }
            }
        }

        private void AddKeyText(string key, RefreshUI refreshUI)
        {
            Text text = new Text(
                key,
                new Vector2(0, -refreshUI.Dimensions.Y + 10),
                Color.White,
                "Ship Mod Key");
            LoadAndAddUIElementRelativeTo(text, refreshUI);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            for (int i = 0; i < ShipModRefreshUI.Count; i++)
            {
                // Mod is running so don't show it on cooldown - maybe have something else here instead
                if (Ship.ShipMods[i].TimeSinceActivation < Ship.ShipMods[i].ShipModData.RunTime)
                {
                    ShipModRefreshUI[i].Update(gameTime, 0);
                }
                else
                {
                    // Mod is on cooldown so show loading bar
                    ShipModRefreshUI[i].Update(gameTime, Ship.ShipMods[i].ShipModData.Cooldown + Ship.ShipMods[i].ShipModData.RunTime - Ship.ShipMods[i].TimeSinceActivation);
                }
            }
        }
    }
}
