using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace TransvoxelXnaStudio.Framework
{
    public abstract class MouseKeyboard
    {
        [Flags]
        private enum KeyStates
        {
            Down = 1,
            None = 0,
            Toggled = 2
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        private static extern short GetKeyState(int keyCode);

        private static KeyStates GetKeyState(Keys key)
        {
            KeyStates state = KeyStates.None;

            short retVal = GetKeyState((int)key);

            //If the high-order bit is 1, the key is down; otherwise, it is up.
            if ((retVal & 0x8000) == 0x8000)
                state |= KeyStates.Down;

            //If the low-order bit is 1, the key is toggled.
            if ((retVal & 1) == 1)
                state |= KeyStates.Toggled;

            return state;
        }

        public static bool IsKeyDown(Keys key)
        { return (GetKeyState(key) & KeyStates.Down) == KeyStates.Down; }

        public static bool IsKeyToggled(Keys key)
        { return (GetKeyState(key) & KeyStates.Toggled) == KeyStates.Toggled; }
    }
}