using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToGalaxyCustomData;
using ToGalaxyGameLibrary.Screens_and_ScreenManager;

namespace ToGalaxyGameLibrary.UI
{
    public class Slider : Bar
    {
        private Texture2D ForegroundTexture
        {
            get;
            set;
        }

        private Vector2 ForegroundPosition
        {
            get;
            set;
        }

        public Button FullButton
        {
            get;
            private set;
        }

        public Button OffButton
        {
            get;
            private set;
        }

        public Slider(float storedValue, string dataAsset, Vector2 position, Vector2 scale, string name, float rotation = 0)
            : base(storedValue, dataAsset, position, scale, name, rotation)
        {
            ForegroundPosition = position;
            PercentageFilled = (int)(storedValue * 100);
        }

        private void SetUpButtons(ContentManager content)
        {
            OffButton = new Button(
                "XML/UI/Buttons/MenuButton",
                new Vector2(Bounds.Left - Bounds.Width / 4, Position.Y),
                Button.defaultColour,
                Button.highlightedColour,
                "Off Button",
                "Off");
            OffButton.InteractEvent += OffButtonEvent;
            OffButton.LoadContent(content);

            FullButton = new Button(
                "XML/UI/Buttons/MenuButton",
                new Vector2(Bounds.Right + Bounds.Width / 4, Position.Y),
                Button.defaultColour,
                Button.highlightedColour,
                "Full Button",
                "100%");
            FullButton.InteractEvent += FullButtonEvent;
            FullButton.LoadContent(content);
        }

        private void OffButtonEvent(object sender, EventArgs e)
        {
            PercentageFilled = 0;
            OnInteract(new Vector2(Bounds.Left, Position.Y));
        }

        private void FullButtonEvent(object sender, EventArgs e)
        {
            PercentageFilled = 100;
            OnInteract(new Vector2(Bounds.Right, Position.Y));
        }

        public override void LoadContent(ContentManager content)
        {
            base.LoadContent(content);

            if (DataAsset != "")
            {
                try
                {
                    float ratio = (float)PercentageFilled / 100f;

                    // Try to see if we have a string for just a texture
                    ForegroundTexture = content.Load<Texture2D>(DataAsset + "Foreground");
                    SourceRectangle = new Rectangle(0, 0, (int)(Texture.Width * ratio * Scale.X), (int)(Texture.Height * Scale.Y));
                    ForegroundPosition = new Vector2(Bounds.Left + ratio * Texture.Width - Texture.Width / 2 * ratio * Scale.X, Position.Y);
                }
                catch
                {
                    // If not, we load the XML file instead
                    Data = content.Load<UIElementData>(DataAsset);

                    if (Data != null)
                    {
                        if (Data.TextureAsset != "")
                        {
                            // BackgroundTexture = content.Load<Texture2D>(Data.BackgroundTextureAsset);
                        }
                    }
                }
            }

            SetUpButtons(content);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            FullButton.Update(gameTime);
            OffButton.Update(gameTime);
        }

        public override void CheckForInteraction(Vector2 clickedPosition)
        {
            base.CheckForInteraction(clickedPosition);

            FullButton.CheckForInteraction(clickedPosition);
            OffButton.CheckForInteraction(clickedPosition);
        }

        public override void CheckClicked(InGameMouse mouse)
        {
            base.CheckClicked(mouse);

            FullButton.CheckClicked(mouse);
            OffButton.CheckClicked(mouse);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (IsActive())
            {
                if (Texture != null)
                {
                    spriteBatch.Draw(Texture, Position, null, Colour * Opacity, Rotation, new Vector2(Bounds.Width / 2, Bounds.Height / 2), Scale, SpriteEffects.None, 0);
                }

                if (Texture != null)
                {
                    spriteBatch.Draw(ForegroundTexture, ForegroundPosition, SourceRectangle, Colour * Opacity, Rotation, new Vector2(SourceRectangle.Center.X, SourceRectangle.Center.Y), Scale, SpriteEffects.None, 0);
                }

                FullButton.Draw(spriteBatch);
                OffButton.Draw(spriteBatch);
            }
        }

        protected override void OnInteract(Vector2 clickedPosition)
        {
            base.OnInteract(clickedPosition);

            // Calculate where the mouse was clicked in relation to the length of the bar as a whole
            float ratio = (float)(clickedPosition.X - Bounds.Left) / (float)Bounds.Width;

            PercentageFilled = (int)(ratio * 100);
            SourceRectangle = new Rectangle(0, 0, (int)(Texture.Width * ratio * Scale.X), (int)(Texture.Height * Scale.Y));
            ForegroundPosition = new Vector2(clickedPosition.X - Texture.Width / 2 * ratio * Scale.X, Position.Y);
        }

        public void RemoveButtons()
        {
            FullButton.DisableAndHide();
            OffButton.DisableAndHide();
        }
    }
}
