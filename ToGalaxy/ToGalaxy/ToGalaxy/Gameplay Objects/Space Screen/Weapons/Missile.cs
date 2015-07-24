using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToGalaxy.UI;
using ToGalaxyCustomData;
using ToGalaxyGameLibrary.Game_Objects;
using ToGalaxyGameLibrary.Screens_and_ScreenManager;

namespace ToGalaxy.Gameplay_Objects.Space_Screen
{
    public class Missile : Bullet
    {
        private MissileData MissileData
        {
            get;
            set;
        }

        public AnimatedGameObject MissileTrail
        {
            get;
            private set;
        }

        private Engine Engine
        {
            get;
            set;
        }

        private GameObject TargetShip
        {
            get;
            set;
        }

        public Missile(string dataAsset, Vector2 destinationPoint, float lifeTimer)
            : base(dataAsset, destinationPoint, lifeTimer)
        {

        }

        public Missile(string dataAsset, Vector2 startingPoint, Vector2 destinationPoint, float lifeTimer)
            : base(dataAsset, startingPoint, destinationPoint, lifeTimer)
        {

        }

        public override void LoadContent(ContentManager content)
        {
            base.LoadContent(content);

            if (DataAsset != "")
            {
                MissileData = content.Load<MissileData>(DataAsset);

                if (MissileData.MissileEngineAsset != "")
                {
                    float sinRot = (float)Math.Sin(Rotation);
                    float cosRot = (float)Math.Cos(Rotation);

                    Engine = new Engine("XML/Engines/" + MissileData.MissileEngineAsset, this, MissileData.MissileEngineHardpoint);
                    Engine.LoadContent(content);
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (Engine != null)
            {
                Moving = true;
                Engine.Update(gameTime);

                IncreaseVelocity(new Vector2(-Velocity.X / 5, Engine.EngineData.EngineSpeed * Engine.EngineTrail.Opacity / 10f));

                if (TargetShip != null)
                {
                    Vector2 difference = difference = TargetShip.Position - Position;
                    Rotation = (float)Math.Atan2(difference.X, -difference.Y);

                    if (!TargetShip.IsAlive())
                    {
                        Die();
                    }
                }
                else
                {
                    // Home the missiles towards the destination
                    if (Velocity.X < -0.001)
                    {
                        Vector2 difference = Destination - Position;
                        Rotation = (float)Math.Atan2(difference.X, -difference.Y);
                    }

                    // If the missile reaches the destination it dies (really for when the target ship has died but the missile has already fired)
                    /*if ((Position - Destination).Length() < 10)
                    {
                        Die();
                    }*/
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Engine != null)
            {
                Engine.Draw(spriteBatch);
            }

            base.Draw(spriteBatch);
        }

        public void SetTarget(GameObject target)
        {
            TargetShip = target;
        }
    }
}
