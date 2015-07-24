using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToGalaxyGameLibrary.Screens_and_ScreenManager
{
    public class InGameMouse
    {
        public static Vector2 ScreenPosition
        {
            get;
            private set;
        }

        public static Vector2 InGamePosition
        {
            get;
            private set;
        }

        private Texture2D Texture
        {
            get;
            set;
        }

        #region Mouse Click Information

        public Vector2 LastLeftClickedPosition
        {
            get;
            private set;
        }

        public Vector2 LastMiddleClickedPosition
        {
            get;
            private set;
        }

        public Vector2 LastRightClickedPosition
        {
            get;
            private set;
        }

        public bool IsLeftClicked
        {
            get;
            private set;
        }

        public bool IsMiddleClicked
        {
            get;
            private set;
        }

        public bool IsRightClicked
        {
            get;
            private set;
        }

        #endregion

        private ScreenManager ScreenManager
        {
            get;
            set;
        }

        public MouseState PreviousMouseState
        {
            get;
            private set;
        }

        public MouseState CurrentMouseState
        {
            get;
            private set;
        }

        public bool Enabled
        {
            get;
            private set;
        }

        public InGameMouse(ScreenManager screenManager)
        {
            ScreenManager = screenManager;
            IsLeftClicked = false;
            IsMiddleClicked = false;
            IsRightClicked = false;
            PreviousMouseState = Mouse.GetState();
            CurrentMouseState = Mouse.GetState();

            Enable();
        }

        public void LoadContent(ContentManager content)
        {
            Texture = content.Load<Texture2D>("Sprites/UI/Mouse/cursor_blue");
        }

        public void Update(GameTime gameTime)
        {
            if (Enabled)
            {
                PreviousMouseState = CurrentMouseState;
                CurrentMouseState = Mouse.GetState();

                ScreenPosition = new Vector2(CurrentMouseState.X, CurrentMouseState.Y);
                // Need to translate into game coordinates here using the Camera Position.
                InGamePosition = ScreenPosition - ScreenManager.Camera.Position;

                if (CurrentMouseState.LeftButton == ButtonState.Pressed)
                {
                    IsLeftClicked = true;
                    LastLeftClickedPosition = InGamePosition;
                }
                else
                {
                    IsLeftClicked = false;
                }

                if (CurrentMouseState.MiddleButton == ButtonState.Pressed)
                {
                    IsMiddleClicked = true;
                    LastMiddleClickedPosition = InGamePosition;
                }
                else
                {
                    IsMiddleClicked = false;
                }

                if (CurrentMouseState.RightButton == ButtonState.Pressed)
                {
                    // Since this will be directing the player ship to areas in game space, we need this to be in game space too.
                    IsRightClicked = true;
                    LastRightClickedPosition = InGamePosition;
                }
                else
                {
                    IsRightClicked = false;
                }

                if (ScreenManager.Camera.CameraType == CameraType.Manual)
                {
                    UpdateCamera(gameTime);
                }
            }
        }

        // A function to change the position of the MANUAL camera if the mouse is towards the edge of the screen.
        private void UpdateCamera(GameTime gameTime)
        {
            if (Texture != null)
            {
                int threshold = 5;
                Vector2 deltaPos = Vector2.Zero;

                // Mouse is within thresold of the right hand side of the screen so we need to move the camera left
                if (ScreenManager.Viewport.Width - ScreenPosition.X < threshold)
                {
                    deltaPos.X -= ScreenManager.Camera.ScrollDelta;
                }

                // Mouse is within threshold of the left hand side of the screen so we need to move the camera right
                if (ScreenPosition.X - ScreenManager.Viewport.X < threshold)
                {
                    deltaPos.X += ScreenManager.Camera.ScrollDelta;
                }

                // Mouse is within threshold of the top of the screen so we need to move the camera down
                if (ScreenPosition.Y - ScreenManager.Viewport.Y < threshold)
                {
                    deltaPos.Y += ScreenManager.Camera.ScrollDelta;
                }

                // Mouse is within threshold of the bottom of the screen so we need to move the camera up
                if (ScreenManager.Viewport.Height - ScreenPosition.Y < threshold + Texture.Height / 2)
                {
                    deltaPos.Y -= ScreenManager.Camera.ScrollDelta;
                }

                ScreenManager.Camera.TranslateCamera(deltaPos);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (Enabled && Texture != null)
            {
                spriteBatch.Draw(Texture, ScreenPosition, Color.White);
            }
        }

        public void Enable()
        {
            Enabled = true;
        }

        public void Disable()
        {
            Enabled = false;
        }
    }
}
