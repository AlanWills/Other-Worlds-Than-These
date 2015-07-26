using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToGalaxy.AI;
using ToGalaxy.Gameplay_Objects.Space_Screen;
using ToGalaxy.UI;
using ToGalaxyCustomData;
using ToGalaxyGameLibrary.Audio;
using ToGalaxyGameLibrary.Game_Objects;
using ToGalaxyGameLibrary.Screens_and_ScreenManager;
using ToGalaxyGameLibrary.UI;

namespace ToGalaxy.Gameplay_Objects
{
    public class Ship : GameObject
    {
        #region Data

        public ShipData ShipData
        {
            get;
            private set;
        }

        #endregion

        #region Piloting and Steering Properties - Destination, etc...

        public bool ManualSteering
        {
            get;
            set;
        }

        public bool InterialDampeners
        {
            get;
            set;
        }

        public Vector2 Destination
        {
            get;
            protected set;
        }

        public bool DestinationReached
        {
            get
            {
                if (!ManualSteering)
                {
                    return (Destination - Position).LengthSquared() < 10 ? true : false;
                }
                else
                {
                    // If we are moving we have NOT reached our destination using manual steering
                    return !Moving;
                }
            }
        }

        public MovementBehaviourStateMachine Movement
        {
            get;
            protected set;
        }

        #endregion

        #region Armour, Shields, Hull and Energy

        public Shield Shield
        {
            get;
            private set;
        }

        public int Armour
        {
            get;
            protected set;
        }

        public int Hull
        {
            get;
            protected set;
        }

        #endregion

        #region Turrets

        public List<Turret> Turrets
        {
            get;
            private set;
        }

        public int MinimumTurretRange
        {
            get
            {
                if (Turrets.Count == 0)
                {
                    return 0;
                }
                else
                {
                    int minTurretRange = Turrets[0].TurretData.Range;
                    foreach (Turret turret in Turrets)
                    {
                        if (turret.TurretData.Range < minTurretRange)
                            minTurretRange = turret.TurretData.Range;
                    }

                    return minTurretRange;
                }
            }
        }

        public bool AutoTurrets
        {
            get;
            set;
        }

        #endregion

        #region Extra Decals - Mouse Over UI

        public ShipMouseOverUI ShipMouseOverUI
        {
            get;
            private set;
        }

        #endregion

        #region Engines Information

        public List<Engine> Engines
        {
            get;
            private set;
        }

        public float Speed
        {
            get
            {
                float delta = 0;
                foreach (Engine engine in Engines)
                {
                    delta += engine.Speed;
                }

                // Speed is affected by the ships mass - 1000 tonnes provides the engines written functioning capacity, 
                // less or more will increase/detriment performance
                return SpeedMultiplier * delta * 1000f / ShipData.Mass;
            }
        }

        public float SpeedMultiplier
        {
            get;
            set;
        }

        public float RotateSpeed
        {
            get
            {
                float rotateDelta = 0;
                foreach (Engine engine in Engines)
                {
                    rotateDelta += engine.RotateSpeed;
                }

                // RotateSpeed is affected by the ships mass - 1000 tonnes provides the engines written functioning capacity, 
                // less or more will increase/detriment performance
                return rotateDelta * 1000f / ShipData.Mass;
            }
        }

        public float RotateSpeedMultiplier
        {
            get;
            set;
        }

        #endregion

        #region Mods

        public List<ShipMod> ShipMods
        {
            get;
            private set;
        }

        #endregion

        public Sensor Sensors
        {
            get;
            private set;
        }

        public int TotalWorth
        {
            get
            {
                int totalWorth = 0;
                if (ShipData != null)
                {
                    totalWorth += ShipData.Cost;
                    foreach (Turret turret in Turrets)
                    {
                        totalWorth += turret.TurretData.Cost;
                    }

                    foreach (Engine engine in Engines)
                    {
                        totalWorth += engine.EngineData.Cost;
                    }

                    if (Shield != null)
                    {
                        totalWorth += Shield.ShieldData.Cost;
                    }

                    if (Sensors != null)
                    {
                        totalWorth += Sensors.SensorData.Cost;
                    }
                }

                return totalWorth;
            }
        }

        public Ship(string dataAsset, Vector2 startingPosition)
            : base(dataAsset)
        {
            Position = startingPosition;
            Destination = startingPosition;

            ManualSteering = false;
            AutoTurrets = true;
            InterialDampeners = true;
            SpeedMultiplier = 1;
            RotateSpeedMultiplier = 1;

            Turrets = new List<Turret>();
            Engines = new List<Engine>();
            ShipMods = new List<ShipMod>();
            Movement = new MovementBehaviourStateMachine(this);
        }

