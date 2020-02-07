using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MM3.Interface
{
    public partial class DrawingForm : BaseForm
    {
        public DrawingForm()
        {
            InitializeComponent();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            this.Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var canvas = e.Graphics;
            canvas.Clear(Color.White);
            canvas.DrawArc(Pens.Black, new Rectangle(100, 100, 4, 4), 180, 90);
            canvas.DrawArc(Pens.Black, new Rectangle(100, 100, 4, 4), 270, 90);
            canvas.DrawArc(Pens.Black, new Rectangle(100, 100, 4, 4), 0, 180);
            canvas.DrawLine(Pens.Black, new Point(102, 104), new Point(102, 106));

        }
    }
}
