using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToGalaxyCustomData
{
    public class AnimatedGameObjectData : GameObjectData
    {
        public float TimePerFrame
        {
            get;
            set;
        }

        // Number of images in each direction of the sprite sheet
        public Vector2 SpriteSheetFrameDimensions
        {
            get;
            set;
        }

        public int DefaultFrame
        {
            get;
            set;
        }
    }
}
