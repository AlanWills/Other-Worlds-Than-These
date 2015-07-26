using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToGalaxyGameLibrary.Game_Objects;
using ToGalaxyGameLibrary.Maths;
using ToGalaxyGameLibrary.Screens_and_ScreenManager;

namespace ToGalaxy.Gameplay_Objects.Space_Screen
{
    public class BeamTurret : Turret
    {
        private Beam Beam
        {
            get;
            set;
        }

        private bool OnCooldown
        {
            get;
            set;
        }

        private float opacity = 0;
        private float beamFireTime = 0;
        private float beamDamageTimer = 0;

        public BeamTurret(string dataAsset, Ship parentShip, Vector2 hardPointLocation, Keys fireKey)
            : base(dataAsset, parentShip, hardPointLocation, fireKey)
        {
            
        }

        public override void SetUpSpawnPool()
        {
            Beam = new Beam(TurretData.BulletAsset, this, Position, Position, TurretData.BulletLifeTime);
            Beam.LoadContent(ScreenManager.Content);
            Beam.SetOpacity(opacity);
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

            if (Beam != null)
            {
                Beam.Update(gameTime);

                if (difference.Length() <= TurretData.Range)
                {
                    if (ParentShip.AutoTurrets)
                    {
                        if (Target == null || !Target.IsAlive())
                        {
                            ResetBeam(gameTime);
                        }
                        else
                        {
                            if (OnCooldown)
                            {
                                ResetBeam(gameTime);
                            }
                            else
                            {
                                if (beamFireTime <= Beam.BeamData.BeamFireTime)
                                {
                                    beamFireTime += (float)gameTime.ElapsedGameTime.Milliseconds / 1000f;
                                    FireBullet();
                                }
                                else
                                {
                                    OnCooldown = true;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (Mouse.GetState().RightButton == ButtonState.Pressed || ScreenManager.Input.IsKeyDown(FireKey))
                        {
                            if (OnCooldown)
                            {
                                ResetBeam(gameTime);
                            }
                            else
                            {
                                if (beamFireTime <= Beam.BeamData.BeamFireTime)
                                {
                                    beamFireTime += (float)gameTime.ElapsedGameTime.Milliseconds / 1000f;
                                    FireBullet();
                                }
                                else
                                {
                                    OnCooldown = true;
                                }
                            }
                        }
                        else
                        {
                            ResetBeam(gameTime);
                        }
                    }
                }

                if (Beam.Opacity > 0)
                {
                    TurretFiringSoundEffect.SetVolume(Beam.Opacity / 2 * ScreenManager.Settings.OptionsData.SoundEffectsVolume);
                    TurretFiringSoundEffect.Play();
                }
            }
        }

        private void ResetBeam(GameTime gameTime)
        {
            opacity -= (float)gameTime.ElapsedGameTime.Milliseconds / 1000f;
            opacity = Math.Max(0, opacity);
            Beam.SetOpacity(opacity);

            beamFireTime = 0;
            beamDamageTimer = 0;
            CurrentFireTimer += (float)gameTime.ElapsedGameTime.Milliseconds / 1000;
            OnCooldown = !CanFire;
        }

        protected override void FireBullet()
        {
            if (Beam != null)
            {
                if (Target != null)
                {
                    Beam.SetBulletTargetDestination(Target.Position);
                }
                else
                {
                    Beam.SetBulletTargetDestination(TargetLocation);
                }

                CurrentFireTimer = 0;

                opacity += 0.01f;
                opacity = Math.Min(opacity, 1);
                Beam.SetOpacity(opacity);
            }
        }

        public override GameObject CheckBulletsHitTarget(Ship ship)
        {
            if (Beam != null)
            {
                if (_2DGeometry.LineIntersectsRect(Position, Beam.Destination, ship.Bounds))
                {
                    Beam.SetBulletTargetDestination(ship.Position);

                    if (beamDamageTimer > 0.2f)
                    {
                        ship.Damage(TurretData.BulletDamage);
                        beamDamageTimer = 0;
                    }
                    else
                    {
                        beamDamageTimer += 0.01f;
                    }

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

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            if (Beam != null)
            {
                Beam.Draw(spriteBatch);
            }
        }
    }
}
