using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToGalaxyCustomData;
using ToGalaxyGameLibrary.Game_Objects;

namespace ToGalaxy.Gameplay_Objects
{
    public enum ShieldState
    {
        Active,
        Charging,
        Depleted
    }

    public class Shield : GameObject
    {
        #region Data

        public ShieldData ShieldData
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

        public int ShieldStrength
        {
            get;
            private set;
        }

        public float ShieldStrengthMultiplier
        {
            get;
            set;
        }

        public ShieldState ShieldState
        {
            get
            {
                if (ShieldStrength == 0)
                {
                    return ShieldState.Depleted;
                }
                else if (timeSinceDamageTaken >= chargeDelayTimer && ShieldStrength < ShieldData.Strength)
                {
                    return ShieldState.Charging;
                }
                else
                {
                    return ShieldState.Active;
                }
            }
        }

        private const float resetOpacity = 0.1f;
        private float timeSinceDamageTaken = 0;
        public float chargeDelayTimer = 0, regenTimer = 0;

        public Shield(string dataAsset, Vector2 startingPosition, Ship parentShip)
            : base(dataAsset, startingPosition)
        {
            ParentShip = parentShip;
            ShieldStrengthMultiplier = 1;
        }

        public override void LoadContent(ContentManager content)
        {
            base.LoadContent(content);

            float maxDimension = (float)Math.Max((float)ParentShip.Texture.Width / Texture.Width, (float)ParentShip.Texture.Height / Texture.Height);

            Scale = new Vector2(maxDimension, maxDimension);
            ShieldData = content.Load<ShieldData>(DataAsset);

            if (ShieldData != null)
            {
                ShieldStrength = (int)(ShieldData.Strength * ShieldStrengthMultiplier);
                Colour = new Color(ShieldData.Colour);
                chargeDelayTimer = ShieldData.RechargeDelay;
                SetOpacity(resetOpacity);
            }
        }

        public override void Update(GameTime gameTime)
        {
            Position = ParentShip.Position;

            // Deals with shield flare effect
            SetOpacity(Opacity - (float)gameTime.ElapsedGameTime.Milliseconds / 1000);
            if (Opacity < resetOpacity)
            {
                Opacity = resetOpacity;
            }

            timeSinceDamageTaken += (float)gameTime.ElapsedGameTime.Milliseconds / 1000;

            // Recharge the shields depending on whether they were completely depleted or just partially
            if (ShieldStrength == 0)
            {
                // Shields completely depleted so come back fully charged after 30s
                regenTimer += (float)gameTime.ElapsedGameTime.Milliseconds / 1000;

                if (regenTimer >= ShieldData.DepletionDelay)
                {
                    ShieldStrength = (int)(ShieldData.Strength * ShieldStrengthMultiplier);
                    timeSinceDamageTaken = 0;
                    regenTimer = 0;
                }
            }
            // The time since damage taken is greater than how long we have to wait to start recharging
            else if (timeSinceDamageTaken >= chargeDelayTimer)
            {
                if (ShieldStrength < (int)(ShieldData.Strength * ShieldStrengthMultiplier))
                {
                    // Shields partially depleted so we regen as specified in the data
                    regenTimer += (float)gameTime.ElapsedGameTime.Milliseconds / 1000;

                    // Enough time has passed to regen 1 shield strength
                    if (regenTimer >= 1/ShieldData.RechargePerSecond)
                    {
                        ShieldStrength++;
                        regenTimer = 0;
                    }
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // Only draw if shield is working
            // Maybe draw at different opacity based on current strength or something?
            if (ShieldStrength > 0)
            {
                base.Draw(spriteBatch);
            }
        }

        public void Damage(int damage)
        {
            // Damage was taken so we completely reset all recharge timers
            ShieldStrength -= damage;
            timeSinceDamageTaken = 0;
            regenTimer = 0;

            // For shield flare effect
            Opacity = 1;

            if (ShieldStrength > 0)
            {
                chargeDelayTimer = ShieldData.RechargeDelay;
            }
        }
    }
}
