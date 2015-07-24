using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToGalaxyGameLibrary.UI
{
    public class ConfirmationDialogBox : DialogBox
    {
        public Button ConfirmButton
        {
            get;
            private set;
        }

        public Button CancelButton
        {
            get;
            private set;
        }

        public ConfirmationDialogBox(string dataAsset, string text, Vector2 position, Vector2 dimensions, Color colour, string name, float opacity = 1f)
            : base(dataAsset, text, position, dimensions, colour, name, opacity)
        {
            ConfirmButton = new Button(
                "XML/UI/Buttons/MenuButton",
                new Vector2(-Dimensions.X / 4, Dimensions.Y / 5),
                new Color(0, 0.318f, 0.49f),
                new Color(0, 0.71f, 0.988f),
                "Confirm Button",
                "Confirm");

            CancelButton = new Button(
                "XML/UI/Buttons/MenuButton",
                new Vector2(Dimensions.X / 4, Dimensions.Y / 5),
                new Color(0.588f, 0, 0),
                new Color(1f, 0, 0),
                "Cancel Button",
                "Cancel");
        }

        public override void LoadContent(ContentManager content)
        {
            base.LoadContent(content);

            ConfirmButton.LoadContent(content);
            LoadAndAddUIElement(ConfirmButton, ConfirmButton.Position);

            CancelButton.LoadContent(content);
            LoadAndAddUIElement(CancelButton, CancelButton.Position);
        }
    }
}
