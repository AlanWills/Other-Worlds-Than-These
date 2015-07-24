using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToGalaxyCustomData;
using ToGalaxyGameLibrary.Game_Objects;

namespace ToGalaxy.Gameplay_Objects.Space_Screen
{
    public class Sensor : GameObject
    {
        public SensorData SensorData
        {
            get;
            private set;
        }

        private GameObject ParentGameObject
        {
            get;
            set;
        }

        public Sensor(string dataAsset, GameObject parentGameObject)
            : base(dataAsset, parentGameObject.Position)
        {
            ParentGameObject = parentGameObject;
        }

        public override void LoadContent(ContentManager content)
        {
            base.LoadContent(content);

            SensorData = content.Load<SensorData>(DataAsset);

            float smallestDimension = Math.Min(ParentGameObject.Bounds.Width, ParentGameObject.Bounds.Height);
            Scale = new Vector2(Math.Min(0.3f * smallestDimension / Texture.Width, 1), Math.Min(0.3f * smallestDimension / Texture.Width, 1));
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            SetPosition(ParentGameObject.Position);
            Rotation += (float)gameTime.ElapsedGameTime.Milliseconds / 500;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Status == GameObjectStatus.Alive)
            {
                if (Texture != null)
                {
                    spriteBatch.Draw(Texture, Position, null, Colour * Opacity, Rotation, new Vector2(Texture.Width / 2, 3 * Texture.Height / 4), Scale, SpriteEffects.None, 0);
                }
            }
        }
    }
}
