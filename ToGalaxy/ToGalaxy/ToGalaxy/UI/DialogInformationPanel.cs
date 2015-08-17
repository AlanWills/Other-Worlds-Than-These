using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToGalaxy.Screens.Gameplay_Screens;
using ToGalaxyCustomData;
using ToGalaxyGameLibrary;
using ToGalaxyGameLibrary.Screens_and_ScreenManager.Managers;
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

        public static SortedList<float, string> AllStrings
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

        private static float totalGameTime = 0;

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

        }

        private void PreviousString()
        {
            if (CurrentStringCounter > 0)
            {
                CurrentStringCounter--;
                DialogBoxText.ChangeText(AllStrings.ElementAt(CurrentStringCounter).Value);
            }
        }

        private void NextString()
        {
            if (CurrentStringCounter < MaxUnlockedStringCounter)
            {
                CurrentStringCounter++;
                DialogBoxText.ChangeText(AllStrings.ElementAt(CurrentStringCounter).Value);
            }
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

            if (InputManager.KeyReleased(Keys.Tab))
            {
                if (InputManager.IsKeyDown(Keys.LeftShift))
                {
                    PreviousString();
                }
                else
                {
                    NextString();
                }
            }
        }

        public static void AddString(string text, float time)
        {
            AllStrings.Add(time, text);
        }

        public static void AddStringFromCurrentTime(string text, float time)
        {
            AllStrings.Add(time + totalGameTime, text);
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
        }   
    }
}
