using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToGalaxyGameLibrary.Game_Objects;
using ToGalaxyGameLibrary.Screens_and_ScreenManager;

namespace ToGalaxyGameLibrary.UI
{
    public class MouseOverUI : Panel
    {
        private object ParentObject
        {
            get;
            set;
        }

        public MouseOverUI(object parentObject, string dataAsset, Vector2 position, Vector2 dimensions, Color colour, string name, float opacity = 0.25f)
            : base(dataAsset, position, dimensions, colour, name, opacity)
        {
            ParentObject = parentObject;
        }
    }
}
