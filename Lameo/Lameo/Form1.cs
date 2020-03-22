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
        private Point camera;
        private int tileSize = 8;
        private int cameraSpeed = 25;
        private WorldGenerator worldGenerator;
        private int stepCounter = 0;
        private Random rng = new Random();
        private int yRenderPosition = 0;
        private int yRenderStart = 10;
        private int yRenderGain = 12;
        private Graphics yRenderCanvas;

        public Form1()
        {
            this.InitializeComponent();
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);
            this.timer = new Timer();
            this.timer.Interval = 1;
            this.timer.Tick += Timer_Tick;
            this.timer.Start();
            this.camera = new Point((this.ClientRectangle.Width / 2), (this.ClientRectangle.Height / 2));
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (this.worldGenerator != null)
            {
                this.stepCounter += 1;
                if (this.stepCounter > this.inputStepSpeed.XIntValue)
                {
                    for (int i = 0; i < this.inputStepsPerTick.XIntValue; i++)
                    {
                        this.worldGenerator.Tick();
                    }
                    this.stepCounter = 0;
                }
            }
            this.UpdateInputs();
            this.Invalidate();
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);
            if (e.KeyCode == Keys.F1)
            {
                using (var input = new TextInputForm())
                {
                    if (input.ShowDialog() == DialogResult.OK)
                    {
                        this.inputSeed.XText = input.Text;
                        this.InitializeWorldGenerator();
                    }
                }
            }
            if (e.KeyCode == Keys.F2)
            {
                this.inputSeed.XText = this.rng.Next().ToString();
                this.InitializeWorldGenerator();
            }
            if (e.KeyCode == Keys.Up) this.camera.Y -= this.cameraSpeed;
            if (e.KeyCode == Keys.Right) this.camera.X += this.cameraSpeed;
            if (e.KeyCode == Keys.Down) this.camera.Y += this.cameraSpeed;
            if (e.KeyCode == Keys.Left) this.camera.X -= this.cameraSpeed;
        }

        private void InitializeWorldGenerator()
        {
            this.worldGenerator = new WorldGenerator(this.inputSeed.XIntValue );
            this.worldGenerator.SizeThreshold = this.inputSizeThreshold.XIntValue;
            this.UpdateInputs();
        }

        private void UpdateInputs()
        {
            if (this.worldGenerator == null) return;
            this.inputClosedCount.XText = this.worldGenerator.Closed.Count.ToString();
            this.inputAddQueueCount.XText = this.worldGenerator.AddQueue.Count.ToString();
            this.inputStatus.XText = this.worldGenerator.Status.ToString();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var canvas = e.Graphics;
            this.yRenderPosition = this.yRenderStart;
            this.yRenderCanvas = canvas;

            if (this.worldGenerator != null)
            {
                foreach(var tile in this.worldGenerator.Closed)
                {
                    var renderTile = new Rectangle((tile.X * this.tileSize) + this.camera.X, (tile.Y * this.tileSize) + this.camera.Y, this.tileSize, this.tileSize);
                    canvas.FillRectangle(Brushes.DarkGreen, renderTile);
                }
            }

            var stepPercent = (float)this.stepCounter / (float)this.inputStepSpeed.XIntValue;
            this.PaintText(stepPercent.ToString("P"));
        }

        private void PaintText(string text)
        {
            this.yRenderCanvas.DrawString(text, this.Font, Brushes.White, new Point(10, this.yRenderPosition));
            this.yRenderPosition += this.yRenderGain;
        }
    }
}
