using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToGalaxyCustomData
{
    public class TurretData : GameObjectData
    {
        public string Type
        {
            get;
            set;
        }

        public float FireTimer
        {
            get;
            set;
        }

        public int Range
        {
            get;
            set;
        }

        public int BulletDamage
        {
            get;
            set;
        }

        public Vector2 BulletVelocity
        {
            get;
            set;
        }

        public float BulletLifeTime
        {
            get;
            set;
        }

        public string BulletAsset
        {
            get;
            set;
        }

        public string TurretSoundEffectAsset
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
