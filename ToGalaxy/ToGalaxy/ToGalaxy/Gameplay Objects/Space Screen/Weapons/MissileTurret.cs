using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToGalaxyGameLibrary.Game_Objects;
using ToGalaxyGameLibrary.Screens_and_ScreenManager;

namespace ToGalaxy.Gameplay_Objects.Space_Screen
{
    public class MissileTurret : Turret
    {
        private Missile Missile
        {
            get;
            set;
        }

        public MissileTurret(string dataAsset, Ship parentShip, Vector2 hardPointLocation, Keys fireKey)
            : base(dataAsset, parentShip, hardPointLocation, fireKey)
        {

        }

        public override void LoadContent(ContentManager content)
        {
            base.LoadContent(content);

            if (TurretData != null)
            {
                Explosion = new AnimatedGameObject("XML/FX/Explosion", Vector2.Zero, false, false);
                Explosion.LoadContent(ScreenManager.Content);
                Explosion.SetScale(new Vector2(TurretData.BulletDamage / 20f, TurretData.BulletDamage / 20f));

                // Currently not used in favour of using SoundEffectsManager
                /*BulletFiringSoundEffect = new MySoundEffect(TurretData.TurretSoundEffectAsset, false, 0.3f);
                BulletFiringSoundEffect.LoadContent(content);*/
            }
        }

        public override void SetUpSpawnPool()
        {
            for (int i = 0; i < (int)(20 / TurretData.FireTimer); i++)
            {
                Missile missile = new Missile(TurretData.BulletAsset, Position, TurretData.BulletVelocity, TurretData.BulletLifeTime);
                missile.LoadContent(ScreenManager.Content);
                missile.SetScale(Scale);
                BulletSpawnPool.Add(missile);
            }
        }

        protected override void FireBullet()
        {
            CurrentFireTimer = 0;

            if (BulletSpawnPool.Count > 0)
            {
                Missile firedBullet = (Missile)BulletSpawnPool[0];
                BulletSpawnPool.RemoveAt(0);

                if (firedBullet != null)
                {
                    if (Target != null)
                    {
                        // Use this for non-homing missiles
                        // firedBullet.SetBulletTargetDestination(Target.Position);

                        // Otherwise for homing missiles
                        firedBullet.SetTarget(Target);
                    }
                    else
                    {
                        firedBullet.SetBulletTargetDestination(TargetLocation);
                    }
                }

                firedBullet.SetPosition(Position);
                firedBullet.SetRotation(Rotation);
                firedBullet.SetVelocity(new Vector2(Math.Sign(WeaponHardPoint.X) * (3 + (ParentShip.Bounds.Width / 2 - Math.Abs(WeaponHardPoint.X)) / 2), 0));

                if (firedBullet.MissileTrail != null)
                {
                    firedBullet.MissileTrail.SetOpacity(0);
                }
                
                ActiveBullets.Add(firedBullet);
                ScreenManager.SoundEffects.Play(TurretData.TurretSoundEffectAsset);
            }
        }

        public override GameObject CheckBulletsHitTarget(Ship ship)
        {
            foreach (Missile bullet in ActiveBullets)
            {
                if (ship.Bounds.Intersects(bullet.Bounds))
                {
                    AnimatedGameObject explosion = Explosion.Clone();
                    explosion.SetPosition(bullet.Position);
                    ExplosionsToAdd.Add(explosion);

                    bullet.Die();
                    ship.Damage(TurretData.BulletDamage);

                    return ship;
                }
            }

            return null;
        }

        public override GameObject CheckBulletsHitTargets(List<Ship> ships)
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
    }
}
