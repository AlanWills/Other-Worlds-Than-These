using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToGalaxy.Gameplay_Objects;

namespace ToGalaxy
{
    public static class ModFunctionManager
    {
        public static Dictionary<string, Func<Ship, bool>> ModEvents
        {
            get;
            private set;
        }

        public static void SetUpEvents()
        {
            ModEvents = new Dictionary<string, Func<Ship, bool>>();

            ModEvents.Add("Increase Speed", IncreaseSpeedEvent);
        }

        private static bool IncreaseSpeedEvent(Ship playerShip)
        {
            playerShip.IncreaseVelocity(new Vector2(0, 0.1f) * playerShip.Speed);

            return true;
        }
    }
}
