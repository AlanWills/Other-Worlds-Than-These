using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToGalaxyCustomData;
using ToGalaxyGameLibrary.Screens_and_ScreenManager;

namespace ToGalaxyGameLibrary.Game_Objects
{
    public enum GameObjectStatus
    {
        Alive,
        Dead
    }

    public class GameObject
    {
        #region XML Data

        public string DataAsset
        {
            get;
            private set;
        }

        public GameObjectData Data
        {
            get;
            protected set;
        }

        #endregion

        #region Spatial Information (Positions, Rotations, Origins etc.)

        public Vector2 Position
        {
            get;
            protected set;
        }

        public float Rotation
        {
            get;
            protected set;
        }

        public Vector2 Origin
        {
            get
            {
                if (Texture != null)
                {
                    return new Vector2(Texture.Width / 2, Texture.Height / 2);
                }
                else
                {
                    return Vector2.Zero;
                }
            }
        }

        public Vector2 Centre
        {
            get
            {
                return Position + Origin;
            }
        }

        public Vector2 Scale
        {
            get;
            protected set;
        }

        #endregion

        #region Physics Properties - Velocities etc.

        // Velocity in local space with positive y up and positive x to the right
        public Vector2 Velocity
        {
            get;
            protected set;
        }

        public bool Moving
        {
            get;
            protected set;
        }

        #endregion

        #region Visual Information (Textures etc.)

        public Texture2D Texture
        {
            get;
            protected set;
        }

        protected Color Colour
        {
            get;
            set;
        }

        public float Opacity
        {
            get;
            protected set;
        }

        #endregion

        #region Selection Bool and Click Bounds

        public bool Selected
        {
            get;
            protected set;
        }

        public Rectangle Bounds
        {
            get
            {
                if (Texture != null)
                {
                    return new Rectangle((int)(Position.X - Texture.Width / 2), (int)(Position.Y - Texture.Height / 2), Texture.Width, Texture.Height);
                }
                else
                {
                    return new Rectangle((int)Position.X, (int)Position.Y, 0, 0);
                }
            }
        }

        #endregion

        public GameObjectStatus Status
        {
            get;
            private set;
        }

        public GameObject(string dataAsset)
        {
            DataAsset = dataAsset;
            Selected = false;
            Colour = Color.White;
            Opacity = 1f;
            Scale = new Vector2(1, 1);
            Status = GameObjectStatus.Alive;
        }

        public GameObject(string dataAsset, Vector2 startingPosition)
        {
            DataAsset = dataAsset;
            Position = startingPosition;
            Selected = false;
            Colour = Color.White;
            Opacity = 1f;
            Scale = new Vector2(1, 1);
            Status = GameObjectStatus.Alive;
        }

        public virtual void LoadContent(ContentManager content)
        {
            if (DataAsset != "")
            {
                try
                {
                    // Try to see if we have a string for just a texture
                    Texture = content.Load<Texture2D>(DataAsset);
                }
                catch
                {
                    Data = content.Load<GameObjectData>(DataAsset);

                    if (Data != null)
                    {
                        if (Data.TextureAsset != "")
                        {
                            Texture = content.Load<Texture2D>(Data.TextureAsset);
                        }
                    }
                    else
                    {
                        throw new NullReferenceException();
                    }
                }
            }
            else
            {
                throw new NullReferenceException();
            }
        }

        public virtual void Update(GameTime gameTime)
        {
            if (Status == GameObjectStatus.Alive)
            {
                if (Velocity.LengthSquared() >= 0.1f)
                {
                    float sinRot = (float)Math.Sin(Rotation);
                    float cosRot = (float)Math.Cos(Rotation);

                    // Tried to add an x component for velocity here, but it's giving some weird results.
                    // For now assuming that velocity is purely a y component vector.
                    Position += new Vector2(cosRot * Velocity.X + sinRot * Velocity.Y, sinRot * Velocity.X - cosRot * Velocity.Y);
                    Moving = true;
                }
                else
                {
                    Moving = false;
                }
            }
        }

        public virtual void CheckMouseInteraction(InGameMouse mouse)
        {
            if (Status == GameObjectStatus.Alive)
            {
                if (mouse.IsLeftClicked)
                {
                    Point clickPoint = new Point((int)mouse.LastLeftClickedPosition.X, (int)mouse.LastLeftClickedPosition.Y);

                    if (Bounds.Contains(clickPoint))
                    {
                        Selected = true;
                    }
                    else
                    {
                        Selected = false;
                    }
                }
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (Status == GameObjectStatus.Alive)
            {
                if (Texture != null)
                {
                    spriteBatch.Draw(Texture, Position, null, Colour * Opacity, Rotation, Origin, Scale, SpriteEffects.None, 0);
                }
            }
        }

        public void SetPosition(Vector2 position)
        {
            Position = position;
        }

        public void SetPosition(Point position)
        {
            Position = new Vector2((int)position.X, (int)position.Y);
        }

        public void SetRotation(float rotation)
        {
            Rotation = rotation;
        }

        public void SetScale(Vector2 scale)
        {
            Scale = scale;
        }

        public void SetOpacity(float opacity)
        {
            Opacity = opacity;
        }

        public void SetColour(Color colour)
        {
            Colour = colour;
        }

        public void SetVelocity(Vector2 velocity)
        {
            Velocity = velocity;
        }

        public void IncreaseVelocity(Vector2 deltaVelocity)
        {
            Velocity += deltaVelocity;
        }

        public bool IsAlive()
        {
            return Status == GameObjectStatus.Alive;
        }

        public virtual void Alive()
        {
            Status = GameObjectStatus.Alive;
        }

        public virtual void Die()
        {
            Status = GameObjectStatus.Dead;
        }

        /*
        public virtual GameObject Clone()
        {
            GameObject clone = new GameObject(DataAsset);

            clone.Data = Data;
            clone.Position = Position;
            clone.Rotation = Rotation;
            clone.Velocity = Velocity;
            clone.Texture = Texture;
            clone.Opacity = Opacity;
            clone.Selected = Selected;

            return clone;
        }*/
    }
}
