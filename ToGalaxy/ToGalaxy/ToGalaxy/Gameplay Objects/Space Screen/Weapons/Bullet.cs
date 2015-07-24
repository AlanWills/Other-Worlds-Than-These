using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToGalaxyGameLibrary.Game_Objects;

namespace ToGalaxy.Gameplay_Objects.Space_Screen
{
    public class Bullet : GameObject
    {
        #region Bullet Destination Properties

        // Bullet will die on collision with enemy ship or upon reaching destination point
        // Allows autonomous removal of bullets by just setting their status to Dead - checked in Turret
        public Vector2 Destination
        {
            get;
            protected set;
        }

        protected float LifeTimer
        {
            get;
            set;
        }

        #endregion

        public float lifeTimer = 0;

        public Bullet(string dataAsset, Vector2 destinationPoint, float lifeTimer)
            : base(dataAsset)
        {
            Destination = destinationPoint;
            LifeTimer = lifeTimer;
        }

        public Bullet(string dataAsset, Vector2 startingPoint, Vector2 destinationPoint, float lifeTimer)
            : base(dataAsset, startingPoint)
        {
            Destination = destinationPoint;
            LifeTimer = lifeTimer;
        }

        public void SetBulletTargetDestination(Vector2 destination)
        {
            Destination = destination;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            lifeTimer += (float)gameTime.ElapsedGameTime.Milliseconds / 1000f;
            if (lifeTimer > LifeTimer)
            {
                Die();
            }
        }

        public override void Die()
        {
            // Add sound effect or explosion or something

            base.Die();
        }
    }
}
