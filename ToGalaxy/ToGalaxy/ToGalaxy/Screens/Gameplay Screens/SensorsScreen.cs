﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToGalaxy.Gameplay_Objects;
using ToGalaxy.Gameplay_Objects.Space_Screen;
using ToGalaxyGameLibrary.Game_Objects;
using ToGalaxyGameLibrary.Screens_and_ScreenManager;
using ToGalaxyGameLibrary.UI;

namespace ToGalaxy.Screens.Gameplay_Screens
{
    public class SensorsScreen : Screen
    {
        private ExtendedScreenManager ExtendedScreenManager
        {
            get;
            set;
        }

        private SpaceScreen SpaceScreen
        {
            get;
            set;
        }

        private Dictionary<GameObject, Image> SensorImages
        {
            get;
            set;
        }

        private Dictionary<GameObject, Image> SensorImagesToRemove
        {
            get;
            set;
        }

        private Dictionary<string, Texture2D> Thumbnails
        {
            get;
            set;
        }

        public SensorsScreen(ExtendedScreenManager screenManager, SpaceScreen spaceScreen, string screenDataAsset = "XML/Gameplay Screens/SensorsScreen")
            : base(screenManager, screenDataAsset)
        {
            ExtendedScreenManager = screenManager;
            SpaceScreen = spaceScreen;

            SensorImages = new Dictionary<GameObject, Image>();
            SensorImagesToRemove = new Dictionary<GameObject, Image>();
            Thumbnails = new Dictionary<string, Texture2D>();
        }

        private void SetUpUI()
        {
            Image spaceScreenBackground = new Image(
                SpaceScreen.ScreenData.BackgroundTextureAsset,
                new Vector2(ScreenManager.Viewport.Width / 2, ScreenManager.Viewport.Height / 2),
                ScreenManager.Viewport.Width,
                ScreenManager.Viewport.Height,
                "Space Screen Background");
            spaceScreenBackground.SetOpacity(0.5f);
            AddScreenUIElement(spaceScreenBackground);

            Texture2D thumbnail = ScreenManager.Content.Load<Texture2D>("Sprites/UI/Command Markers/ObjectSensorMarker");
            Thumbnails.Add("Object", thumbnail);

            thumbnail = ScreenManager.Content.Load<Texture2D>("Sprites/UI/Command Markers/EnemySensorMarker");
            Thumbnails.Add("Enemy", thumbnail);

            thumbnail = ScreenManager.Content.Load<Texture2D>("Sprites/UI/Command Markers/PlayerSensorMarker");
            Thumbnails.Add("Player", thumbnail);
        }

        public override void LoadContent()
        {
            base.LoadContent();

            SetUpUI();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            UpdateUI();
        }

