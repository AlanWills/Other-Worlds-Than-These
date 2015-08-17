using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToGalaxyGameLibrary.Game_Objects;
using ToGalaxyGameLibrary.Screens_and_ScreenManager.Managers;
using ToGalaxyGameLibrary.UI;

namespace ToGalaxyGameLibrary.Screens_and_ScreenManager
{
    public enum CameraType
    {
        Focused,
        Manual,
        Fixed,
        MovingToPoint
    }

    public class Camera2D
    {
        public Matrix viewMatrix;

        public ScreenManager ScreenManager
        {
            get;
            private set;
        }

        private GameObject FocusedObject
        {
            get;
            set;
        }

        public Rectangle BackgroundRectangle
        {
            get;
            set;
        }

        public Vector2 Position
        {
            get;
            private set;
        }

        public Vector2 Destination
        {
            get;
            private set;
        }

        private float LerpDelta
        {
            get;
            set;
        }

        public Rectangle ViewRectangle
        {
            get;
            private set;
        }

        public float ScrollDelta
        {
            get;
            set;
        }

        public float Zoom
        {
            get;
            private set;
        }

        public CameraType CameraType
        {
            get;
            private set;
        }

        public static float BackgroundMultiplier = 10f;

        public Camera2D(ScreenManager screenManager)
        {
            ScreenManager = screenManager;
            Position = Vector2.Zero;
            ScrollDelta = 17f;
            LerpDelta = 3f;
            Zoom = 1f;
            ViewRectangle = new Rectangle(0, 0, ScreenManager.Viewport.Width, ScreenManager.Viewport.Height);
            BackgroundRectangle = new Rectangle(0, 0, ScreenManager.Viewport.Width, ScreenManager.Viewport.Height);
            CameraType = CameraType.Fixed;
        }

        #region Functions for Setting Camera Type

        // Used for space screen - default camera is manual movement using arrow keys/mouse
        public void SetManualCamera()
        {
            CameraType = CameraType.Manual;
        }

        // For ship interior we have a fixed camera so that we can use the XTiled Map manipulator instead
        // Also use this in menu screens too.
        public void SetFixedScreenCamera(bool reset = true)
        {
            CameraType = CameraType.Fixed;
            ClearFocusedObject();

            if (reset)
            {
                Position = Vector2.Zero;
                BackgroundRectangle = new Rectangle(0, 0, ScreenManager.Viewport.Width, ScreenManager.Viewport.Height);
            }
        }

        // Used for focusing and following a specific game object
        public void SetCameraToFocusOnObject(GameObject gameObject)
        {
            CameraType = CameraType.Focused;
            SetFocusedObject(gameObject);
        }

        // Set the object to focus
        private void SetFocusedObject(GameObject gameObject)
        {
            FocusedObject = gameObject;
        }

        // Clear the focus and move back to manual
        private void ClearFocusedObject()
        {
            FocusedObject = null;
        }

        #endregion

        public void Update()
        {
            if (CameraType == CameraType.Focused)
            {
                if (InputManager.IsKeyDown(Keys.PageUp))
                {
                    Zoom -= 0.01f;
                    if (Zoom < 0.75f)
                    {
                        Zoom = 0.75f;
                    }
                }
                if (InputManager.IsKeyDown(Keys.PageDown))
                {
                    Zoom += 0.01f;
                    if (Zoom > 1.5f)
                    {
                        Zoom = 1.5f;
                    }
                }

                if (FocusedObject != null && FocusedObject.Texture != null)
                {
                    UpdateFocusedCamera();
                }
                else
                {
                    UpdateManualCamera();
                }
            }
            else if (CameraType == CameraType.Manual)
            {
                if (InputManager.IsKeyDown(Keys.PageUp))
                {
                    Zoom -= 0.01f;
                    if (Zoom < 0.5f)
                    {
                        Zoom = 0.5f;
                    }
                }
                if (InputManager.IsKeyDown(Keys.PageDown))
                {
                    Zoom += 0.01f;
                    if (Zoom > 2f)
                    {
                        Zoom = 2f;
                    }
                }

                UpdateManualCamera();
            }
            else if (CameraType == CameraType.MovingToPoint)
            {
                Vector2 diff = Destination - ScreenManager.Camera.Position;
                if (diff.LengthSquared() <= 10)
                {
                    Position = Destination;
                    CameraType = CameraType.Manual;
                    LerpDelta = 3f;
                }
                else
                {
                    diff.Normalize();
                    diff *= LerpDelta;
                    ScreenManager.Camera.TranslateCamera(diff);
                }
            }

            viewMatrix = Matrix.CreateTranslation(new Vector3(Position.X, Position.Y, 0))
                        * Matrix.CreateScale(new Vector3(Zoom, Zoom, 1));

            UpdateViewRectangle();
            UpdateBackground();
        }

        #region Update Functions for the different Camera Types

        private void UpdateFocusedCamera()
        {
            Position = ScreenManager.ScreenCentre - FocusedObject.Position;
        }

        private void UpdateManualCamera()
        {
            float xMove = 0, yMove = 0;

            if (InputManager.IsKeyDown(Keys.Left))
            {
                xMove += ScrollDelta;
            }
            if (InputManager.IsKeyDown(Keys.Right))
            {
                xMove -= ScrollDelta;
            }
            if (InputManager.IsKeyDown(Keys.Up))
            {
                yMove += ScrollDelta;
            }
            if (InputManager.IsKeyDown(Keys.Down))
            {
                yMove -= ScrollDelta;
            }

            TranslateCamera(new Vector2(xMove, yMove));
        }

        #endregion

        public void UpdateBackground()
        {
            BackgroundRectangle = new Rectangle(
                (int)(-Position.X / (Zoom * BackgroundMultiplier)),
                (int)(-Position.Y / (Zoom * BackgroundMultiplier)),
                ScreenManager.Viewport.Width,
                ScreenManager.Viewport.Height);
        }

        public void UpdateViewRectangle()
        {
            ViewRectangle = new Rectangle(
                                    (int)(-Position.X /*- ScreenManager.Viewport.Width / (2f * Zoom)*/),
                                    (int)(-Position.Y /*- ScreenManager.Viewport.Height / (2f * Zoom)*/),
                                    (int)(ScreenManager.Viewport.Width / Zoom),
                                    (int)(ScreenManager.Viewport.Height / Zoom));
        }

        public void TranslateCamera(Vector2 translate)
        {
            Position += translate;
        }

        public void SetPosition(Vector2 position)
        {
            Position = ScreenManager.ScreenCentre - position;
            UpdateBackground();
            UpdateViewRectangle();
        }

        public void MoveToDestination(Vector2 destination, float lerpDelta = 3f)
        {
            Destination = ScreenManager.ScreenCentre - destination;
            LerpDelta = lerpDelta;
            CameraType = CameraType.MovingToPoint;
        }

        public bool IsVisible(GameObject gameObject)
        {
            if (ViewRectangle.Intersects(gameObject.Bounds))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsVisible(UIElement uielement)
        {
            if (ViewRectangle.Intersects(uielement.Bounds))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
