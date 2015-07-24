using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToGalaxyGameLibrary.UI
{
    public class RefreshUI : Image
    {
        private Bar CooldownBar
        {
            get;
            set;
        }

        private float StoredValue
        {
            get;
            set;
        }

        public RefreshUI(string dataAsset, Vector2 position, float width, float height, float storedValue, string name, bool preserveAspectRatio = true, float rotation = 0)
            : base(dataAsset, position, width, height, name, preserveAspectRatio, rotation)
        {
            StoredValue = storedValue;
        }

        public override void LoadContent(ContentManager content)
        {
            base.LoadContent(content);

            // Want the bar to go down and up rather than side to side.
            CooldownBar = new Bar(
                StoredValue,
                "Sprites/UI/Panels/Panel",
                Position,
                new Vector2(Bounds.Height, Bounds.Width),
                Color.White,
                "Cooldown Bar",
                3 * MathHelper.PiOver2);
            CooldownBar.LoadContent(content);
            CooldownBar.SetOpacity(0);
        }

        public void Update(GameTime gameTime, float currentValue)
        {
            base.Update(gameTime);

            CooldownBar.Update(gameTime, currentValue);

            if (CooldownBar.PercentageFilled > 0)
            {
                CooldownBar.SetOpacity(0.2f);
            }
            else
            {
                CooldownBar.SetOpacity(0);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            CooldownBar.Draw(spriteBatch);
        }
    }
}
