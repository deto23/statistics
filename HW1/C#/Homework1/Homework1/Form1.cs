using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Homework1
{

    partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;

            Pen pen = new Pen(Color.Black, 5);

            Brush brush = new SolidBrush(Color.Black);


            // Disegna un cerchio al centro del form
            int x_circle = this.Width / 2;
            int y_circle = this.Height / 2;
            int r_circle = 50;

            int x1_line = (this.Width / 2) - 100; 
            int y1_line = 100;
            int x2_line = (this.Width / 2) + 100;
            int y2_line = 100;

            int x_point = (this.Width / 2);
            int y_point = 150;

            int x_rectangle = (this.Width / 2) - 100; 
            int y_rectangle = 325;
            int width = 200;
            int height = 100;

            //line
            g.DrawLine(pen, x1_line, y1_line, x2_line, y2_line);

            //point
            g.FillEllipse(brush, x_point - 1, y_point - 1, 10, 10);

            //circle
            g.DrawEllipse(pen, x_circle - r_circle, y_circle - r_circle, 2 * r_circle, 2 * r_circle);

            //rectangle
            g.DrawRectangle(pen, x_rectangle, y_rectangle, width, height);
        }
    }
}
