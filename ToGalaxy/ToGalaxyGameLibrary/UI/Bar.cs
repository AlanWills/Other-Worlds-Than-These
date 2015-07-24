using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToGalaxyGameLibrary.UI
{
    public class Bar : Image
    {
        // This is going to represent the full value (i.e. bar at 100%) and with which we can create ratios 
        public float StoredValue
        {
            get;
            set;
        }

        public float PercentageFilled
        {
            get;
            protected set;
        }

        // Rather than rescaling the image we will only draw part of it based on the stored value
        protected Rectangle SourceRectangle
        {
            get;
            set;
        }

        // Here scale is used to work out whether we need to stretch the bar if the texture width is greater than 180, otherwise
        // we just use part of it.
        public Bar(float storedValue, string dataAsset, Vector2 position, Vector2 scale, string name, float rotation = 0)
            : base(dataAsset, position, scale, name, rotation)
        {
            StoredValue = storedValue;
        }

        public Bar(float storedValue, string dataAsset, Vector2 position, Vector2 scale, Color colour, string name, float rotation = 0)
            : base(dataAsset, position, scale, colour, name, rotation)
        {
            StoredValue = storedValue;
        }

        public override void LoadContent(ContentManager content)
        {
            base.LoadContent(content);

            SourceRectangle = new Rectangle(0, 0, (int)Texture.Width, (int)Texture.Height);
        }

        // Use this to only update the bar's width
        public void Update(GameTime gameTime, float currentValue)
        {
            base.Update(gameTime);

            // Update the size of the bar
            PercentageFilled = (float)currentValue / StoredValue;
            SourceRectangle = new Rectangle(0, (int)(Texture.Height * Scale.Y / 2), (int)(Texture.Width * PercentageFilled * Scale.X), (int)(Texture.Height * Scale.Y));
        }

        // Use this to update the bar's width and it's position for moving bars
        public void Update(GameTime gameTime, float currentValue, Vector2 position)
        {
            base.Update(gameTime);

            Update(gameTime, currentValue);

            // Also update position
            SetPosition(position + new Vector2(Texture.Width * Scale.X / 2, Texture.Height * Scale.Y / 2));
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (IsActive())
            {
                if (Texture != null)
                {
                    spriteBatch.Draw(Texture, Position, SourceRectangle, Colour * Opacity, Rotation, new Vector2(Texture.Width * Scale.X / 2, Texture.Height * Scale.Y / 2), new Vector2(1, 1), SpriteEffects.None, 0);
                }
            }
        }
    }
}
