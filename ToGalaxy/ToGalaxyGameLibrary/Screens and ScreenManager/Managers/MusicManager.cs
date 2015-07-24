using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ToGalaxyGameLibrary.Audio;

namespace ToGalaxyGameLibrary.Screens_and_ScreenManager
{
    public class MusicManager
    {
        #region Music Dictionaries

        private Dictionary<string, MySong> MenuMusic
        {
            get;
            set;
        }

        private Dictionary<string, MySong> BattleMusic
        {
            get;
            set;
        }

        private Dictionary<string, MySong> AmbientMusic
        {
            get;
            set;
        }

        #endregion

        private string MusicDirectory
        {
            get;
            set;
        }

        public string CurrentMusicStylePlaying
        {
            get;
            private set;
        }

        public string CurrentSongPlaying
        {
            get;
            private set;
        }

        private List<string> PlayList
        {
            get;
            set;
        }

        bool playListIsPlaying = false;
        int playListCounter = 0;
        Random rand = new Random();

        public MusicManager(string musicDirectory)
        {
            MenuMusic = new Dictionary<string, MySong>();
            BattleMusic = new Dictionary<string, MySong>();
            AmbientMusic = new Dictionary<string, MySong>();
            PlayList = new List<string>();

            MusicDirectory = musicDirectory;
        }

        #region Load Content Functions

        public void LoadContent(ContentManager content)
        {
            LoadMenuMusic(content);
            LoadBattleMusic(content);
            LoadAmbientMusic(content);
        }

        private void LoadMenuMusic(ContentManager content)
        {
            DirectoryInfo directory = new DirectoryInfo(content.RootDirectory + "/" + MusicDirectory + "/Menu Music");
            if (!directory.Exists)
                throw new DirectoryNotFoundException();

            FileInfo[] files = directory.GetFiles("*.xnb*");
            foreach (FileInfo file in files)
            {
                string key = Path.GetFileNameWithoutExtension(file.Name);
                MySong song = new MySong(MusicDirectory + "/Menu Music" + "/" + key);

                song.LoadContent(content);
                AddSong(MenuMusic, song, key);
            }
        }

        private void LoadBattleMusic(ContentManager content)
        {
            DirectoryInfo directory = new DirectoryInfo(content.RootDirectory + "/" + MusicDirectory + "/Battle Music");
            if (!directory.Exists)
                throw new DirectoryNotFoundException();

            FileInfo[] files = directory.GetFiles("*.xnb*");
            foreach (FileInfo file in files)
            {
                string key = Path.GetFileNameWithoutExtension(file.Name);
                MySong song = new MySong(MusicDirectory + "/Battle Music" + "/" + key);

                song.LoadContent(content);
                AddSong(BattleMusic, song, key);
            }
        }

        private void LoadAmbientMusic(ContentManager content)
        {
            DirectoryInfo directory = new DirectoryInfo(content.RootDirectory + "/" + MusicDirectory + "/Ambient Music");
            if (!directory.Exists)
                throw new DirectoryNotFoundException();

            FileInfo[] files = directory.GetFiles("*.xnb*");
            foreach (FileInfo file in files)
            {
                string key = Path.GetFileNameWithoutExtension(file.Name);
                MySong song = new MySong(MusicDirectory + "/Ambient Music" + "/" + key);

                song.LoadContent(content);
                AddSong(AmbientMusic, song, key);
            }
        }

        private void AddSong(Dictionary<string, MySong> songDictionary, MySong song, string key)
        {
            if (!songDictionary.ContainsValue(song) && !songDictionary.ContainsKey(key))
            {
                songDictionary.Add(key, song);
            }
        }

        #endregion

        #region Playing Music Functions

        public void Play(string songName)
        {
            if (MenuMusic.ContainsKey(songName) == true)
            {
                MySong song;
                MenuMusic.TryGetValue(songName, out song);

                if (song != null)
                {
                    PlaySong(song);
                    CurrentMusicStylePlaying = "Menu";
                }
            }
            else if (BattleMusic.ContainsKey(songName) == true)
            {
                MySong song;
                BattleMusic.TryGetValue(songName, out song);

                if (song != null)
                {
                    PlaySong(song);
                    CurrentMusicStylePlaying = "Battle";
                }
            }
            else if (AmbientMusic.ContainsKey(songName) == true)
            {
                MySong song;
                AmbientMusic.TryGetValue(songName, out song);

                if (song != null)
                {
                    PlaySong(song);
                    CurrentMusicStylePlaying = "Ambient";
                }
            }
        }

        private void PlaySong(MySong song)
        {
            // Only play the song if it is not already playing - otherwise just do nothing
            if (CurrentSongPlaying != song.Song.Name)
            {
                MediaPlayer.Play(song.Song);
                CurrentSongPlaying = song.Song.Name;
                MediaPlayer.Volume = ScreenManager.Settings.OptionsData.MusicVolume;
            }
        }

        public void Play(List<string> songNames)
        {
            PlayList.Clear();

            playListCounter = 0;
            playListIsPlaying = true;

            foreach (string songName in songNames)
            {
                PlayList.Add(songName);
            }

            Play(PlayList[playListCounter]);
        }

        public void PlayMenuMusic()
        {
            playListIsPlaying = false;
            int songNumber = rand.Next(MenuMusic.Count);

            PlaySong(MenuMusic.Values.ElementAt(songNumber));
            CurrentMusicStylePlaying = "Menu";
        }

        public void PlayBattleMusic()
        {
            playListIsPlaying = false;
            int songNumber = rand.Next(BattleMusic.Count);

            PlaySong(BattleMusic.Values.ElementAt(songNumber));
            CurrentMusicStylePlaying = "Battle";
        }

        public void PlayAmbientMusic()
        {
            playListIsPlaying = false;
            int songNumber = rand.Next(AmbientMusic.Count);

            PlaySong(AmbientMusic.Values.ElementAt(songNumber));
            CurrentMusicStylePlaying = "Ambient";
        }

        #endregion

        public void Stop()
        {
            MediaPlayer.Stop();
        }

        public void Update()
        {
            MediaPlayer.Volume = ScreenManager.Settings.OptionsData.MusicVolume;

            if (playListIsPlaying)
            {
                if (MediaPlayer.State == MediaState.Stopped)
                {
                    playListCounter++;
                    playListCounter %= PlayList.Count;

                    Play(PlayList[playListCounter]);
                }
            }
        }
    }
}