        private void UpdateUI()
        {
            float range = SpaceScreen.PlayerShip.Sensors != null ? SpaceScreen.PlayerShip.Sensors.SensorData.Range : SpaceScreen.PlayerShip.MinimumTurretRange;

            foreach (GameObject gameObject in SpaceScreen.Objects)
            {
                float distance = (gameObject.Position - SpaceScreen.PlayerShip.Position).Length();
                if (!SensorImages.ContainsKey(gameObject))
                {
                    if (distance <= range)
                    {
                        AddImage(gameObject);
                    }
                }
                else
                {
                    if (distance > range)
                    {
                        SensorImagesToRemove.Add(gameObject, SensorImages[gameObject]);
                    }
                }
            }

            foreach (EnemyShip enemyShip in SpaceScreen.EnemyShips)
            {
                float distance = (enemyShip.Position - SpaceScreen.PlayerShip.Position).Length();
                if (!SensorImages.ContainsKey(enemyShip))
                {
                    if (distance <= range)
                    {
                        AddImage(enemyShip);
                    }
                }
                else
                {
                    if (distance > range)
                    {
                        SensorImagesToRemove.Add(enemyShip, SensorImages[enemyShip]);
                    }
                }
            }

            if (!SensorImages.ContainsKey(SpaceScreen.PlayerShip))
            {
                AddImage(SpaceScreen.PlayerShip);
            }

            foreach (KeyValuePair<GameObject, Image> sensorImage in SensorImages)
            {
                if (SpaceScreen.Objects.Contains(sensorImage.Key))
                {
                    sensorImage.Value.SetPosition(new Vector2(ScreenManager.Viewport.Width / 2, ScreenManager.Viewport.Height / 2) + sensorImage.Key.Position / Camera2D.BackgroundMultiplier);
                }
                else if (SpaceScreen.EnemyShips.Contains(sensorImage.Key))
                {
                    sensorImage.Value.SetPosition(new Vector2(ScreenManager.Viewport.Width / 2, ScreenManager.Viewport.Height / 2) + sensorImage.Key.Position / Camera2D.BackgroundMultiplier);
                    sensorImage.Value.SetRotation(sensorImage.Key.Rotation);
                }
                else if (SpaceScreen.PlayerShip == (sensorImage.Key as PlayerShip))
                {
                    sensorImage.Value.SetPosition(new Vector2(ScreenManager.Viewport.Width / 2, ScreenManager.Viewport.Height / 2) + sensorImage.Key.Position / Camera2D.BackgroundMultiplier);
                    sensorImage.Value.SetRotation(sensorImage.Key.Rotation);
                }
                else
                {
                    SensorImagesToRemove.Add(sensorImage.Key, sensorImage.Value);
                }
            }

            foreach (KeyValuePair<GameObject, Image> image in SensorImagesToRemove)
            {
                RemoveScreenUIElement(image.Value);
                SensorImages.Remove(image.Key);
            }

            SensorImagesToRemove.Clear();
        }

        private void AddImage(GameObject gameObject)
        {
            // If the object is large enough, just add a smaller copy to the sensors screen
            if (gameObject.Bounds.Width > 80 || gameObject.Bounds.Height > 150)
            {
                Image sensorImage;

                if (gameObject.Data != null)
                {
                    sensorImage = new Image(
                        gameObject.Data.TextureAsset,
                        gameObject.Position / (Camera2D.BackgroundMultiplier),
                        new Vector2(1 / Camera2D.BackgroundMultiplier, 1 / Camera2D.BackgroundMultiplier),
                        gameObject.Data.Name,
                        gameObject.Rotation);
                    sensorImage.SetHoverInfoText(sensorImage.Name);
                }
                else
                {
                    string[] strings = gameObject.DataAsset.Split('/');

                    sensorImage = new Image(
                        gameObject.DataAsset,
                        gameObject.Position / (Camera2D.BackgroundMultiplier),
                        new Vector2(1 / Camera2D.BackgroundMultiplier, 1 / Camera2D.BackgroundMultiplier),
                        strings.Last(),
                        gameObject.Rotation);
                }

                sensorImage.SetTexture(gameObject.Texture);

                SensorImages.Add(gameObject, sensorImage);
                AddScreenUIElement(sensorImage);
            }
            else
            {
                string imageAsset = "Object";
                if (gameObject as EnemyShip != null)
                {
                    imageAsset = "Enemy";
                }
                else if (gameObject as PlayerShip != null)
                {
                    imageAsset = "Player";
                }

                Image sensorImage = new Image(
                    "",
                    new Vector2(ScreenManager.Viewport.Width / 2, ScreenManager.Viewport.Height / 2) +  gameObject.Position / (Camera2D.BackgroundMultiplier),
                    "Sensor Image");

                if (gameObject.Data != null) { sensorImage.SetHoverInfoText(gameObject.Data.Name); }

                sensorImage.SetTexture(Thumbnails[imageAsset]);
                AddScreenUIElement(sensorImage);

                SensorImages.Add(gameObject, sensorImage);
            }
        }
    }
}