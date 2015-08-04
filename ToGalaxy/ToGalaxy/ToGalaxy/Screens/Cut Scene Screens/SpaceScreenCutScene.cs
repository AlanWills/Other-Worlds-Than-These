using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToGalaxy.Gameplay_Objects;
using ToGalaxy.Gameplay_Objects.Space_Screen;
using ToGalaxy.Screens.Gameplay_Screens;
using ToGalaxyGameLibrary.Game_Objects;
using ToGalaxyGameLibrary.Screens_and_ScreenManager;

namespace ToGalaxy.Screens.Cut_Scene_Screens
{
    public class SpaceScreenCutScene : CutSceneScreen
    {
        public ExtendedScreenManager ExtendedScreenManager
        {
            get;
            private set;
        }

        protected List<Ship> PlayerShips
        {
            get;
            private set;
        }

        private List<Ship> PlayerShipsToRemove
        {
            get;
            set;
        }

        protected List<Ship> EnemyShips
        {
            get;
            private set;
        }

        private List<Ship> EnemyShipsToRemove
        {
            get;
            set;
        }

        public SpaceScreenCutScene(ExtendedScreenManager screenManager, string screenDataAsset)
            : base(screenManager, screenDataAsset)
        {
            ExtendedScreenManager = screenManager;

            PlayerShips = new List<Ship>();
            PlayerShipsToRemove = new List<Ship>();
            EnemyShips = new List<Ship>();
            EnemyShipsToRemove = new List<Ship>();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            foreach (Ship playerShip in PlayerShips)
            {
                if (!GameObjects.Contains(playerShip))
                {
                    PlayerShipsToRemove.Add(playerShip);
                }
            }

            foreach (Ship playerShip in PlayerShipsToRemove)
            {
                PlayerShips.Remove(playerShip);
            }

            PlayerShipsToRemove.Clear();

            foreach (Ship enemyShip in EnemyShips)
            {
                if (!GameObjects.Contains(enemyShip))
                {
                    EnemyShipsToRemove.Add(enemyShip);
                }
            }

            foreach (Ship enemyShip in EnemyShipsToRemove)
            {
                EnemyShips.Remove(enemyShip);
            }

            EnemyShipsToRemove.Clear();

            foreach (Ship playerShip in PlayerShips)
            {
                playerShip.FindTurretTarget(EnemyShips);
                playerShip.CheckBulletCollisionsWithTargets(EnemyShips);
            }

            foreach (Ship enemyShip in EnemyShips)
            {
                enemyShip.FindTurretTarget(PlayerShips);
                enemyShip.CheckBulletCollisionsWithTargets(PlayerShips);
            }
        }

        public override void AddGameObjectEvent(float activationTime, GameObject gameObject)
        {
            CutSceneEventArgs eventArgs = new CutSceneEventArgs("Add GameObject", activationTime);
            gameObject.LoadContent(ScreenManager.Content);
            eventArgs.GameObject = gameObject;

            Ship ship = gameObject as Ship;
            if (ship != null)
            {
                ship.SetUpTurretSpawnPools();
                eventArgs.GameObject = ship;
            }

            EventsList.Add(activationTime, eventArgs);
        }

        public override bool AddGameObject(CutSceneEventArgs eventArgs)
        {
            bool added = base.AddGameObject(eventArgs);
            if (added)
            {
                Ship ship = eventArgs.GameObject as Ship;
                if (ship != null)
                {
                    PlayerShip playerShip = ship as PlayerShip;
                    EnemyShip enemyShip = ship as EnemyShip;
                    if (playerShip != null)
                    {
                        PlayerShips.Add(playerShip);
                    }
                    else if (enemyShip != null)
                    {
                        EnemyShips.Add(enemyShip);
                    }
                }
            }

            return added;
        } 

        public override bool Move(CutSceneEventArgs eventArgs)
        {
            if (eventArgs.ObjectName != null)
            {
                Ship ship = (Ship)GetGameObject(eventArgs.ObjectName);
                if (ship != null)
                {
                    ship.SetDestination(eventArgs.MoveDestination);
                }
            }
            else if (eventArgs.GameObject != null)
            {
                Ship ship = (Ship)eventArgs.GameObject;
                if (ship != null)
                {
                    ship.SetDestination(eventArgs.MoveDestination);
                }
            }

            return true;
        }
    }
}