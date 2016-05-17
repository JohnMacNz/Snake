/* Author: John R. McLaren
 * Created: 6/5/2016
 * Rev: 1.0.0
 * 
 * Snake Class Source Code, child of Snake.sln
 * Handles snake rendering logic
 */

using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace Snake
{
    class Snake
    {
        Shape shapes = new Shape();
        Grid grid = new Grid();
        Apple apple = new Apple();

        // adds new segment to list
        public void GrowSnake(string direction, PictureBox pictureBox, List<Point> list, Color rgb)
        {
            Point coordinates = list[list.Count - 1]; // last/bottom of the list

            switch (direction)
            {
                case "up":
                    list.Add(new Point(coordinates.X, coordinates.Y - grid.SquareSize)); // new head pos
                    shapes.DrawSolidSquare(pictureBox, coordinates.X, coordinates.Y, grid.SquareSize, rgb); // draw head
                    break;
                case "down":
                    list.Add(new Point(coordinates.X, coordinates.Y + grid.SquareSize));
                    shapes.DrawSolidSquare(pictureBox, coordinates.X, coordinates.Y, grid.SquareSize, rgb);
                    break;
                case "left":
                    list.Add(new Point(coordinates.X - grid.SquareSize, coordinates.Y));
                    shapes.DrawSolidSquare(pictureBox, coordinates.X, coordinates.Y, grid.SquareSize, rgb);
                    break;
                case "right":
                    list.Add(new Point(coordinates.X + grid.SquareSize, coordinates.Y));
                    shapes.DrawSolidSquare(pictureBox, coordinates.X, coordinates.Y, grid.SquareSize, rgb);
                    break;
            }
        }

        // removes tail position and redraws snake
        public void MoveSnake(string direction, PictureBox pictureBox, List<Point> snakeCoords, Color rgb)
        {
            Point lastPoint = snakeCoords[snakeCoords.Count - 1]; // last item on list list
            Point firstPoint = snakeCoords[0]; // first item on list list

            switch (direction)
            {
                case "up":
                    snakeCoords.Add(new Point(lastPoint.X, lastPoint.Y - grid.SquareSize)); // new head pos
                    shapes.DrawSolidSquare(pictureBox, firstPoint.X, firstPoint.Y, grid.SquareSize, Color.FromArgb(32, 32, 32)); // erase tail
                    shapes.DrawSolidSquare(pictureBox, lastPoint.X, lastPoint.Y, grid.SquareSize, rgb);
                    snakeCoords.RemoveAt(0); // remove tail pos
                    break;
                case "down":
                    snakeCoords.Add(new Point(lastPoint.X, lastPoint.Y + grid.SquareSize));
                    shapes.DrawSolidSquare(pictureBox, firstPoint.X, firstPoint.Y, grid.SquareSize, Color.FromArgb(32, 32, 32));
                    shapes.DrawSolidSquare(pictureBox, lastPoint.X, lastPoint.Y, grid.SquareSize, rgb);
                    snakeCoords.RemoveAt(0);
                    break;
                case "left":
                    snakeCoords.Add(new Point(lastPoint.X - grid.SquareSize, lastPoint.Y));
                    shapes.DrawSolidSquare(pictureBox, firstPoint.X, firstPoint.Y, grid.SquareSize, Color.FromArgb(32, 32, 32));
                    shapes.DrawSolidSquare(pictureBox, lastPoint.X, lastPoint.Y, grid.SquareSize, rgb);
                    snakeCoords.RemoveAt(0);
                    break;
                case "right":
                    snakeCoords.Add(new Point(lastPoint.X + grid.SquareSize, lastPoint.Y));
                    shapes.DrawSolidSquare(pictureBox, firstPoint.X, firstPoint.Y, grid.SquareSize, Color.FromArgb(32, 32, 32));
                    shapes.DrawSolidSquare(pictureBox, lastPoint.X, lastPoint.Y, grid.SquareSize, rgb);
                    snakeCoords.RemoveAt(0);
                    break;
            }
        }
    }
}
