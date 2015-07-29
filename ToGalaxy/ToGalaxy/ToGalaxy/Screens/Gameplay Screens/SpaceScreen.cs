using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToGalaxy.Gameplay_Objects;
using ToGalaxy.Gameplay_Objects.Space_Screen;
using ToGalaxy.Screens.Menu_Screens;
using ToGalaxy.UI;
using ToGalaxyCustomData;
using ToGalaxyGameLibrary.Game_Objects;
using ToGalaxyGameLibrary.Screens_and_ScreenManager;
using ToGalaxyGameLibrary.UI;

namespace ToGalaxy.Screens.Gameplay_Screens
{
    public class SpaceScreen : Screen
    {
        #region Space Screen Data

        public SpaceScreenData SpaceScreenData
        {
            get;
            private set;
        }

        #endregion

        #region Game Object Lists

        public PlayerShip PlayerShip
        {
            get;
            private set;
        }

        public List<Ship> EnemyShips
        {
            get;
            private set;
        }

        private Dictionary<string, List<EnemyShip>> RandomEnemyShipSpawnPool
        {
            get;
            set;
        }

        public List<GameObject> Objects
        {
            get;
            private set;
        }

        #endregion

        #region UI

        public SpaceScreenHud HUD
        {
            get;
            private set;
        }

        #endregion

        public ExtendedScreenManager ExtendedScreenManager
        {
            get;
            private set;
        }

        private float timeSinceLastRandomEnemy = 0;

        public SpaceScreen(ExtendedScreenManager screenManager, string screenDataAsset)
            : base(screenManager, screenDataAsset)
        {
            ExtendedScreenManager = screenManager;
            EnemyShips = new List<Ship>();
            RandomEnemyShipSpawnPool = new Dictionary<string, List<EnemyShip>>();
            Objects = new List<GameObject>();
        }

        public void SetUpSpaceScreen()
        {
            if (ScreenDataAsset != "")
            {
                SpaceScreenData = ScreenManager.Content.Load<SpaceScreenData>(ScreenDataAsset);
            }

            SetUpPresetObjects();
            SetUpStartingRandomObjects();
            SetUpPlayerShip();
            SetUpStartingEnemies();
            SetUpHUD();
        }

        private void SetUpPlayerShip()
        {
            PlayerShip = ExtendedScreenManager.Session.PlayerShip;
            PlayerShip.SetUpTurretSpawnPools();
            PlayerShip.SetUpShipUI(ScreenManager.Content);
            PlayerShip.SetPosition(ScreenManager.ScreenCentre);
            PlayerShip.MoveOrder(PlayerShip.Position);

            AddGameObject(PlayerShip);
            AddSensorImage(PlayerShip);

            ExtendedScreenManager.Camera.SetCameraToFocusOnObject(PlayerShip);
        }

        private void SetUpStartingEnemies()
        {
            for (int enemyIndex = 0; enemyIndex < SpaceScreenData.RandomEnemiesNames.Count; enemyIndex++)
            {
                RandomEnemyShipSpawnPool.Add(SpaceScreenData.RandomEnemiesNames[enemyIndex], new List<EnemyShip>());

                for (int i = 0; i < 10; i++)
                {
                    EnemyShip enemyShip = new EnemyShip("XML/Cut Scenes/Ships/Pirate Ships/" + SpaceScreenData.RandomEnemiesNames[enemyIndex], Vector2.Zero, null);
                    enemyShip.LoadContent(ScreenManager.Content);
                    enemyShip.SetUpTurretSpawnPools();
                    enemyShip.SetUpShipUI(ScreenManager.Content);
                    RandomEnemyShipSpawnPool[SpaceScreenData.RandomEnemiesNames[enemyIndex]].Add(enemyShip);
                }
            }

            // Deserialize the enemies from the space screen data XML
            for (int i = 0; i < SpaceScreenData.EnemiesNames.Count; i++)
            {
                if (i < SpaceScreenData.EnemiesPositions.Count)
                {
                    EnemyShip enemyShip = new EnemyShip("XML/Cut Scenes/Ships/Pirate Ships/" + SpaceScreenData.EnemiesNames[i], SpaceScreenData.EnemiesPositions[i], null);
                    LoadAndAddEnemy(enemyShip);
                }
            }
        }

