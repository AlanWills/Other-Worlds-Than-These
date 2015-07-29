using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToGalaxyCustomData;
using ToGalaxyGameLibrary.Game_Objects;
using ToGalaxyGameLibrary.UI;

namespace ToGalaxyGameLibrary.Screens_and_ScreenManager
{
    public enum ScreenState
    {
        Active,
        Frozen,
        Hidden,
        Dead
    }

    public class Screen
    {
        #region GameObject Lists

        protected List<GameObject> GameObjects
        {
            get;
            private set;
        }

        private List<GameObject> GameObjectsToAdd
        {
            get;
            set;
        }

        private List<GameObject> GameObjectsToRemove
        {
            get;
            set;
        }

        #endregion

        #region UI Lists

        private List<Button> Buttons
        {
            get;
            set;
        }

        // All Screen UIElements are independent of the camera
        protected List<UIElement> ScreenUIElements
        {
            get;
            private set;
        }

        private List<UIElement> ScreenUIElementsToAdd
        {
            get;
            set;
        }

        private List<UIElement> ScreenUIElementsToRemove
        {
            get;
            set;
        }

        // All In Game UIElements are imagined as in the game world so are dependent on the camera
        private List<UIElement> InGameUIElements
        {
            get;
            set;
        }

        private List<UIElement> InGameUIElementsToAdd
        {
            get;
            set;
        }

        private List<UIElement> InGameUIElementsToRemove
        {
            get;
            set;
        }

        #endregion

        #region Screen Data

        protected string ScreenDataAsset
        {
            get;
            private set;
        }

        public ScreenData ScreenData
        {
            get;
            private set;
        }

        #endregion

        protected Texture2D Background
        {
            get;
            private set;
        }

        protected ScreenManager ScreenManager
        {
            get;
            private set;
        }

        private bool JustStartedUpdating
        {
            get;
            set;
        }

        public ScreenState ScreenState
        {
            get;
            set;
        }

        public Screen(ScreenManager screenManager, string screenDataAsset)
        {
            ScreenManager = screenManager;

            GameObjects = new List<GameObject>();
            GameObjectsToAdd = new List<GameObject>();
            GameObjectsToRemove = new List<GameObject>();

            ScreenUIElements = new List<UIElement>();
            ScreenUIElementsToAdd = new List<UIElement>();
            ScreenUIElementsToRemove = new List<UIElement>();
            InGameUIElements = new List<UIElement>();
            InGameUIElementsToAdd = new List<UIElement>();
            InGameUIElementsToRemove = new List<UIElement>();
            Buttons = new List<Button>();

            ScreenState = ScreenState.Active;
            ScreenDataAsset = screenDataAsset;

            JustStartedUpdating = true;
        }

        public virtual void LoadContent()
        {
            if (ScreenDataAsset != "")
            {
                ScreenData = ScreenManager.Content.Load<ScreenData>(ScreenDataAsset);

                if (ScreenData != null)
                {
                    if (ScreenData.BackgroundTextureAsset != "")
                    {
                        Background = ScreenManager.Content.Load<Texture2D>(ScreenData.BackgroundTextureAsset);
                    }
                }
            }

            foreach (UIElement ui in ScreenUIElementsToAdd)
            {
                ui.LoadContent(ScreenManager.Content);
                ScreenUIElements.Add(ui);
            }
        }

        public void StartMusic()
        {
            if (ScreenData.Music.Count > 0)
            {
                ScreenManager.Music.Play(ScreenData.Music);
            }
        }

        public virtual void AtStartOfUpdate()
        {
            StartMusic();

            JustStartedUpdating = false;
        }

        public virtual void Update(GameTime gameTime)
        {
            if (ScreenState == ScreenState.Hidden || ScreenState == ScreenState.Active)
            {
                if (JustStartedUpdating)
                {
                    AtStartOfUpdate();
                }

                HandleGameObjects(gameTime);
                HandleUIElements(gameTime);
            }
        }

        #region Game Object and UI Update Functions

        protected virtual void HandleGameObjects(GameTime gameTime)
        {
            foreach (GameObject gameObject in GameObjectsToAdd)
            {
                GameObjects.Add(gameObject);
            }

            GameObjectsToAdd.Clear();

            foreach (GameObject gameObject in GameObjects)
            {
                gameObject.CheckMouseInteraction(ScreenManager.Mouse);
                gameObject.Update(gameTime);

                if (gameObject.Status == GameObjectStatus.Dead)
                {
                    RemoveGameObject(gameObject);
                }
            }

            foreach (GameObject gameObject in GameObjectsToRemove)
            {
                GameObjects.Remove(gameObject);
            }

            GameObjectsToRemove.Clear();
        }

        protected virtual void HandleUIElements(GameTime gameTime)
        {
            // Handle the Screen UIElements
            foreach (UIElement ui in ScreenUIElementsToAdd)
            {
                ScreenUIElements.Add(ui);
            }

            ScreenUIElementsToAdd.Clear();

            if (ScreenState == ScreenState.Active)
            {
                foreach (UIElement ui in ScreenUIElements)
                {
                    ui.CheckClicked(ScreenManager.Mouse);
                    ui.Update(gameTime);

                    if (ui.State == UIState.Dead)
                    {
                        RemoveScreenUIElement(ui);
                    }
                }
            }

            foreach (UIElement ui in ScreenUIElementsToRemove)
            {
                ScreenUIElements.Remove(ui);
            }

            ScreenUIElementsToRemove.Clear();

            // Handle the In Game UIElements
            foreach (UIElement ui in InGameUIElementsToAdd)
            {
                InGameUIElements.Add(ui);
            }

            InGameUIElementsToAdd.Clear();

            if (ScreenState == ScreenState.Active)
            {
                foreach (UIElement ui in InGameUIElements)
                {
                    ui.CheckClicked(ScreenManager.Mouse);
                    ui.Update(gameTime);

                    if (ui.State == UIState.Dead)
                    {
                        RemoveInGameUIElement(ui);
                    }
                }
            }

            foreach (UIElement ui in InGameUIElementsToRemove)
            {
                InGameUIElements.Remove(ui);
            }

            InGameUIElementsToRemove.Clear();

            if (ScreenState == ScreenState.Active)
            {
                foreach (Button button in Buttons)
                {
                    button.CheckClicked(ScreenManager.Mouse);
                    button.Update(gameTime);
                }
            }
        }

