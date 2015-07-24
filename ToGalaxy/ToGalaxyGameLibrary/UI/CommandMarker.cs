using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToGalaxyGameLibrary.UI
{
    public class CommandMarker : Image
    {
        #region Timers for Removal of Marker

        // LifeTime measure in milliseconds for ease with rounding issues
        private float LifeTime
        {
            get;
            set;
        }

        private float CurrentTimeAlive = 0f;

        #endregion

        public CommandMarker(string dataAsset, Vector2 position, string name)
            : base(dataAsset, position, name)
        {
            LifeTime = 500f;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (IsActive())
            {
                CurrentTimeAlive += Convert.ToInt32(gameTime.ElapsedGameTime.Milliseconds);
                Opacity = 1 - (CurrentTimeAlive / LifeTime);

                if (CurrentTimeAlive > LifeTime)
                {
                    State = UIState.Dead;
                }
            }
        }
    }
}
