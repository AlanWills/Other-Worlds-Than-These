using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToGalaxy.Screens.Cut_Scene_Screens.Cut_Scenes;
using ToGalaxyGameLibrary.Screens_and_ScreenManager.Screens;

namespace ToGalaxy.Screens.Cut_Scene_Screens.Text_Cut_Scenes
{
    public class IntroTextCutScene3 : TextCutSceneScreen
    {
        private ExtendedScreenManager ExtendedScreenManager
        {
            get;
            set;
        }

        public IntroTextCutScene3(ExtendedScreenManager screenManager, string textAsset = "XML/Cut Scenes/Cut Scene Texts/IntroTextCutScene3")
            : base(screenManager, textAsset)
        {
            ExtendedScreenManager = screenManager;
        }

        public override void SetUpNextScreen()
        {
            SetNextScreen(new IntroCutScene3(ExtendedScreenManager));
            AddLoadNextScreenEvent((Strings.Count - 1) * 7 + 1);
        }
    }
}
