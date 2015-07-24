using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToGalaxy.Gameplay_Objects;
using ToGalaxy.Gameplay_Objects.Space_Screen;
using ToGalaxy.Screens.Gameplay_Screens;
using ToGalaxyCustomData;
using ToGalaxyGameLibrary.Game_Objects;
using ToGalaxyGameLibrary.Screens_and_ScreenManager;
using ToGalaxyGameLibrary.UI;

namespace ToGalaxy.UI
{
    public class SensorsUI : Panel
    {
        private SensorsUIData SensorsUIData
        {
            get;
            set;
        }

        #region Markers

        private Texture2D PlayerMarkerTexture
        {
            get;
            set;
        }

        private Texture2D EnemyMarkerTexture
        {
            get;
            set;
        }

        #endregion

        // Hacky way of getting the gameobjects, but I think it beats passing lists in to several wrapper functions
        // MUCH simpler, but there may be a better way
        private SpaceScreen SpaceScreen
        {
            get;
            set;
        }

        private Camera2D Camera
        {
            get;
            set;
        }

        // A multiplier - a value of 1 means the range of the sensors is just the screen viewport
        // a value of 2 means the range of the snesors is twice the screen viewport
        public int Range
        {
            get;
            private set;
        }

        public SensorsUI(string dataAsset, Vector2 position, SpaceScreen spaceScreen, int range, Color colour, string name, float opacity = 0.3f)
            : base(dataAsset, position, colour, name, opacity)
        {
            paddingVector = new Vector2(0, 0);

            SpaceScreen = spaceScreen;
            Camera = SpaceScreen.ExtendedScreenManager.Camera;
            Range = range;
        }

        public SensorsUI(string dataAsset, Vector2 position, Vector2 dimensions, SpaceScreen spaceScreen, int range, Color colour, string name, float opacity = 0.3f)
            : base(dataAsset, position, dimensions, colour, name, opacity)
        {
            paddingVector = new Vector2(0, 0);

            SpaceScreen = spaceScreen;
            Camera = SpaceScreen.ExtendedScreenManager.Camera;
            Range = range;
        }

        public override void LoadContent(ContentManager content)
        {
            base.LoadContent(content);

            if (DataAsset != "")
            {
                SensorsUIData = content.Load<SensorsUIData>(DataAsset);

                if (SensorsUIData != null)
                {
                    PlayerMarkerTexture = content.Load<Texture2D>(SensorsUIData.PlayerMarkerAsset);
                    EnemyMarkerTexture = content.Load<Texture2D>(SensorsUIData.EnemyMarkerAsset);
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            SetRotation(Rotation + (float)gameTime.ElapsedGameTime.Milliseconds / 500);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // Add the new sensor markers
            ActiveUIElements.Clear();

            foreach (EnemyShip enemyShip in SpaceScreen.EnemyShips)
            {
                // Only add marker if the enemy is within sensor range of the player
                if ((enemyShip.Position - SpaceScreen.PlayerShip.Position).Length() <= Range)
                {
                    AddSensorMarker(enemyShip, EnemyMarkerTexture);
                }
            }

            // Always add player sensor marker - to show where player is
            AddSensorMarker(SpaceScreen.PlayerShip, PlayerMarkerTexture);

            base.Draw(spriteBatch);
        }

        private void AddSensorMarker(GameObject gameObject, Texture2D markerImage)
        {
            Vector2 sensorPosition = CalculateSensorPosition(gameObject);
            Image marker = new Image(SensorsUIData.EnemyMarkerAsset, sensorPosition, "Enemy Marker", gameObject.Rotation);
            marker.SetTexture(markerImage);

            LoadAndAddUIElement(marker, marker.Position);
        }

        private Vector2 CalculateSensorPosition(GameObject gameObject)
        {
            float tempSensPosX = (gameObject.Position.X + Camera.Position.X - SpaceScreen.ExtendedScreenManager.Viewport.Width / 2) / (float)Range;
            float tempSensPosY = (gameObject.Position.Y + Camera.Position.Y - SpaceScreen.ExtendedScreenManager.Viewport.Height / 2) / (float)Range;

            tempSensPosX = MathHelper.Clamp(tempSensPosX, -1, 1);
            tempSensPosY = MathHelper.Clamp(tempSensPosY, -1, 1);

            return new Vector2(Dimensions.X * tempSensPosX / 2, Dimensions.Y * tempSensPosY / 2);
        }
    }
}