        private void SetUpPresetObjects()
        {
            for (int i = 0; i < SpaceScreenData.PresetObjectsNames.Count; i++)
            {
                if (i < SpaceScreenData.PresetObjectsPositions.Count)
                {
                    if (SpaceScreenData.PresetObjectsNames[i] == SpaceScreenData.Exit)
                    {
                        InteractableGameObject interactablePresetObject = new InteractableGameObject("Sprites/Space Objects/" + SpaceScreenData.PresetObjectsNames[i], SpaceScreenData.PresetObjectsPositions[i]);
                        interactablePresetObject.InteractEvent += PlayerExitEvent;

                        LoadAndAddObject(interactablePresetObject);
                    }
                    else if (SpaceScreenData.MustActivateObjects.Contains(SpaceScreenData.PresetObjectsNames[i]))
                    {
                        InteractableGameObject interactablePresetObject = new InteractableGameObject("Sprites/Space Objects/" + SpaceScreenData.PresetObjectsNames[i], SpaceScreenData.PresetObjectsPositions[i]);
                        interactablePresetObject.InteractEvent += ActivateObjectEvent;

                        LoadAndAddObject(interactablePresetObject);
                    }
                    else
                    {
                        GameObject presetObject = new GameObject("Sprites/Space Objects/" + SpaceScreenData.PresetObjectsNames[i], SpaceScreenData.PresetObjectsPositions[i]);

                        LoadAndAddObject(presetObject);
                    }
                }
            }
        }

        private void SetUpStartingRandomObjects()
        {
            Random random = new Random();

            for (int y = -(int)(Camera2D.BackgroundMultiplier * ScreenManager.Viewport.Height * 0.5f); y < Camera2D.BackgroundMultiplier * ScreenManager.Viewport.Height *0.5f; y += 30)
            {
                for (int x = -(int)(Camera2D.BackgroundMultiplier * ScreenManager.Viewport.Width* 0.5f); x < Camera2D.BackgroundMultiplier * ScreenManager.Viewport.Width * 0.5f; x += 30)
                {
                    for (int i = 0; i < SpaceScreenData.RandomObjectsNames.Count; i++)
                    {
                        float objectProbability = (float)random.NextDouble();

                        if (i < SpaceScreenData.RandomObjectsProbability.Count && objectProbability > 1 - SpaceScreenData.RandomObjectsProbability[i])
                        {
                            Vector2 position = -ScreenManager.Camera.Position + new Vector2(x, y);

                            GameObject randomObject = new GameObject("Sprites/Space Objects/" + SpaceScreenData.RandomObjectsNames[i], position);
                            randomObject.SetRotation(MathHelper.ToRadians(90));
                            randomObject.IncreaseVelocity(new Vector2(0, 1));

                            LoadAndAddObject(randomObject);

                            break;
                        }
                    }
                }
            }
        }

        private void SetUpHUD()
        {
            HUD = new SpaceScreenHud(
                "",
                this,
                ScreenManager.ScreenCentre,
                2 * ScreenManager.ScreenCentre,
                Color.White,
                "HUD");
            AddScreenUIElement(HUD);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (ScreenState == ScreenState.Hidden || ScreenState == ScreenState.Active)
            {
                bool successfulAttack = false;
                if (PlayerShip.AutoTurrets)
                {
                    successfulAttack = CheckForAttackOrder();
                }

                // To avoid repetition of UI, check for a move order only if we have not received a successful attack order
                if (!PlayerShip.ManualSteering && !successfulAttack)
                {
                    CheckForMoveOrder();
                }

                AddRandomEnemies(gameTime);
                AddRandomObjects();
                CheckEnemiesFindPlayer();
                CheckBulletCollisions();
                RemoveDeadEnemies();
                // RemoveDeadRandomObjects();
                UpdateHUD();
            }

            // When battle is over play ambient music again
            if ((EnemyShips.Count == 0) && (ScreenManager.Music.CurrentMusicStylePlaying != "Ambient"))
            {
                ScreenManager.Music.PlayAmbientMusic();
            }
        }

        private void CheckEnemiesFindPlayer()
        {
            foreach (EnemyShip enemy in EnemyShips)
            {
                enemy.FindMovementTarget(new List<Ship>() { PlayerShip });
                enemy.FindTurretTarget(new List<Ship>() { PlayerShip });
            }
        }

        private void UpdateHUD()
        {
            bool shipSelected = false;

            foreach (EnemyShip enemy in EnemyShips)
            {
                if (enemy.Selected)
                {
                    HUD.ShowShipInfo(enemy);
                    shipSelected = true;
                }
            }

            if (PlayerShip.Selected)
            {
                HUD.ShowShipInfo(PlayerShip);
                shipSelected = true;
            }

            if (!shipSelected)
            {
                HUD.ClearShipInfo();
            }
        }

