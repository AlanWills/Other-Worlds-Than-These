using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToGalaxyCustomData
{
    public class EngineData : GameObjectData
    {
        public float EngineSpeed
        {
            get;
            set;
        }

        public float EngineRotateSpeed
        {
            get;
            set;
        }

        public string EngineSoundAsset
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
