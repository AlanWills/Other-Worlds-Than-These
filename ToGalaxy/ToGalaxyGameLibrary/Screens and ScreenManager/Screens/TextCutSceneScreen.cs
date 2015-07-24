using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToGalaxyGameLibrary.UI;

namespace ToGalaxyGameLibrary.Screens_and_ScreenManager.Screens
{
    public class TextCutSceneScreen : CutSceneScreen
    {
        public List<string> Strings
        {
            get;
            private set;
        }

        private string TextDataAsset
        {
            get;
            set;
        }

        public TextCutSceneScreen(ScreenManager screenManager, string textDataAsset, string screenDataAsset = "XML/Cut Scenes/Level Data/TextCutSceneScreen")
            : base(screenManager, screenDataAsset)
        {
            Strings = new List<string>();
            TextDataAsset = textDataAsset;
        }

        public override void LoadContent()
        {
            Strings = ScreenManager.Content.Load<List<string>>(TextDataAsset);

            base.LoadContent();
        }

        public override void AddDialogBoxes()
        {
            for (int i = 0; i < Strings.Count; i++)
            {
                AddDialogBoxEvent(i * 7, Strings[i]);
            }
        }

        public override bool AddDialogBox(CutSceneEventArgs eventArgs)
        {
            DialogBox dialogBox = new DialogBox(
                "",
                eventArgs.ObjectName,
                new Vector2(ScreenManager.Viewport.Width / 2, ScreenManager.Viewport.Height / 2),
                new Vector2(3 * ScreenManager.Viewport.Width / 4, ScreenManager.Viewport.Height),
                Color.Black,
                "Dialog Box",
                1f,
                eventArgs.MoveSpeed);
            AddScreenUIElement(dialogBox);

            return true;
        }
    }
}
