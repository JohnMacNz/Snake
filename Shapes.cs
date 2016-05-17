/* Author: John R. McLaren
 * Created: 6/5/2016
 * Rev: 1.0.0
 * 
 * Shapes Class Source Code, child of Snake.sln
 * Handles shape drawing logic
 */

using System.Drawing;
using System.Windows.Forms;

namespace Snake
{
    class Shape
    {
        // circle drawing logic (solid)
        public void DrawSolidCircle(PictureBox pictureBox, int posX, int posY, int size, Color rgb)
        {
            Graphics graphicObj = pictureBox.CreateGraphics();
            SolidBrush myBrush = new SolidBrush(rgb); // create brush object
            Pen myPen = new Pen(rgb); // create pen object

            graphicObj.DrawEllipse(myPen, posX, posY, size, size);
            graphicObj.FillEllipse(myBrush, posX, posY, size, size);

            myBrush.Dispose(); // erase brush
            myPen.Dispose(); // erase pen
            graphicObj.Dispose(); // erase graphics object
        }
        // square drawing logic (solid)
        public void DrawSolidSquare(PictureBox pictureBox, int posX, int posY, int size, Color rgb)
        {
            Graphics graphicObj = pictureBox.CreateGraphics();
            SolidBrush myBrush = new SolidBrush(rgb); // create brush object
            graphicObj.FillRectangle(myBrush, posX + 1, posY + 1, size, size); // fill square

            myBrush.Dispose(); // erase brush
            graphicObj.Dispose(); // erase graphics object
        }

        // square drawing logic (hollow)
        public void DrawSquare(PictureBox pictureBox, int posX, int posY, int size, Color rgb)
        {
            Graphics graphicObj = pictureBox.CreateGraphics();
            Pen myPen = new Pen(rgb, 2); // create pen object
            graphicObj.DrawRectangle(myPen, posX, posY, size, size); // draw square

            myPen.Dispose(); // erase pen
            graphicObj.Dispose(); // erase graphics object
        }
    }
}
