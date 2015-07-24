using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToGalaxyCustomData
{
    public class MissileData : GameObjectData
    {
        public string MissileEngineAsset
        {
            get;
            set;
        }

        public Vector2 MissileEngineHardpoint
        {
            get;
            set;
        }
    }
}
