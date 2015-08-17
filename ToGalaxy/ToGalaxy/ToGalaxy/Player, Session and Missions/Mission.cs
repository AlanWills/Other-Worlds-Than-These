using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToGalaxy.Gameplay_Objects.Space_Screen.Mission_GameObjects;
using ToGalaxyCustomData;

namespace ToGalaxy.Player_and_Session
{
    public enum MissionType { DestroyTarget, EscapeEnemies, SurviveAttack, CaptureObject }

    public class Mission
    {
        #region Data

        public MissionData MissionData
        {
            get;
            private set;
        }

        #endregion

        #region Mission Objects and Enemies

        public List<MissionEnemy> MissionEnemies
        {
            get;
            private set;
        }

        public List<MissionInteractableObject> MustInteractObjects
        {
            get;
            private set;
        }

        public MissionInteractableObject Exit
        {
            get;
            private set;
        }

        #endregion

        public bool Completed
        {
            get { return MissionEnemies.Count == 0 && MustInteractObjects.Count == 0; }
        }

        public Mission(MissionData missionData)
        {
            MissionData = missionData;
            MissionEnemies = new List<MissionEnemy>();
            MustInteractObjects = new List<MissionInteractableObject>();
        }

        public void LoadContent(ContentManager content)
        {
            int counter = 0;
            foreach (MissionEnemyData enemy in MissionData.EnemiesToKill)
            {
                MissionEnemy missionEnemy = new MissionEnemy(this, enemy);
                MissionEnemies.Add(missionEnemy);
                counter++;
            }

            counter = 0;
            foreach (MissionInteractableObjectData interactableObject in MissionData.MustInteractObjects)
            {
                MissionInteractableObject missionObject = new MissionInteractableObject(this, interactableObject);
                MustInteractObjects.Add(missionObject);
                counter++;
            }

            if (MissionData.ExitObject.TextureAsset != "")
            {
                Exit = new MissionInteractableObject(this, MissionData.ExitObject);
            }
        }
    }
}
