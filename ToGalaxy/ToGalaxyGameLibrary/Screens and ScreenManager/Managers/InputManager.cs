using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToGalaxyGameLibrary.Screens_and_ScreenManager.Managers
{
    public class InputManager
    {
        public KeyboardState CurrentKeyboardState
        {
            get;
            private set;
        }

        public KeyboardState PreviousKeyboardState
        {
            get;
            private set;
        }

        public Keys[] PressedKeys
        {
            get
            {
                return CurrentKeyboardState.GetPressedKeys();
            }
        }

        public InputManager()
        {

        }

        public void Update()
        {
            PreviousKeyboardState = CurrentKeyboardState;
            CurrentKeyboardState = Keyboard.GetState();
        }

        public bool IsKeyDown(Keys key)
        {
            return CurrentKeyboardState.IsKeyDown(key);
        }
    }
}