        private void CheckBulletCollisions()
        {
            bool playerHit = false, enemyHit = false;

            enemyHit = PlayerShip.CheckBulletCollisionsWithTargets(EnemyShips);

            foreach (EnemyShip enemy in EnemyShips)
            {
                playerHit = enemy.CheckBulletCollisionsWithTarget(PlayerShip);
            }

            if ((playerHit || enemyHit) && (ScreenManager.Music.CurrentMusicStylePlaying != "Battle"))
            {
                ScreenManager.Music.PlayBattleMusic();
            }
        }

        private void AddRandomEnemies(GameTime gameTime)
        {
            timeSinceLastRandomEnemy += (float)gameTime.ElapsedGameTime.Milliseconds / 1000;
            Random random = new Random();

            float enemyProbability = (float)random.NextDouble() / 10;

            if (enemyProbability * timeSinceLastRandomEnemy > 1 - SpaceScreenData.RandomEncounterProbability && PlayerShip != null)
            {
                int enemyNumber = random.Next(0, SpaceScreenData.RandomEnemiesNames.Count);
                float angle = (float)(random.NextDouble() * MathHelper.TwoPi);
                Vector2 position = new Vector2((float)Math.Cos(angle) * 1000f, (float)Math.Sin(angle) * 1000f) + PlayerShip.Position;

                EnemyShip enemy = RandomEnemyShipSpawnPool[SpaceScreenData.RandomEnemiesNames[enemyNumber]][0];
                RandomEnemyShipSpawnPool[SpaceScreenData.RandomEnemiesNames[enemyNumber]].RemoveAt(0);
                AddEnemy(enemy);

                timeSinceLastRandomEnemy = 0;
            }
        }

        private void AddRandomObjects()
        {
            Random random = new Random();

            for (int i = 0; i < SpaceScreenData.RandomObjectsNames.Count; i++)
            {
                float objectProbability = (float)random.NextDouble();

                if (i < SpaceScreenData.RandomObjectsProbability.Count && objectProbability > 1 - SpaceScreenData.RandomObjectsProbability[i])
                {
                    Vector2 position = (-ScreenManager.Camera.Position / ScreenManager.Camera.Zoom) + new Vector2(0, random.Next(0, (int)(ScreenManager.Viewport.Height / ScreenManager.Camera.Zoom)));

                    GameObject randomObject = new GameObject("Sprites/Space Objects/" + SpaceScreenData.RandomObjectsNames[i], position);
                    randomObject.SetRotation(MathHelper.ToRadians(90));
                    randomObject.IncreaseVelocity(new Vector2(0, 1));

                    LoadAndAddObject(randomObject);
                }
            }
        }

        private void AddEnemy(EnemyShip enemy)
        {
            EnemyShips.Add(enemy);
            AddGameObject(enemy);
            AddSensorImage(enemy);
        }

        private void AddObject(GameObject gameObject)
        {
            Objects.Add(gameObject);
            AddGameObject(gameObject);
            AddSensorImage(gameObject);
        }

        private void LoadAndAddEnemy(EnemyShip enemy)
        {
            EnemyShips.Add(enemy);
            LoadAndAddGameObject(enemy);
            enemy.SetUpTurretSpawnPools();
            enemy.SetUpShipUI(ScreenManager.Content);
            AddSensorImage(enemy);
        }

        private void LoadAndAddObject(GameObject gameObject)
        {
            Objects.Add(gameObject);
            LoadAndAddGameObject(gameObject);
            AddSensorImage(gameObject);
        }

        private void AddSensorImage(GameObject gameObject)
        {
            ExtendedScreenManager.SensorsScreen.AddImage(gameObject);
        }

        private void RemoveDeadEnemies()
        {
            List<EnemyShip> enemiesToRemove = new List<EnemyShip>();

            foreach (EnemyShip enemy in EnemyShips)
            {
                if (enemy.Status == GameObjectStatus.Dead)
                {
                    enemiesToRemove.Add(enemy);
                }
            }

            foreach (EnemyShip enemy in enemiesToRemove)
            {
                EnemyShips.Remove(enemy);
                ExtendedScreenManager.SensorsScreen.RemoveImage(enemy);
                LoadAndAddGameObject(new AnimatedGameObject("XML/FX/Explosion", enemy.Position, true, false));

                foreach (KeyValuePair<string, List<EnemyShip>> randomShipList in RandomEnemyShipSpawnPool)
                {
                    if (enemy.ShipData.Name == randomShipList.Key)
                    {
                        randomShipList.Value.Add(enemy);
                    }
                }
            }

            PlayerShip.CheckTargets(EnemyShips);
        }

