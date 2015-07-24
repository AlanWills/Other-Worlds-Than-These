using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToGalaxyCustomData
{
    public class ShipData : GameObjectData
    {
        public List<Vector2> WeaponHardPoints
        {
            get;
            set;
        }

        public List<Vector2> EngineHardPoints
        {
            get;
            set;
        }

        public List<string> TurretNames
        {
            get;
            set;
        }

        public string ShieldAsset
        {
            get;
            set;
        }

        public int Armour
        {
            get;
            set;
        }

        public int Hull
        {
            get;
            set;
        }

        public float Mass
        {
            get;
            set;
        }

        public List<string> EngineNames
        {
            get;
            set;
        }

        public int ShipModSlots
        {
            get;
            set;
        }

        public List<string> ShipModNames
        {
            get;
            set;
        }

        public string SensorName
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
