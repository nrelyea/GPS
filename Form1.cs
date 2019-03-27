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
        List<List<List<int>>> speedLimitMatrix = new List<List<List<int>>> { };

        public Form1(int gS, List<List<List<int>>> sLM)
        {
            InitializeComponent();
            gridSize = gS;
            speedLimitMatrix = sLM;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.DoubleBuffered = true;

            this.Paint += new PaintEventHandler(Form1_Paint);

        }

        public void Form1_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {

            //Draw Starting Grid & Speed Limits
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

            //writeSpeedLimit(e, 0, 0, 0, 100);
            //writeSpeedLimit(e, 2, 1, 1, 100);


            //drawPoint(e, 200, 400);

            //Point previous = new Point(100, 300);
            //Point current = new Point(200, 400);
            // e.Graphics.DrawLine(Pens.Black, previous, current);



        }

        private void tnrAppTimer_Tick(object sender, EventArgs e)
        {
            this.Refresh();
        }

        public void drawPoint(System.Windows.Forms.PaintEventArgs e, int x, int y)
        {


            int pointSize = 10;

            Rectangle pt = new Rectangle(x - pointSize / 2, y - pointSize / 2, pointSize, pointSize);
            System.Drawing.SolidBrush redBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Red);
            e.Graphics.FillEllipse(redBrush, pt);
            e.Graphics.DrawEllipse(Pens.Black, pt);
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
    }
}
