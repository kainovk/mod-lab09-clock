using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace Clock
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            timer1.Interval = 1000;
            timer1.Enabled = true;
            ClientSize = new Size(800, 800);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.Invalidate();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Pen pen = new Pen(Color.Black, 2);
            Brush brush = new SolidBrush(Color.Indigo);
            Brush brushBackground = new SolidBrush(Color.LightGoldenrodYellow);
            Graphics g = e.Graphics;
            GraphicsState gs;

            int w = ClientSize.Width;
            int h = ClientSize.Height;
            g.TranslateTransform(w / 2, h / 2);
            //g.ScaleTransform(w / 200, h / 200);

            g.DrawEllipse(pen, -w / 2, -h / 2, w, h);
            g.FillEllipse(brushBackground, -w / 2 + pen.Width, -h / 2 + pen.Width, w - 2 * pen.Width, h - 2 * pen.Width);

            Pen penHour = new Pen(Color.Brown, 5);
            Pen penMinute = new Pen(Color.Brown, 5);

            DrawClockLines(g, penMinute, 6, 20);
            DrawClockLines(g, penHour, 30, 40);

            DrawNumbers(g, new Point(0, 0), w / 2);

            Pen penSecondPointer = new Pen(Color.Black, 2);
            Pen penMinutePointer = new Pen(Color.Black, 4);
            Pen penHourPointer = new Pen(Color.Black, 8);
            DateTime now = DateTime.Now;

            gs = g.Save();
            g.RotateTransform(6 * now.Second);
            g.DrawLine(penSecondPointer, 0, h / 10, 0, (float)(-h / 2.5));
            g.Restore(gs);

            penMinutePointer.StartCap = LineCap.RoundAnchor;
            penMinutePointer.EndCap = LineCap.SquareAnchor;
            gs = g.Save();
            g.RotateTransform(6 * now.Minute + now.Second / 10);
            g.DrawLine(penMinutePointer, 0, 0, 0, (float)(-h / 3));
            g.Restore(gs);

            penMinutePointer.EndCap = LineCap.Round;
            gs = g.Save();
            g.RotateTransform(30 * (now.Hour % 12) + now.Minute / 2 + now.Second / 120);
            g.DrawLine(penHourPointer, 0, 0, 0, (float)(-h / 4));
            g.Restore(gs);
        }

        void DrawClockLines(Graphics g, Pen p, int diff, int length)
        {
            Point center = new Point(0, 0);
            int clockRadius = (int)Math.Min(ClientSize.Width / 2.5, ClientSize.Height / 2.5);

            for (int angle = 0; angle < 360; angle += diff)
            {
                double radians = angle * Math.PI / 180;

                int xStart = center.X + (int)((clockRadius - length) * Math.Cos(radians));
                int yStart = center.Y + (int)((clockRadius - length) * Math.Sin(radians));
                Point start = new Point(xStart, yStart);

                int xEnd = center.X + (int)(clockRadius * Math.Cos(radians));
                int yEnd = center.Y + (int)(clockRadius * Math.Sin(radians));
                Point end = new Point(xEnd, yEnd);

                g.DrawLine(p, start, end);
            }
        }

        private void DrawNumbers(Graphics graphics, Point center, int radius)
        {
            float fontSize = radius / 20f;
            Font font = new Font("Arial", fontSize, FontStyle.Bold);

            for (int hour = 1; hour <= 12; hour++)
            {
                float angle = hour * 30;
                float x = center.X + (float)(radius * 0.9 * Math.Cos((angle - 90) * Math.PI / 180));
                float y = center.Y + (float)(radius * 0.9 * Math.Sin((angle - 90) * Math.PI / 180));
                graphics.DrawString(hour.ToString(), font, Brushes.Black, x, y);
            }
        }
    }
}
