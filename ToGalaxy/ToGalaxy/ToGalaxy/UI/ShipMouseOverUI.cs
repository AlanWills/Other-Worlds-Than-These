using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToGalaxy.Gameplay_Objects;
using ToGalaxy.Gameplay_Objects.Space_Screen;
using ToGalaxyGameLibrary.Screens_and_ScreenManager;
using ToGalaxyGameLibrary.UI;

namespace ToGalaxy.UI
{
    public class ShipMouseOverUI : MouseOverUI
    {
        private Bar ShieldBar
        {
            get;
            set;
        }

        private Bar ArmourBar
        {
            get;
            set;
        }

        private Bar HullBar
        {
            get;
            set;
        }

        private CommandMarker DestinationMarker
        {
            get;
            set;
        }

        private List<Image> AttackTargetMarkers
        {
            get;
            set;
        }

        private Ship ParentShip
        {
            get;
            set;
        }

        public ShipMouseOverUI(Ship parentShip, string dataAsset, Vector2 position, Vector2 dimensions, Color colour, string name, float opacity = 0.25f)
            : base(parentShip, dataAsset, position, dimensions, colour, name, opacity)
        {
            ParentShip = parentShip;
            AttackTargetMarkers = new List<Image>();
        }

        public override void LoadContent(ContentManager content)
        {
            base.LoadContent(content);

            SetUpBars(content);
        }

        private void SetUpBars(ContentManager content)
        {
            ArmourBar = new Bar(
                ParentShip.Armour,
                "Sprites/UI/Bars/ArmourBar",
                ParentShip.Position - new Vector2(0, ParentShip.Bounds.Height / 2 + 12),
                new Vector2((float)ParentShip.Bounds.Width / 180, 0.5f),
                "Armour Bar");
            ArmourBar.LoadContent(content);
            ArmourBar.EnableAndDisableEvent += MouseOverActivationEvent;

            HullBar = new Bar(
                ParentShip.Hull,
                "Sprites/UI/Bars/HullBar",
                ParentShip.Position - new Vector2(0, ParentShip.Bounds.Height / 2 + 5),
                new Vector2((float)ParentShip.Bounds.Width / 180, 0.5f),
                "Hull Bar");
            HullBar.LoadContent(content);
            HullBar.EnableAndDisableEvent += MouseOverActivationEvent;

            if (ParentShip.Shield != null)
            {
                ShieldBar = new Bar(
                    ParentShip.Shield.ShieldData.Strength,
                    "Sprites/UI/Bars/ShieldBar",
                    ParentShip.Position - new Vector2(0, ParentShip.Bounds.Height / 2 + 19),
                    new Vector2((float)ParentShip.Bounds.Width / 180, 0.5f),
                    "Shield Bar");
                ShieldBar.LoadContent(content);
                ShieldBar.EnableAndDisableEvent += MouseOverActivationEvent;
            }

            DestinationMarker = new CommandMarker(
                "Sprites/UI/Command Markers/MoveMarker",
                ParentShip.Destination,
                "Destination Marker");
            DestinationMarker.LoadContent(content);
            DestinationMarker.EnableAndDisableEvent += MouseOverActivationEvent;

            foreach (Turret turret in ParentShip.Turrets)
            {
                Vector2 targetPosition = turret.Target != null ? turret.Target.Position : ParentShip.Position;
                Image attackTargetMarker = new Image(
                    "Sprites/UI/Command Markers/AttackMarker",
                    targetPosition,
                    "Turret Target Marker");
                attackTargetMarker.LoadContent(content);
                attackTargetMarker.EnableAndDisableEvent += MouseOverActivationEvent;
                AttackTargetMarkers.Add(attackTargetMarker);
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (ParentShip != null)
            {
                SetPosition(ParentShip.Position);

                if (ParentShip.Shield != null)
                {
                    ShieldBar.Update(gameTime, ParentShip.Shield.ShieldStrength, ParentShip.Position - new Vector2(ParentShip.Bounds.Width / 2, ParentShip.Bounds.Height / 2 + 19));
                }

                ArmourBar.Update(gameTime, ParentShip.Armour, ParentShip.Position - new Vector2(ParentShip.Bounds.Width / 2, ParentShip.Bounds.Height / 2 + 12));
                HullBar.Update(gameTime, ParentShip.Hull, ParentShip.Position - new Vector2(ParentShip.Bounds.Width / 2, ParentShip.Bounds.Height / 2 + 5));

                DestinationMarker.SetPosition(ParentShip.Destination);
                DestinationMarker.Update(gameTime);
                
                for (int i = 0; i < ParentShip.Turrets.Count; i++)
                {
                    Vector2 targetPosition = ParentShip.Turrets[i].Target != null ? ParentShip.Turrets[i].Target.Position : ParentShip.Position;
                    if (ParentShip.Turrets[i].Target != null)
                    {
                        AttackTargetMarkers[i].SetPosition(ParentShip.Turrets[i].Target.Position);
                        AttackTargetMarkers[i].Update(gameTime);
                        if (IsActive())
                        {
                            AttackTargetMarkers[i].SetOpacity(AttackTargetMarkers[i].Opacity - 0.01f);
                        }
                        else
                        {
                            AttackTargetMarkers[i].SetOpacity(1f);
                        }
                    }
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            if (IsActive())
            {
                if (ParentShip != null)
                {
                    if (ParentShip.Shield != null)
                    {
                        ShieldBar.Draw(spriteBatch);
                    }

                    ArmourBar.Draw(spriteBatch);
                    HullBar.Draw(spriteBatch);

                    if ((DestinationMarker.Position - ParentShip.Position).LengthSquared() > 1f)
                    {
                        DestinationMarker.Draw(spriteBatch);
                    }
                    
                    for (int i = 0; i < ParentShip.Turrets.Count; i++)
                    {
                        if (ParentShip.Turrets[i].Target != null)
                        {
                            AttackTargetMarkers[i].Draw(spriteBatch);
                        }
                    }
                }
            }
        }

        private void MouseOverActivationEvent(object sender, EventArgs e)
        {
            UIElement uielement = sender as UIElement;
            if (uielement != null)
            {
                if (ParentShip.Bounds.Contains(new Point((int)InGameMouse.InGamePosition.X, (int)InGameMouse.InGamePosition.Y)))
                {
                    uielement.Activate();
                }
                else
                {
                    uielement.DisableAndHide();
                }
            }
        }
    }
}
