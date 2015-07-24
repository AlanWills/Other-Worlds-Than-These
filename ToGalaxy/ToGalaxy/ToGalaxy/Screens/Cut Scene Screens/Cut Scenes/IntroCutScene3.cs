using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToGalaxy.Gameplay_Objects;
using ToGalaxy.Gameplay_Objects.Space_Screen;
using ToGalaxy.Screens.Menu_Screens;

namespace ToGalaxy.Screens.Cut_Scene_Screens.Cut_Scenes
{
    public class IntroCutScene3 : SpaceScreenCutScene
    {
        public IntroCutScene3(ExtendedScreenManager screenManager, string screenDataAsset = "XML/Cut Scenes/Level Data/IntroCutScene3")
            : base(screenManager, screenDataAsset)
        {

        }

        public override void AddGameObjects()
        {
            PlayerShip playerShip;

            for (int i = 0; i < 3; i++)
            {
                playerShip = new PlayerShip("XML/Cut Scenes/Ships/UIN Ships/Dragonfly", new Vector2(ScreenManager.Viewport.Width / 2, 100 * (i + 1)));
                playerShip.SetRotation(3 * MathHelper.PiOver2);
                AddGameObjectEvent(2, playerShip);
                AddMoveEvent(58, playerShip, new Vector2(ScreenManager.Viewport.Width / 2 + (1 - i) * 100, ScreenManager.Viewport.Height / 10));
                AddMoveEvent(81, playerShip, new Vector2(ScreenManager.Viewport.Width / 2 + (1 - i) * 100, -100));
                AddRotationEvent(64, playerShip, 0);
            }

            for (int i = 0; i < 5; i++)
            {
                playerShip = new PlayerShip("XML/Cut Scenes/Ships/UIN Ships/Hurricane", new Vector2(3 * ScreenManager.Viewport.Width / 5, 100 * (i + 1)));
                playerShip.SetRotation(3 * MathHelper.PiOver2);
                AddGameObjectEvent(4, playerShip);
                AddMoveEvent(58, playerShip, new Vector2(ScreenManager.Viewport.Width / 4, ScreenManager.Viewport.Height / 2 + (2 - i) * 100));
                AddMoveEvent(64, playerShip, new Vector2(ScreenManager.Viewport.Width / 6, ScreenManager.Viewport.Height / 2 + (2 - i) * 100));
                AddRotationEvent(63, playerShip, 3 * MathHelper.PiOver2);
            }

            for (int i = 0; i < 5; i++)
            {
                playerShip = new PlayerShip("XML/Cut Scenes/Ships/UIN Ships/Tornado", new Vector2(ScreenManager.Viewport.Width / 2, ScreenManager.Viewport.Height - 100 * (i + 1)));
                playerShip.SetRotation(3 * MathHelper.PiOver2);
                AddGameObjectEvent(9, playerShip);
                AddMoveEvent(58, playerShip, new Vector2(3 * ScreenManager.Viewport.Width / 4, ScreenManager.Viewport.Height / 2 + (2 - i) * 100));
                AddMoveEvent(88, playerShip, new Vector2(5 * ScreenManager.Viewport.Width / 6, ScreenManager.Viewport.Height / 2 + (2 - i) * 100));
                AddRotationEvent(63, playerShip, MathHelper.PiOver2);
            }

            for (int i = 0; i < 3; i++)
            {
                playerShip = new PlayerShip("XML/Cut Scenes/Ships/UIN Ships/Tsunami", new Vector2(2 * ScreenManager.Viewport.Width / 5, ScreenManager.Viewport.Height - 100 * (i + 1)));
                playerShip.SetRotation(3 * MathHelper.PiOver2);
                AddGameObjectEvent(7, playerShip);
                AddMoveEvent(58, playerShip, new Vector2(ScreenManager.Viewport.Width / 2 + (1 - i) * 100, 9 * ScreenManager.Viewport.Height / 10));
                AddRotationEvent(63, playerShip, MathHelper.Pi);
            }

            playerShip = new PlayerShip("XML/Cut Scenes/Ships/UIN Ships/Austria", new Vector2(3 * ScreenManager.Viewport.Width / 4, ScreenManager.Viewport.Height / 10));
            playerShip.SetRotation(3 * MathHelper.PiOver2);
            AddGameObjectEvent(5, playerShip);
            AddMoveEvent(48, playerShip, new Vector2(ScreenManager.Viewport.Width / 4, ScreenManager.Viewport.Height / 7));
            AddRotationEvent(65, playerShip, 3 * MathHelper.PiOver2);

            playerShip = new PlayerShip("XML/Cut Scenes/Ships/UIN Ships/Austria", new Vector2(9 * ScreenManager.Viewport.Width / 10, ScreenManager.Viewport.Height / 4));
            playerShip.SetRotation(3 * MathHelper.PiOver2);
            AddGameObjectEvent(8, playerShip);
            AddMoveEvent(48, playerShip, new Vector2(ScreenManager.Viewport.Width / 4, 6 * ScreenManager.Viewport.Height / 7));
            AddRotationEvent(65, playerShip, 3 * MathHelper.PiOver2);

            playerShip = new PlayerShip("XML/Cut Scenes/Ships/UIN Ships/Eagle", new Vector2(3 * ScreenManager.Viewport.Width / 4, ScreenManager.Viewport.Height / 3));
            playerShip.SetRotation(3 * MathHelper.PiOver2);
            AddGameObjectEvent(2, playerShip);
            AddMoveEvent(48, playerShip, new Vector2(2 * ScreenManager.Viewport.Width / 5, 2 * ScreenManager.Viewport.Height / 7));
            AddRotationEvent(56, playerShip, 0);
            AddMoveEvent(150, playerShip, new Vector2(ScreenManager.Viewport.Width / 4, playerShip.Position.Y));

            playerShip = new PlayerShip("XML/Cut Scenes/Ships/UIN Ships/Eagle", new Vector2(3 * ScreenManager.Viewport.Width / 5, ScreenManager.Viewport.Height / 2 + 150));
            playerShip.SetRotation(3 * MathHelper.PiOver2);
            AddGameObjectEvent(10, playerShip);
            AddMoveEvent(48, playerShip, new Vector2(3 * ScreenManager.Viewport.Width / 5, 2 * ScreenManager.Viewport.Height / 7));
            AddRotationEvent(56, playerShip, 0);
            AddMoveEvent(150, playerShip, new Vector2(ScreenManager.Viewport.Width / 4, playerShip.Position.Y));

            playerShip = new PlayerShip("XML/Cut Scenes/Ships/UIN Ships/Hawk", new Vector2(7 * ScreenManager.Viewport.Width / 8, 3 * ScreenManager.Viewport.Height / 4));
            playerShip.SetRotation(3 * MathHelper.PiOver2);
            AddGameObjectEvent(4, playerShip);
            AddMoveEvent(48, playerShip, new Vector2(2 * ScreenManager.Viewport.Width / 5, 5 * ScreenManager.Viewport.Height / 7));
            AddRotationEvent(56, playerShip, MathHelper.Pi);
            AddMoveEvent(150, playerShip, new Vector2(ScreenManager.Viewport.Width / 4, playerShip.Position.Y));

            playerShip = new PlayerShip("XML/Cut Scenes/Ships/UIN Ships/Hawk", new Vector2(2 * ScreenManager.Viewport.Width / 3, 5 * ScreenManager.Viewport.Height / 6));
            playerShip.SetRotation(3 * MathHelper.PiOver2);
            AddGameObjectEvent(6, playerShip);
            AddMoveEvent(48, playerShip, new Vector2(3 * ScreenManager.Viewport.Width / 5, 5 * ScreenManager.Viewport.Height / 7));
            AddRotationEvent(56, playerShip, MathHelper.Pi);
            AddMoveEvent(150, playerShip, new Vector2(ScreenManager.Viewport.Width / 4, playerShip.Position.Y));

            EnemyShip enemyShip;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    enemyShip = new EnemyShip("XML/Cut Scenes/Ships/Pirate Ships/Gnat", new Vector2(-500, ScreenManager.Viewport.Height / 2 + (1 - i) * 100), null);
                    AddGameObjectEvent(62 + j * 8, enemyShip);
                    AddMoveEvent(63 + j * 8, enemyShip, new Vector2(ScreenManager.Viewport.Width / 4, enemyShip.Position.Y));
                }
            }

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    enemyShip = new EnemyShip("XML/Cut Scenes/Ships/Pirate Ships/Gnat", new Vector2(ScreenManager.Viewport.Width / 2 + (1 - i) * 100, -500), null);
                    AddGameObjectEvent(78 + j * 8, enemyShip);
                    AddMoveEvent(79 + j * 8, enemyShip, new Vector2(enemyShip.Position.X, ScreenManager.Viewport.Height / 10));
                }
            }

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    enemyShip = new EnemyShip("XML/Cut Scenes/Ships/Pirate Ships/Gnat", new Vector2(ScreenManager.Viewport.Width + 500, ScreenManager.Viewport.Height / 2 + (1 - i) * 100), null);
                    AddGameObjectEvent(86 + j * 8, enemyShip);
                    AddMoveEvent(87 + j * 8, enemyShip, new Vector2(3 * ScreenManager.Viewport.Width / 4, enemyShip.Position.Y));
                }
            }

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    enemyShip = new EnemyShip("XML/Cut Scenes/Ships/Pirate Ships/Gnat", new Vector2(ScreenManager.Viewport.Width / 2 + (1 - i) * 100, ScreenManager.Viewport.Height + 500), null);
                    AddGameObjectEvent(86 + j * 8, enemyShip);
                    AddMoveEvent(87 + j * 8, enemyShip, new Vector2(enemyShip.Position.X, 9 * ScreenManager.Viewport.Height / 10));
                }
            }

            enemyShip = new EnemyShip("XML/Cut Scenes/Ships/Pirate Ships/Tyrant", new Vector2(-500, ScreenManager.Viewport.Height / 2 - 250), null);
            enemyShip.SetRotation(MathHelper.PiOver2);
            AddGameObjectEvent(153, enemyShip);
            AddMoveEvent(153, enemyShip, new Vector2(ScreenManager.Viewport.Width / 2, enemyShip.Position.Y));

            enemyShip = new EnemyShip("XML/Cut Scenes/Ships/Pirate Ships/Tyrant", new Vector2(-500, ScreenManager.Viewport.Height / 2 + 250), null);
            AddGameObjectEvent(153, enemyShip);
            enemyShip.SetRotation(MathHelper.PiOver2);
            AddMoveEvent(153, enemyShip, new Vector2(ScreenManager.Viewport.Width / 2, enemyShip.Position.Y));

            enemyShip = new EnemyShip("XML/Cut Scenes/Ships/Pirate Ships/Hulk", new Vector2(-300, ScreenManager.Viewport.Height / 2), null);
            AddGameObjectEvent(133, enemyShip);
            enemyShip.SetRotation(MathHelper.PiOver2);
            AddMoveEvent(133, enemyShip, new Vector2(ScreenManager.Viewport.Width / 2, enemyShip.Position.Y));
        }

        public override void AddDialogBoxes()
        {
            AddDialogBoxEvent(3, "'What the hell happened!'", 3f);
            AddDialogBoxEvent(7, "'I don't know sir, I'm still trying to figure it out'", 4f);
            AddDialogBoxEvent(12, "'Sir, we appear to have entered some kind of inhibition field.  It must be responsible for dropping us out of hyperspace");
            AddDialogBoxEvent(19, "'Can we re-enter hyperspace?'", 3f);
            AddDialogBoxEvent(23, "'I don't know sir, but I'm working on it'", 3f);
            AddDialogBoxEvent(27, "'Errr sir, we have multiple contacts closing on this position'", 4f);
            AddDialogBoxEvent(32, "'Where are they coming from corporal?'", 2f);
            AddDialogBoxEvent(35, "'Everywhere sir!  They're coming from everywhere!'", 3f);
            AddDialogBoxEvent(41, "'8th fleet, this is Captain Charn.  We have incoming hostile units'", 4f);
            AddDialogBoxEvent(46, "'Frigates, stay tight to me.  Fighters, form a screen around them'");
            AddDialogBoxEvent(53, "'Battle stations everyone'", 3f);
            AddDialogBoxEvent(114, "'Is it over?'", new Vector2(ScreenManager.Viewport.Width / 2, ScreenManager.Viewport.Height / 2), 3f);
            AddDialogBoxEvent(118, "'No corporal, something tells me it is not'", new Vector2(ScreenManager.Viewport.Width / 2, ScreenManager.Viewport.Height / 2), 4f);
            AddDialogBoxEvent(123, "'8th fleet this is Captain Charn.  Do not let down your guard, something tells me this is not over'", new Vector2(ScreenManager.Viewport.Width / 2, ScreenManager.Viewport.Height / 2));
            AddDialogBoxEvent(134, "'What is THAT sir?!'", new Vector2(3 * ScreenManager.Viewport.Width / 4, ScreenManager.Viewport.Height / 2), 2f);
            AddDialogBoxEvent(137, "'UIN Command this is Captain Charn of the 8th fleet.  We have been attacked by heavily armed pirates with advanced weaponry'",  new Vector2(3 * ScreenManager.Viewport.Width / 4, ScreenManager.Viewport.Height / 2), 4f);
            AddDialogBoxEvent(183, "'Captain Charn this is UIN Command please respond'", new Vector2(ScreenManager.Viewport.Width / 4, ScreenManager.Viewport.Height / 2), 4f);
            AddDialogBoxEvent(188, "...", new Vector2(ScreenManager.Viewport.Width / 4, ScreenManager.Viewport.Height / 2));
            AddDialogBoxEvent(197, "'Captain Charn this is UIN Command please respond'", new Vector2(ScreenManager.Viewport.Width / 4, ScreenManager.Viewport.Height / 2), 4f);
            AddDialogBoxEvent(202, "...", new Vector2(ScreenManager.Viewport.Width / 4, ScreenManager.Viewport.Height / 2));
        }

        public override void AddCameraMovement()
        {
            AddCameraMoveEvent(62, new Vector2(ScreenManager.Viewport.Width / 4, ScreenManager.Viewport.Height / 2), 1.5f);
            AddCameraMoveEvent(87, new Vector2(3 * ScreenManager.Viewport.Width / 4, 4 * ScreenManager.Viewport.Height / 5));
            AddCameraMoveEvent(105, new Vector2(ScreenManager.Viewport.Width / 2, ScreenManager.Viewport.Height / 2));
            AddCameraMoveEvent(133, new Vector2(-280, ScreenManager.Viewport.Height / 2));
            AddCameraMoveEvent(137, new Vector2(ScreenManager.Viewport.Width / 4, ScreenManager.Viewport.Height / 2), 1f);
        }

        public override void SetUpNextScreen()
        {
            SetNextScreen(new MainMenuScreen(ExtendedScreenManager, "XML/Menu Screens/MainMenuScreen"));
            AddLoadNextScreenEvent(209);
        }
    }
}
