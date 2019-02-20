using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Windows.Forms;

namespace SnakeGame
{
    class Input
    {
        private static Hashtable keyTable = new Hashtable();
        // create a new instance of Hastable class
        // this class is used to optimise the keys entered

        public static bool KeyPress(Keys key)
        {
            // this function will return a key back to the class
            if (keyTable[key] == null)
            {
                // if the hastable is empty, return false
                return false;
            }
            // if the hashtable is not empty, return true
            return (bool)keyTable[key];
        }

        public static void changeState(Keys key, bool state)
        {
            // this function will change the state of keys and the player
            // contains two arguments, key and state
            keyTable[key] = state;
        }
    }
}
