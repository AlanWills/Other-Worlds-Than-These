using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToGalaxy.Player_and_Session;
using ToGalaxy.UI;
using ToGalaxyCustomData;

namespace ToGalaxy.Gameplay_Objects.Space_Screen.Mission_GameObjects
{
    public class MissionEnemy : EnemyShip
    {
        private Mission Mission
        {
            get;
            set;
        }

        public MissionEnemyData MissionEnemyData
        {
            get;
            private set;
        }

        public MissionEnemy(Mission mission, MissionEnemyData missionEnemyData)
            : base(missionEnemyData.EnemyDataAsset, missionEnemyData.Position, null)
        {
            Mission = mission;
            MissionEnemyData = missionEnemyData;

            for (int i = 0; i < missionEnemyData.ObjectDialogData.Dialog.Count; i++)
            {
                if (i < missionEnemyData.ObjectDialogData.TimeToShow.Count)
                {
                    DialogInformationPanel.AddString(missionEnemyData.ObjectDialogData.Dialog[i], missionEnemyData.ObjectDialogData.TimeToShow[i]);
                }
            }
        }

        public override void Die()
        {
            base.Die();

            Mission.MissionEnemies.Remove(this);

            for (int i = 0; i < MissionEnemyData.OnDestructionDialog.Dialog.Count; i++)
            {
                if (i < MissionEnemyData.OnDestructionDialog.TimeToShow.Count)
                {
                    DialogInformationPanel.AddStringFromCurrentTime(MissionEnemyData.ObjectDialogData.Dialog[i], MissionEnemyData.ObjectDialogData.TimeToShow[i]);
                }
            }
        }
    }
}
