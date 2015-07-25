using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToGalaxy.Gameplay_Objects.Space_Screen;
using ToGalaxyGameLibrary.UI;

namespace ToGalaxy.Gameplay_Objects
{
    public class PlayerShip : Ship
    {
        private Image SelectionUI
        {
            get;
            set;
        }

        public PlayerShip(string dataAsset, Vector2 startingPosition)
            : base(dataAsset, startingPosition)
        {
            ManualSteering = true;
            AutoTurrets = false;
            InterialDampeners = true;

            Selected = true;
        }

        public override void LoadContent(ContentManager content)
        {
            base.LoadContent(content);

            // Again Position does not matter here as we will set it when the UI becomes active - when the player is selected
            // We are going to need to scale this based on the player ship's size
            SelectionUI = new Image(
                "Sprites/UI/Icons/SelectionIcon",
                Position,
                new Vector2(0.5f, 0.5f),
                "SelectionIcon");

            SelectionUI.LoadContent(content);
        }

        public override void LoadShipMods(ContentManager content)
        {
            if (ShipData.ShipModSlots > 0)
            {
                ShipMods.Clear();

                int keyCounter = 0;
                List<Keys> activationKeys = new List<Keys>(){ Keys.Q, Keys.E, Keys.R, Keys.T, Keys.Y };
                for (int i = 0; i < ShipData.ShipModNames.Count; i++)
                {
                    if (i < ShipData.ShipModSlots)
                    {
                        ShipMod shipMod = new ShipMod("XML/Ship Mods/" + ShipData.ShipModNames[i], Position, this);
                        shipMod.LoadContent(content);

                        if (shipMod.ShipModData.Active)
                        {
                            shipMod.SetActivationKey(activationKeys[keyCounter]);
                            keyCounter++;
                        }

                        ShipMods.Add(shipMod);
                    }
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            CheckInput(gameTime);

            base.Update(gameTime);
        }

        private void CheckInput(GameTime gameTime)
        {
            if (ManualSteering)
            {
                PerformManualPiloting(gameTime);
            }

            /*if (Keyboard.GetState().IsKeyDown(Keys.C))
            {
                foreach (Turret turret in Turrets)
                {
                    turret.ClearTarget();
                }
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                Movement.Stop();
            }*/
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Selected)
            {
                SelectionUI.SetPosition(Position);
                SelectionUI.Draw(spriteBatch);
            }

            base.Draw(spriteBatch);
        }

        #region Manual and Automatic Piloting Functions

        private void PerformManualPiloting(GameTime gameTime)
        {
            KeyboardState keyboard = Keyboard.GetState();

            // Rotation Checks
            if (keyboard.IsKeyDown(Keys.A))
            {
                Rotation += -RotateSpeed;
            }

            if (keyboard.IsKeyDown(Keys.D))
            {
                Rotation += RotateSpeed;
            }

            // Movement checks
            Vector2 delta = Vector2.Zero;
            if (keyboard.IsKeyDown(Keys.W))
            {
                delta += new Vector2(0, 1f) * Speed;
            }

            if (keyboard.IsKeyDown(Keys.S))
            {
                delta += new Vector2(0, -1f) * Speed;
            }

            if (keyboard.IsKeyDown(Keys.A) ||
                keyboard.IsKeyDown(Keys.D) ||
                keyboard.IsKeyDown(Keys.W) ||
                keyboard.IsKeyDown(Keys.S))
            {
                Moving = true;
                Selected = true;
            }
            else
            {
                Moving = false;
            }

            Velocity += delta * (float)gameTime.ElapsedGameTime.Milliseconds / 1000f;
            // Done like this to make it quicker
            float velSquared = Velocity.LengthSquared();
            if (velSquared > Speed * Speed)
            {
                Velocity *= 1/velSquared;
                Velocity *= Speed * Speed;
            }

            SetDestination(Position);
        }

        public bool MoveOrder(Vector2 position)
        {
            if (Selected)
            {
                Movement.ClearTarget();
                SetDestination(position);
                return true;
            }

            return false;
        }

        public bool AttackOrder(Ship targetShip)
        {
            if (Selected)
            {
                SetTarget(targetShip);
                // SetDestination(targetShip.Position);
                return true;
            }

            return false;
        }

        #endregion
    }
}
