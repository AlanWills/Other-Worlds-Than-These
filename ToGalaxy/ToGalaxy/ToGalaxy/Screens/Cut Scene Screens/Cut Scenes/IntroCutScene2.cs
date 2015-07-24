using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToGalaxy.Gameplay_Objects;
using ToGalaxy.Screens.Cut_Scene_Screens.Text_Cut_Scenes;
using ToGalaxyGameLibrary.Game_Objects;

namespace ToGalaxy.Screens.Cut_Scene_Screens
{
    public class IntroCutScene2 : SpaceScreenCutScene
    {
        public IntroCutScene2(ExtendedScreenManager screenManager, string dataAsset = "XML/Cut Scenes/Level Data/IntroCutScene2")
            : base(screenManager, dataAsset)
        {
            
        }

        public override void AddGameObjects()
        {
            PlayerShip playerShip = new PlayerShip("XML/Cut Scenes/Ships/UIN Ships/Barracuda", new Vector2(ScreenManager.Viewport.Width / 2, 2000));
            AddGameObjectEvent(0, playerShip);
            AddMoveEvent(1, playerShip, new Vector2(playerShip.Position.X, 1400));
            AddMoveEvent(17, playerShip, new Vector2(playerShip.Position.X, ScreenManager.Viewport.Height / 2 + 500));

            ScreenManager.Camera.SetPosition(playerShip.Position);

            GameObject spaceStation = new GameObject("Sprites/Space Objects/Space Stations/LargeSpaceStation", new Vector2(ScreenManager.Viewport.Width / 2, ScreenManager.Viewport.Height / 2));
            AddGameObjectEvent(1, spaceStation);

            for (int i = 0; i < 3; i++)
            {
                playerShip = new PlayerShip("XML/Cut Scenes/Ships/UIN Ships/Dragonfly", new Vector2(200 * (i + 1) + ScreenManager.Viewport.Width / 2, 2300));
                AddGameObjectEvent(0, playerShip);
                AddMoveEvent(0, playerShip, new Vector2(playerShip.Position.X, 1300));
                AddMoveEvent(49, playerShip, new Vector2(-200, ScreenManager.Viewport.Height / 2));
                AddRemoveGameObjectEvent(55, playerShip);
            }

            for (int i = 0; i < 5; i++)
            {
                playerShip = new PlayerShip("XML/Cut Scenes/Ships/UIN Ships/Hurricane", new Vector2(150 * (2 - i) + ScreenManager.Viewport.Width / 2, 2000));
                AddGameObjectEvent(10, playerShip);
                AddMoveEvent(10, playerShip, new Vector2(80, 500 + 100 * (2 - i)));
                AddMoveEvent(51, playerShip, new Vector2(-200, ScreenManager.Viewport.Height / 2));
                AddRemoveGameObjectEvent(57, playerShip);
            }

            for (int i = 0; i < 5; i++)
            {
                playerShip = new PlayerShip("XML/Cut Scenes/Ships/UIN Ships/Tornado", new Vector2(150 * (2 - i) + ScreenManager.Viewport.Width / 2, 2000));
                AddGameObjectEvent(20, playerShip);
                AddMoveEvent(20, playerShip, new Vector2(ScreenManager.Viewport.Width - 100, 500 + 100 * (2 - i)));
                AddMoveEvent(53, playerShip, new Vector2(-200, ScreenManager.Viewport.Height / 2));
                AddRemoveGameObjectEvent(63, playerShip);
            }

            for (int i = 0; i < 3; i++)
            {
                playerShip = new PlayerShip("XML/Cut Scenes/Ships/UIN Ships/Tsunami", new Vector2(-100 * (i + 1) + ScreenManager.Viewport.Width / 2, 2000));
                AddGameObjectEvent(30, playerShip);
                AddMoveEvent(30, playerShip, new Vector2(playerShip.Position.X, 30));
                AddMoveEvent(55, playerShip, new Vector2(-200, ScreenManager.Viewport.Height / 2));
                AddRemoveGameObjectEvent(59, playerShip);
            }

            playerShip = new PlayerShip("XML/Cut Scenes/Ships/UIN Ships/Austria", new Vector2(ScreenManager.Viewport.Width / 2 - 500, ScreenManager.Viewport.Height / 2));
            playerShip.SetRotation(3 * MathHelper.PiOver2);
            AddGameObjectEvent(0, playerShip);
            AddMoveEvent(60, playerShip, new Vector2(-200, ScreenManager.Viewport.Height / 2));

            playerShip = new PlayerShip("XML/Cut Scenes/Ships/UIN Ships/Austria", new Vector2(ScreenManager.Viewport.Width / 2 + 500, ScreenManager.Viewport.Height / 2));
            playerShip.SetRotation(MathHelper.PiOver2);
            AddGameObjectEvent(0, playerShip);
            AddMoveEvent(60, playerShip, new Vector2(-200, ScreenManager.Viewport.Height / 2));

            float rootTwoRecip = (float)Math.Sin(MathHelper.PiOver4);

            playerShip = new PlayerShip("XML/Cut Scenes/Ships/UIN Ships/Eagle", new Vector2(ScreenManager.Viewport.Width / 2 - 450 * rootTwoRecip, ScreenManager.Viewport.Height / 2 + 450 * rootTwoRecip));
            playerShip.SetRotation(7 * MathHelper.PiOver4);
            AddGameObjectEvent(0, playerShip);
            AddMoveEvent(65, playerShip, new Vector2(-200, ScreenManager.Viewport.Height / 2));

            playerShip = new PlayerShip("XML/Cut Scenes/Ships/UIN Ships/Eagle", new Vector2(ScreenManager.Viewport.Width / 2 + 450 * rootTwoRecip, ScreenManager.Viewport.Height / 2 + 450 * rootTwoRecip));
            playerShip.SetRotation(MathHelper.PiOver4);
            AddGameObjectEvent(0, playerShip);
            AddMoveEvent(65, playerShip, new Vector2(-200, ScreenManager.Viewport.Height / 2));

            playerShip = new PlayerShip("XML/Cut Scenes/Ships/UIN Ships/Hawk", new Vector2(ScreenManager.Viewport.Width / 2 - 450 * rootTwoRecip, ScreenManager.Viewport.Height / 2 - 450 * rootTwoRecip));
            playerShip.SetRotation(5 * MathHelper.PiOver4);
            AddGameObjectEvent(0, playerShip);
            AddMoveEvent(70, playerShip, new Vector2(-200, ScreenManager.Viewport.Height / 2));

            playerShip = new PlayerShip("XML/Cut Scenes/Ships/UIN Ships/Hawk", new Vector2(ScreenManager.Viewport.Width / 2 + 450 * rootTwoRecip, ScreenManager.Viewport.Height / 2 - 450 * rootTwoRecip));
            playerShip.SetRotation(3 * MathHelper.PiOver4);
            AddGameObjectEvent(0, playerShip);
            AddMoveEvent(70, playerShip, new Vector2(-200, ScreenManager.Viewport.Height / 2));
        }

