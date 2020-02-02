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
    public partial class MainForm : BaseForm
    {
        private sealed class UIOptions
        {
            public bool ShowGrid = false;
            public int TileSize = 16;
            public bool RenderDebugInfo = false;
        }

        private sealed class TextRenderer
        {
            private Graphics canvas;
            private Point renderOrigin;
            private Point currentRenderPosition;
            private Font font;
            private int lineSize = 12;

            public TextRenderer(Graphics canvas,Font font, Point renderOrigin)
            {
                this.canvas = canvas;
                this.renderOrigin = renderOrigin;
                this.currentRenderPosition = this.renderOrigin;
                this.font = font;
            }

            public void Draw(string text)
            {
                this.canvas.DrawString(text, this.font, Brushes.White, this.currentRenderPosition);
                this.currentRenderPosition.Y += this.lineSize;
            }
        }

        private Simulation.World world;
        private Point cameraPosition;
        private UIOptions uiOptions = new UIOptions();

        public MainForm()
        {
            InitializeComponent();
            this.world = new Simulation.World(100);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (e.KeyCode == Keys.F1) this.uiOptions.ShowGrid = !this.uiOptions.ShowGrid;
            if (e.KeyCode == Keys.F2) this.uiOptions.RenderDebugInfo = !this.uiOptions.RenderDebugInfo;

            this.Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var canvas = e.Graphics;
            var centerScreen = new Point(this.ClientRectangle.Width / 2, this.ClientRectangle.Height / 2);
            var tilesPerScreen = new Point((this.ClientRectangle.Width / this.uiOptions.TileSize) + 1, (this.ClientRectangle.Height / this.uiOptions.TileSize) + 1);
            var tilesPerHalfScreen = new Point((tilesPerScreen.X / 2) + 1, (tilesPerScreen.Y / 2) + 1);
            var cameraTile = this.world.WrapPosition(new Simulation.Position(this.cameraPosition.X / this.uiOptions.TileSize, this.cameraPosition.Y / this.uiOptions.TileSize));
            var cameraTileOffset = new Point(this.cameraPosition.X - (cameraTile.X * this.uiOptions.TileSize), this.cameraPosition.Y - (cameraTile.Y * this.uiOptions.TileSize));
            var renderOriginTilePosition = this.world.WrapPosition(new Simulation.Position(cameraTile.X - tilesPerHalfScreen.X, cameraTile.Y - tilesPerHalfScreen.Y));
            var renderOriginTileScreenPosition = new Point(centerScreen.X + (cameraTileOffset.X - (tilesPerHalfScreen.X * this.uiOptions.TileSize)), centerScreen.Y + (cameraTileOffset.Y - (tilesPerHalfScreen.Y * this.uiOptions.TileSize)));

            canvas.Clear(Color.Black);

            for (var x = 0; x <= tilesPerScreen.X; x++)
            {
                for (var y = 0; y <= tilesPerScreen.Y; y++)
                {
                    var currentTilePosition = this.world.WrapPosition(new Simulation.Position(renderOriginTilePosition.X + x, renderOriginTilePosition.Y + y));
                    var currentTileOffset = new Point(renderOriginTileScreenPosition.X + (x * this.uiOptions.TileSize), renderOriginTileScreenPosition.Y + (y * this.uiOptions.TileSize));
                    var currentTileRenderArea = new Rectangle(currentTileOffset.X, currentTileOffset.Y, this.uiOptions.TileSize, this.uiOptions.TileSize);
                    var currentTile = this.world.GetTile(currentTilePosition);
                    if (currentTile.IsLand) canvas.FillRectangle(Brushes.DarkGreen, currentTileRenderArea);
                    else canvas.FillRectangle(Brushes.DarkBlue, currentTileRenderArea);
                    if (this.uiOptions.ShowGrid) canvas.DrawRectangle(Pens.Black, currentTileRenderArea);
                }
            }

            canvas.DrawLine(Pens.White, new Point(centerScreen.X, centerScreen.Y - 8), new Point(centerScreen.X, centerScreen.Y + 8));
            canvas.DrawLine(Pens.White, new Point(centerScreen.X - 8, centerScreen.Y), new Point(centerScreen.X + 8, centerScreen.Y));

            if (this.uiOptions.RenderDebugInfo)
            {
                var textRenderer = new TextRenderer(canvas, this.Font, new Point(10, 10));
                textRenderer.Draw("centerScreen: " + centerScreen.ToString());
                textRenderer.Draw("tilesPerScreen: " + tilesPerScreen.ToString());
                textRenderer.Draw("tilesPerHalfScreen: " + tilesPerHalfScreen.ToString());
                textRenderer.Draw("cameraTile: " + cameraTile.ToString());
                textRenderer.Draw("cameraTileOffset: " + cameraTileOffset.ToString());
                textRenderer.Draw("renderOriginTilePosition: " + renderOriginTilePosition.ToString());
                textRenderer.Draw("renderOriginTileScreenPosition: " + renderOriginTileScreenPosition.ToString());
            }
        }
    }
}
