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
        int gridSize = 0;

        public Form1(int gS)
        {
            InitializeComponent();
            gridSize = gS;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.DoubleBuffered = true;

            this.Paint += new PaintEventHandler(Form1_Paint);

        }

        public void Form1_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {

            drawPoint(e, 200, 400);

        }

        private void tnrAppTimer_Tick(object sender, EventArgs e)
        {
            this.Refresh();
        }

        public void drawPoint(System.Windows.Forms.PaintEventArgs e, int x, int y)
        {
            TextFormatFlags flags = TextFormatFlags.Bottom | TextFormatFlags.EndEllipsis;

            int pointSize = 10;

            Rectangle pt = new Rectangle(x, y, pointSize, pointSize);
            System.Drawing.SolidBrush redBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Red);
            e.Graphics.FillEllipse(redBrush, pt);
            e.Graphics.DrawEllipse(Pens.Black, pt);

            //TextRenderer.DrawText(e.Graphics, "p", this.Font,
            //new Rectangle(x + pointSize, y + (pointSize / 2), 40, 20), SystemColors.ControlText, flags);




        }

    }
}