        public override void AddDialogBoxes()
        {
            AddDialogBoxEvent(6, "'UIN Frigate Inheritance requesting permission to dock'");
            AddDialogBoxEvent(15, "'UIN Frigate Inheritance, this is UIN station J'henna.  Permission granted, proceed to docking bay 5'", new Vector2(ScreenManager.Viewport.Width / 4, 4 * ScreenManager.Viewport.Height / 7));
            AddDialogBoxEvent(25, "'Docking complete, all crew prepare to disembark.  Repairs of the frigate will begin immediately'", new Vector2(ScreenManager.Viewport.Width / 4, 4 * ScreenManager.Viewport.Height / 7));
            AddDialogBoxEvent(40, "'All crew disembarked.  Ready all maintenance crews at docking bay 5'", new Vector2(ScreenManager.Viewport.Width / 6, 15 * ScreenManager.Viewport.Height / 16));
            AddDialogBoxEvent(47, "'8th fleet, you are cleared to leave.  All docking clamps retracted'", new Vector2(ScreenManager.Viewport.Width / 6, 15 * ScreenManager.Viewport.Height / 16));
        }

        public override void AddCameraMovement()
        {
            AddCameraMoveEvent(0, new Vector2(ScreenManager.Viewport.Width / 2, ScreenManager.Viewport.Height), 1.5f);
            AddCameraMoveEvent(32, new Vector2(ScreenManager.Viewport.Width / 2, ScreenManager.Viewport.Height / 2));
        }

        public override void SetUpNextScreen()
        {
            SetNextScreen(new IntroTextCutScene3(ExtendedScreenManager));
            AddLoadNextScreenEvent(90);
        }
    }
}
