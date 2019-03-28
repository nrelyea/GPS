using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace GPS
{
    public partial class Form1 : Form
    {
        int gridSize = 100;
        string path = "";
        List<List<List<int>>> speedLimitMatrix = new List<List<List<int>>> { };

        public Form1(int gS, List<List<List<int>>> sLM, string p)
        {
            InitializeComponent();
            gridSize = gS;
            speedLimitMatrix = sLM;
            path = p;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.DoubleBuffered = true;

            this.Paint += new PaintEventHandler(Form1_Paint);

        }

        public void Form1_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {

            drawGrid(e, speedLimitMatrix);

            drawPath(e, path);



        }

        private void tnrAppTimer_Tick(object sender, EventArgs e)
        {
            this.Refresh();
        }

        // draw point
        public void drawPoint(System.Windows.Forms.PaintEventArgs e, int x, int y)
        {
            int pointSize = 10;

            Rectangle pt = new Rectangle(x - pointSize / 2, y - pointSize / 2, pointSize, pointSize);
            System.Drawing.SolidBrush redBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Red);
            e.Graphics.FillEllipse(redBrush, pt);
            e.Graphics.DrawEllipse(Pens.Black, pt);
        }

        // Draw Starting Grid & Speed Limits
        public void drawGrid(System.Windows.Forms.PaintEventArgs e, List<List<List<int>>> speedLimitMatrix)
        {
            for (int i = 0; i < speedLimitMatrix.Count; i++)
            {
                for (int j = 0; j < speedLimitMatrix.Count; j++)
                {
                    if (i < speedLimitMatrix.Count - 1)
                    {
                        e.Graphics.DrawLine(Pens.Black, new Point(gridSize + gridSize * i, gridSize + gridSize * j), new Point(gridSize + gridSize * (i + 1), gridSize + gridSize * (j)));
                        writeSpeedLimit(e, i, j, 0, speedLimitMatrix[i][j][0]);
                    }
                    if (j < speedLimitMatrix.Count - 1)
                    {
                        e.Graphics.DrawLine(Pens.Black, new Point(gridSize + gridSize * i, gridSize + gridSize * j), new Point(gridSize + gridSize * (i), gridSize + gridSize * (j + 1)));
                        writeSpeedLimit(e, i, j, 1, speedLimitMatrix[i][j][1]);
                    }

                    drawPoint(e, gridSize + gridSize * i, gridSize + gridSize * j);
                }
            }
        }

        // draw the proposed path
        public void drawPath(System.Windows.Forms.PaintEventArgs e, string path)
        {
            TextFormatFlags flags = TextFormatFlags.Bottom | TextFormatFlags.EndEllipsis;

            if (path.Length != ((speedLimitMatrix.Count - 1) * 2) || path.Count(f => f == 'R') != path.Count(f => f == 'D'))
            {
                invalidPath();
            }
            else
            {
                TextRenderer.DrawText(e.Graphics, "Path length: " + pathLength(speedLimitMatrix, path).ToString(), this.Font,
                new Rectangle(0, 0, 100, 15), SystemColors.ControlText, flags);

                System.Drawing.SolidBrush greenBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Lime);
                int x = gridSize;
                int y = gridSize;
                while (path.Length > 0)
                {
                    if (path[0] == 'R')
                    {
                        e.Graphics.FillRectangle(greenBrush, new Rectangle(x - 3, y - 3, gridSize + 6, 6));
                        x += gridSize;
                    }
                    else if (path[0] == 'D')
                    {
                        e.Graphics.FillRectangle(greenBrush, new Rectangle(x - 3, y - 3, 6, gridSize + 6));
                        y += gridSize;
                    }
                    else
                    {
                        invalidPath();
                        break;
                    }

                    path = path.Substring(1);
                }



            }

            void invalidPath()
            {
                e.Graphics.FillRectangle(new System.Drawing.SolidBrush(System.Drawing.Color.Red), new Rectangle(0, 0, 80, 20));
                TextRenderer.DrawText(e.Graphics, "INVALID PATH", this.Font,
                new Rectangle(0, 0, 100, 15), SystemColors.ControlText, flags);
            }
        }

        public void writeSpeedLimit(System.Windows.Forms.PaintEventArgs e, int x, int y, int direction, int speed)
        {
            TextFormatFlags flags = TextFormatFlags.Bottom | TextFormatFlags.EndEllipsis;
            if (direction == 0)
            {
                TextRenderer.DrawText(e.Graphics, speed.ToString(), this.Font,
                new Rectangle((gridSize * x - 12) + gridSize + gridSize / 2, (gridSize * y + 2) + gridSize, 25, 15), SystemColors.ControlText, flags);
            }
            else
            {
                TextRenderer.DrawText(e.Graphics, speed.ToString(), this.Font,
                new Rectangle((gridSize * x + 2) + gridSize, (gridSize * y - 5) + gridSize + gridSize / 2, 25, 15), SystemColors.ControlText, flags);
            }

        }

        public int pathLength(List<List<List<int>>> matrix, string path)
        {
            int length = 0;
            int x = 0;
            int y = 0;


            while (path.Length > 0)
            {
                if (path[0] == 'R')
                {
                    length += matrix[x][y][0];
                    x += 1;
                }
                else if (path[0] == 'D')
                {
                    length += matrix[x][y][1];
                    y += 1;
                }
                path = path.Substring(1);
            }

            return length;
        }
    }
}
