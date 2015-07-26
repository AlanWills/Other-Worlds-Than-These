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

        public List<string> PresetObjectsNames
        {
            get;
            set;
        }

        public List<Vector2> PresetObjectsPositions
        {
            get;
            set;
        }

        public string Exit
        {
            get;
            set;
        }

        public List<string> MustActivateObjects
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }

        public string LevelThumbnailAsset
        {
            get;
            set;
        }
    }
}
