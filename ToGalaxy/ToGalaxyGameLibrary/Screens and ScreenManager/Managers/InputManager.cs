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
        public static KeyboardState CurrentKeyboardState
        {
            get;
            private set;
        }

        public static KeyboardState PreviousKeyboardState
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

        // Makes the KeyPressed and KeyReleased methods return false by setting the previous keyboardstate to the current keyboardstate
        public static void Flush()
        {
            PreviousKeyboardState = CurrentKeyboardState;
        }

        public static bool KeyReleased(Keys key)
        {
            return CurrentKeyboardState.IsKeyUp(key) && PreviousKeyboardState.IsKeyDown(key);
        }

        public static bool KeyPressed(Keys key)
        {
            return CurrentKeyboardState.IsKeyDown(key) && PreviousKeyboardState.IsKeyUp(key);
        }

        public static bool IsKeyDown(Keys key)
        {
            return CurrentKeyboardState.IsKeyDown(key);
        }
    }
}
