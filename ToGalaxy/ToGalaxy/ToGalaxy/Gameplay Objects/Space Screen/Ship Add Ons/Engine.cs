using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToGalaxyCustomData;
using ToGalaxyGameLibrary.Audio;
using ToGalaxyGameLibrary.Game_Objects;
using ToGalaxyGameLibrary.Screens_and_ScreenManager;
using ToGalaxyGameLibrary.UI;

namespace ToGalaxy.Gameplay_Objects.Space_Screen
{
    public class Engine : GameObject
    {
        public EngineData EngineData
        {
            get;
            private set;
        }

        private GameObject ParentGameObject
        {
            get;
            set;
        }

        private Vector2 EngineHardPoint
        {
            get;
            set;
        }

        public float Speed
        {
            get
            {
                return EngineData.EngineSpeed * EngineTrail.Opacity;
            }
        }

        public float RotateSpeed
        {
            get
            {
                return EngineData.EngineRotateSpeed * EngineTrail.Opacity;
            }
        }

        private Vector2 TrailScale
        {
            get;
            set;
        }

        #region Engine Decals and Effects

        public MySoundEffect EngineSoundEffect
        {
            get;
            private set;
        }

        public AnimatedGameObject EngineTrail
        {
            get;
            private set;
        }

        private AnimatedGameObject EngineSmoke
        {
            get;
            set;
        }

        private List<AnimatedGameObject> ActiveSmoke
        {
            get;
            set;
        }

        private List<AnimatedGameObject> SmokeToAdd
        {
            get;
            set;
        }

        private List<AnimatedGameObject> SmokeToRemove
        {
            get;
            set;
        }

        #endregion

        private float smokeTimer = 0;

        public Engine(string dataAsset, GameObject parentGameObject, Vector2 engineHardPoint)
            : base(dataAsset, parentGameObject.Position + engineHardPoint)
        {
            ParentGameObject = parentGameObject;
            EngineHardPoint = engineHardPoint;

            ActiveSmoke = new List<AnimatedGameObject>();
            SmokeToAdd = new List<AnimatedGameObject>();
            SmokeToRemove = new List<AnimatedGameObject>();
        }

        public override void LoadContent(ContentManager content)
        {
            base.LoadContent(content);

            EngineData = content.Load<EngineData>(DataAsset);

            if (EngineData != null)
            {
                SetUpEffects(content);
            }
        }

        private void SetUpEffects(ContentManager content)
        {
            // Position and rotation do not matter for now - need to update them when the ship is moving
            EngineTrail = new AnimatedGameObject("XML/FX/EngineTrail", Vector2.Zero);
            EngineTrail.LoadContent(content);
            TrailScale = new Vector2(Math.Min(ParentGameObject.Bounds.Width / 120f, 1), Math.Min(ParentGameObject.Bounds.Height / 100f, 2));
            EngineTrail.SetScale(TrailScale);
            EngineTrail.SetOpacity(0);

            EngineSmoke = new AnimatedGameObject("XML/FX/Smoke", Vector2.Zero, false, false);
            EngineSmoke.LoadContent(content);
            EngineSmoke.SetScale(new Vector2(EngineTrail.FrameDimensions.X * EngineTrail.Scale.X / (2 * EngineSmoke.FrameDimensions.X), EngineTrail.FrameDimensions.Y * EngineTrail.Scale.X / (2 * EngineSmoke.FrameDimensions.Y)));

            EngineSoundEffect = new MySoundEffect(EngineData.EngineSoundAsset, false, ScreenManager.Settings.OptionsData.SoundEffectsVolume * 0.03f);
            EngineSoundEffect.LoadContent(content);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            UpdateEngineSmoke(gameTime);

            float sinRot = (float)Math.Sin(Rotation);
            float cosRot = (float)Math.Cos(Rotation);

            if (ParentGameObject != null)
            {
                SetRotation(ParentGameObject.Rotation);

                sinRot = (float)Math.Sin(Rotation);
                cosRot = (float)Math.Cos(Rotation);
                SetPosition(ParentGameObject.Position + new Vector2(cosRot * EngineHardPoint.X- sinRot * EngineHardPoint.Y, sinRot * EngineHardPoint.X + cosRot * EngineHardPoint.Y));
            }

            EngineTrail.Update(gameTime);
            EngineTrail.SetRotation(Rotation);

            if (ParentGameObject.Velocity.LengthSquared() >= 0.1f || ParentGameObject.Moving)
            {
                smokeTimer += (float)gameTime.ElapsedGameTime.Milliseconds / 1000f;

                float opacity = (float)Math.Min(1, EngineTrail.Opacity + (float)gameTime.ElapsedGameTime.Milliseconds / 750);
                EngineTrail.SetOpacity(opacity);
                EngineTrail.Play();
                EngineSoundEffect.Play();

                if (smokeTimer > 0.1f)
                {
                    AnimatedGameObject smoke = EngineSmoke.Clone();
                    smoke.SetPosition(EngineTrail.Position + new Vector2(-sinRot * EngineTrail.FrameDimensions.Y * EngineTrail.Scale.Y * 0.5f, cosRot * EngineTrail.FrameDimensions.Y * EngineTrail.Scale.Y * 0.5f));
                    smoke.SetRotation(EngineTrail.Rotation + MathHelper.Pi);
                    SmokeToAdd.Add(smoke);
                    smokeTimer = 0;
                }
            }
            else
            {
                float opacity = (float)Math.Max(0, EngineTrail.Opacity - (float)gameTime.ElapsedGameTime.Milliseconds / 500);
                if (opacity > 0)
                {
                    EngineTrail.SetOpacity(opacity);
                }
                else
                {
                    EngineTrail.Stop();
                }

                EngineSoundEffect.Stop();
                smokeTimer = 0;
            }

            EngineTrail.SetPosition(Position + new Vector2(-sinRot * EngineTrail.Scale.X * EngineTrail.FrameDimensions.X / 2, cosRot * EngineTrail.Scale.X * EngineTrail.FrameDimensions.X / 2));
        }

        private void UpdateEngineSmoke(GameTime gameTime)
        {
            foreach (AnimatedGameObject smoke in SmokeToAdd)
            {
                smoke.Play();
                ActiveSmoke.Add(smoke);
            }

            SmokeToAdd.Clear();

            foreach (AnimatedGameObject smoke in ActiveSmoke)
            {
                smoke.SetOpacity(0.25f - (float)smoke.CurrentFrame / (float)(4 * smoke.AnimationData.SpriteSheetFrameDimensions.X * smoke.AnimationData.SpriteSheetFrameDimensions.Y));
                smoke.Update(gameTime);

                if (smoke.Status == GameObjectStatus.Dead)
                {
                    SmokeToRemove.Add(smoke);
                }
            }

            foreach (AnimatedGameObject smoke in SmokeToRemove)
            {
                smoke.Stop();
                ActiveSmoke.Remove(smoke);
            }

            SmokeToRemove.Clear();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            EngineTrail.Draw(spriteBatch);

            foreach (AnimatedGameObject smoke in ActiveSmoke)
            {
                smoke.Draw(spriteBatch);
            }
        }
    }
}
