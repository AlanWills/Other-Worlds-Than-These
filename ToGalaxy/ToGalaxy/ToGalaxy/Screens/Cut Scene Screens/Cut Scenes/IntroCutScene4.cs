using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToGalaxy.Gameplay_Objects;
using ToGalaxy.Screens.Menu_Screens;
using ToGalaxyGameLibrary.Game_Objects;
using ToGalaxyGameLibrary.Screens_and_ScreenManager;

namespace ToGalaxy.Screens.Cut_Scene_Screens.Cut_Scenes
{
    public class IntroCutScene4 : CutSceneScreen
    {
        private ExtendedScreenManager ExtendedScreenManager
        {
            get;
            set;
        }

        public IntroCutScene4(ExtendedScreenManager screenManager, string screenDataAsset = "XML/Cut Scenes/Level Data/IntroCutScene4")
            : base(screenManager, screenDataAsset)
        {
            ExtendedScreenManager = screenManager;
        }

        public override void AddGameObjects()
        {
            GameObject spaceStation = new GameObject("Sprites/Space Objects/Space Stations/LargeSpaceStation", new Vector2(ScreenManager.Viewport.Width / 2, ScreenManager.Viewport.Height / 2));
            AddGameObjectEvent(0, spaceStation);

            PlayerShip playerShip = new PlayerShip("XML/Cut Scenes/Ships/UIN Ships/Barracuda", ScreenManager.ScreenCentre + new Vector2(0, 500));
            playerShip.AutoTurrets = true;
            playerShip.ManualSteering = false;
            AddGameObjectEvent(0, playerShip);
        }

        public override void AddDialogBoxes()
        {
            AddDialogBoxEvent(3, "'All station personnel, we have just lost communications with the 8th fleet as well as the rest of UIN command'", new Vector2(ScreenManager.Viewport.Width / 6, 15 * ScreenManager.Viewport.Height / 16));
            AddDialogBoxEvent(10, "'From their final communications, we can determine that they were eradicated by a powerful pirate armada, which has since continued along the outskirts of UIN space'", new Vector2(ScreenManager.Viewport.Width / 6, 15 * ScreenManager.Viewport.Height / 16), 7f);
            AddDialogBoxEvent(18, "'They have also co-ordinated attacks across our entire outer-rim communications network, disabling all communications with other stations as well as UIN command'", new Vector2(ScreenManager.Viewport.Width / 6, 15 * ScreenManager.Viewport.Height / 16), 7f);
            AddDialogBoxEvent(26, "'We are beginning evacuation of the station, but believe that other stations in the outer rim may be unaware of the imminent danger'", new Vector2(ScreenManager.Viewport.Width / 6, 15 * ScreenManager.Viewport.Height / 16));
            AddDialogBoxEvent(33, "'Consequently, one such captain has volunteered to travel to the UIN Battle Station Turin in the Arno system, which is sufficiently equipped to broadcast a message to all UIN stations and ships in this quadrant'", new Vector2(ScreenManager.Viewport.Width / 6, 15 * ScreenManager.Viewport.Height / 16), 8f);
            AddDialogBoxEvent(42, "'Could this captain please report to the hangar where a ship will be outfitted immediately'", new Vector2(ScreenManager.Viewport.Width / 6, 15 * ScreenManager.Viewport.Height / 16));
        }


        public override void SetUpNextScreen()
        {
            SetNextScreen(new ShipUpgradeScreen(ExtendedScreenManager, "XML/Menu Screens/ShipUpgradeScreen"));
            AddLoadNextScreenEvent(49);
        }
    }
}
