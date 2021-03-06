﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToGalaxyCustomData
{
    public class ShipModData : GameObjectData
    {
        public string ModFunctionName
        {
            get;
            set;
        }

        public string Type
        {
            get;
            set;
        }

        public bool Active
        {
            get;
            set;
        }

        public float RunTime
        {
            get;
            set;
        }

        public float Cooldown
        {
            get;
            set;
        }

        public int Cost
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }
    }
}