        private void RemoveDeadRandomObjects()
        {
            foreach (GameObject randomObject in Objects)
            {
                if (randomObject.Position.X + ScreenManager.Camera.Position.X > 3 * ScreenManager.Viewport.Width / (2 * ScreenManager.Camera.Zoom) ||
                    randomObject.Position.X + ScreenManager.Camera.Position.X < -3 * ScreenManager.Viewport.Width / (2 * ScreenManager.Camera.Zoom))
                {
                    randomObject.Die();
                }
                else if (randomObject.Position.Y + ScreenManager.Camera.Position.Y > 3 * ScreenManager.Viewport.Height / (2 * ScreenManager.Camera.Zoom) ||
                    randomObject.Position.Y + ScreenManager.Camera.Position.Y < -3 * ScreenManager.Viewport.Height / (2 * ScreenManager.Camera.Zoom))
                {
                    randomObject.Die();
                }
            }
        }

        #region Extra Input Checks - for Screen Swap and Move/Attack Orders

        private bool CheckForMoveOrder()
        {
            // If the player successfully locked in a new target destination, add a marker UI
            if (PlayerShip.AutoTurrets)
            {
                if (ScreenManager.Mouse.PreviousMouseState.RightButton == ButtonState.Released && ScreenManager.Mouse.IsRightClicked)
                {
                    if (PlayerShip.MoveOrder(ScreenManager.Mouse.LastRightClickedPosition))
                    {
                        AddMarker(ScreenManager.Mouse.LastRightClickedPosition, "Move");
                        return true;
                    }
                }
            }
            else
            {
                if (ScreenManager.Mouse.PreviousMouseState.MiddleButton == ButtonState.Released && ScreenManager.Mouse.IsMiddleClicked)
                {
                    if (PlayerShip.MoveOrder(ScreenManager.Mouse.LastMiddleClickedPosition))
                    {
                        AddMarker(ScreenManager.Mouse.LastMiddleClickedPosition, "Move");
                        return true;
                    }
                }
            }
            
            return false;
        }

        private bool CheckForAttackOrder()
        {
            foreach (EnemyShip enemyShip in EnemyShips)
            {
                if (ScreenManager.Mouse.PreviousMouseState.RightButton == ButtonState.Released && ScreenManager.Mouse.IsRightClicked)
                {
                    if (enemyShip.Bounds.Contains((int)ScreenManager.Mouse.LastRightClickedPosition.X, (int)ScreenManager.Mouse.LastRightClickedPosition.Y))
                    {
                        if (PlayerShip.AttackOrder(enemyShip))
                        {
                            AddMarker(enemyShip.Position, "Attack");
                            return true;
                        }
                    }
                }
            }

            PlayerShip.FindTurretTarget(EnemyShips);

            return false;
        }

        #endregion

        #region Adding Command Marker Functions

        private void AddMarker(Vector2 position, string type)
        {
            CommandMarker marker = new CommandMarker("Sprites/UI/Command Markers/" + type + "Marker", position, type + " Marker");

            AddInGameUIElement(marker);
        }

        #endregion

        private void PlayerExitEvent(object sender, EventArgs e)
        {
            GameObject exit = sender as GameObject;
            if (exit != null)
            {
                if (SpaceScreenData.MustActivateObjects.Count == 0)
                {
                    if ((PlayerShip.Position - exit.Position).Length() < Math.Max(exit.Bounds.Width, exit.Bounds.Height) + 30)
                    {
                        ExtendedScreenManager.Session.SetCurrentLevel(ExtendedScreenManager.Session.CurrentLevel + 1);
                        ExtendedScreenManager.LoadAndAddScreen(new ShipUpgradeScreen(ExtendedScreenManager, "XML/Menu Screens/ShipUpgradeScreen"));
                        this.Die();
                    }
                }
            }
        }

        private void ActivateObjectEvent(object sender, EventArgs e)
        {
            GameObject activateableObject = sender as GameObject;
            if (activateableObject != null)
            {
                if ((PlayerShip.Position - activateableObject.Position).Length() < Math.Max(activateableObject.Bounds.Width, activateableObject.Bounds.Height) + 30)
                {
                    string[] name = activateableObject.DataAsset.Split('/');
                    if (SpaceScreenData.MustActivateObjects.Contains(name[name.Length - 2] + "/" + name.Last()))
                    {
                        SpaceScreenData.MustActivateObjects.Remove(name[name.Length - 2] + "/" + name.Last());
                    }
                }
            }
        }
    }
}