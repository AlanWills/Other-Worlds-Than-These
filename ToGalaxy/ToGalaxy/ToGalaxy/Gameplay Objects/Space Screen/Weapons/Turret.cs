using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToGalaxyCustomData;
using ToGalaxyGameLibrary.Audio;
using ToGalaxyGameLibrary.Game_Objects;
using ToGalaxyGameLibrary.Screens_and_ScreenManager;

namespace ToGalaxy.Gameplay_Objects.Space_Screen
{
    public class Turret : GameObject
    {
        #region Data

        public TurretData TurretData
        {
            get;
            private set;
        }

        #endregion

        #region Bullet Properties

        // Template which we will clone new bullets off
        private Bullet Bullet
        {
            get;
            set;
        }

        protected List<Bullet> ActiveBullets
        {
            get;
            set;
        }

        protected List<Bullet> BulletsToAdd
        {
            get;
            set;
        }

        protected List<Bullet> BulletsToRemove
        {
            get;
            set;
        }

        protected List<Bullet> BulletSpawnPool
        {
            get;
            set;
        }

        protected List<AnimatedGameObject> ActiveExplosions
        {
            get;
            set;
        }

        protected List<AnimatedGameObject> ExplosionsToAdd
        {
            get;
            set;
        }

        protected List<AnimatedGameObject> ExplosionsToRemove
        {
            get;
            set;
        }

        #endregion

        #region Turret Targeting and Firing Properties

        public GameObject Target
        {
            get;
            private set;
        }

        public Vector2 TargetLocation
        {
            get;
            private set;
        }

        public float CurrentFireTimer
        {
            get;
            protected set;
        }

        public bool CanFire
        {
            get
            {
                return CurrentFireTimer >= TurretData.FireTimer;
            }
        }

        public Keys FireKey
        {
            get;
            private set;
        }

        /*
        private MySoundEffect BulletFiringSoundEffect
        {
            get;
            set;
        }*/

        #endregion

        #region Parent Ship and Hard Point Location

        protected Ship ParentShip
        {
            get;
            set;
        }

        public Vector2 WeaponHardPoint
        {
            get;
            private set;
        }

        #endregion

        protected AnimatedGameObject Explosion
        {
            get;
            set;
        }

        protected MySoundEffect TurretFiringSoundEffect
        {
            get;
            set;
        }

        public Turret(string dataAsset, Ship parentShip, Vector2 hardPointLocation, Keys fireKey)
            : base(dataAsset)
        {
            ParentShip = parentShip;
            WeaponHardPoint = hardPointLocation;
            FireKey = fireKey;

            float cosRot = (float)Math.Cos(ParentShip.Rotation);
            float sinRot = (float)Math.Sin(ParentShip.Rotation);
            Position = ParentShip.Position + new Vector2(cosRot * WeaponHardPoint.X - sinRot * WeaponHardPoint.Y, sinRot * WeaponHardPoint.X + cosRot * WeaponHardPoint.Y);

            Target = null;

            BulletsToAdd = new List<Bullet>();
            ActiveBullets = new List<Bullet>();
            BulletsToRemove = new List<Bullet>();
            BulletSpawnPool = new List<Bullet>();

            ExplosionsToAdd = new List<AnimatedGameObject>();
            ActiveExplosions = new List<AnimatedGameObject>();
            ExplosionsToRemove = new List<AnimatedGameObject>();
        }

        public override void LoadContent(ContentManager content)
        {
            base.LoadContent(content);

            if (DataAsset != "")
            {
                TurretData = content.Load<TurretData>(DataAsset);
                CurrentFireTimer = TurretData.FireTimer;

                float scale = Math.Min((float)ParentShip.Bounds.Width / (float)(2 * Bounds.Width), (float)ParentShip.Bounds.Height / (float)(2 * Bounds.Height));
                SetScale(new Vector2(Math.Min(scale, 1), Math.Min(scale, 1)));

                TurretFiringSoundEffect = new MySoundEffect(TurretData.TurretSoundEffectAsset, false, ScreenManager.Settings.OptionsData.SoundEffectsVolume * 0.5f);
                TurretFiringSoundEffect.LoadContent(ScreenManager.Content);
            }
        }

        public virtual void SetUpSpawnPool()
        {
            for (int i = 0; i < Math.Max(2, (int)(2 / TurretData.FireTimer)); i++)
            {
                Bullet bullet = new Bullet(TurretData.BulletAsset, Position, TurretData.BulletVelocity, TurretData.BulletLifeTime);
                bullet.LoadContent(ScreenManager.Content);
                bullet.SetScale(Scale);
                BulletSpawnPool.Add(bullet);
            }
        }

