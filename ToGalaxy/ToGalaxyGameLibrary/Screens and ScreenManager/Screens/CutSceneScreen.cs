using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToGalaxyGameLibrary.Game_Objects;
using ToGalaxyGameLibrary.UI;

namespace ToGalaxyGameLibrary.Screens_and_ScreenManager
{
    public class CutSceneEventArgs : EventArgs
    {
        public string FunctionName { get; set; }
        public float ActivationTime { get; set; }
        public string ObjectName { get; set; }
        public float MoveSpeed { get; set; }
        public Vector2 MoveDestination { get; set; }
        public GameObject GameObject { get; set; }

        public CutSceneEventArgs(string functionName, float activationTime)
        {
            FunctionName = functionName;
            ActivationTime = activationTime;
        }
    }

    public class CutSceneScreen : Screen
    {
        private float TotalRunningTime
        {
            get;
            set;
        }

        protected Dictionary<string, Func<CutSceneEventArgs, bool>> EventsDictionary
        {
            get;
            private set;
        }

        protected List<CutSceneEventArgs> EventsList
        {
            get;
            private set;
        }

        private List<CutSceneEventArgs> EventsToRemove
        {
            get;
            set;
        }

        public Screen NextScreen
        {
            get;
            private set;
        }

        public CutSceneScreen(ScreenManager screenManager, string screenDataAsset)
            : base(screenManager, screenDataAsset)
        {
            TotalRunningTime = 0;
            EventsDictionary = new Dictionary<string, Func<CutSceneEventArgs, bool>>();
            EventsDictionary.Add("Add GameObject", new Func<CutSceneEventArgs, bool>(AddGameObject));
            EventsDictionary.Add("Move GameObject", new Func<CutSceneEventArgs, bool>(Move));
            EventsDictionary.Add("Rotate GameObject", new Func<CutSceneEventArgs, bool>(Rotate));
            EventsDictionary.Add("Move Camera", new Func<CutSceneEventArgs, bool>(MoveCamera));
            EventsDictionary.Add("Remove GameObject", new Func<CutSceneEventArgs, bool>(RemoveGameObject));
            EventsDictionary.Add("Add Dialog Box", new Func<CutSceneEventArgs, bool>(AddDialogBox));
            EventsDictionary.Add("Load Next Screen", new Func<CutSceneEventArgs, bool>(LoadNextScreen));

            EventsList = new List<CutSceneEventArgs>();
            EventsToRemove = new List<CutSceneEventArgs>();
        }

        public virtual void AddGameObjects()
        {

        }

        public virtual void AddDialogBoxes()
        {

        }

        public virtual void AddCameraMovement()
        {

        }

        public virtual void SetUpNextScreen()
        {

        }

        public virtual void SortEventsIntoChronologicalOrder()
        {
            EventsList.Sort(
                delegate(CutSceneEventArgs eventsA, CutSceneEventArgs eventsB)
                {
                    return eventsA.ActivationTime.CompareTo(eventsB.ActivationTime);
                });
        }

        public override void LoadContent()
        {
            base.LoadContent();

            AddGameObjects();
            AddDialogBoxes();
            AddCameraMovement();
            SetUpNextScreen();

            SortEventsIntoChronologicalOrder();
        }

        public override void Update(GameTime gameTime)
        {
            TotalRunningTime += (float)gameTime.ElapsedGameTime.Milliseconds / 1000f;
            foreach (CutSceneEventArgs eventArgs in EventsList)
            {
                if (TotalRunningTime >= eventArgs.ActivationTime)
                {
                    if (EventsDictionary[eventArgs.FunctionName].Invoke(eventArgs))
                    {
                        EventsToRemove.Add(eventArgs);
                    }
                }
                else
                {
                    // If we have reached an event whose activation time is after the current total time,
                    // since all events are in consecutive order we do not need to carry on looping through events.
                    break;
                }
            }

            foreach (CutSceneEventArgs eventArgs in EventsToRemove)
            {
                EventsList.Remove(eventArgs);
            }

            EventsToRemove.Clear();

            base.Update(gameTime);
        }

        public virtual bool AddGameObject(CutSceneEventArgs eventArgs)
        {
            if (eventArgs.GameObject != null)
            {
                AddGameObject(eventArgs.GameObject);
            }

            return true;
        }

        public virtual bool RemoveGameObject(CutSceneEventArgs eventArgs)
        {
            if (eventArgs.ObjectName != null)
            {
                GameObject gameObject = GetGameObject(eventArgs.ObjectName);
                if (gameObject != null)
                {
                    gameObject.Die();
                }
            }
            else if (eventArgs.GameObject != null)
            {
                GameObjects.Remove(eventArgs.GameObject);
            }

            return true;
        }

        public virtual bool Move(CutSceneEventArgs eventArgs)
        {
            return true;
        }

