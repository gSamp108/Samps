using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MM3.Interface
{
    public partial class DrawingForm : BaseForm
    {
        private Simulation.NameGenerator ng = new Simulation.NameGenerator();
        private Random rng = new Random();

        public DrawingForm()
        {
            InitializeComponent();

            if (File.Exists("name.generator.db.bin")) this.ng.LoadFromDisk("name.generator.db.bin");
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

        private void button1_Click(object sender, EventArgs e)
        {
            var ng = new Simulation.NameGenerator();
            ng.AddSourceToDatabase(this.textBox1.Text);
            ng.SaveToDisk("name.generator.db.bin");
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.textBox2.Text += this.ng.GenerateName(this.rng) + Environment.NewLine;
        }
    }
}