        public override void Update(GameTime gameTime)
        {
            // Going to need to change this based on the weapon hardpoints
            float cosRot = (float)Math.Cos(ParentShip.Rotation);
            float sinRot = (float)Math.Sin(ParentShip.Rotation);
            Position = ParentShip.Position + new Vector2(cosRot * WeaponHardPoint.X - sinRot * WeaponHardPoint.Y, sinRot * WeaponHardPoint.X + cosRot * WeaponHardPoint.Y);

            Vector2 difference = Vector2.Zero;
            if (ParentShip.AutoTurrets)
            {
                if (Target != null)
                {
                    difference = Target.Position - Position;
                    Rotation = (float)Math.Atan2(difference.X, -difference.Y);
                }
                else
                {
                    SetRotation(ParentShip.Rotation);
                }
            }
            else
            {
                difference = TargetLocation - Position;
                Rotation = (float)Math.Atan2(difference.X, -difference.Y);
            }

            CurrentFireTimer += (float)gameTime.ElapsedGameTime.Milliseconds / 1000;

            if (CanFire && difference.Length() <= TurretData.Range)
            {
                if (ParentShip.AutoTurrets)
                {
                    if (Target != null)
                    {
                        FireBullet();
                    }
                    else
                    {
                        TurretFiringSoundEffect.Stop();
                    }
                }
                else
                {
                    if (Mouse.GetState().RightButton == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(FireKey))
                    {
                        FireBullet();
                    }
                    else
                    {
                        TurretFiringSoundEffect.Stop();
                    }
                }
            }

            UpdateBullets(gameTime);
            UpdateExplosions(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            foreach (Bullet bullet in ActiveBullets)
            {
                bullet.Draw(spriteBatch);
            }

            foreach (AnimatedGameObject explosion in ActiveExplosions)
            {
                explosion.Draw(spriteBatch);
            }
        }

        #region Handling the Missile and Explosion Spawn Pools

        private void UpdateBullets(GameTime gameTime)
        {
            foreach (Bullet bullet in BulletsToAdd)
            {
                ActiveBullets.Add(bullet);
            }

            BulletsToAdd.Clear();

            foreach (Bullet bullet in ActiveBullets)
            {
                bullet.Update(gameTime);

                if (bullet.Status == GameObjectStatus.Dead)
                {
                    BulletsToRemove.Add(bullet);
                }
            }

            foreach (Bullet bullet in BulletsToRemove)
            {
                bullet.Alive();
                bullet.lifeTimer = 0;
                ActiveBullets.Remove(bullet);
                BulletSpawnPool.Add(bullet);
            }

            BulletsToRemove.Clear();
        }

        private void UpdateExplosions(GameTime gameTime)
        {
            foreach (AnimatedGameObject explosion in ExplosionsToAdd)
            {
                explosion.Play();
                ActiveExplosions.Add(explosion);
            }

            ExplosionsToAdd.Clear();

            foreach (AnimatedGameObject explosion in ActiveExplosions)
            {
                explosion.Update(gameTime);

                if (explosion.Status == GameObjectStatus.Dead)
                {
                    ExplosionsToRemove.Add(explosion);
                }
            }

            foreach (AnimatedGameObject explosion in ExplosionsToRemove)
            {
                explosion.Stop();
                ActiveExplosions.Remove(explosion);
            }

            ExplosionsToRemove.Clear();
        }

        #endregion

        #region Firing and Checking for Bullet Collisions

        protected virtual void FireBullet()
        {
            CurrentFireTimer = 0;

            if (BulletSpawnPool.Count > 0)
            {
                Bullet firedBullet = BulletSpawnPool[0];
                BulletSpawnPool.RemoveAt(0);

                if (firedBullet != null)
                {
                    if (Target != null)
                    {
                        float length = (Target.Position - Position).Length();
                        float sinRot = (float)Math.Sin(Target.Rotation);
                        float cosRot = (float)Math.Cos(Target.Rotation);

                        Vector2 diff = new Vector2(cosRot * Target.Velocity.X + sinRot * Target.Velocity.Y, sinRot * Target.Velocity.X - cosRot * Target.Velocity.Y);
                        firedBullet.SetBulletTargetDestination(Target.Position + diff * length);
                    }
                    else
                    {
                        firedBullet.SetBulletTargetDestination(TargetLocation);
                    }
                }

                firedBullet.SetPosition(Position);
                firedBullet.SetRotation(Rotation);
                firedBullet.SetVelocity(TurretData.BulletVelocity);

                ActiveBullets.Add(firedBullet);
                TurretFiringSoundEffect.Play();
            }
        }

        public virtual GameObject CheckBulletsHitTarget(Ship ship)
        {
            foreach (Bullet bullet in ActiveBullets)
            {
                if (ship.Bounds.Intersects(bullet.Bounds))
                {
                    bullet.Die();
                    ship.Damage(TurretData.BulletDamage);

                    return ship;
                }
            }

            return null;
        }

        public virtual GameObject CheckBulletsHitTargets(List<Ship> ships)
        {
            foreach (Ship ship in ships)
            {
                if (CheckBulletsHitTarget(ship) != null)
                {
                    return ship;
                }
            }

            return null;
        }

        #endregion

        #region Target Setting

        public void SetTarget(GameObject target)
        {
            Target = target;
        }

        public void SetTargetLocation(Vector2 targetLocation)
        {
            TargetLocation = targetLocation;
        }

        public void ClearTarget()
        {
            Target = null;
        }

        #endregion
    }
}
