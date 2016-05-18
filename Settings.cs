/* Author: John R. McLaren
 * Created: 6/5/2016
 * Rev: 1.0.0
 * 
 * Settings Class Source Code, child of Snake.sln
 * Handles Game Variables
 */
using System.Drawing;

namespace Snake
{
    class Settings
    {
        public Color Color = new Color();
        public int Difficulty = 1;
        public int Score { get; set; } = 0;
        public bool KeyPressed { get; set; } = false; // default false
        public bool GameStarted { get; set; } = false; // default false
        public string Direction { get; set; } // direction of movement
    }
}
