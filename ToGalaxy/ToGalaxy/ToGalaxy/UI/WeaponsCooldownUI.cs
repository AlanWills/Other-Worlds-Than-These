using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToGalaxy.Gameplay_Objects;
using ToGalaxy.Gameplay_Objects.Space_Screen;
using ToGalaxyGameLibrary.UI;

namespace ToGalaxy.UI
{
    public class WeaponsCooldownUI : Panel
    {
        private PlayerShip Ship
        {
            get;
            set;
        }

        private List<RefreshUI> WeaponRefreshUI
        {
            get;
            set;
        }

        public WeaponsCooldownUI(string dataAsset, PlayerShip ship, Vector2 position, Vector2 dimensions, Color colour, string name, float opacity = 0.25f)
            : base(dataAsset, position, dimensions, colour, name, opacity)
        {
            Ship = ship;
            WeaponRefreshUI = new List<RefreshUI>();
        }

        public override void LoadContent(ContentManager content)
        {
            base.LoadContent(content);

            if (Ship.Turrets.Count > 0)
            {
                RefreshUI weaponRefreshUI = new RefreshUI(
                            Ship.Turrets[0].TurretData.TextureAsset,
                            new Vector2(5, 35 - Dimensions.Y / 2),
                            30,
                            50,
                            Ship.Turrets[0].TurretData.FireTimer,
                            Ship.Turrets[0].TurretData.Name + " Weapon Refresh UI",
                            true);
                // Add it first so that we get the proper screen position for the cooldown bar in the load content method of the RefreshUI
                LoadAndAddUIElement(weaponRefreshUI);
                weaponRefreshUI.LoadContent(content);
                WeaponRefreshUI.Add(weaponRefreshUI);

                for (int i = 1; i < Ship.Turrets.Count; i++)
                {
                    weaponRefreshUI = new RefreshUI(
                        Ship.Turrets[i].TurretData.TextureAsset,
                        new Vector2(0, 55),
                        30,
                        50,
                        Ship.Turrets[i].TurretData.FireTimer,
                        Ship.Turrets[i].TurretData.Name + " Weapon Refresh UI",
                        true);
                    // Add it first so that we get the proper screen position for the cooldown bar in the load content method of the RefreshUI
                    LoadAndAddUIElementRelativeTo(weaponRefreshUI, WeaponRefreshUI[i - 1]);
                    weaponRefreshUI.LoadContent(content);
                    WeaponRefreshUI.Add(weaponRefreshUI);
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            for (int i = 0; i < WeaponRefreshUI.Count; i++)
            {
                WeaponRefreshUI[i].Update(gameTime, Ship.Turrets[i].TurretData.FireTimer - Ship.Turrets[i].CurrentFireTimer);
            }
        }
    }
}
