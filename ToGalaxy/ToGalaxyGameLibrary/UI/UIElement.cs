using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToGalaxyCustomData;
using ToGalaxyGameLibrary.Screens_and_ScreenManager;

namespace ToGalaxyGameLibrary.UI
{
    // Active - shown and can be updated
    // Hidden - not shown or updated
    // Dead - no longer used and should be removed
    public enum UIState
    {
        Active,
        Hidden,
        Dead
    }

    public class UIElement
    {
        #region Data

        // Name is used to remove the UIElement from a Screen's list when it is no longer needed
        // Give the element a name, like "PlayerMoveMarker" and then when you want it to be removed,
        // search through the UIElement list to find it and add it to UIElements to remove.
        public string Name
        {
            get;
            private set;
        }

        // Data Asset may also just be a texture string if we require no other information
        protected string DataAsset
        {
            get;
            set;
        }

        public UIElementData Data
        {
            get;
            protected set;
        }

        #endregion

        #region Spatial Properties

        public Vector2 Position
        {
            get;
            protected set;
        }

        // This is defined as the centre of the full scale texture
        // DO NOT CHANGE
        protected Vector2 Origin
        {
            get
            {
                if (Texture != null)
                {
                    return new Vector2(Texture.Width, Texture.Height) / 2;
                }
                else
                {
                    return Vector2.Zero;
                }
            }
        }

        public Rectangle DestinationRectangle
        {
            get;
            private set;
        }

        public Rectangle Bounds
        {
            get
            {
                return new Rectangle((int)(Position.X - Origin.X * Scale.X), (int)(Position.Y - Origin.Y * Scale.Y), (int)(2 * Origin.X * Scale.X), (int)(2 * Origin.Y * Scale.Y));
            }
        }

        public Vector2 Scale
        {
            get;
            protected set;
        }

        public float Rotation
        {
            get;
            private set;
        }

        #endregion

        #region Visual Properties

        public Texture2D Texture
        {
            get;
            protected set;
        }

        public Color Colour
        {
            get;
            protected set;
        }

        public float Opacity
        {
            get;
            protected set;
        }

        #endregion

        public UIState State
        {
            get;
            protected set;
        }

        public Panel HoverInfo
        {
            get;
            protected set;
        }

        public EventHandler InteractEvent;
        public EventHandler EnableAndDisableEvent;
        public float clickDelay = 0;

        public UIElement(string dataAsset, Vector2 position, string name, float rotation = 0)
        {
            DataAsset = dataAsset;
            Position = position;
            Name = name;
            State = UIState.Active;
            Opacity = 1f;
            Scale = new Vector2(1, 1);
            Rotation = rotation;
            Colour = Color.White;
        }

        public UIElement(string dataAsset, Vector2 position, Vector2 scale, string name, float rotation = 0)
        {
            DataAsset = dataAsset;
            Position = position;
            Name = name;
            Scale = scale;
            Rotation = rotation;
            State = UIState.Active;
            Opacity = 1f;
            Colour = Color.White;
        }

        public UIElement(string dataAsset, Vector2 position, Vector2 scale, Color colour, string name, float rotation = 0)
        {
            DataAsset = dataAsset;
            Position = position;
            Name = name;
            Scale = scale;
            Rotation = rotation;
            State = UIState.Active;
            Opacity = 1f;
            Colour = colour;
        }

        public virtual void LoadContent(ContentManager content)
        {
            if (DataAsset != "")
            {
                try
                {
                    // Try to see if we have a string for just a texture
                    Texture = content.Load<Texture2D>(DataAsset);
                }
                catch
                {
                    // If not, we load the XML file instead
                    try
                    {
                        Data = content.Load<UIElementData>(DataAsset);

                        if (Data != null)
                        {
                            if (Data.TextureAsset != "")
                            {
                                Texture = content.Load<Texture2D>(Data.TextureAsset);
                            }
                        }
                    }
                    catch
                    {

                    }
                }
            }
        }

