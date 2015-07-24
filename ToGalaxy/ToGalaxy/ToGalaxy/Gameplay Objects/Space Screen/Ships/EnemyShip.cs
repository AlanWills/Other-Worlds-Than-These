using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToGalaxy.Gameplay_Objects.Space_Screen
{
    public class EnemyShip : Ship
    {
        public EnemyShip(string dataAsset, Vector2 startingPosition, Ship playerShip)
            : base(dataAsset, startingPosition)
        {
            if (playerShip != null)
            {
                Movement.SetTarget(playerShip);
            }
        }
    }
}