        public void SetUpShipUI(ContentManager content)
        {
            ShipMouseOverUI = new ShipMouseOverUI(
                this,
                "",
                Position,
                Vector2.Zero,
                Color.White,
                "Ship Mouse Over UI");
            ShipMouseOverUI.LoadContent(content);
        }

        public override void LoadContent(ContentManager content)
        {
            base.LoadContent(content);

            if (DataAsset != "")
            {
                ShipData = content.Load<ShipData>(DataAsset);
            }

            if (ShipData != null)
            {
                LoadTurrets(content);
                LoadEngines(content);
                LoadShield(content);
                LoadSensors(content);
                LoadShipMods(content);

                Armour = ShipData.Armour;
                Hull = ShipData.Hull;
            }
        }

        public void LoadTurrets(ContentManager content)
        {
            Turrets.Clear();

            List<Keys> weaponKeys = new List<Keys>() { Keys.D1, Keys.D2, Keys.D3, Keys.D4, Keys.D5, Keys.D6, Keys.D7, Keys.D8, Keys.D9, Keys.D0, Keys.OemMinus, Keys.OemPlus };
            for (int i = 0; i < ShipData.TurretNames.Count; i++)
            {
                if (i < ShipData.WeaponHardPoints.Count)
                {
                    if (ShipData.TurretNames[i] != "")
                    {
                        Turret turret = new Turret("XML/Weapons/" + ShipData.TurretNames[i], this, ShipData.WeaponHardPoints[i], weaponKeys[i]);
                        turret.LoadContent(content);

                        if (turret.TurretData.Type == "Missile")
                        {
                            turret = new MissileTurret(turret.DataAsset, this, ShipData.WeaponHardPoints[i], weaponKeys[i]);
                            turret.LoadContent(content);
                        }
                        else if (turret.TurretData.Type == "Beam")
                        {
                            turret = new BeamTurret(turret.DataAsset, this, ShipData.WeaponHardPoints[i], weaponKeys[i]);
                            turret.LoadContent(content);
                        }

                        Turrets.Add(turret);
                    }
                }
            }
        }

        public void LoadEngines(ContentManager content)
        {
            Engines.Clear();

            for (int i = 0; i < ShipData.EngineNames.Count; i++)
            {
                if (i < ShipData.EngineHardPoints.Count)
                {
                    if (ShipData.EngineNames[i] != "")
                    {
                        Engine engine = new Engine("XML/Engines/" + ShipData.EngineNames[i], this, ShipData.EngineHardPoints[i]);
                        engine.LoadContent(content);

                        Engines.Add(engine);
                    }
                }
            }
        }

        public void LoadShield(ContentManager content)
        {
            if (ShipData.ShieldAsset != "")
            {
                Shield = new Shield("XML/Shields/" + ShipData.ShieldAsset, Position, this);
                Shield.LoadContent(content);
            }
        }

        public void LoadSensors(ContentManager content)
        {
            if (ShipData.SensorName != "")
            {
                Sensors = new Sensor("XML/Sensors/" + ShipData.SensorName, this);
                Sensors.LoadContent(content);
            }
        }

        public virtual void LoadShipMods(ContentManager content)
        {
            if (ShipData.ShipModSlots > 0)
            {
                ShipMods.Clear();

                for (int i = 0; i < ShipData.ShipModNames.Count; i++)
                {
                    if (i < ShipData.ShipModSlots)
                    {
                        ShipMod shipMod = new ShipMod("XML/Ship Mods/" + ShipData.ShipModNames[i], Position, this);
                        shipMod.LoadContent(content);

                        ShipMods.Add(shipMod);
                    }
                }
            }
        }

