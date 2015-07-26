using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToGalaxy.Screens.Cut_Scene_Screens.Cut_Scenes;
using ToGalaxyGameLibrary.Screens_and_ScreenManager.Screens;

namespace ToGalaxy.Screens.Cut_Scene_Screens.Text_Cut_Scenes
{
    public class IntroTextCutScene4 : TextCutSceneScreen
    {
        private ExtendedScreenManager ExtendedScreenManager
        {
            get;
            set;
        }

        public IntroTextCutScene4(ExtendedScreenManager screenManager, string textDataAsset = "XML/Cut Scenes/Cut Scene Texts/IntroTextCutScene4")
            : base(screenManager, textDataAsset)
        {
            ExtendedScreenManager = screenManager;
        }

        public override void SetUpNextScreen()
        {
            SetNextScreen(new IntroCutScene4(ExtendedScreenManager));
            AddLoadNextScreenEvent(Strings.Count * 7);
        }
    }
}
