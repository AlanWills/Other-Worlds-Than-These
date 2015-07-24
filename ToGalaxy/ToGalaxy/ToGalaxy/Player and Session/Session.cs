using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToGalaxy.Gameplay_Objects;

namespace ToGalaxy.Player_and_Session
{
    public class Session
    {
        public PlayerShip PlayerShip
        {
            get;
            set;
        }

        public int CurrentLevel
        {
            get;
            private set;
        }

        public int Money
        {
            get;
            private set;
        }

        public Session()
        {
            CurrentLevel = 3;
            Money = 300000;
        }

        public void LoadContent(ContentManager content)
        {
            try
            {
                PlayerShip.LoadContent(content);
            }
            catch
            {

            }
        }

        public void AddMoney(int money)
        {
            Money += money;
        }
    }
}
