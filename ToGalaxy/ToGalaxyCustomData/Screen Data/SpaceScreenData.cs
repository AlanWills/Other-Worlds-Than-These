using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToGalaxyCustomData
{
    public class SpaceScreenData : ScreenData
    {
        public string Name
        {
            get;
            set;
        }

        public List<string> RandomEnemiesNames
        {
            get;
            set;
        }

        public float RandomEncounterProbability
        {
            get;
            set;
        }

        public List<string> EnemiesNames
        {
            get;
            set;
        }

        public List<Vector2> EnemiesPositions
        {
            get;
            set;
        }

        public List<string> RandomObjectsNames
        {
            get;
            set;
        }

        public List<float> RandomObjectsProbability
        {
            get;
            set;
        }

        public MissionData MissionData
        {
            get;
            set;
        }
    }
}
