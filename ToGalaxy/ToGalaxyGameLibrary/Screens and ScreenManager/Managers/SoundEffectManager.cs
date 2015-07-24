using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ToGalaxyGameLibrary.Audio;

namespace ToGalaxyGameLibrary.Screens_and_ScreenManager
{
    public class SoundEffectManager
    {
        private Dictionary<string, MySoundEffect> SoundEffects
        {
            get;
            set;
        }

        private string SoundEffectsDirectory
        {
            get;
            set;
        }

        public SoundEffectManager(string soundEffectsDirectory)
        {
            SoundEffects = new Dictionary<string, MySoundEffect>();

            SoundEffectsDirectory = soundEffectsDirectory;
        }

        public void LoadContent(ContentManager content)
        {
            DirectoryInfo directory = new DirectoryInfo(content.RootDirectory + "/" + SoundEffectsDirectory);
            if (!directory.Exists)
                throw new DirectoryNotFoundException();

            foreach (DirectoryInfo dir in directory.GetDirectories())
            {
                LoadSubDirectoryFiles(content, dir);
            }
        }

        private void LoadSubDirectoryFiles(ContentManager content, DirectoryInfo subDirectory)
        {
            DirectoryInfo directory = new DirectoryInfo(content.RootDirectory + "/" + SoundEffectsDirectory + "/" + subDirectory.Name);
            if (!directory.Exists)
                throw new DirectoryNotFoundException();

            FileInfo[] files = directory.GetFiles("*.xnb*");
            foreach (FileInfo file in files)
            {
                string key = Path.GetFileNameWithoutExtension(file.Name);
                MySoundEffect soundEffect = new MySoundEffect(SoundEffectsDirectory + "/" + subDirectory.Name + "/" + key);

                soundEffect.LoadContent(content);
                SoundEffects.Add(SoundEffectsDirectory + "/" + subDirectory.Name + "/" + key, soundEffect);
            }
        }

        public void Play(string fileNameOfSoundEffect)
        {
            if (SoundEffects.ContainsKey(fileNameOfSoundEffect))
            {
                SoundEffects[fileNameOfSoundEffect].SetVolume(ScreenManager.Settings.OptionsData.SoundEffectsVolume * SoundEffects[fileNameOfSoundEffect].Volume);
                SoundEffects[fileNameOfSoundEffect].Play();
            }
        }

        public void Stop(string fileNameOfSoundEffect)
        {
            if (SoundEffects.ContainsKey(fileNameOfSoundEffect))
            {
                SoundEffects[fileNameOfSoundEffect].Stop();
            }
        }
    }
}
