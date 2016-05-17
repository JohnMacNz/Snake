/* Author: John R. McLaren
 * Created: 6/5/2016
 * Rev: 1.0.0
 * 
 * Apple Class Source Code, child of Snake.sln
 * Handles apple rendering logic
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Snake
{
    class Apple
    {
        Random rng = new Random();
        Grid grid = new Grid();
        Shape shapes = new Shape();

        // apple rendering logic, assigns new location for apple then draws it
        public void GenerateApple(List<Point> appleCoords, List<Point> snakeCoords, PictureBox picBox)
        {
            appleCoords.Add(new Point(rng.Next(1, grid.GridSize) * grid.SquareSize, rng.Next(1, grid.GridSize) * grid.SquareSize)); // new apple location

            for (int i = 0; i < snakeCoords.Count - 1; i++) // loop for each segment in snake
            {
                if (appleCoords[appleCoords.Count - 1] == snakeCoords[i]) // if apple location same as snake
                {
                    appleCoords.RemoveAt(appleCoords.Count - 1); // remove this location
                    break;
                }
            }
            if (appleCoords.Count > 0) // if apple exists
            {
                shapes.DrawSolidCircle(picBox, appleCoords[appleCoords.Count - 1].X + 3, appleCoords[appleCoords.Count - 1].Y + 3, 10, Color.Red);
            }
            else
            {
                GenerateApple(appleCoords, snakeCoords, picBox); // try again
            }
        }

        // checks apple collision
        public bool HitApple(int hitX, int hitY, List<Point> appleCoords)
        {
            if (appleCoords[appleCoords.Count - 1].X == hitX && appleCoords[appleCoords.Count - 1].Y == hitY) // if apple coordinate same as hitX+hitY
            {
                return true;
            }
            return false;
        }
    }
}
