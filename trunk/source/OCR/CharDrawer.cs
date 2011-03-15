using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace OCR
{
    public class CharDrawer : System.Windows.Forms.PictureBox
    {
        const int PEN_WIDTH = 4;
        
        private List<List<Point>> charPoints;
        
        public CharDrawer()
        {
            this.SetStyles();
            this.Cursor = Cursors.Cross;
            this.BackColor = Color.Black;

            charPoints = new List<List<Point>>();
        }

        public CharDrawer(IContainer container)
        {
            container.Add(this);

            this.SetStyles();
            this.Cursor = Cursors.Cross;
            this.BackColor = Color.Black;

            charPoints = new List<List<Point>>();
        }

        private void SetStyles()
        {
            this.SetStyle(System.Windows.Forms.ControlStyles.UserPaint, true);
            this.SetStyle(System.Windows.Forms.ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(System.Windows.Forms.ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(System.Windows.Forms.ControlStyles.ResizeRedraw, true);
        }


        protected override void OnMouseDown(MouseEventArgs e)
        {
            charPoints.Add(new List<Point>());
            charPoints[charPoints.Count - 1].Add(e.Location);
            
            base.OnMouseDown(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                charPoints[charPoints.Count - 1].Add(e.Location);
                this.Refresh();
            }
            
            base.OnMouseMove(e);
        }

        Pen pen = new Pen(new SolidBrush(Color.White), PEN_WIDTH);
        protected override void OnPaint(PaintEventArgs pe)
        {
            DrawChar(pe.Graphics);
            
            base.OnPaint(pe);
        }

        private void DrawChar(Graphics g)
        {
            foreach (List<Point> curve in this.charPoints)
            {
                if(curve.Count>1)
                    g.DrawCurve(pen, curve.ToArray(), 0.0f);
            }
        }


        public void Reset()
        {
            this.charPoints.Clear();
            this.Refresh();
        }

        public Bitmap SaveDrawing()
        {
            Bitmap bmp = new Bitmap(this.ClientRectangle.Width, this.ClientRectangle.Height, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
            Graphics g = Graphics.FromImage(bmp);
            g.FillRectangle(new SolidBrush(Color.Black), new Rectangle(Point.Empty, bmp.Size));

            DrawChar(g);

            g.Dispose();

            return bmp;
        }

    }
}
