using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MM3.Simulation;

namespace MM3.Interface
{
    public partial class MainForm : BaseForm
    {
        private sealed class UIOptions
        {
            public bool ShowGrid = false;
            public int TileSize = 16;
            public bool RenderDebugInfo = false;
            public Size MountainRenderSize = new Size(4, 6);
            public int MountainsRenderedPerTile = 3;
            public Size ForestRenderSize = new Size(4, 6);
            public int ForestsRenderedPerTile = 3;
            public Size HillRenderSize = new Size(6, 4);
            public int HillsRenderedPerTile = 3;
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

        private sealed class MouseInput
        {
            public bool LeftButtonDown;
            public bool RightButtonDown;
            public Point LeftButtonDownPosition;
            public Point RightButtonDownPosition;
        }

        private sealed class WorldTileRenderingData
        {
            public Point[] RenderPoints;
        }

        private Simulation.World world;
        private Point cameraPosition;
        private Point cameraDragPosition;
        private UIOptions uiOptions = new UIOptions();
        private MouseInput mouseInput = new MouseInput();
        private Dictionary<Simulation.Position, WorldTileRenderingData> tileRenderingData = new Dictionary<Simulation.Position, WorldTileRenderingData>();
        private Random rng = new Random();

        public MainForm()
        {
            InitializeComponent();
            this.world = new World(100);
        }

