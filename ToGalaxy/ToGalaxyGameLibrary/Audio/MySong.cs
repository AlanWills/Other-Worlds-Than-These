using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToGalaxyGameLibrary.Audio
{
    public class MySong
    {
        private string DataAsset
        {
            get;
            set;
        }

        public Song Song
        {
            get;
            private set;
        }

        public MySong(string dataAsset)
        {
            DataAsset = dataAsset;
        }

        public void LoadContent(ContentManager content)
        {
            Song = content.Load<Song>(DataAsset);
        }
    }
}
