/* Author: John R. McLaren
 * Acknowledgement: Iwan Tjin for providing steps towards constructing this application
 * Created: 28/4/2016
 * Rev: 2.2.0
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
        // class objects
        List<Point> snakeCoords = new List<Point>();
        List<Point> appleCoords = new List<Point>();
        Settings settings = new Settings();
        Random rng = new Random();
        Shape shapes = new Shape();
        Snake snake = new Snake();
        Apple apple = new Apple();
        Grid grid = new Grid();

        // form constructor
        public Form()
        {
            string[] args = Environment.GetCommandLineArgs(); // store input (if any) during execution
            try
            {
                if (int.Parse(args[1]) < 8)
                {
                    int.TryParse(args[1], out settings.Difficulty);
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
            InitializeComponent();
        }

        // start button events
        private void btnStart_Click(object sender, EventArgs e)
        {
            if (settings.GameStarted == false)
            {
                settings.GameStarted = true;
                StartGame();
            }
            else
            {
                RestartGame();
            }
        }

        // score button events
        private void btnScores_Click(object sender, EventArgs e)
        {
            DisplayScores();
        }

        // timer events
        private void tmrClock_Tick(object sender, EventArgs e)
        {
            tmrClock.Interval = 105 - (5 * settings.Difficulty);
            settings.KeyPressed = false;
            UpdateForm();
        }

        private void StartGame()
        {
            // initialise variables
            settings.Direction = "up";
            settings.customColor = Color.Lime;
            settings.Score = 0;

            //set form properties
            btnStart.Text = "New Game";
            lblScore.Show();
            lblLevel.Show();
            btnScores.Hide();
            btnStart.Hide();
            changeColors();

            // call methods
            grid.GenerateGrid(picBoxCanvas, Color.FromArgb(32, 32, 32), settings.customColor, true, true);
            GenerateSnake();
            apple.GenerateApple(appleCoords, snakeCoords, picBoxCanvas);
            tmrClock.Start();
        }

        // clears list values, starts game again
        private void RestartGame()
        {
            // delete all items on lists
            snakeCoords.Clear();
            appleCoords.Clear();

            // restart game
            settings.Difficulty = 1;
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
            grid.GenerateGrid(picBoxCanvas, Color.FromArgb(32, 32, 32), settings.customColor, true, true);
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

        private void DisplayScores()
        {
            string fileName = "Scores.txt"; // name of saved file
            string fullPath = Path.GetFullPath(fileName); // find the path to the saved file

            try
            {
                using (StreamReader sr = File.OpenText(fullPath))
                {
                    string[] scoresByLine = File.ReadAllLines(fullPath);
                    string allScores = "";

                    for (int i = 0; i < scoresByLine.Length; i++)
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

        // when called saves score to a text file, if file not present creates new file (default save location is /visualstudio2015/bin)
        private void SaveScore()
        {
            try
            {
                string fileName = "Scores.txt";
                string fullPath = Path.GetFullPath(fileName);
                if (!File.Exists(fullPath)) // if the file does not exist
                {
                    using (StreamWriter writerCreate = File.CreateText(fullPath))
                    {
                        Console.WriteLine("File Successfully Created At Location: " + fullPath);
                        writerCreate.WriteLine(string.Format("Score: {0} Points. {1}", settings.Score, DateTime.Now)); // create file and add score
                        writerCreate.Close(); // release resource
                    }
                }
                else
                {
                    using (StreamWriter writerAdd = File.AppendText(fullPath))
                    {
                        writerAdd.WriteLine(string.Format("Score: {0} Points. {1}", settings.Score, DateTime.Now)); // add new score to file
                        writerAdd.Close();
                    }
                }
            }
            catch
            {
                Console.WriteLine("ERROR: Filepath Could Not Be Located Or Created");
            }
        }

        // changes score label to score value
        private void UpdateScore()
        {
            lblScore.Text = "Score: " + settings.Score;
            lblLevel.Text = "Level " + settings.Difficulty;
        }

        // snake movement logic
        private void UpdateForm()
        {
            Point snakeHead = snakeCoords[snakeCoords.Count - 1];

            // move snake first
            if (apple.HitApple(snakeHead.X, snakeHead.Y, appleCoords) == true) // if snake head coords same as apple coords
            {
                appleCoords.RemoveAt(appleCoords.Count - 1);
                snake.GrowSnake(settings.Direction, picBoxCanvas, snakeCoords, settings.customColor);
                apple.GenerateApple(appleCoords, snakeCoords, picBoxCanvas);
                settings.Score++;

                if (settings.Score % 10 == 0 && settings.Score != 0) // and if score multiple of 10 and not 0
                {
                    // increase difficulty (snake gains energy boost from eating many apples)
                    settings.Difficulty++;

                    switch (settings.Difficulty)
                    {
                        case 1:
                            settings.customColor = Color.Lime;
                            break;
                        case 2:
                            settings.customColor = Color.Cyan;
                            break;
                        case 3:
                            settings.customColor = Color.DeepSkyBlue;
                            break;
                        case 4:
                            settings.customColor = Color.Magenta;
                            break;
                        case 5:
                            settings.customColor = Color.Yellow;
                            break;
                        case 6:
                            settings.customColor = Color.Orange;
                            break;
                        case 7:
                            settings.customColor = Color.Red;
                            break;
                        case 8:
                            WinGame();
                            break;
                        default:
                            settings.customColor = Color.Lime;
                            break;
                    }
                    changeColors();
                }
            }
            else
            {
                snake.MoveSnake(settings.Direction, picBoxCanvas, snakeCoords, settings.customColor);
            }

            // then check for collisions
            if (CheckCollisions(snakeHead.X, snakeHead.Y) == true)
            {
                LoseGame();
            }
            UpdateScore();
        }

        // default snake location and size
        private void GenerateSnake()
        {
            for (int i = 0; i < 3; i++)
            {
                snakeCoords.Add(new Point(grid.SquareSize * grid.GridSize / 2, grid.SquareSize * grid.GridSize / 2 + grid.SquareSize * i)); // generate in the middle
            }
        }

        // collision condition check
        private bool CheckCollisions(int hitX, int hitY)
        {
            if (hitX < 0 || hitX >= grid.SquareSize * grid.GridSize || hitY < 0 || hitY >= grid.SquareSize * grid.GridSize) // if hit the wall
            {
                return true;
            }

            for (int i = 0; i < snakeCoords.Count - 2; i++) // loop for each list segment except head
            {
                if (hitX == snakeCoords[i].X && hitY == snakeCoords[i].Y) // if head point same as segment point there is a collision
                {
                    return true;
                }
            }
            return false;
        }

        // movement key settings
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (settings.KeyPressed == false) // to prevent double movement we read only one action per tick to avoid snake inverting in on itself
            {
                switch (keyData)
                {
                    case Keys.Up:
                        if (settings.Direction != "down")
                        {
                            settings.Direction = "up";
                            settings.KeyPressed = true;
                        }
                        break;
                    case Keys.Down:
                        if (settings.Direction != "up")
                        {
                            settings.Direction = "down";
                            settings.KeyPressed = true;
                        }
                        break;
                    case Keys.Left:
                        if (settings.Direction != "right")
                        {
                            settings.Direction = "left";
                            settings.KeyPressed = true;
                        }
                        break;
                    case Keys.Right:
                        if (settings.Direction != "left")
                        {
                            settings.Direction = "right";
                            settings.KeyPressed = true;
                        }
                        break;
                }
            }
            return true;
        }

        private void changeColors()
        {
            // change grid colour
            grid.GenerateGrid(picBoxCanvas, Color.FromArgb(32, 32, 32), settings.customColor, true, false);
            //change snake colour
            for (int i = 0; i < snakeCoords.Count - 1; i++)
            {
                shapes.DrawSolidSquare(picBoxCanvas, snakeCoords[i].X, snakeCoords[i].Y, grid.SquareSize, settings.customColor);
            }
            //change button and label colours
            foreach (Control c in Controls)
            {
                Label lbl = c as Label; // label object
                Button btn = c as Button;
                if (lbl != null) // if exist
                {
                    lbl.ForeColor = settings.customColor; // set to visible
                }
                if (btn != null)
                {
                    btn.ForeColor = settings.customColor;
                }
            }
        }
    }
}