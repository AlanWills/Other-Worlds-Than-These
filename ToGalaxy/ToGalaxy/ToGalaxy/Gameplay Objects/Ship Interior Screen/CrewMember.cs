using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToGalaxyGameLibrary.Game_Objects;
using ToGalaxyGameLibrary.UI;

namespace ToGalaxy.Gameplay_Objects.Ship_Interior_Screen
{
    public class CrewMember : AnimatedGameObject
    {
        public Image SelectionUI
        {
            get;
            private set;
        }

        public CrewMember(string dataAsset, Vector2 startingPosition)
            : base(dataAsset, startingPosition)
        {
            
        }

        public override void LoadContent(ContentManager content)
        {
            base.LoadContent(content);

            // Again Position does not matter here as we will set it when the UI becomes active - when the player is selected
            // We are going to need to scale this based on the player ship's size
            SelectionUI = new Image(
                "Sprites/UI/Icons/SelectionIcon",
                Position,
                new Vector2(0.5f, 0.5f),
                "SelectionIcon");

            SelectionUI.LoadContent(content);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Selected)
            {
                SelectionUI.SetPosition(Position);
                SelectionUI.Draw(spriteBatch);
            }

            base.Draw(spriteBatch);
        }
    }
}