        public void SetUpTurretSpawnPools()
        {
            foreach(Turret turret in Turrets)
            {
                turret.SetUpSpawnPool();
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (Hull <= 0)
            {
                Die();
            }

            if (!ManualSteering)
            {
                Movement.Update();
                PerformAutomaticPiloting(gameTime);
            }

            if (InterialDampeners)
            {
                Velocity *= 0.97f;
            }

            foreach (Engine engine in Engines)
            {
                engine.Update(gameTime);
            }

            foreach (Turret turret in Turrets)
            {
                turret.Update(gameTime);

                if (!AutoTurrets)
                {
                    ManualTurretTargeting();
                }
            }

            if (Shield != null)
            {
                Shield.Update(gameTime);
            }

            if (Sensors != null)
            {
                Sensors.Update(gameTime);
            }

            foreach (ShipMod shipMod in ShipMods)
            {
                shipMod.Update(gameTime);
            }

            if (ShipMouseOverUI != null)
            {
                ShipMouseOverUI.Update(gameTime);
            }

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            DrawShipUI(spriteBatch);

            foreach (Engine engine in Engines)
            {
                engine.Draw(spriteBatch);
            }

            base.Draw(spriteBatch);

            if (Shield != null)
            {
                Shield.Draw(spriteBatch);
            }

            foreach (Turret turret in Turrets)
            {
                turret.Draw(spriteBatch);
            }

            // The ship is moving
            if (Moving)
            {
                foreach (Engine engine in Engines)
                {
                    engine.Draw(spriteBatch);
                }
            }

            if (Sensors != null)
            {
                Sensors.Draw(spriteBatch);
            }
        }

        private void DrawShipUI(SpriteBatch spriteBatch)
        {
            if (ShipMouseOverUI != null)
            {
                ShipMouseOverUI.Draw(spriteBatch);
            }
        }

        #region Automatic Piloting Functions

        private void PerformAutomaticPiloting(GameTime gameTime)
        {
            Vector2 difference = Destination - Position;
            float desiredAngle;

            // This stops the ship rotating back to angle 0 when it has reached it's destination
            if (difference.Length() >= 2 * RotateSpeed)
            {
                desiredAngle = (float)Math.Atan2(difference.X, -difference.Y);
            }
            else
            {
                desiredAngle = Rotation;
            }

            desiredAngle %= (float)MathHelper.Pi;

            // Rotates to desired angle
            if (Math.Abs(desiredAngle - Rotation) < 2 * RotateSpeed)
            {
                Rotation = desiredAngle;
            }
            else
            {
                // Removing this line means the ship won't stop moving to rotate to new destination - I think this is more realistic
                // Velocity = Vector2.Zero;
                // Change the coordinates so we always assume rotation is 0
                // Only interested in the offset between desiredAngle and Rotation to work out which way to go
                float zeroedDesiredAngle = desiredAngle - Rotation;
                if (zeroedDesiredAngle < -MathHelper.Pi)
                {
                    zeroedDesiredAngle += MathHelper.TwoPi;
                }
                if (zeroedDesiredAngle > MathHelper.Pi)
                {
                    zeroedDesiredAngle -= MathHelper.Pi;
                }

                if (zeroedDesiredAngle > 0)
                {
                    Rotation += RotateSpeed;
                }
                else
                {
                    Rotation -= RotateSpeed;
                }

                // Make sure Rotation is in the bounds -Pi to Pi for comparison with the desiredAngle above
                if (Rotation > MathHelper.Pi)
                {
                    Rotation -= MathHelper.TwoPi;
                }
                if (Rotation < -MathHelper.Pi)
                {
                    Rotation += MathHelper.TwoPi;
                }
            }
            
            // Handles the movement of the ship
            if (DestinationReached)
            {
                // Ship is stationary at it's destination
                Position = Destination;
                Stop();
            }
            else
            {
                // Ship is moving
                Velocity += new Vector2(0, (float)gameTime.ElapsedGameTime.Milliseconds / 1000f) * Speed;
                Moving = true;
            }
        }

        #endregion

        public void SetDestination(Vector2 position)
        {
            Destination = position;
        }

        public void Stop()
        {
            Velocity = Vector2.Zero;
            Moving = false;
        }

        public void Damage(int damage)
        {
            // Ship has a shield
            if (Shield != null)
            {
                // Shield can absorb all damage
                if (Shield.ShieldStrength >= damage)
                {
                    Shield.Damage(damage);
                }
                // Shield can only partially absorb the damage
                else
                {
                    int remainingDamage = damage - Shield.ShieldStrength;
                    Shield.Damage(Shield.ShieldStrength);

                    if (Armour >= remainingDamage)
                    {
                        Armour -= remainingDamage;
                    }
                    else
                    {
                        int remainingDamage2 = remainingDamage - Armour;

                        Armour = 0;
                        Hull -= remainingDamage2;
                    }
                }
            }
            // Ship doesn't have a shield
            else
            {
                if (Armour >= damage)
                {
                    Armour -= damage;
                }
                else
                {
                    int remainingDamage = damage - Armour;

                    Armour = 0;
                    Hull -= remainingDamage;
                }
            }

            if (Hull <= 0)
            {
                Die();
            }
        }

        #region Turret Targeting and Bullet Collisions

        private void ManualTurretTargeting()
        {
            foreach (Turret turret in Turrets)
            {
                turret.SetTargetLocation(InGameMouse.InGamePosition);
            }
        }

        public void SetTarget(Ship target)
        {
            if (!ManualSteering)
            {
                Movement.SetTarget(target);
            }
        }

        public void SetTurretTarget(Ship target)
        {
            foreach (Turret turret in Turrets)
            {
                turret.SetTarget(target);
            }
        }

        public void ClearTarget()
        {
            Movement.ClearTarget();
        }

        public void ClearTurretTargets()
        {
            foreach (Turret turret in Turrets)
            {
                turret.ClearTarget();
            }
        }

        public void FindMovementTarget(List<Ship> objects)
        {
            if (!ManualSteering && Movement.TargetShip == null)
            {
                if (objects.Count > 0)
                {
                    Ship targetShip = null;

                    foreach (Ship ship in objects)
                    {
                        float range = ship.MinimumTurretRange;
                        if (Sensors != null && Sensors.SensorData.Range * Sensors.RangeMultiplier > ship.MinimumTurretRange)
                        {
                            range = Sensors.SensorData.Range * Sensors.RangeMultiplier;
                        }

                        if ((Position - ship.Position).Length() <= range)
                        {
                            if (targetShip != null)
                            {
                                if ((Position - ship.Position).Length() < (Position - targetShip.Position).Length())
                                {
                                    targetShip = ship;
                                }
                            }
                            else
                            {
                                targetShip = ship;
                            }
                        }
                    }

                    if (targetShip != null)
                    {
                        SetTarget(targetShip);
                    }
                    else
                    {
                        ClearTarget();
                    }
                }
                else
                {
                    ClearTarget();
                }
            }
            else
            {
                ClearTarget();
            }
        }

        public void FindTurretTarget(List<Ship> objects)
        {
            if (AutoTurrets)
            {
                if (objects.Count > 0)
                {
                    Ship targetShip = null;

                    foreach (Ship ship in objects)
                    {
                        float range = ship.MinimumTurretRange;
                        if (Sensors != null && Sensors.SensorData.Range * Sensors.RangeMultiplier > ship.MinimumTurretRange)
                        {
                            range = Sensors.SensorData.Range * Sensors.RangeMultiplier;
                        }

                        if ((Position - ship.Position).Length() <= range)
                        {
                            if (targetShip != null)
                            {
                                if ((Position - ship.Position).Length() < (Position - targetShip.Position).Length())
                                {
                                    targetShip = ship;
                                }
                            }
                            else
                            {
                                targetShip = ship;
                            }
                        }
                    }

                    if (targetShip != null)
                    {
                        SetTurretTarget(targetShip);
                    }
                    else
                    {
                        ClearTurretTargets();
                    }
                }
                else
                {
                    ClearTurretTargets();
                }
            }
            else
            {
                ClearTurretTargets();
            }
        }

        public void CheckTargets(List<Ship> shipsToCheck)
        {
            // If our Movement State Machine had a target, but it no longer exists the ship should stop and become idle if no move order is locked in.
            if (Movement.TargetShip != null && !shipsToCheck.Contains(Movement.TargetShip))
            {
                Movement.Stop();
            }

            foreach (Turret turret in Turrets)
            {
                if (!shipsToCheck.Contains(turret.Target))
                {
                    turret.ClearTarget();
                }
            }
        }

        public bool CheckBulletCollisionsWithTarget(Ship ship)
        {
            bool bulletHit = false;

            foreach (Turret turret in Turrets)
            {
                if (turret.CheckBulletsHitTarget(ship) != null)
                {
                    bulletHit = true;
                }
            }

            return bulletHit;
        }

        public bool CheckBulletCollisionsWithTargets(List<Ship> ships)
        {
            bool bulletHit = false;

            foreach (Turret turret in Turrets)
            {
                Ship hitShip = turret.CheckBulletsHitTargets(ships) as EnemyShip;
                if (hitShip != null)
                {
                    bulletHit = true;
                    // hitShip.SetTarget(this);
                }
            }

            return bulletHit;
        }

        #endregion

        public override void Die()
        {
            base.Die();

            foreach (Engine engine in Engines)
            {
                engine.EngineSoundEffect.Stop();
            }
        }
    }
}
