using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToGalaxyCustomData;
using ToGalaxyGameLibrary.Game_Objects;

namespace ToGalaxy.Gameplay_Objects.Space_Screen
{
    public class ShipMod : GameObject
    {
        #region Data

        public ShipModData ShipModData
        {
            get;
            private set;
        }

        #endregion

        private Ship ParentShip
        {
            get;
            set;
        }

        #region Activation Properties

        private Keys ActivationKey
        {
            get;
            set;
        }

        public float TimeSinceActivation
        {
            get;
            private set;
        }

        private bool ModFinishedRunning
        {
            get
            {
                // If it is a passive it is never finished running
                // If it is active, we check if it has exceeded the running time
                bool finished = false;
                if (ShipModData.Active)
                {
                    finished = TimeSinceActivation > ShipModData.RunTime;
                }

                return finished;
            }
        }

        private bool CanActivate
        {
            get
            {
                return TimeSinceActivation > (ShipModData.RunTime + ShipModData.Cooldown) && ShipModData.Active;
            }
        }

        #endregion

        public Action<Ship, bool, float> ActivationEvent;

        public ShipMod(string dataAsset, Vector2 position, Ship parentShip, Keys key = Keys.None)
            : base(dataAsset, position)
        {
            ParentShip = parentShip;
            ActivationKey = key;
        }

        public override void LoadContent(ContentManager content)
        {
            base.LoadContent(content);

            if (DataAsset != "")
            {
                ShipModData = content.Load<ShipModData>(DataAsset);
                TimeSinceActivation = ShipModData.Cooldown;
                ActivationEvent = ModFunctionManager.ModEvents[ShipModData.ModFunctionName];
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // Don't want to draw this in game
            // Want UI instead.
        }

        public override void Update(GameTime gameTime)
        {
            if (ParentShip != null)
            {
                SetPosition(ParentShip.Position);
            }

            // This is just responsible for indicating the mod has been activated
            if (CanActivate)
            {
                // If it has a key it is for a player ship
                if (ActivationKey != Keys.None)
                {
                    if (Keyboard.GetState().GetPressedKeys().Contains(ActivationKey))
                    {
                        TimeSinceActivation = 0;
                    }
                }
                // Otherwise it is for an enemy ship and should be activated when possible
                else
                {
                    TimeSinceActivation = 0;
                }
            }

            if (ActivationEvent != null)
            {
                ActivationEvent(ParentShip, ModFinishedRunning, TimeSinceActivation);
                TimeSinceActivation += (float)gameTime.ElapsedGameTime.Milliseconds / 1000f;
            }
        }
    }
}
