using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lameo
{
    public partial class LabeledTextbox : UserControl
    {
        private string label;
        private string text;
        private bool isreadonly;


        public string XLabel
        {
            get { return this.label; }
            set
            {
                this.label = value;
                this.Invalidate();
            }
        }
        public string XText
        {
            get { return this.text; }
            set {
                this.text = value;
                this.Invalidate();
            }
        }
        public int XIntValue
        {
            get
            {
                var result = 0;
                int.TryParse(this.text, out result);
                return result;
            }
        }
        public bool XReadOnly
        {
            get { return this.isreadonly; }
            set { this.isreadonly = value; }
        }

        public LabeledTextbox()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            this.Height = 20;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var canvas = e.Graphics;
            var label = this.XLabel + ": ";
            var lsize = canvas.MeasureString(label, this.Font);
            using (var formatter = new StringFormat())
            {
                formatter.LineAlignment = StringAlignment.Center;
                formatter.Alignment = StringAlignment.Near;

                canvas.DrawString(label, this.Font, Brushes.White, this.ClientRectangle, formatter);
                canvas.DrawString(this.XText, this.Font, Brushes.White, new Rectangle((int)lsize.Width, 0, this.ClientRectangle.Width, this.ClientRectangle.Height), formatter);
            }
        }

        protected override void OnDoubleClick(EventArgs e)
        {
            base.OnDoubleClick(e);
            if (!this.isreadonly)
            {
                using (var input = new TextInputForm())
                {
                    if (input.ShowDialog()== DialogResult.OK)
                    {
                        this.XText = input.Result;
                    }
                }
            }
        }
    }
}
