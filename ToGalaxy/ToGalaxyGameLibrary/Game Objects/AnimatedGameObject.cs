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
    public class AnimatedGameObject : GameObject
    {
        #region Data

        public AnimatedGameObjectData AnimationData
        {
            get;
            private set;
        }

        #endregion

        #region Animation Data

        private Rectangle SourceRectangle
        {
            get;
            set;
        }
        
        // The pixel dimensions of one frame of the animation
        public Vector2 FrameDimensions
        {
            get;
            private set;
        }

        public int CurrentFrame
        {
            get;
            private set;
        }

        protected bool IsPlaying
        {
            get;
            set;
        }

        protected bool Continual
        {
            get;
            set;
        }

        #endregion

        public Rectangle AnimationBounds
        {
            get
            {
                return new Rectangle(
                    (int)(Position.X - FrameDimensions.X / 2),
                    (int)(Position.Y - FrameDimensions.Y / 2),
                    (int)FrameDimensions.Y,
                    (int)FrameDimensions.X);
            }
        }

        private float currentTimeOnFrame = 0;

        public AnimatedGameObject(string dataAsset, bool isPlaying = false, bool continual = true)
            : base(dataAsset)
        {
            IsPlaying = isPlaying;
            Continual = continual;
        }

        public AnimatedGameObject(string dataAsset, Vector2 startingPosition, bool isPlaying = false, bool continual = true)
            : base(dataAsset, startingPosition)
        {
            IsPlaying = isPlaying;
            Continual = continual;
        }

        public override void LoadContent(ContentManager content)
        {
            base.LoadContent(content);

            if (DataAsset != "")
            {
                AnimationData = content.Load<AnimatedGameObjectData>(DataAsset);

                if (AnimationData != null)
                {
                    FrameDimensions = new Vector2(Texture.Width / AnimationData.SpriteSheetFrameDimensions.X, Texture.Height / AnimationData.SpriteSheetFrameDimensions.Y);
                    CurrentFrame = AnimationData.DefaultFrame;
                }
            }
        }

        // frame is 0 indexed
        private void SetSourceRectangleBasedOnFrame(int frame)
        {
            if (AnimationData != null)
            {
                int cols = (int)AnimationData.SpriteSheetFrameDimensions.X;

                SourceRectangle = new Rectangle(
                                        (int)((frame % cols) * FrameDimensions.X), 
                                        (int)((frame / cols) * FrameDimensions.Y), 
                                        (int)FrameDimensions.X, 
                                        (int)FrameDimensions.Y);
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (IsPlaying)
            {
                currentTimeOnFrame += (float)(gameTime.ElapsedGameTime.Milliseconds) / 1000;

                if (currentTimeOnFrame >= AnimationData.TimePerFrame)
                {
                    CurrentFrame++;

                    // If the animation should only play once, we remove it once it reaches the last frame
                    if (!Continual)
                    {
                        if (CurrentFrame == AnimationData.SpriteSheetFrameDimensions.X * AnimationData.SpriteSheetFrameDimensions.Y - 1)
                        {
                            Die();
                        }
                    }
                    else
                    {
                        CurrentFrame %= (int)(AnimationData.SpriteSheetFrameDimensions.X * AnimationData.SpriteSheetFrameDimensions.Y);
                    }

                    currentTimeOnFrame = 0;
                }
            }
            else
            {
                CurrentFrame = AnimationData.DefaultFrame;
            }

            SetSourceRectangleBasedOnFrame(CurrentFrame);
        }

        public override void CheckMouseInteraction(InGameMouse mouse)
        {
            Point clickPoint = new Point((int)InGameMouse.InGamePosition.X, (int)InGameMouse.InGamePosition.Y);

            if (Texture != null)
            {
                if (AnimationBounds.Contains(clickPoint))
                {
                    Selected = true;
                }
                else
                {
                    Selected = false;
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Texture != null)
            {
                // The origin of the texture for drawing is defined as the centre of the first image in the sprite sheet because this is how XNA draws it.
                // Saves a lot of calculations with offsets etc.
                spriteBatch.Draw(Texture, Position, SourceRectangle, Color.White * Opacity, Rotation, new Vector2(FrameDimensions.X / 2, FrameDimensions.Y / 2), Scale, SpriteEffects.None, 0);
            }
        }

        public void Play()
        {
            IsPlaying = true;
        }

        public void Stop()
        {
            IsPlaying = false;
        }

        public AnimatedGameObject Clone()
        {
            AnimatedGameObject gameObject = new AnimatedGameObject(DataAsset, Position, false, Continual);

            gameObject.AnimationData = AnimationData;
            gameObject.Texture = Texture;
            gameObject.Rotation = Rotation;
            gameObject.Scale = Scale;
            gameObject.Opacity = 1;
            gameObject.FrameDimensions = FrameDimensions;
            gameObject.CurrentFrame = AnimationData.DefaultFrame;

            gameObject.SetSourceRectangleBasedOnFrame(gameObject.CurrentFrame);

            return gameObject;
        }
    }
}
