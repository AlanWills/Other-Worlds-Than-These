using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToGalaxyGameLibrary.Screens_and_ScreenManager;

namespace ToGalaxyGameLibrary.UI
{
    public class Panel : UIElement
    {
        private List<UIElement> UIElementsToAdd
        {
            get;
            set;
        }

        private List<UIElement> UIElementsToRemove
        {
            get;
            set;
        }

        protected List<UIElement> ActiveUIElements
        {
            get;
            set;
        }

        public Vector2 Dimensions
        {
            get;
            private set;
        }

        public Vector2 paddingVector = new Vector2(0, 0);

        public Panel(string dataAsset, Vector2 position, Color colour, string name, float opacity = 0.25f)
            : base(dataAsset, position, name)
        {
            UIElementsToAdd = new List<UIElement>();
            ActiveUIElements = new List<UIElement>();
            UIElementsToRemove = new List<UIElement>();

            Colour = colour;
            Opacity = opacity;
        }

        public Panel(string dataAsset, Vector2 position, Vector2 dimensions, Color colour, string name, float opacity = 0.25f)
            : base(dataAsset, position, name)
        {
            UIElementsToAdd = new List<UIElement>();
            ActiveUIElements = new List<UIElement>();
            UIElementsToRemove = new List<UIElement>();

            Dimensions = dimensions;
            Colour = colour;
            Opacity = opacity;
        }

