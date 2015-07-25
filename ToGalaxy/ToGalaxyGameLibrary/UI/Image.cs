using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToGalaxyGameLibrary.UI
{
    public class Image : UIElement
    {
        public bool PreserveAspectRatio
        {
            get;
            private set;
        }

        public Vector2 Dimensions
        {
            get;
            private set;
        }

        public Image(string dataAsset, Vector2 position, string name, float rotation = 0)
            : base(dataAsset, position, name, rotation)
        {

        }

        public Image(string dataAsset, Vector2 position, Vector2 scale, string name, float rotation = 0)
            : base(dataAsset, position, scale, name, rotation)
        {

        }

        public Image(string dataAsset, Vector2 position, Vector2 scale, Color colour, string name, float rotation = 0)
            : base(dataAsset, position, scale, colour, name, rotation)
        {

        }

        public Image(string dataAsset, Vector2 position, float width, float height, string name, bool preserveAspectRatio = true, float rotation = 0)
            : base(dataAsset, position, name, rotation)
        {
            PreserveAspectRatio = preserveAspectRatio;
            Dimensions = new Vector2(width, height);
        }

        public Image(string dataAsset, Vector2 position, float width, float height, Color colour, string name, bool preserveAspectRatio = true, float rotation = 0)
            : base(dataAsset, position, new Vector2(1, 1), colour, name, rotation)
        {
            PreserveAspectRatio = preserveAspectRatio;
            Dimensions = new Vector2(width, height);
        }

        public override void LoadContent(ContentManager content)
        {
            base.LoadContent(content);

            ScaleTexture();
        }

        private void ScaleTexture()
        {
            if (Texture != null && Dimensions.LengthSquared() != 0)
            {
                if (PreserveAspectRatio)
                {
                    float scale = (float)Math.Min(Dimensions.X / (float)Texture.Width, Dimensions.Y / (float)Texture.Height);
                    Scale = new Vector2(scale);
                }
                else
                {
                    Scale = new Vector2(Dimensions.X / (float)Texture.Width, Dimensions.Y / (float)Texture.Height);
                }
            }
        }

        public void SetTexture(Texture2D texture)
        {
            Texture = texture;

            ScaleTexture();
        }

        public void SetTextureFromString(string textureAsset, ContentManager content)
        {
            if (Data != null)
            {
                Data.TextureAsset = textureAsset;
            }

            try
            {
                Texture = content.Load<Texture2D>(textureAsset);

                ScaleTexture();
            }
            catch
            {

            }
        }
    }
}
