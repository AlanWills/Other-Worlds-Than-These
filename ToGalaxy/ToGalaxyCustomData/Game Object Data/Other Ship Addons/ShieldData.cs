using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToGalaxyCustomData
{
    public class ShieldData : GameObjectData
    {
        public int Strength
        {
            get;
            set;
        }

        // How much shield strength is recharged per second
        public float RechargePerSecond
        {
            get;
            set;
        }

        // WHen damaged the shields will not begin recharging unless damage has not been taken for this amount of time
        public float RechargeDelay
        {
            get;
            set;
        }

        // How long the shields take to be fully restored when depleted fully
        public float DepletionDelay
        {
            get;
            set;
        }

        // Colour of the shield
        public Vector4 Colour
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
