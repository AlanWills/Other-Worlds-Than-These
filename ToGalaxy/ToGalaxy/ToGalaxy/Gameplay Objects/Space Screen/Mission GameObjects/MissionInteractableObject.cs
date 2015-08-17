using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToGalaxy.Player_and_Session;
using ToGalaxy.UI;
using ToGalaxyCustomData;
using ToGalaxyGameLibrary.Game_Objects;

namespace ToGalaxy.Gameplay_Objects.Space_Screen.Mission_GameObjects
{
    public class MissionInteractableObject : InteractableGameObject
    {
        protected Mission Mission
        {
            get;
            private set;
        }

        public MissionInteractableObjectData MissionObjectData
        {
            get;
            private set;
        }

        public MissionInteractableObject(Mission mission, MissionInteractableObjectData missionObjectData)
            : base(missionObjectData.TextureAsset, missionObjectData.Position)
        {
            Mission = mission;
            MissionObjectData = missionObjectData;

            for (int i = 0; i < missionObjectData.ObjectDialogData.Dialog.Count; i++)
            {
                if (i < missionObjectData.ObjectDialogData.TimeToShow.Count)
                {
                    DialogInformationPanel.AddString(missionObjectData.ObjectDialogData.Dialog[i], missionObjectData.ObjectDialogData.TimeToShow[i]);
                }
            }
        }

        public override void OnInteract()
        {
            base.OnInteract();

            for (int i = 0; i < MissionObjectData.OnInteractDialog.Dialog.Count; i++)
            {
                if (i < MissionObjectData.ObjectDialogData.TimeToShow.Count)
                {
                    DialogInformationPanel.AddStringFromCurrentTime(MissionObjectData.ObjectDialogData.Dialog[i], MissionObjectData.ObjectDialogData.TimeToShow[i]);
                }
            }
        }

        public override void Die()
        {
            base.Die();

            Mission.MustInteractObjects.Remove(this);
        }
    }
}
