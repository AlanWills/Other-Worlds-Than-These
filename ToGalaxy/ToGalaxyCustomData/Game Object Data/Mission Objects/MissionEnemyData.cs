using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToGalaxyCustomData
{
    public class MissionEnemyData : MissionObjectData
    {
        public string EnemyDataAsset
        {
            get;
            set;
        }

        public SpaceScreenDialogData OnDestructionDialog
        {
            get;
            set;
        }
    }
}