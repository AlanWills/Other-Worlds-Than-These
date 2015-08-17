using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToGalaxy.Player_and_Session;
using ToGalaxyCustomData;
using ToGalaxyGameLibrary.Game_Objects;

namespace ToGalaxy.Gameplay_Objects.Space_Screen.Mission_GameObjects
{
    public class MissionExit : MissionInteractableObject
    {
        public MissionExit(Mission mission, MissionInteractableObjectData exitData)
            : base(mission, exitData)
        {

        }

        public override void OnInteract()
        {
            base.OnInteract();

            if (Mission.Completed)
            {
                // Add changing screen here
            }
        }
    }
}
