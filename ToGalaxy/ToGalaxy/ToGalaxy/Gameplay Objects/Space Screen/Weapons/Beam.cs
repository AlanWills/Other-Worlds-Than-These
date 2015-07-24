using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToGalaxyCustomData;

namespace ToGalaxy.Gameplay_Objects.Space_Screen
{
    public class Beam : Bullet
    {
        #region Data

        public BeamData BeamData
        {
            get;
            private set;
        }

        #endregion

        private Texture2D FringeTexture
        {
            get;
            set;
        }

        private Color FringeColour
        {
            get;
            set;
        }

        private Turret ParentTurret
        {
            get;
            set;
        }

        public Beam(string dataAsset, Turret parentTurret, Vector2 destinationPoint, float lifeTimer)
            : base(dataAsset, destinationPoint, lifeTimer)
        {
            ParentTurret = parentTurret;
        }

        public Beam(string dataAsset, Turret parentTurret, Vector2 startingPoint, Vector2 destinationPoint, float lifeTimer)
            : base(dataAsset, startingPoint, destinationPoint, lifeTimer)
        {
            ParentTurret = parentTurret;
        }

        public override void LoadContent(ContentManager content)
        {
            base.LoadContent(content);

            if (DataAsset != "")
            {
                BeamData = content.Load<BeamData>(DataAsset);
                FringeTexture = content.Load<Texture2D>(BeamData.FringeTextureAsset);
                FringeColour = new Color(BeamData.FringeColour);
            }
        }

        public override void Update(GameTime gameTime)
        {
            SetPosition((ParentTurret.Position + Destination) / 2);

            Vector2 difference = Destination - Position;
            SetRotation((float)Math.Atan2(difference.X, -difference.Y));

            SetScale(new Vector2(ParentTurret.Scale.X / 3, (Destination - Position).Length() / (2 * Texture.Width)));
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            spriteBatch.Draw(FringeTexture, Position, null, FringeColour * Opacity, Rotation, Origin, Scale, SpriteEffects.None, 0);
        }
    }
}