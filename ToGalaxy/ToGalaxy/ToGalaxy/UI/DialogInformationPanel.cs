using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToGalaxy.Screens.Gameplay_Screens;
using ToGalaxyCustomData;
using ToGalaxyGameLibrary;
using ToGalaxyGameLibrary.UI;

namespace ToGalaxy.UI
{
    public class DialogInformationPanel : DialogBox
    {
        #region Data

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

        #endregion

        private SortedList<float, string> AllStrings
        {
            get;
            set;
        }

        private int CurrentStringCounter
        {
            get;
            set;
        }

        private int MaxUnlockedStringCounter
        {
            get;
            set;
        }

        #region Extra UI

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

        #endregion

        private float totalGameTime = 0;

        public DialogInformationPanel(string dataAsset, string text, SpaceScreen spaceScreen, Vector2 position, Vector2 dimensions, Color colour, string name, float opacity = 0.5f, float lifeTime = float.MaxValue)
            : base(dataAsset, text, position, dimensions, colour, name, opacity, lifeTime)
        {
            AllStrings = new SortedList<float, string>();
            CurrentStringCounter = 0;
            MaxUnlockedStringCounter = 0;
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
            PreviousStringButton.EnableAndDisableEvent += ButtonActivationEvent;
            LoadAndAddUIElement(PreviousStringButton);

            NextStringButton = new Button(
                Button.defaultButtonAsset,
                new Vector2(Dimensions.X * 0.3f, Dimensions.Y * 0.3f),
                Button.defaultColour,
                Button.highlightedColour,
                "Next String Button",
                "Next");
            NextStringButton.InteractEvent += NextStringEvent;
            NextStringButton.EnableAndDisableEvent += ButtonActivationEvent;
            LoadAndAddUIElement(NextStringButton);
        }

        private void PreviousStringEvent(object sender, EventArgs e)
        {
            SpaceScreen.PlayerShip.Select();
            CurrentStringCounter--;
            DialogBoxText.ChangeText(AllStrings.ElementAt(CurrentStringCounter).Value);
        }

        private void NextStringEvent(object sender, EventArgs e)
        {
            SpaceScreen.PlayerShip.Select();
            CurrentStringCounter++;
            DialogBoxText.ChangeText(AllStrings.ElementAt(CurrentStringCounter).Value);
        }

        public override void LoadContent(ContentManager content)
        {
            base.LoadContent(content);

            SetUpButtons();

            DialogData = content.Load<SpaceScreenDialogData>(String.Format("XML/Space Data/Dialog Data/Level{0}Dialog", SpaceScreen.ExtendedScreenManager.Session.CurrentLevel));
            
            for (int i = 0; i < DialogData.Dialog.Count; i++)
            {
                if (i < DialogData.TimeToShow.Count)
                {
                    AddString(DialogData.Dialog[i], DialogData.TimeToShow[i]);
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            totalGameTime += (float)gameTime.ElapsedGameTime.Milliseconds / 1000f;
            
            int startingIndex = MaxUnlockedStringCounter;
            for (int i = startingIndex + 1; i < AllStrings.Count; i++)
            {
                // Enough time has passed to show this string
                if (AllStrings.ElementAt(i).Key <= totalGameTime)
                {
                    CurrentStringCounter = i;
                    MaxUnlockedStringCounter = i;
                }
                else
                {
                    break;
                }
            }

            DialogBoxText.ChangeText(AllStrings.ElementAt(CurrentStringCounter).Value);
        }

        public void AddString(string text, float time)
        {
            AllStrings.Add(time, text);
        }

        private void ButtonActivationEvent(object sender, EventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                if (button.Name == "Next String Button")
                {
                    if (CurrentStringCounter < MaxUnlockedStringCounter)
                    {
                        button.Activate();
                    }
                    else
                    {
                        button.DisableAndHide();
                    }
                }
                else if (button.Name == "Previous String Button")
                {
                    if (CurrentStringCounter > 1)
                    {
                        button.Activate();
                    }
                    else
                    {
                        button.DisableAndHide();
                    }
                }
            }
        }   }
    }
}