        public virtual bool Rotate(CutSceneEventArgs eventArgs)
        {
            if (eventArgs.GameObject != null)
            {
                Vector2 difference = new Vector2((float)Math.Sin(eventArgs.MoveSpeed), -(float)Math.Cos(eventArgs.MoveSpeed));
                float desiredAngle = (float)Math.Atan2(difference.X, -difference.Y);

                desiredAngle %= (float)MathHelper.Pi;

                // Rotates to desired angle
                if (Math.Abs(desiredAngle - eventArgs.GameObject.Rotation) < 2 * 0.01f)
                {
                    eventArgs.GameObject.SetRotation(desiredAngle);

                    return true;
                }
                else
                {
                    // Velocity = Vector2.Zero;
                    // Change the coordinates so we always assume rotation is 0
                    // Only interested in the offset between desiredAngle and Rotation to work out which way to go
                    float zeroedDesiredAngle = desiredAngle - eventArgs.GameObject.Rotation;
                    if (zeroedDesiredAngle < -MathHelper.Pi)
                    {
                        zeroedDesiredAngle += MathHelper.TwoPi;
                    }
                    if (zeroedDesiredAngle > MathHelper.Pi)
                    {
                        zeroedDesiredAngle -= MathHelper.Pi;
                    }

                    if (zeroedDesiredAngle > 0)
                    {
                        eventArgs.GameObject.SetRotation(eventArgs.GameObject.Rotation + 0.01f);
                    }
                    else
                    {
                        eventArgs.GameObject.SetRotation(eventArgs.GameObject.Rotation - 0.01f);
                    }

                    // Make sure Rotation is in the bounds -Pi to Pi for comparison with the desiredAngle above
                    if (eventArgs.GameObject.Rotation > MathHelper.Pi)
                    {
                        eventArgs.GameObject.SetRotation(eventArgs.GameObject.Rotation - MathHelper.TwoPi);
                    }
                    if (eventArgs.GameObject.Rotation < -MathHelper.Pi)
                    {
                        eventArgs.GameObject.SetRotation(eventArgs.GameObject.Rotation + MathHelper.TwoPi);
                    }

                    return false;
                }
            }

            return true;
        }

        public virtual bool MoveCamera(CutSceneEventArgs eventArgs)
        {
            ScreenManager.Camera.MoveToDestination(eventArgs.MoveDestination, eventArgs.MoveSpeed);
            if (ScreenManager.Camera.CameraType == CameraType.MovingToPoint)
            {
                return false;
            }
            else
            {
                ScreenManager.Camera.SetFixedScreenCamera(false);
                return true;
            }
        }

        public virtual bool AddDialogBox(CutSceneEventArgs eventArgs)
        {
            DialogBox dialogBox = new DialogBox(
                "Sprites/UI/Panels/Panel",
                eventArgs.ObjectName,
                eventArgs.MoveDestination,
                new Vector2(ScreenManager.Viewport.Width / 3, ScreenManager.Viewport.Height / 8),
                new Color(0, 0.1f, 0, 0),
                "Dialog Box",
                0.5f,
                eventArgs.MoveSpeed);
            AddScreenUIElement(dialogBox);

            return true;
        }

        public virtual bool LoadNextScreen(CutSceneEventArgs eventArgs)
        {
            if (NextScreen != null)
            {
                ScreenManager.LoadAndAddScreen(NextScreen);
                Die();
            }

            return true;
        }

        #region Add Event Functions

        public virtual void AddGameObjectEvent(float activationTime, GameObject gameObject)
        {
            CutSceneEventArgs eventArgs = new CutSceneEventArgs("Add GameObject", activationTime);
            eventArgs.GameObject = gameObject;
            eventArgs.GameObject.LoadContent(ScreenManager.Content);

            EventsList.Add(eventArgs);
        }

        public virtual void AddRemoveGameObjectEvent(float activationTime, GameObject gameObject)
        {
            CutSceneEventArgs eventArgs = new CutSceneEventArgs("Add GameObject", activationTime);
            eventArgs.GameObject = gameObject;

            EventsList.Add(eventArgs);
        }

        public virtual void AddRemoveGameObjectEvent(float activationTime, string gameObjectName)
        {
            CutSceneEventArgs eventArgs = new CutSceneEventArgs("Add GameObject", activationTime);
            eventArgs.ObjectName = gameObjectName;

            EventsList.Add(eventArgs);
        }

        public void AddMoveEvent(float activationTime, GameObject objectToMove, Vector2 position)
        {
            CutSceneEventArgs eventArgs = new CutSceneEventArgs("Move GameObject", activationTime);
            eventArgs.MoveDestination = position;
            eventArgs.GameObject = objectToMove;

            EventsList.Add(eventArgs);
        }

        public void AddRotationEvent(float activationTime, GameObject objectToMove, float rotation)
        {
            CutSceneEventArgs eventArgs = new CutSceneEventArgs("Rotate GameObject", activationTime);
            eventArgs.MoveSpeed = rotation;
            eventArgs.GameObject = objectToMove;

            EventsList.Add(eventArgs);
        }

        public void AddCameraMoveEvent(float activationTime, Vector2 position, float moveSpeed = 3f)
        {
            CutSceneEventArgs eventArgs = new CutSceneEventArgs("Move Camera", activationTime);
            eventArgs.MoveDestination = position;
            eventArgs.MoveSpeed = moveSpeed;

            EventsList.Add(eventArgs);
        }

        public void AddDialogBoxEvent(float activationTime, string text, float lifeTimer = 6f)
        {
            CutSceneEventArgs eventArgs = new CutSceneEventArgs("Add Dialog Box", activationTime);
            eventArgs.ObjectName = text;
            eventArgs.MoveSpeed = lifeTimer;
            eventArgs.MoveDestination = new Vector2(ScreenManager.Viewport.Width / 4, ScreenManager.Viewport.Height / 2);

            EventsList.Add(eventArgs);
        }

        public void AddDialogBoxEvent(float activationTime, string text, Vector2 position, float lifeTimer = 6f)
        {
            CutSceneEventArgs eventArgs = new CutSceneEventArgs("Add Dialog Box", activationTime);
            eventArgs.ObjectName = text;
            eventArgs.MoveSpeed = lifeTimer;
            eventArgs.MoveDestination = position;

            EventsList.Add(eventArgs);
        }

        public void AddLoadNextScreenEvent(float activationTime)
        {
            CutSceneEventArgs eventArgs = new CutSceneEventArgs("Load Next Screen", activationTime);

            EventsList.Add(eventArgs);
        }

        public void SetNextScreen(Screen screen)
        {
            NextScreen = screen;
        }

        #endregion
    }
}
