using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToGalaxy.Gameplay_Objects;
using ToGalaxy.Gameplay_Objects.Space_Screen;
using ToGalaxy.Screens.Cut_Scene_Screens.Text_Cut_Scenes;

namespace ToGalaxy.Screens.Cut_Scene_Screens
{
    public class IntroCutScene1 : SpaceScreenCutScene
    {
        public IntroCutScene1(ExtendedScreenManager screenManager, string screenDataAsset = "XML/Cut Scenes/Level Data/IntroCutScene1")
            : base(screenManager, screenDataAsset)
        {
            
        }

        public override void AddGameObjects()
        {
            PlayerShip playerShip = new PlayerShip("XML/Cut Scenes/Ships/UIN Ships/Barracuda", new Vector2(ScreenManager.Viewport.Width / 2, 5 * ScreenManager.Viewport.Height / 6));
            AddGameObjectEvent(0, playerShip);
            AddMoveEvent(1, playerShip, new Vector2(ScreenManager.Viewport.Width / 2, ScreenManager.Viewport.Height / 2));
            AddMoveEvent(29, playerShip, new Vector2(3 * ScreenManager.Viewport.Width / 4, 5 * ScreenManager.Viewport.Height / 6));
            AddMoveEvent(42, playerShip, new Vector2(3 * ScreenManager.Viewport.Width / 4, ScreenManager.Viewport.Height / 4));
            AddMoveEvent(57, playerShip, new Vector2(5 * ScreenManager.Viewport.Width / 4, ScreenManager.Viewport.Height / 4));

            EnemyShip enemyShip;
            int enemyAppearanceOffset = 23;

            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < Math.Round((double)(i + 1) / 2f, MidpointRounding.AwayFromZero); j++)
                {
                    enemyShip = new EnemyShip("XML/Cut Scenes/Ships/Pirate Ships/Gnat", new Vector2(1000 - 100 * (((i + 3) / 2) - j), -ScreenManager.Viewport.Height / 2), playerShip);
                    AddGameObjectEvent(enemyAppearanceOffset + i * 5, enemyShip);

                    enemyShip = new EnemyShip("XML/Cut Scenes/Ships/UIN Ships/Harpoon", new Vector2(1000 - 100 * (((i + 3) / 2) - j), -ScreenManager.Viewport.Height / 2), playerShip);
                    AddGameObjectEvent(enemyAppearanceOffset + i * 5, enemyShip);
                }
            }

            for (int i = 0; i < 3; i++)
            {
                enemyShip = new EnemyShip("XML/Cut Scenes/Ships/Pirate Ships/Gnat", new Vector2(-50, 50 + i * ScreenManager.Viewport.Height / 8), playerShip);
                AddGameObjectEvent(enemyAppearanceOffset + 42, enemyShip);

                enemyShip = new EnemyShip("XML/Cut Scenes/Ships/UIN Ships/Harpoon", new Vector2(-50, 50 + i * ScreenManager.Viewport.Height / 8), playerShip);
                AddGameObjectEvent(enemyAppearanceOffset + 44, enemyShip);

                enemyShip = new EnemyShip("XML/Cut Scenes/Ships/UIN Ships/Harpoon", new Vector2(-50, 50 + i * ScreenManager.Viewport.Height / 8), playerShip);
                AddGameObjectEvent(enemyAppearanceOffset + 45, enemyShip);
            }

            AddRemoveGameObjectEvent(69, playerShip);
        }

        public override void AddCameraMovement()
        {
            AddCameraMoveEvent(18, new Vector2(ScreenManager.Viewport.Width / 2, ScreenManager.Viewport.Height / 6));
            AddCameraMoveEvent(29, new Vector2(ScreenManager.Viewport.Width / 2, ScreenManager.Viewport.Height / 2));
        }

        public override void AddDialogBoxes()
        {
            AddDialogBoxEvent(0, "'This is UIN Frigate Inheritance making scheduled patrol report'");
            AddDialogBoxEvent(7, "'It's all quiet out here'", 3f);
            AddDialogBoxEvent(11, "'Wait'", 1f);
            AddDialogBoxEvent(13, "'Command, we have activity on our long range sensors'", 4f);
            AddDialogBoxEvent(18, "'We can confirm, we have multiple hostiles closing in fast on our position'");
            AddDialogBoxEvent(28, "'Defensive Maneouvers'", 2f);
            AddDialogBoxEvent(31, "'All hands, prepare for impact. Full power to shields and keep those weapons firing'");
            AddDialogBoxEvent(43, "'Hull breaches across decks 3 through 8.  We're losing power to shields and have sustained major casualties.  Command, please advise'");
            AddDialogBoxEvent(55, "'UIN Frigate Inheritance, this is UIN command.  We advise a full retreat.  Rendevouz with the 8th fleet at the J'hal station where you will be debriefed'");
            AddDialogBoxEvent(62, "'Good luck'", 2f);
            AddDialogBoxEvent(65, "'Command Out'", 2f);
        }

        public override void SetUpNextScreen()
        {
            SetNextScreen(new IntroTextCutScene2(ExtendedScreenManager));
            AddLoadNextScreenEvent(72);
        }
    }
}
