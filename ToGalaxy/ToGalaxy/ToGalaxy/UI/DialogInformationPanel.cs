using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToGalaxy.Screens.Gameplay_Screens;
using ToGalaxyCustomData;
using ToGalaxyGameLibrary.UI;

namespace ToGalaxy.UI
{
    public class DialogInformationPanel : DialogBox
    {
        private SpaceScreen SpaceScreen
        {
            get;
            set;
        }

        private SpaceScreenDialogData DialogData
        {
            get;
            set;
        }

        private List<string> AllStrings
        {
            get;
            set;
        }

        private int StringCounter
        {
            get;
            set;
        }

        private Button PreviousStringButton
        {
            get;
            set;
        }

        private Button NextStringButton
        {
            get;
            set;
        }

        public DialogInformationPanel(string dataAsset, string text, SpaceScreen spaceScreen, Vector2 position, Vector2 dimensions, Color colour, string name, float opacity = 0.5f, float lifeTime = float.MaxValue)
            : base(dataAsset, text, position, dimensions, colour, name, opacity, lifeTime)
        {
            AllStrings = new List<string>();
            StringCounter = 0;
            SpaceScreen = spaceScreen;
        }

        private void SetUpButtons()
        {
            PreviousStringButton = new Button(
                Button.defaultButtonAsset,
                new Vector2(-Dimensions.X * 0.3f, Dimensions.Y * 0.3f),
                Button.defaultColour,
                Button.highlightedColour,
                "Previous String Button",
                "Previous");
            PreviousStringButton.InteractEvent += PreviousStringEvent;
            LoadAndAddUIElement(PreviousStringButton);

            NextStringButton = new Button(
                Button.defaultButtonAsset,
                new Vector2(Dimensions.X * 0.3f, Dimensions.Y * 0.3f),
                Button.defaultColour,
                Button.highlightedColour,
                "Next String Button",
                "Next");
            NextStringButton.InteractEvent += NextStringEvent;
            LoadAndAddUIElement(NextStringButton);
        }

        private void PreviousStringEvent(object sender, EventArgs e)
        {
            SpaceScreen.PlayerShip.Select();

            if (StringCounter > 0)
            {
                StringCounter--;
                DialogBoxText.ChangeText(AllStrings[StringCounter]);
            }
        }

        private void NextStringEvent(object sender, EventArgs e)
        {
            SpaceScreen.PlayerShip.Select();

            if (StringCounter < AllStrings.Count - 1)
            {
                StringCounter++;
            }
        }

        public override void LoadContent(ContentManager content)
        {
            base.LoadContent(content);

            SetUpButtons();

            DialogData = content.Load<SpaceScreenDialogData>(String.Format("XML/Space Data/Dialog Data/Level{0}Dialog", SpaceScreen.ExtendedScreenManager.Session.CurrentLevel));
        }
    }
}
