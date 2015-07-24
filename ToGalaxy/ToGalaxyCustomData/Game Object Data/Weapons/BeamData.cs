using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToGalaxyCustomData
{
    public class BeamData : GameObjectData
    {
        public string BeamColour
        {
            get;
            set;
        }

        public string FringeTextureAsset
        {
            get;
            set;
        }

        public Vector4 FringeColour
        {
            get;
            set;
        }

        public float BeamFireTime
        {
            get;
            set;
        }
    }
}
