using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToGalaxyCustomData;
using ToGalaxyGameLibrary.Audio;
using ToGalaxyGameLibrary.Screens_and_ScreenManager;

namespace ToGalaxyGameLibrary.UI
{
    public class Button : UIElement
    {
        #region Data

        private ButtonData ButtonData
        {
            get;
            set;
        }

        #endregion

        #region Textures for Default, Highlighted and Pressed

        private Texture2D DefaultTexture
        {
            get;
            set;
        }

        private Texture2D HighlightedTexture
        {
            get;
            set;
        }

        private Texture2D PressedTexture
        {
            get;
            set;
        }

        #endregion

        #region Colours of Button - Default, Highlighted and Current

        private Color DefaultColour
        {
            get;
            set;
        }

        private Color HighlightedColour
        {
            get;
            set;
        }

        private Color CurrentColour
        {
            get;
            set;
        }

        #endregion

        private Text Text
        {
            get;
            set;
        }

        private float TextureResetTime
        {
            get;
            set;
        }

        private float ColourResetTime
        {
            get;
            set;
        }

        // Used to track to see when the mouse first entered so that we play the highlighted sound once per entry by the mouse
        private bool CanPlayHighlightedSound
        {
            get;
            set;
        }

        // Default colours which tend to be used a lot
        public static Color defaultColour = new Color(0, 0.318f, 0.49f);
        public static Color highlightedColour = new Color(0, 0.71f, 0.988f);

        float currentTimeDown = 0;
        float timeSinceHighlighted = 1;

        public Button(string dataAsset, Vector2 position, Color defaultColour, Color highlightedColour, string name, string text = "")
            : base(dataAsset, position, name)
        {
            TextureResetTime = 0.1f;
            ColourResetTime = 0.3f;
            Text = new Text(text, this, new Vector2(0, 0), Color.White, text + " Text");
            DefaultColour = defaultColour;
            HighlightedColour = highlightedColour;
            CurrentColour = DefaultColour;
        }

        public override void LoadContent(ContentManager content)
        {
            base.LoadContent(content);

            if (DataAsset != "")
            {
                ButtonData = content.Load<ButtonData>(DataAsset);

                if (ButtonData != null)
                {
                    DefaultTexture = Texture;

                    if (ButtonData.HighlightedTextureAsset != "")
                    {
                        HighlightedTexture = content.Load<Texture2D>(ButtonData.HighlightedTextureAsset);
                    }
                    if (ButtonData.PressedTextureAsset != "")
                    {
                        PressedTexture = content.Load<Texture2D>(ButtonData.PressedTextureAsset);
                    }
                }
            }

            Text.LoadContent(content);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            currentTimeDown += (float)gameTime.ElapsedGameTime.Milliseconds / 1000;
            timeSinceHighlighted += (float)gameTime.ElapsedGameTime.Milliseconds / 1000;

            if (currentTimeDown >= TextureResetTime && Texture == PressedTexture)
            {
                Texture = DefaultTexture;
            }

            if (timeSinceHighlighted >= ColourResetTime)
            {
                timeSinceHighlighted = ColourResetTime;
                CurrentColour = DefaultColour;
            }

            if (Text != null)
            {
                Text.Update(gameTime);
            }
        }

        public override void CheckClicked(InGameMouse mouse)
        {
            // Mouse is over button
            if (Bounds.Contains((int)InGameMouse.ScreenPosition.X, (int)InGameMouse.ScreenPosition.Y))
            {
                if (HighlightedTexture != null)
                {
                    Texture = HighlightedTexture;
                }

                if (CanPlayHighlightedSound)
                {
                    ScreenManager.SoundEffects.Play(ButtonData.HighlightedSoundAsset);
                    CanPlayHighlightedSound = false;
                }

                CurrentColour = HighlightedColour;
                timeSinceHighlighted = 0;
            }
            else if (timeSinceHighlighted < ColourResetTime)
            {
                CanPlayHighlightedSound = true;
                CurrentColour = Color.Lerp(HighlightedColour, DefaultColour, timeSinceHighlighted / ColourResetTime);
            }
            else
            {
                CanPlayHighlightedSound = true;
            }

            base.CheckClicked(mouse);
        }

        protected override void OnInteract(Vector2 clickedPosition)
        {
            ScreenManager.SoundEffects.Play(ButtonData.PressedSoundAsset);

            if (PressedTexture != null)
            {
                Texture = PressedTexture;
            }

            currentTimeDown = 0;
            base.OnInteract(clickedPosition);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (IsActive())
            {
                if (Texture != null)
                {
                    spriteBatch.Draw(Texture, Position, null, CurrentColour * Opacity, Rotation, Origin, Scale, SpriteEffects.None, 0);
                }

                Text.Draw(spriteBatch);
            }
        }

        public void ChangeText(string newText)
        {
            Text.ChangeText(newText);
        }
    }
}
