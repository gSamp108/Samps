using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lameo
{
    public partial class Form1 : Form
    {
        private Timer timer;
        private World world;
        private Point camera;
        private int tileSize = 8;
        private int worldSize = 1000;
        private int cameraSpeed = 25;
        private int addThreshold = 5;
        private int removeThreshold = 4;
        private WorldGenerator worldGenerator;
        private int ticksPerStep = 10;
        private int stepCounter = 0;

        public Form1()
        {
            this.InitializeComponent();
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);
            this.timer = new Timer();
            this.timer.Interval = 1;
            this.timer.Tick += Timer_Tick;
            this.timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            this.stepCounter += 1;
            if (this.stepCounter>this.ticksPerStep)
            {
                if (this.worldGenerator == null) this.worldGenerator = new WorldGenerator();
                this.worldGenerator.Tick();
                this.stepCounter = 0;
            }
            this.Invalidate();
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);
            if (e.KeyCode == Keys.Space) this.world = new World(this.worldSize, this.addThreshold, this.removeThreshold);
            if (e.KeyCode == Keys.Up) this.camera.Y -= this.cameraSpeed;
            if (e.KeyCode == Keys.Right) this.camera.X += this.cameraSpeed;
            if (e.KeyCode == Keys.Down) this.camera.Y += this.cameraSpeed;
            if (e.KeyCode == Keys.Left) this.camera.X -= this.cameraSpeed;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var canvas = e.Graphics;

            if (this.worldGenerator != null)
            {
                foreach(var tile in this.worldGenerator.closed)
                {
                    var renderTile = new Rectangle((tile.X * this.tileSize) + this.camera.X, (tile.Y * this.tileSize) + this.camera.Y, this.tileSize, this.tileSize);
                    canvas.FillRectangle(Brushes.DarkGreen, renderTile);
                }
            }

            var stepPercent = (float)this.stepCounter / (float)this.ticksPerStep;
            canvas.DrawString(stepPercent.ToString("P"), this.Font, Brushes.White, new Point(10, 10));
        }
    }
}