        // Use for UIElements you won't interact with
        public virtual void Update(GameTime gameTime)
        {
            clickDelay += (float)gameTime.ElapsedGameTime.Milliseconds / 1000f;

            if (EnableAndDisableEvent != null)
            {
                EnableAndDisableEvent(this, EventArgs.Empty);
            }

            if (HoverInfo != null)
            {
                HoverInfo.Update(gameTime);
            }
        }

        // Use for those that will be interacted with by the mouse
        public virtual void CheckClicked(InGameMouse mouse)
        {
            if (HoverInfo != null)
            {
                if (IsActive())
                {
                    if (Bounds.Contains(new Point((int)InGameMouse.ScreenPosition.X, (int)InGameMouse.ScreenPosition.Y)))
                    {
                        HoverInfo.Activate();
                    }
                    else
                    {
                        HoverInfo.DisableAndHide();
                    }
                }
            }

            if (mouse.IsLeftClicked && mouse.PreviousMouseState.LeftButton == ButtonState.Released && clickDelay > 0.3f)
            {
                CheckForInteraction(InGameMouse.ScreenPosition);
            }
        }

        public virtual void CheckForInteraction(Vector2 clickedPosition)
        {
            if (IsActive())
            {
                // This is possibly wrong.  Think we need to scale the bounds rectangle using Scale.
                if (Bounds.Contains(new Point((int)clickedPosition.X, (int)clickedPosition.Y)))
                {
                    OnInteract(clickedPosition);
                }
            }
        }

        public bool IsActive()
        {
            return State == UIState.Active;
        }

        public virtual void Activate()
        {
            State = UIState.Active;
        }

        public virtual void DisableAndHide()
        {
            State = UIState.Hidden;
        }

        public void Die()
        {
            State = UIState.Dead;
            if (HoverInfo != null)
            {
                HoverInfo.Die();
            }
        }

        protected virtual void OnInteract(Vector2 clickedPosition)
        {
            clickDelay = 0;

            if (InteractEvent != null)
            {
                InteractEvent(this, EventArgs.Empty);
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (IsActive())
            {
                if (Texture != null)
                {
                    spriteBatch.Draw(Texture, Position, null, Colour * Opacity, Rotation, Origin, Scale, SpriteEffects.None, 0);

                    if (HoverInfo != null)
                    {
                        HoverInfo.Draw(spriteBatch);
                    }
                }
            }
        }

        #region Public Setter Functions for Position, Rotation and Opacity

        public void SetPosition(Vector2 position)
        {
            Position = position;
        }

        public void SetRotation(float rotation)
        {
            Rotation = rotation;
            Rotation %= MathHelper.TwoPi;
        }

        public void SetOpacity(float opacity)
        {
            Opacity = opacity;
        }

        public void SetScale(Vector2 scale)
        {
            Scale = scale;
        }

        public void SetColour(Color colour)
        {
            Colour = colour;
        }

        public void SetHoverInfoText(string text)
        {
            Text infoText = new Text(
                text,
                Vector2.Zero,
                100,
                Color.White,
                "Hover Info Text");
            infoText.LoadContent(ScreenManager.Content);

            HoverInfo = new Panel(
                "Sprites/UI/Panels/Panel",
                Position,
                2 * infoText.TextOrigin,
                Color.Black,
                "Hover Info",
                0.5f);
            HoverInfo.LoadContent(ScreenManager.Content);
            HoverInfo.AddUIElement(infoText, infoText.Position);
        }

        public void SetHoverInfoText(string text, Vector2 position)
        {
            Text infoText = new Text(
                text,
                Vector2.Zero,
                100,
                Color.White,
                "Hover Info Text");
            infoText.LoadContent(ScreenManager.Content);

            HoverInfo = new Panel(
                "Sprites/UI/Panels/Panel",
                position,
                2 * infoText.TextOrigin,
                Color.Black,
                "Hover Info",
                0.5f);
            HoverInfo.LoadContent(ScreenManager.Content);
            HoverInfo.AddUIElement(infoText, infoText.Position);
        }

        #endregion
    }
}
