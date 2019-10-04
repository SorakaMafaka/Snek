using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

// This class is going to take care of checking if buttons are pressed
namespace Snek
{
    class Input
    {
        // This is a list of available buttons on the keyboard
        private static Hashtable keyTable = new Hashtable();


        public static bool KeyPressed(Keys key)
        {
            if(keyTable[key] == null)
            {
                return false;
            }
            return (bool) keyTable[key];
        }

        public static void ChangeState(Keys key, bool state)
        {
            keyTable[key] = state;
        }

    }
}