        protected override void OnResizeEnd(EventArgs e)
        {
            base.OnResizeEnd(e);
            this.Invalidate();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (e.KeyCode == Keys.F1) this.uiOptions.ShowGrid = !this.uiOptions.ShowGrid;
            if (e.KeyCode == Keys.F2) this.uiOptions.RenderDebugInfo = !this.uiOptions.RenderDebugInfo;
            if (e.KeyCode == Keys.F3) (new DrawingForm()).Show();
            if (e.KeyCode == Keys.F4) this.world = new World(100);

            this.Invalidate();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left)
            {
                this.mouseInput.LeftButtonDown = true;
                this.mouseInput.LeftButtonDownPosition = e.Location;
            }
            if (e.Button == MouseButtons.Right)
            {
                this.mouseInput.RightButtonDown = true;
                this.mouseInput.RightButtonDownPosition = e.Location;
                this.cameraDragPosition = this.cameraPosition;
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (this.mouseInput.RightButtonDown)
            {
                var cameraDragDelta = new Point(this.mouseInput.RightButtonDownPosition.X - e.X, this.mouseInput.RightButtonDownPosition.Y - e.Y);
                this.cameraPosition = new Point(this.cameraDragPosition.X + cameraDragDelta.X, this.cameraDragPosition.Y + cameraDragDelta.Y);
                this.cameraPosition = this.cameraPosition.Wrap(new Rectangle(0, 0, this.world.Size * this.uiOptions.TileSize, this.world.Size * this.uiOptions.TileSize));
                this.Invalidate();
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (e.Button == MouseButtons.Left)this.mouseInput.LeftButtonDown = false;
            if (e.Button == MouseButtons.Right)this.mouseInput.RightButtonDown = false;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var canvas = e.Graphics;
            var centerScreen = new Point(this.ClientRectangle.Width / 2, this.ClientRectangle.Height / 2);
            var tilesPerScreen = new Point((this.ClientRectangle.Width / this.uiOptions.TileSize) + 1, (this.ClientRectangle.Height / this.uiOptions.TileSize) + 1);
            var cameraTile = this.world.WrapPosition(new Simulation.Position(this.cameraPosition.X / this.uiOptions.TileSize, this.cameraPosition.Y / this.uiOptions.TileSize));
            var cameraTileOffset = new Point((cameraTile.X * this.uiOptions.TileSize) - this.cameraPosition.X, (cameraTile.Y * this.uiOptions.TileSize) - this.cameraPosition.Y);

            canvas.Clear(Color.Black);

            for (var x = 0; x <= tilesPerScreen.X; x++)
            {
                for (var y = 0; y <= tilesPerScreen.Y; y++)
                {
                    var currentTilePosition = this.world.WrapPosition(new Simulation.Position(cameraTile.X + x, cameraTile.Y + y));
                    var currentTileRenderArea = new Rectangle(cameraTileOffset.X + (x*this.uiOptions.TileSize), cameraTileOffset.Y + (y * this.uiOptions.TileSize), this.uiOptions.TileSize, this.uiOptions.TileSize);
                    var currentTile = this.world.GetTile(currentTilePosition);
                    if (currentTile.Terrain== Simulation.Tile.TerrainTypes.Ocean) canvas.FillRectangle(Brushes.DarkBlue, currentTileRenderArea);
                    else if (currentTile.Terrain == Simulation.Tile.TerrainTypes.Shallow) canvas.FillRectangle(Brushes.Blue, currentTileRenderArea);
                    else
                    {
                        if (currentTile.Biome == Tile.BiomeTypes.Cold) canvas.FillRectangle(Brushes.LightGray, currentTileRenderArea);
                        else if (currentTile.Biome == Tile.BiomeTypes.Hot) canvas.FillRectangle(Brushes.YellowGreen, currentTileRenderArea);
                        else canvas.FillRectangle(Brushes.DarkGreen, currentTileRenderArea);

                        if (currentTile.Terrain == Simulation.Tile.TerrainTypes.Forest) this.PaintForest(currentTile,canvas, currentTileRenderArea);
                        else if (currentTile.Terrain == Simulation.Tile.TerrainTypes.Hill) this.PaintHill(currentTile, canvas, currentTileRenderArea);
                        else if (currentTile.Terrain == Simulation.Tile.TerrainTypes.Mountain) this.PaintMountain(currentTile, canvas, currentTileRenderArea);
                    }
                    if (this.uiOptions.ShowGrid) canvas.DrawRectangle(Pens.Black, currentTileRenderArea);
                }
            }

            if (this.uiOptions.RenderDebugInfo)
            {
                var textRenderer = new TextRenderer(canvas, this.Font, new Point(10, 10));
                textRenderer.Draw("centerScreen: " + centerScreen.ToString());
                textRenderer.Draw("tilesPerScreen: " + tilesPerScreen.ToString());
                textRenderer.Draw("cameraTile: " + cameraTile.ToString());
                textRenderer.Draw("cameraTileOffset: " + cameraTileOffset.ToString());
            }
        }

        private void GenerateTileRenderingData(Position tilePosition, Size availableRenderSpace, int featuresPerTile)
        {
            var renderingData = new WorldTileRenderingData() { RenderPoints = new Point[featuresPerTile] };
            for (int i = 0; i < renderingData.RenderPoints.Length; i++)
            {
                renderingData.RenderPoints[i] = new Point(this.rng.Next(availableRenderSpace.Width), this.rng.Next(availableRenderSpace.Height));
            }
            this.tileRenderingData.Add(tilePosition, renderingData);
        }

        private void PaintHill(Tile tile, Graphics canvas, Rectangle renderArea)
        {
            if (!this.tileRenderingData.ContainsKey(tile.Position)) this.GenerateTileRenderingData(tile.Position, new Size(this.uiOptions.TileSize - this.uiOptions.HillRenderSize.Width, this.uiOptions.TileSize - this.uiOptions.HillRenderSize.Height), this.uiOptions.HillsRenderedPerTile);

            var currentRenderingData = this.tileRenderingData[tile.Position];
            var humpWidthDistance = this.uiOptions.HillRenderSize.Width / 4;
            var humpHeightDistance = this.uiOptions.HillRenderSize.Height / 2;
            for (int i = 0; i < currentRenderingData.RenderPoints.Length; i++)
            {
                var renderOrigin = new Point(renderArea.X + currentRenderingData.RenderPoints[i].X, renderArea.Y + currentRenderingData.RenderPoints[i].Y);
                var renderPath = new Point[]
                {
                    new Point(renderOrigin.X, renderOrigin.Y + this.uiOptions.HillRenderSize.Height),
                    new Point(renderOrigin.X + humpWidthDistance, renderOrigin.Y+humpHeightDistance),
                    new Point(renderOrigin.X + (humpWidthDistance*2), renderOrigin.Y),
                    new Point(renderOrigin.X + (humpWidthDistance*3), renderOrigin.Y+humpHeightDistance),
                    new Point(renderOrigin.X + this.uiOptions.HillRenderSize.Width, renderOrigin.Y + this.uiOptions.HillRenderSize.Height),
                };

                canvas.FillPolygon(Brushes.DarkGoldenrod, renderPath);
                canvas.DrawLines(Pens.Black, renderPath);
            }
        }

        private void PaintMountain(Simulation.Tile tile, Graphics canvas, Rectangle renderArea)
        {
            if (!this.tileRenderingData.ContainsKey(tile.Position)) this.GenerateTileRenderingData(tile.Position, new Size(this.uiOptions.TileSize - this.uiOptions.MountainRenderSize.Width, this.uiOptions.TileSize - this.uiOptions.MountainRenderSize.Height), this.uiOptions.MountainsRenderedPerTile);

            var currentRenderingData = this.tileRenderingData[tile.Position];
            var peakDistance = this.uiOptions.MountainRenderSize.Width / 2;
            for (int i = 0; i < currentRenderingData.RenderPoints.Length; i++)
            {
                var renderOrigin = new Point(renderArea.X + currentRenderingData.RenderPoints[i].X, renderArea.Y + currentRenderingData.RenderPoints[i].Y);
                var renderPath = new Point[]
                {
                    new Point(renderOrigin.X, renderOrigin.Y + this.uiOptions.MountainRenderSize.Height),
                    new Point(renderOrigin.X + peakDistance, renderOrigin.Y),
                    new Point(renderOrigin.X + this.uiOptions.MountainRenderSize.Width, renderOrigin.Y + this.uiOptions.MountainRenderSize.Height)
                };

                canvas.FillPolygon(Brushes.Gray, renderPath);
                canvas.DrawLines(Pens.Black, renderPath);
            }
        }

        private void PaintForest(Simulation.Tile tile, Graphics canvas, Rectangle renderArea)
        {
            if (!this.tileRenderingData.ContainsKey(tile.Position)) this.GenerateTileRenderingData(tile.Position, new Size(this.uiOptions.TileSize - this.uiOptions.ForestRenderSize.Width, this.uiOptions.TileSize - this.uiOptions.ForestRenderSize.Height), this.uiOptions.ForestsRenderedPerTile);

            var currentRenderingData = this.tileRenderingData[tile.Position];
            for (int i = 0; i < currentRenderingData.RenderPoints.Length; i++)
            {
                var renderOrigin = new Point(renderArea.X + currentRenderingData.RenderPoints[i].X, renderArea.Y + currentRenderingData.RenderPoints[i].Y);
                canvas.FillEllipse(Brushes.Green, new Rectangle(renderOrigin.X, renderOrigin.Y, this.uiOptions.ForestRenderSize.Width, this.uiOptions.ForestRenderSize.Height / 2));
                canvas.DrawEllipse(Pens.Black, new Rectangle(renderOrigin.X, renderOrigin.Y, this.uiOptions.ForestRenderSize.Width, this.uiOptions.ForestRenderSize.Height / 2));
                canvas.DrawLine(Pens.Black, new Point(renderOrigin.X + (this.uiOptions.ForestRenderSize.Width / 2), renderOrigin.Y + (this.uiOptions.ForestRenderSize.Height / 2)), new Point(renderOrigin.X + (this.uiOptions.ForestRenderSize.Width / 2), renderOrigin.Y + this.uiOptions.ForestRenderSize.Height));
            }

        }
    }
}
