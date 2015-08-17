using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToGalaxyGameLibrary.Screens_and_ScreenManager;
using ToGalaxyGameLibrary.Screens_and_ScreenManager.Managers;

namespace ToGalaxyGameLibrary.Game_Objects
{
    public class InteractableGameObject : GameObject
    {
        public EventHandler InteractEvent;

        private Keys InteractKey
        {
            get;
            set;
        }

        public InteractableGameObject(string dataAsset, Keys key = Keys.Enter)
            : base(dataAsset)
        {
            InteractKey = key;
        }

        public InteractableGameObject(string dataAsset, Vector2 position, Keys key = Keys.Enter)
            : base(dataAsset, position)
        {
            InteractKey = key;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (IsAlive())
            {
                if (InputManager.IsKeyDown(InteractKey))
                {
                    OnInteract();
                }
            }
        }

        public virtual void OnInteract()
        {
            if (InteractEvent != null)
            {
                InteractEvent(this, EventArgs.Empty);
            }
        }
    }
}