        public override void LoadContent(ContentManager content)
        {
            base.LoadContent(content);

            if (Texture != null)
            {
                if (Dimensions == Vector2.Zero)
                {
                    Dimensions = new Vector2(Texture.Width, Texture.Height);
                }

                Scale = new Vector2(Dimensions.X / Texture.Width, Dimensions.Y / Texture.Height);
            }

            foreach (UIElement uielement in UIElementsToAdd)
            {
                uielement.LoadContent(content);
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
    
            foreach (UIElement uielement in UIElementsToAdd)
            {
                ActiveUIElements.Add(uielement);
            }

            UIElementsToAdd.Clear();

            foreach (UIElement uielement in ActiveUIElements)
            {
                uielement.Update(gameTime);

                if (uielement.State == UIState.Dead)
                {
                    UIElementsToRemove.Add(uielement);
                }
            }

            foreach (UIElement uielement in UIElementsToRemove)
            {
                ActiveUIElements.Remove(uielement);
            }

            UIElementsToRemove.Clear();
        }

        public override void CheckClicked(InGameMouse mouse)
        {
            base.CheckClicked(mouse);

            foreach (UIElement uielement in ActiveUIElements)
            {
                uielement.CheckClicked(mouse);

                /*if (uielement as Button != null)
                {
                    uielement.CheckClicked(mouse);
                }
                else if (uielement as Image != null)
                {
                    uielement.CheckClicked(mouse);
                }
                else
                {
                    uielement.CheckForInteraction(InGameMouse.ScreenPosition);
                }*/
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            if (IsActive())
            {
                foreach (UIElement uielement in ActiveUIElements)
                {
                    uielement.Draw(spriteBatch);
                }
            }
        }

        #region Loading and Adding UIElements

        public void LoadAndAddUIElement(UIElement uielement)
        {
            uielement.LoadContent(ScreenManager.Content);
            uielement.SetPosition(uielement.Position + Position - paddingVector);
            UIElementsToAdd.Add(uielement);

            // UpdateDimensions(uielement);
        }

        public void AddUIElement(UIElement uielement)
        {
            uielement.SetPosition(uielement.Position + Position - paddingVector);
            UIElementsToAdd.Add(uielement);

            // UpdateDimensions(uielement);
        }

        public void LoadAndAddUIElement(UIElement uielement, Vector2 positionRelativeToPanelCentre)
        {
            uielement.LoadContent(ScreenManager.Content);
            uielement.SetPosition(positionRelativeToPanelCentre + Position - paddingVector);
            UIElementsToAdd.Add(uielement);

            // UpdateDimensions(uielement);
        }

        public void AddUIElement(UIElement uielement, Vector2 positionRelativeToPanelCentre)
        {
            uielement.SetPosition(positionRelativeToPanelCentre + Position - paddingVector);
            UIElementsToAdd.Add(uielement);

            // UpdateDimensions(uielement);
        }

        public void LoadAndAddUIElementRelativeTo(UIElement uielementToAdd, UIElement uielementRelativeTo)
        {
            uielementToAdd.LoadContent(ScreenManager.Content);
            uielementToAdd.SetPosition(uielementRelativeTo.Position + uielementToAdd.Position);
            UIElementsToAdd.Add(uielementToAdd);

            // UpdateDimensions(uielementToAdd);
        }

        public void AddUIElementRelativeTo(UIElement uielementToAdd, UIElement uielementRelativeTo)
        {
            uielementToAdd.SetPosition(uielementRelativeTo.Position + uielementToAdd.Position);
            UIElementsToAdd.Add(uielementToAdd);

            // UpdateDimensions(uielementToAdd);
        }

        public void LoadAndAddUIElementRelativeTo(UIElement uielementToAdd, string uielementRelativeToName)
        {
            UIElement elementRelativeTo = ActiveUIElements.Find(x => x.Name == uielementRelativeToName);

            if (elementRelativeTo != null)
            {
                LoadAndAddUIElementRelativeTo(uielementToAdd, elementRelativeTo, uielementToAdd.Position);
            }
        }

        public void AddUIElementRelativeTo(UIElement uielementToAdd, string uielementRelativeToName)
        {
            UIElement elementRelativeTo = ActiveUIElements.Find(x => x.Name == uielementRelativeToName);

            if (elementRelativeTo != null)
            {
                AddUIElementRelativeTo(uielementToAdd, elementRelativeTo, uielementToAdd.Position);
            }
        }

        public void LoadAndAddUIElementRelativeTo(UIElement uielementToAdd, UIElement uielementRelativeTo, Vector2 offset)
        {
            uielementToAdd.LoadContent(ScreenManager.Content);
            uielementToAdd.SetPosition(uielementRelativeTo.Position + offset);
            UIElementsToAdd.Add(uielementToAdd);

            // UpdateDimensions(uielementToAdd);
        }

        public void AddUIElementRelativeTo(UIElement uielementToAdd, UIElement uielementRelativeTo, Vector2 offset)
        {
            uielementToAdd.SetPosition(uielementRelativeTo.Position + offset);
            UIElementsToAdd.Add(uielementToAdd);

            // UpdateDimensions(uielementToAdd);
        }

        public void LoadAndAddUIElementRelativeTo(UIElement uielementToAdd, string uielementRelativeToName, Vector2 offset)
        {
            UIElement elementRelativeTo = ActiveUIElements.Find(x => x.Name == uielementRelativeToName);

            if (elementRelativeTo != null)
            {
                LoadAndAddUIElementRelativeTo(uielementToAdd, elementRelativeTo, offset);
            }
        }

        public void AddUIElementRelativeTo(UIElement uielementToAdd, string uielementRelativeToName, Vector2 offset)
        {
            UIElement elementRelativeTo = ActiveUIElements.Find(x => x.Name == uielementRelativeToName);

            if (elementRelativeTo != null)
            {
                AddUIElementRelativeTo(uielementToAdd, elementRelativeTo, offset);
            }
        }

        #endregion

        public override void Activate()
        {
            base.Activate();   

            foreach (UIElement uielement in ActiveUIElements)
            {
                uielement.Activate();
            }
        }

        public override void DisableAndHide()
        {
            base.DisableAndHide();

            foreach (UIElement uielement in UIElementsToAdd)
            {
                uielement.DisableAndHide();
            }

            foreach (UIElement uielement in ActiveUIElements)
            {
                uielement.DisableAndHide();
            }
        }


        public UIElement GetScreenUIElement(string name)
        {
            UIElement element = ActiveUIElements.Find(x => x.Name == name);

            if (element == null)
            {
                element = UIElementsToAdd.Find(x => x.Name == name);
            }

            return element;
        }

        // Remove a Screen UIElement referenced by a name rather than the actual object itself
        public bool RemoveScreenUIElement(string name)
        {
            UIElement ui = ActiveUIElements.Find(x => x.Name == name);

            if (ui == null)
            {
                ui = UIElementsToAdd.Find(x => x.Name == name);
            }

            if (ui != null)
            {
                UIElementsToRemove.Add(ui);
                return true;
            }

            return false;
        }

        public void LoadAndAddNewTextEntryBelowPrevious(Text text)
        {
            text.LoadContent(ScreenManager.Content);

            Text previousText = null;
            foreach (UIElement uielement in ActiveUIElements)
            {
                if (uielement as Text != null)
                {
                    if (previousText != null)
                    {
                        if (uielement.Position.Y > previousText.Position.Y)
                        {
                            previousText = (Text)uielement;
                        }
                    }
                    else
                    {
                        previousText = (Text)uielement;
                    }
                }
            }

            foreach (UIElement uielement in UIElementsToAdd)
            {
                if (uielement as Text != null)
                {
                    if (previousText != null)
                    {
                        if (uielement.Position.Y > previousText.Position.Y)
                        {
                            previousText = (Text)uielement;
                        }
                    }
                    else
                    {
                        previousText = (Text)uielement;
                    }
                }
            }

            if (previousText != null)
            {
                text.SetPosition(new Vector2(0, 4 * previousText.TextOrigin.Y));
                AddUIElementRelativeTo(text, previousText);
            }
            else
            {
                AddUIElement(text);
            }
        }

        public void Clear()
        {
            ActiveUIElements.Clear();
        }

        // Experimental Function - still needs work
        private void UpdateDimensions(UIElement uielement)
        {
            float increaseWidth = 0, increaseHeight = 0;

            // The left hand side of the object - padding lies to the left of the bounds so we need to increase the bounds
            if (Bounds.Left > uielement.Bounds.Left)
            {
                // Increase the width by twice this much - add on the same amount to both sides
                increaseWidth = 2 * (Bounds.Left - uielement.Bounds.Left);
            }
            // Top of the uielement has gone over the bounds of the panel
            if (Bounds.Top > uielement.Bounds.Top)
            {
                increaseHeight = 2 * (Bounds.Top - uielement.Bounds.Top);
            }
            // Right of the uielement has gone over the right of the panel
            if (Bounds.Right < uielement.Bounds.Right)
            {
                increaseWidth = (float)Math.Max(increaseWidth, 2 * (uielement.Bounds.Right - Bounds.Right));
            }
            if (Bounds.Bottom < uielement.Bounds.Bottom)
            {
                increaseHeight = (float)Math.Max(increaseHeight, 2 * (uielement.Bounds.Bottom - Bounds.Bottom));
            }

            Dimensions += new Vector2(increaseWidth, increaseHeight);
            Scale = new Vector2(Dimensions.X / Texture.Width, Dimensions.Y / Texture.Height);

            if (ActiveUIElements.Count == 1)
            {
                SetPosition(Position - paddingVector);
            }
            else
            {
                SetPosition(Position + new Vector2(0, paddingVector.Y) + new Vector2(0, uielement.Bounds.Height / 2));
            }
        }
    }
}
