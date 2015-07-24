using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToGalaxyGameLibrary.Audio
{
    public class MySoundEffect
    {
        private string SoundEffectAsset
        {
            get;
            set;
        }

        public bool Loop
        {
            get;
            private set;
        }

        public float Volume
        {
            get;
            private set;
        }

        private SoundEffect Sound
        {
            get;
            set;
        }

        private SoundEffectInstance SoundInstance
        {
            get;
            set;
        }

        public MySoundEffect(string asset, bool looped = false, float volume = 1)
        {
            SoundEffectAsset = asset;
            Loop = looped;
            Volume = volume;
        }

        public void LoadContent(ContentManager content)
        {
            try
            {
                Sound = content.Load<SoundEffect>(SoundEffectAsset);
                SoundInstance = Sound.CreateInstance();
                SoundInstance.IsLooped = Loop;
                SoundInstance.Volume = Volume;
            }
            catch
            {

            }
        }

        public void Play()
        {
            if (SoundInstance != null)
            {
                if (SoundInstance.State == SoundState.Stopped)
                {
                    SoundInstance.Play();
                }
                else
                {
                    SoundInstance.Resume();
                }
            }
        }

        public void Stop()
        {
            if (SoundInstance != null)
            {
                SoundInstance.Stop(true);
            }
        }

        public void SetVolume(float volume)
        {
            if (SoundInstance != null)
            {
                Volume = volume;
                SoundInstance.Volume = Volume;
            }
        }
    }
}
