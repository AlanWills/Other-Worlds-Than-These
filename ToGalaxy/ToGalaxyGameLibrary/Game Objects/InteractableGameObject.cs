using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToGalaxyGameLibrary.Screens_and_ScreenManager;

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

        public InteractableGameObject(string dataAsset, Keys key)
            : base(dataAsset)
        {
            InteractKey = key;
        }

        public InteractableGameObject(string dataAsset, Vector2 position, Keys key)
            : base(dataAsset, position)
        {
            InteractKey = key;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (IsAlive())
            {
                if (ScreenManager.Input.IsKeyDown(InteractKey))
                {
                    if (InteractEvent != null)
                    {
                        InteractEvent(this, EventArgs.Empty);
                    }
                }
            }
        }
    }
}
