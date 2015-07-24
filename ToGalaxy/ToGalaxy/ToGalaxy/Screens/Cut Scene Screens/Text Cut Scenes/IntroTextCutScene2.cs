using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToGalaxyGameLibrary.Screens_and_ScreenManager.Screens;

namespace ToGalaxy.Screens.Cut_Scene_Screens.Text_Cut_Scenes
{
    public class IntroTextCutScene2 : TextCutSceneScreen
    {
        private ExtendedScreenManager ExtendedScreenManager
        {
            get;
            set;
        }

        public IntroTextCutScene2(ExtendedScreenManager screenManager, string textAsset = "XML/Cut Scenes/Cut Scene Texts/IntroTextCutScene2")
            : base(screenManager, textAsset)
        {
            ExtendedScreenManager = screenManager;
        }

        public override void SetUpNextScreen()
        {
            SetNextScreen(new IntroCutScene2(ExtendedScreenManager));
            AddLoadNextScreenEvent((Strings.Count - 1) * 7 + 1);
        }
    }
}
