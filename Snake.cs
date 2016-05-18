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
        Shape Shape = new Shape();
        Grid Grid = new Grid();

        // adds new segment to snake
        public void Grow(string direction, PictureBox pictureBox, List<Point> list, Color rgb)
        {
            Point coordinates = list[list.Count - 1]; // last/bottom of the list

            switch (direction)
            {
                case "up":
                    list.Add(new Point(coordinates.X, coordinates.Y - Grid.SquareSize)); // new head pos
                    Shape.DrawSolidSquare(pictureBox, coordinates.X, coordinates.Y, Grid.SquareSize, rgb); // draw head
                    break;
                case "down":
                    list.Add(new Point(coordinates.X, coordinates.Y + Grid.SquareSize));
                    Shape.DrawSolidSquare(pictureBox, coordinates.X, coordinates.Y, Grid.SquareSize, rgb);
                    break;
                case "left":
                    list.Add(new Point(coordinates.X - Grid.SquareSize, coordinates.Y));
                    Shape.DrawSolidSquare(pictureBox, coordinates.X, coordinates.Y, Grid.SquareSize, rgb);
                    break;
                case "right":
                    list.Add(new Point(coordinates.X + Grid.SquareSize, coordinates.Y));
                    Shape.DrawSolidSquare(pictureBox, coordinates.X, coordinates.Y, Grid.SquareSize, rgb);
                    break;
            }
        }

        // removes tail and redraws snake at new location
        public void Move(string direction, PictureBox pictureBox, List<Point> snakeCoords, Color rgb)
        {
            Point lastPoint = snakeCoords[snakeCoords.Count - 1]; // last item on list list
            Point firstPoint = snakeCoords[0]; // first item on list list

            switch (direction)
            {
                case "up":
                    snakeCoords.Add(new Point(lastPoint.X, lastPoint.Y - Grid.SquareSize)); // new head pos
                    Shape.DrawSolidSquare(pictureBox, firstPoint.X, firstPoint.Y, Grid.SquareSize, Color.FromArgb(32, 32, 32)); // erase tail
                    Shape.DrawSolidSquare(pictureBox, lastPoint.X, lastPoint.Y, Grid.SquareSize, rgb);
                    snakeCoords.RemoveAt(0); // remove tail pos
                    break;
                case "down":
                    snakeCoords.Add(new Point(lastPoint.X, lastPoint.Y + Grid.SquareSize));
                    Shape.DrawSolidSquare(pictureBox, firstPoint.X, firstPoint.Y, Grid.SquareSize, Color.FromArgb(32, 32, 32));
                    Shape.DrawSolidSquare(pictureBox, lastPoint.X, lastPoint.Y, Grid.SquareSize, rgb);
                    snakeCoords.RemoveAt(0);
                    break;
                case "left":
                    snakeCoords.Add(new Point(lastPoint.X - Grid.SquareSize, lastPoint.Y));
                    Shape.DrawSolidSquare(pictureBox, firstPoint.X, firstPoint.Y, Grid.SquareSize, Color.FromArgb(32, 32, 32));
                    Shape.DrawSolidSquare(pictureBox, lastPoint.X, lastPoint.Y, Grid.SquareSize, rgb);
                    snakeCoords.RemoveAt(0);
                    break;
                case "right":
                    snakeCoords.Add(new Point(lastPoint.X + Grid.SquareSize, lastPoint.Y));
                    Shape.DrawSolidSquare(pictureBox, firstPoint.X, firstPoint.Y, Grid.SquareSize, Color.FromArgb(32, 32, 32));
                    Shape.DrawSolidSquare(pictureBox, lastPoint.X, lastPoint.Y, Grid.SquareSize, rgb);
                    snakeCoords.RemoveAt(0);
                    break;
            }
        }
    }
}
