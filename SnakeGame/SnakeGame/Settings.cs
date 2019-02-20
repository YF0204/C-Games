using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame
{
    public enum Directions
    {
        // using an enam class to set directions

        Left,
        Right,
        Up,
        Down
    };

    class Settings
    {
        public static int Width { get; set; } // set the width as int
        public static int Height { get; set; } // set the height as int
        public static int Speed { get; set; } // set the speed as int
        public static int Score { get; set; } // set the score as int
        public static int Points { get; set; } // set the points as int
        public static bool GameOver { get; set; } // set the game over as bool
        public static Directions direction { get; set; } // set the direction to enum class above

        public Settings()
        {
            // set the default settings
            Width = 16; // set width to 16
            Height = 16; // set height to 16
            Speed = 20; // set speed to 20
            Score = 0; // set score to 0
            Points = 100; // set points to 100
            GameOver = false; // set game over to false
            direction = Directions.Down; // default direction is down
        }
    }
}