        #endregion

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (ScreenState == ScreenState.Active || ScreenState == ScreenState.Frozen)
            {
                foreach (UIElement ui in InGameUIElements)
                {
                    if (ui.Texture != null)
                    {
                        if (ScreenManager.Camera.IsVisible(ui))
                        {
                            ui.Draw(spriteBatch);
                        }
                    }
                }

                foreach (GameObject gameObject in GameObjects)
                {
                    if (gameObject.Texture != null)
                    {
                        if (ScreenManager.Camera.IsVisible(gameObject))
                        {
                            gameObject.Draw(spriteBatch);
                        }
                    }
                }
            }
        }

        public virtual void DrawBackground(SpriteBatch spriteBatch)
        {
            if (ScreenState != ScreenState.Hidden)
            {
                if (Background != null)
                {
                    // Draw the background texture always relative to the player - so have the centre of the texture
                    // focused on the player - avoid having to stretch, can just use 1920x1080 texture.
                    spriteBatch.Draw(Background, ScreenManager.Viewport.Bounds, ScreenManager.Camera.BackgroundRectangle, Color.White * 0.6f);
                }
            }
        }

        public virtual void DrawCameraIndependentObjects(SpriteBatch spriteBatch)
        {
            if (ScreenState == ScreenState.Active || ScreenState == ScreenState.Frozen)
            {
                foreach (UIElement ui in ScreenUIElements)
                {
                    ui.Draw(spriteBatch);
                }
            }
        }

        #region Functions for Adding/Removing GameObjects

        public void LoadAndAddGameObject(GameObject gameObject)
        {
            gameObject.LoadContent(ScreenManager.Content);
            GameObjectsToAdd.Add(gameObject);
        }

        public void AddGameObject(GameObject gameObject)
        {
            GameObjectsToAdd.Add(gameObject);
        }

        public GameObject GetGameObject(string name)
        {
            GameObject gameObject = GameObjects.Find(x => x.Data.Name == name);

            if (gameObject == null)
            {
                gameObject = GameObjectsToAdd.Find(x => x.Data.Name == name);
            }

            return gameObject;
        }

        public bool RemoveGameObject(GameObject gameObject)
        {
            if (GameObjects.Contains(gameObject))
            {
                GameObjectsToRemove.Add(gameObject);
                return true;
            }

            return false;
        }

        #endregion

        #region Adding/Removing Functions for Screen UIElements

        // Add a camera independent Screen UIElement
        public void AddScreenUIElement(UIElement ui)
        {
            ui.LoadContent(ScreenManager.Content);
            ScreenUIElementsToAdd.Add(ui);
        }

        // Remove a specific UIElement from the screen's Screen UIElements
        public bool RemoveScreenUIElement(UIElement ui)
        {
            if (ScreenUIElements.Contains(ui))
            {
                ScreenUIElementsToRemove.Add(ui);
                return true;
            }

            return false;
        }

        public UIElement GetScreenUIElement(string name)
        {
            UIElement element = ScreenUIElements.Find(x => x.Name == name);

            if (element == null)
            {
                element = ScreenUIElementsToAdd.Find(x => x.Name == name);
            }

            return element;
        }

        // Remove a Screen UIElement referenced by a name rather than the actual object itself
        public bool RemoveScreenUIElement(string name)
        {
            UIElement ui = ScreenUIElements.Find(x => x.Name == name);

            if (ui == null)
            {
                ui = ScreenUIElementsToAdd.Find(x => x.Name == name);
            }

            if (ui != null)
            {
                ScreenUIElementsToRemove.Add(ui);
                return true;
            }

            return false;
        }

        #endregion

        #region Adding/Removing Functions for In Game UIElements

        // Add a camera dependent In Game UIElement
        public void AddInGameUIElement(UIElement ui)
        {
            ui.LoadContent(ScreenManager.Content);
            InGameUIElementsToAdd.Add(ui);
        }

        // Remove a specific UIElement from the screen's Screen UIElements
        public bool RemoveInGameUIElement(UIElement ui)
        {
            if (InGameUIElements.Contains(ui))
            {
                InGameUIElementsToRemove.Add(ui);
                return true;
            }

            return false;
        }

        // Remove an In Game UIElement referenced by a name rather than the actual object itself
        public bool RemoveInGameUIElement(string name)
        {
            UIElement ui = InGameUIElements.Find(x => x.Name == name);

            if (ui != null)
            {
                InGameUIElementsToRemove.Add(ui);
                return true;
            }

            return false;
        }

        #endregion

        public virtual void Activate()
        {
            ScreenState = ScreenState.Active;
        }

        public virtual void Freeze()
        {
            ScreenState = ScreenState.Frozen;
        }

        public virtual void Die()
        {
            ScreenState = ScreenState.Dead;

            foreach (GameObject gameObject in GameObjectsToAdd)
            {
                gameObject.Die();
            }

            foreach (GameObject gameObject in GameObjects)
            {
                gameObject.Die();
            }
        }

        public void Dispose()
        {
            GameObjects.Clear();
            ScreenUIElements.Clear();
            InGameUIElements.Clear();
        }
    }
}
