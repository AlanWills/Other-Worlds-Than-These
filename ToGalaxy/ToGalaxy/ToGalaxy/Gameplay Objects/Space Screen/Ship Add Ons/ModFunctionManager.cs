using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToGalaxy.Gameplay_Objects;
using ToGalaxy.Gameplay_Objects.Space_Screen;

namespace ToGalaxy
{
    public static class ModFunctionManager
    {
        public static Dictionary<string, Action<Ship, bool>> ModEvents
        {
            get;
            private set;
        }

        public static void SetUpEvents()
        {
            ModEvents = new Dictionary<string, Action<Ship, bool>>();

            ModEvents.Add("Increase Speed", IncreaseSpeedEvent);
            ModEvents.Add("Over Heat Turrets", OverHeatTurretsEvent);
            ModEvents.Add("Passive Turret Fire Rate Steroid", PassiveTurretFireRateEvent);
        }

        private static void IncreaseSpeedEvent(Ship ship, bool finishedRunning)
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

        public static void OverHeatTurretsEvent(Ship ship, bool finishedRunning)
        {
            foreach (Turret turret in ship.Turrets)
            {
                if (!finishedRunning)
                {
                    turret.FireTimerMultiplier = 5f;
                }
                else
                {
                    turret.FireTimerMultiplier = 0.2f;
                }
            }
        }

        public static void PassiveTurretFireRateEvent(Ship ship, bool finishedRunning)
        {
            foreach (Turret turret in ship.Turrets)
            {
                turret.FireTimerMultiplier = 1.25f;
            }
        }
    }
}
