using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToGalaxyCustomData
{
    public class ButtonData : UIElementData
    {
        public string HighlightedTextureAsset
        {
            get;
            set;
        }

        public string PressedTextureAsset
        {
            get;
            set;
        }

        public string HighlightedSoundAsset
        {
            get;
            set;
        }

        public string PressedSoundAsset
        {
            get;
            set;
        }
    }
}
