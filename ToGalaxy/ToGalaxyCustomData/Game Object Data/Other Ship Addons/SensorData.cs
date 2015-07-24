using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToGalaxyCustomData
{
    public class SensorData : GameObjectData
    {
        // For player used to show enemy on radar
        // For enemy, used to find player
        public int Range
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }

        public int Cost
        {
            get;
            set;
        }
    }
}
