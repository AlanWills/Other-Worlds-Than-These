using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToGalaxyGameLibrary.UI
{
    public class DialogBox : Panel
    {
        public Text DialogBoxText
        {
            get;
            private set;
        }

        public float LifeTime
        {
            get;
            private set;
        }

        private float timeAlive = 0;

        public DialogBox(string dataAsset, string text, Vector2 position, Vector2 dimensions, Color colour, string name, float opacity = 1f, float lifeTime = 0)
            : base(dataAsset, position, dimensions, colour, name, opacity)
        {
            DialogBoxText = new Text(
                text,
                new Vector2(0, 0),
                9 * dimensions.X / 10,
                Color.White,
                name + " Text");

            LifeTime = lifeTime;
        }

        public override void LoadContent(ContentManager content)
        {
            base.LoadContent(content);

            LoadAndAddUIElement(DialogBoxText, DialogBoxText.Position);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // A lifetime of 0 represents we wish the box to last forever
            if (LifeTime > 0)
            {
                timeAlive += (float)gameTime.ElapsedGameTime.Milliseconds / 1000f;
                if (timeAlive >= LifeTime)
                {
                    Die();
                }
            }
        }
    }
}
