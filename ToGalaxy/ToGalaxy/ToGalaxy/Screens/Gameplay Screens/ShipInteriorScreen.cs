using FuncWorks.XNA.XTiled;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToGalaxy.Gameplay_Objects.Ship_Interior_Screen;
using ToGalaxyCustomData;
using ToGalaxyGameLibrary.Screens_and_ScreenManager;

namespace ToGalaxy.Screens.Gameplay_Screens
{
    public class ShipInteriorScreen : Screen
    {
        #region Ship Interior Screen Data

        public ShipInteriorScreenData Data
        {
            get;
            private set;
        }

        #endregion

        #region Interior Tile Map

        public Map Map
        {
            get;
            private set;
        }

        private Rectangle MapViewRectangle
        {
            get;
            set;
        }

        #endregion

        #region Crew List

        private List<CrewMember> Crew
        {
            get;
            set;
        }

        #endregion

        public ShipInteriorScreen(ExtendedScreenManager screenManager, string dataAsset)
            : base(screenManager, dataAsset)
        {
            Crew = new List<CrewMember>();
            ScreenState = ScreenState.Hidden;
        }

        public override void LoadContent()
        {
            base.LoadContent();

            if (ScreenDataAsset != "")
            {
                Data = ScreenManager.Content.Load<ShipInteriorScreenData>(ScreenDataAsset);

                if (Data != null)
                {
                    Map = ScreenManager.Content.Load<Map>(Data.TileMapAsset);
                    MapViewRectangle = ScreenManager.Camera.ViewRectangle;
                }
            }

            SetUpCrew();
        }

        private void SetUpCrew()
        {
            CrewMember crew = new CrewMember(
                                    "XML/Crew/HumanCrewMember",
                                    new Vector2(Map.Bounds.Center.X, Map.Bounds.Center.Y));

            LoadAndAddGameObject(crew);
            Crew.Add(crew);
        }

        public override void Update(GameTime gameTime)
        {
            if (ScreenState == ScreenState.Active || ScreenState == ScreenState.Hidden)
            {
                CheckForScreenSwap();
            }


            if (ScreenManager.Mouse.IsRightClicked)
            {
                List<TileData> clickedTiles = (List<TileData>)Map.GetTilesInRegion(new Rectangle((int)ScreenManager.Mouse.LastRightClickedPosition.X, (int)ScreenManager.Mouse.LastRightClickedPosition.Y, 1, 1));

                if (clickedTiles != null)
                {
                    Vector2 position = new Vector2(
                        (int)(clickedTiles[0].Target.Center.X - clickedTiles[0].Target.Width / 2),
                        (int)(clickedTiles[0].Target.Center.Y - clickedTiles[0].Target.Height / 2));

                    Crew[0].SetPosition(position);
                }
            }

            base.Update(gameTime);
        }

        private void CheckForScreenSwap()
        {
            if ((ScreenManager.Input.IsKeyDown(Keys.F)) && (ScreenManager.Input.PreviousKeyboardState.IsKeyUp(Keys.F)))
            {
                if (ScreenState == ScreenState.Active)
                {
                    ScreenState = ScreenState.Hidden;
                }
                else if (ScreenState == ScreenState.Hidden)
                {
                    ScreenState = ScreenState.Active;
                    ScreenManager.Camera.SetPosition(new Vector2(ScreenManager.Viewport.Width / 2 - Map.Bounds.Center.X, ScreenManager.Viewport.Height / 2 - Map.Bounds.Center.Y));
                }
            }
        }

        public override void DrawBackground(SpriteBatch spriteBatch)
        {
            if (ScreenState == ScreenState.Active || ScreenState == ScreenState.Frozen)
            {
                if (Map != null)
                {
                    Map.Draw(spriteBatch, ScreenManager.Camera.ViewRectangle);
                }
            }
        }
    }
}
