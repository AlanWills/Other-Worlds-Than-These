using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToGalaxyCustomData
{
    public class MissionData
    {
        public string MissionType
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }

        public string MissionThumbnailAsset
        {
            get;
            set;
        }

        public List<MissionEnemyData> EnemiesToKill
        {
            get;
            set;
        }

        public List<MissionInteractableObjectData> MustInteractObjects
        {
            get;
            set;
        }

        public MissionInteractableObjectData ExitObject
        {
            get;
            set;
        }
    }
}
