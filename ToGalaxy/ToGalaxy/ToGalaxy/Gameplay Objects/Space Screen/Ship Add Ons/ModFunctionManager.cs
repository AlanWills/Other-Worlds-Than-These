using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToGalaxy.Gameplay_Objects;
using ToGalaxy.Gameplay_Objects.Space_Screen;
using ToGalaxyGameLibrary.Screens_and_ScreenManager;

namespace ToGalaxy
{
    public static class ModFunctionManager
    {
        public static Dictionary<string, Action<Ship, bool, float>> ModEvents
        {
            get;
            private set;
        }

        public static void SetUpEvents()
        {
            ModEvents = new Dictionary<string, Action<Ship, bool, float>>();

            ModEvents.Add("Increase Speed", IncreaseSpeedEvent);
            ModEvents.Add("Over Heat Turrets", OverHeatTurretsEvent);
            ModEvents.Add("Passive Turret Fire Rate Steroid", PassiveTurretFireRateEvent);
            ModEvents.Add("Auto Target Turrets", AutoTargetTurretsEvent);
            ModEvents.Add("Homing Missiles", HomingMissilesEvent);
            ModEvents.Add("Passive Increase Move and Rotate Speed", PassiveIncreaseMoveAndRotateSpeedsEvent);
            ModEvents.Add("Auto Movement", AutoMovementEvent);
            ModEvents.Add("Inertial Dampeners", InertialDampenersEvent);
            ModEvents.Add("Passive Shield Strength Increase", PassiveIncreaseShieldStrengthEvent);
            ModEvents.Add("Shield Strength Increase", IncreaseShieldStrengthEvent);
            ModEvents.Add("Passive Shield Restore Rate Increase", PassiveIncreaseShieldRestoreRate);
            ModEvents.Add("Passive Sensor Range Increase", PassiveIncreaseSensorRange);
            ModEvents.Add("Emergency Temporal Shift", EmergencyTemporalShiftEvent);
        }

        #region Offensive

        private static void OverHeatTurretsEvent(Ship ship, bool finishedRunning, float timeSinceActivation)
        {
            foreach (Turret turret in ship.Turrets)
            {
                if (!finishedRunning)
                {
                    turret.FireTimerMultiplier = 5f;
                }
                else if (timeSinceActivation < 10)
                {
                    turret.FireTimerMultiplier = 0.5f;
                }
                else
                {
                    turret.FireTimerMultiplier = 1f;
                }
            }
        }

        private static void PassiveTurretFireRateEvent(Ship ship, bool finishedRunning, float timeSinceActivation)
        {
            foreach (Turret turret in ship.Turrets)
            {
                turret.FireTimerMultiplier = 1.25f;
            }
        }

        private static void AutoTargetTurretsEvent(Ship ship, bool finishedRunning, float timeSinceActivation)
        {
            if (!finishedRunning)
            {
                ship.AutoTurrets = !ship.AutoTurrets;
            }
        }

        private static void HomingMissilesEvent(Ship ship, bool finishedRunning, float timeSinceActivation)
        {
            if (!finishedRunning)
            {
                foreach (MissileTurret turret in ship.Turrets)
                {
                    turret.Homing = !turret.Homing;
                }
            }
        }

        #endregion

        #region Defensive

        private static void PassiveIncreaseShieldStrengthEvent(Ship ship, bool finishedRunning, float timeSinceActivation)
        {
            if (ship.Shield != null)
            {
                ship.Shield.ShieldStrengthMultiplier = 1.2f;
            }
        }

        private static void PassiveIncreaseShieldRestoreRate(Ship ship, bool finishedRunning, float timeSinceActivation)
        {
            if (ship.Shield != null)
            {
                ship.Shield.regenTimer *= 1.2f;
                ship.Shield.chargeDelayTimer *= 0.9f;
                ship.Shield.timeSinceDamageTaken *= 1.2f;
            }
        }

        private static void IncreaseShieldStrengthEvent(Ship ship, bool finishedRunning, float timeSinceActivation)
        {
            if (ship.Shield != null)
            {
                if (!finishedRunning)
                {
                    ship.Shield.ShieldStrength += (int)(timeSinceActivation * ship.Shield.ShieldData.Strength / 20f);
                    ship.Shield.ShieldStrength = Math.Min(ship.Shield.ShieldStrength, ship.Shield.ShieldData.Strength);
                }
            }
        }

        #endregion

        #region Utility

        private static void IncreaseSpeedEvent(Ship ship, bool finishedRunning, float timeSinceActivation)
        {
            if (!finishedRunning)
            {
                ship.SpeedMultiplier = 3f;
            }
            else
            {
                ship.SpeedMultiplier = 1f;
            }
        }

        private static void PassiveIncreaseMoveAndRotateSpeedsEvent(Ship ship, bool finishedRunning, float timeSinceActivation)
        {
            ship.SpeedMultiplier = 1.25f;
            ship.RotateSpeedMultiplier = 1.25f;
        }

        private static void AutoMovementEvent(Ship ship, bool finishedRunning, float timeSinceActivation)
        {
            if (!finishedRunning)
            {
                ship.ManualSteering = !ship.ManualSteering;
            }
        }

        private static void InertialDampenersEvent(Ship ship, bool finishedRunning, float timeSinceActivation)
        {
            if (!finishedRunning)
            {
                ship.InterialDampeners = !ship.InterialDampeners;
            }
        }

        private static void PassiveIncreaseSensorRange(Ship ship, bool finishedRunning, float timeSinceActivation)
        {
            if (ship.Sensors != null)
            {
                ship.Sensors.RangeMultiplier = 1.5f;
            }
        }

        private static void EmergencyTemporalShiftEvent(Ship ship, bool finishedRunning, float timeSinceActivation)
        {
            if (!finishedRunning)
            {
                Random random = new Random();

                ship.SetPosition(new Vector2(random.Next(-3000, 3000), random.Next(-3000, 3000)));
                ship.SetDestination(ship.Position);
            }
        }

        #endregion
    }
}
