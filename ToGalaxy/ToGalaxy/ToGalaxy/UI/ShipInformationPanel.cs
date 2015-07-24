using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToGalaxy.Gameplay_Objects;
using ToGalaxyGameLibrary;
using ToGalaxyGameLibrary.UI;

namespace ToGalaxy.UI
{
    public class ShipInformationPanel : Panel
    {
        private Ship Ship
        {
            get;
            set;
        }

        private Image ShipImage
        {
            get;
            set;
        }

        private Text ShipName
        {
            get;
            set;
        }

        private Bar ShieldBar
        {
            get;
            set;
        }

        private Bar ArmourBar
        {
            get;
            set;
        }

        private Bar HullBar
        {
            get;
            set;
        }

        private Text ShieldStatusText
        {
            get;
            set;
        }

        public ShipInformationPanel(string dataAsset, Vector2 position, Vector2 dimensions, Color colour, string name, float opacity = 0.4f)
            : base(dataAsset, position, dimensions, colour, name, opacity)
        {
            
        }

        public void CreateUI()
        {
            ShipImage = new Image(
                "",
                new Vector2(-Dimensions.X / 4, 0),
                Dimensions.X / 2 - Dimensions.X / 10,
                Dimensions.Y - Dimensions.Y / 5,
                "Ship Image");
            LoadAndAddUIElement(ShipImage, ShipImage.Position);

            ShipName = new Text(
                "",
                this,
                new Vector2(Dimensions.X / 4, -Dimensions.Y / 3),
                Color.White,
                "Ship Name Text",
                "Fonts/SmallGameUIFont");
            LoadAndAddUIElement(ShipName, ShipName.Position);

            ShieldBar = new Bar(
                0,
                "Sprites/UI/Bars/ShieldBar",
                new Vector2(Dimensions.X / 4, -Dimensions.Y / 9),
                new Vector2(1),
                "Shield Bar");
            LoadAndAddUIElement(ShieldBar, ShieldBar.Position);

            ArmourBar = new Bar(
                0,
                "Sprites/UI/Bars/ArmourBar",
                new Vector2(Dimensions.X / 4, 0),
                new Vector2(1),
                "Armour Bar");
            LoadAndAddUIElement(ArmourBar, ArmourBar.Position);

            HullBar = new Bar(
                0,
                "Sprites/UI/Bars/HullBar",
                new Vector2(Dimensions.X / 4, Dimensions.Y / 9),
                new Vector2(1),
                "Hull Bar");
            LoadAndAddUIElement(HullBar, HullBar.Position);

            ShieldStatusText = new Text(
                "",
                this,
                new Vector2(Dimensions.X / 6, 3 * Dimensions.Y / 10),
                Color.Cyan,
                "Shield Status Text",
                "Fonts/SmallGameUIFont");
            LoadAndAddUIElement(ShieldStatusText, ShieldStatusText.Position);
        }

        public override void LoadContent(ContentManager content)
        {
            CreateUI();

            base.LoadContent(content);
        }

        public void SetShip(Ship ship)
        {
            Ship = ship;
            UpdateUI();
        }

        private void UpdateUI()
        {
            if (Ship != null)
            {
                ShipImage.SetTexture(Ship.Texture);
                ShipName.ChangeText(Ship.ShipData.Name);

                if (Ship.Shield != null)
                {
                    ShieldBar.StoredValue = Ship.Shield.ShieldData.Strength;
                }
                else
                {
                    ShieldBar.DisableAndHide();
                }

                ArmourBar.StoredValue = Ship.ShipData.Armour;
                HullBar.StoredValue = Ship.ShipData.Hull;
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (Ship != null)
            {
                if (Ship.Shield != null)
                {
                    ShieldBar.Update(gameTime, Ship.Shield.ShieldStrength);
                    
                    if (Ship.Shield.ShieldState == ShieldState.Active)
                    {
                        ShieldStatusText.ChangeText("Shields At " + (int)(ShieldBar.PercentageFilled * 100) + "%");
                        ShieldStatusText.ChangeColour(Color.Cyan);
                    }
                    else if (Ship.Shield.ShieldState == ShieldState.Charging)
                    {
                        ShieldStatusText.ChangeText("Shields Charging " + (int)(ShieldBar.PercentageFilled * 100) + "%");
                        ShieldStatusText.ChangeColour(Color.Yellow);
                    }
                    else
                    {
                        ShieldStatusText.ChangeText("Shields Restored in " + (int)(Ship.Shield.ShieldData.DepletionDelay - Ship.Shield.regenTimer) + "s");
                        ShieldStatusText.ChangeColour(Color.Red);
                    }
                }
                else
                {
                    if (Ship.Armour > 0)
                    {
                        ShieldStatusText.ChangeText("Armour At " + (int)(Ship.Armour * 100 / Ship.ShipData.Armour) + "%");
                        ShieldStatusText.ChangeColour(Color.Yellow);
                    }
                    else
                    {
                        ShieldStatusText.ChangeText("Hull At " + (int)(Ship.Hull * 100 / Ship.ShipData.Hull) + "%");
                        ShieldStatusText.ChangeColour(Color.Red);
                    }
                }

                ArmourBar.Update(gameTime, Ship.Armour);
                HullBar.Update(gameTime, Ship.Hull);
                ShieldStatusText.Update(gameTime);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Ship != null)
            {
                base.Draw(spriteBatch);
            }
        }
    }
}
