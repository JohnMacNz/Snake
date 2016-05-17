/* Author: John R. McLaren
 * Created: 6/5/2016
 * Rev: 1.0.0
 * 
 * Grid Class Source Code, child of Snake.sln
 * Handles grid properties and rendering
 */

using System.Drawing;
using System.Windows.Forms;

namespace Snake
{
    class Grid
    {
        public int GridSize { get; set; } = 20; // default size 20
        public int SquareSize { get; set; } = 15; // default size 15

        // draws the playing field
        public void GenerateGrid(PictureBox pictureBox, Color canvasColor, Color borderColor, bool drawBorder, bool drawCanvas)
        {
            Point p = new Point(); // new point
            Grid grid = new Grid(); // New Grid
            Shape shape = new Shape();
            Graphics graphicObj = pictureBox.CreateGraphics();

            if (drawCanvas == true)
            {
                for (int i = 0; i < GridSize * GridSize; i++) // loop for size of grid
                {
                    shape.DrawSolidSquare(pictureBox, p.X, p.Y, SquareSize, canvasColor); // draw square
                    p.X += SquareSize; // next square

                    if (p.X == SquareSize * GridSize) // if reached end of row
                    {
                        p.X = 0; // reset posX
                        p.Y += SquareSize; // next row
                    }
                }
            }

            if (drawBorder == true)
            {
                shape.DrawSquare(pictureBox, 0, 0, SquareSize * GridSize + 2, borderColor); // draw border
            }
        }
    }
}
