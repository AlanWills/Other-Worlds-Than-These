using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToGalaxyCustomData
{
    public class MissionObjectData : GameObjectData
    {
        public SpaceScreenDialogData ObjectDialogData
        {
            get;
            set;
        }

        public Vector2 Position
        {
            get;
            set;
        }
    }
}
