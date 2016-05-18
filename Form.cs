/* Author: John R. McLaren
 * Acknowledgement: Iwan Tjin for providing steps towards constructing this application
 * Created: 28/4/2016
 * Rev: 2.2.2
 * 
 * Form Source Code, child of Snake.sln
 * Handles form logic
 */

using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace Snake
{
    public partial class Form : System.Windows.Forms.Form
    {   
        // Class Objects
        List<Point> snakeCoords = new List<Point>();
        List<Point> appleCoords = new List<Point>();
        Settings Settings = new Settings();
        Random rng = new Random();
        Shape Shapes = new Shape();
        Snake Snake = new Snake();
        Apple Apple = new Apple();
        Grid Grid = new Grid();

        // Form constructor
        public Form()
        {
            string[] args = Environment.GetCommandLineArgs(); // gets an input (if any) before execution
            try
            {
                if (int.Parse(args[1]) < 8) // input less than 8
                {
                    int.TryParse(args[1], out Settings.Difficulty); // set difficulty of game to input
                }
                else
                {
                    Console.WriteLine("Invalid input. Input must be an integer between 1 - 7");
                }
            }
            catch
            {
                Console.WriteLine("No input found. Program will run as default.");
            }
            InitializeComponent(); // load the form
        }

        // Start button events
        private void SelectStartButton(object sender, EventArgs e)
        {
            if (Settings.GameStarted == false)
            {
                Settings.GameStarted = true;
                StartGame();
            }
            else
            {
                RestartGame();
            }
        }

        // score button events
        private void SelectScoreButton(object sender, EventArgs e)
        {
            DisplayScores();
        }

        // timer events
        private void OnTimerTick(object sender, EventArgs e)
        {
            tmrClock.Interval = 105 - (5 * Settings.Difficulty);
            Settings.KeyPressed = false;
            UpdateForm();
        }

        private void StartGame()
        {
            // initialise variables
            Settings.Direction = "up";
            Settings.Color = Color.Lime;
            Settings.Score = 0;

            //set form properties
            btnStart.Text = "New Game";
            lblScore.Show();
            lblLevel.Show();
            btnScores.Hide();
            btnStart.Hide();
            changeColors();

            // call methods
            Grid.Generate(picBoxCanvas, Color.FromArgb(32, 32, 32), Settings.Color, true, true);
            GenerateSnake();
            Apple.Generate(appleCoords, snakeCoords, picBoxCanvas);
            tmrClock.Start();
        }

        // clears list values, starts game again
        private void RestartGame()
        {
            // delete all items on lists
            snakeCoords.Clear();
            appleCoords.Clear();

            // restart game
            Settings.Difficulty = 1;
            StartGame();
            UpdateScore();
        }

        // clears the playing canvas
        private void StopGame()
        {
            UpdateScore();
            btnScores.Show();
            btnStart.Show();
            snakeCoords.Clear();
            appleCoords.Clear();
            Grid.Generate(picBoxCanvas, Color.FromArgb(32, 32, 32), Settings.Color, true, true);
        }

        private void LoseGame()
        {
            tmrClock.Stop(); // stop timer
            SaveScore(); // save current score to a file

            DialogResult result = MessageBox.Show("You died! Play again?", "Game Over", MessageBoxButtons.YesNo); // displays yes no message box
            if (result == DialogResult.Yes)
            {
                RestartGame();
            }
            else
            {
                StopGame();
            }
        }

        private void WinGame()
        {
            tmrClock.Stop(); // stop timer
            SaveScore(); // save current score to a file

            MessageBox.Show("You are a master of Snake. Few can match your skill and quick wit. Congratulations. You have completed the game.", "You Win!");
        }

        // changes score labels to score values
        private void UpdateScore()
        {
            lblScore.Text = "Score: " + Settings.Score;
            lblLevel.Text = "Level " + Settings.Difficulty;
        }

        // snake movement logic
        private void UpdateForm()
        {
            Point snakeHead = snakeCoords[snakeCoords.Count - 1];

            // a) move snake first
            if (Apple.Hit(snakeHead.X, snakeHead.Y, appleCoords) == true) // if snake hits apple
            {
                appleCoords.RemoveAt(appleCoords.Count - 1);
                Snake.Grow(Settings.Direction, picBoxCanvas, snakeCoords, Settings.Color);
                Apple.Generate(appleCoords, snakeCoords, picBoxCanvas);
                Settings.Score++;

                if (Settings.Score % 10 == 0 && Settings.Score != 0) // and if score multiple of 10
                {
                    // increase difficulty (snake gains energy boost from eating a certain amount of apples)
                    Settings.Difficulty++;

                    switch (Settings.Difficulty)
                    {
                        case 1:
                            Settings.Color = Color.Lime;
                            break;
                        case 2:
                            Settings.Color = Color.Cyan;
                            break;
                        case 3:
                            Settings.Color = Color.DeepSkyBlue;
                            break;
                        case 4:
                            Settings.Color = Color.Magenta;
                            break;
                        case 5:
                            Settings.Color = Color.Yellow;
                            break;
                        case 6:
                            Settings.Color = Color.Orange;
                            break;
                        case 7:
                            Settings.Color = Color.Red;
                            break;
                        case 8:
                            WinGame();
                            break;
                        default:
                            Settings.Color = Color.Lime;
                            break;
                    }
                    changeColors();
                }
            }
            else
            {
                Snake.Move(Settings.Direction, picBoxCanvas, snakeCoords, Settings.Color);
            }

            // b) then check for collisions
            if (CheckCollisions(snakeHead.X, snakeHead.Y) == true)
            {
                LoseGame();
            }
            UpdateScore();
        }

        // creates a default snake location and size
        private void GenerateSnake()
        {
            for (int i = 0; i < 3; i++)
            {
                snakeCoords.Add(new Point(Grid.SquareSize * Grid.GridSize / 2, Grid.SquareSize * Grid.GridSize / 2 + Grid.SquareSize * i)); // generate in the middle
            }
        }

        // collision conditions
        private bool CheckCollisions(int hitX, int hitY)
        {
            if (hitX < 0 || hitX >= Grid.SquareSize * Grid.GridSize || hitY < 0 || hitY >= Grid.SquareSize * Grid.GridSize) // hit the wall
            {
                return true;
            }

            for (int i = 0; i < snakeCoords.Count - 2; i++) // loop for each list segment except head
            {
                if (hitX == snakeCoords[i].X && hitY == snakeCoords[i].Y) // hit itself
                {
                    return true;
                }
            }
            return false;
        }

        // movement key settings
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (Settings.KeyPressed == false) // to prevent double movement we get only one action per tick to avoid snake inverting in on itself
            {
                switch (keyData)
                {
                    case Keys.Up:
                        if (Settings.Direction != "down")
                        {
                            Settings.Direction = "up";
                            Settings.KeyPressed = true;
                        }
                        break;
                    case Keys.Down:
                        if (Settings.Direction != "up")
                        {
                            Settings.Direction = "down";
                            Settings.KeyPressed = true;
                        }
                        break;
                    case Keys.Left:
                        if (Settings.Direction != "right")
                        {
                            Settings.Direction = "left";
                            Settings.KeyPressed = true;
                        }
                        break;
                    case Keys.Right:
                        if (Settings.Direction != "left")
                        {
                            Settings.Direction = "right";
                            Settings.KeyPressed = true;
                        }
                        break;
                }
            }
            return true;
        }

        private void changeColors()
        {
            // change grid colour
            Grid.Generate(picBoxCanvas, Color.FromArgb(32, 32, 32), Settings.Color, true, false);

            //change snake colour
            for (int i = 0; i < snakeCoords.Count - 1; i++)
            {
                Shapes.DrawSolidSquare(picBoxCanvas, snakeCoords[i].X, snakeCoords[i].Y, Grid.SquareSize, Settings.Color);
            }
            //change button and label colours
            foreach (Control c in Controls)
            {
                Label lbl = c as Label; // label object
                Button btn = c as Button;
                if (lbl != null) // if exist
                {
                    lbl.ForeColor = Settings.Color; // set to visible
                }
                if (btn != null)
                {
                    btn.ForeColor = Settings.Color;
                }
            }
        }

        private void DisplayScores()
        {
            string fileName = "Scores.txt"; // name of saved file
            string fullPath = Path.GetFullPath(fileName); // the path to the saved file

            try
            {
                using (StreamReader ReadFile = File.OpenText(fullPath))
                {
                    string[] scoresByLine = File.ReadAllLines(fullPath);
                    string allScores = "";

                    for (int i = 0; i < scoresByLine.Length && i < 10; i++)
                    {
                        allScores += scoresByLine[i] + "\n";
                    }
                    MessageBox.Show(allScores, "Previous Scores");
                }
            }
            catch
            {
                Console.WriteLine("ERROR: Filepath Could Not Be Located Or Read");
            }
        }

        // saves score to a text file, if file not present creates new file (default save location is /visualstudio2015/bin)
        private void SaveScore()
        {
            try
            {
                string fileName = "Scores.txt";
                string fullPath = Path.GetFullPath(fileName);
                if (!File.Exists(fullPath)) // if the file does not exist
                {
                    using (StreamWriter NewFile = File.CreateText(fullPath)) // create a new file
                    {
                        Console.WriteLine("File Successfully Created At Location: " + fullPath);
                        NewFile.WriteLine(string.Format("1. Score: {0} Points. {1}", Settings.Score, DateTime.Now.ToShortDateString())); // create file and add score
                        NewFile.Close(); // release resource
                    }
                }
                else // add new score to file
                {
                    string[] scoresByLine = File.ReadAllLines(fullPath);
                    using (StreamWriter AddToFile = File.AppendText(fullPath))
                    {
                        AddToFile.WriteLine(string.Format("{0}. Score: {1} Points. {2}", scoresByLine.Length + 1, Settings.Score, DateTime.Now.ToShortDateString()));
                        AddToFile.Close();
                    }
                }
            }
            catch
            {
                Console.WriteLine("ERROR: Filepath Could Not Be Located Or Created");
            }
        }
    }
}
