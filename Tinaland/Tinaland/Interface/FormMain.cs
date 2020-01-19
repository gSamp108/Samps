using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tinaland.Interface
{
    public partial class FormMain : Form
    {
        private Timer updateTimer;
        private HashSet<Keys> pressedKeys = new HashSet<Keys>();
        private Game.World world;
        private HudPainter hudPainter;
        private DebugSystem debugSystem = new DebugSystem();
        private Game.Entity Player;

        public FormMain()
        {
            this.InitializeComponent();
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);
            this.world = new Game.World(new Game.Engine(), 4);
            this.Player = new Game.Entity(this.world);

            this.updateTimer = new Timer();
            this.updateTimer.Interval = 1;
            this.updateTimer.Tick += UpdateTimer_Tick;
            this.updateTimer.Start();
        }

        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            this.GameTick();
            this.Invalidate();
        }

        private void GameTick()
        {
            this.InputTick();
            this.world.Tick();
        }

        private void InputTick()
        {
            if (this.pressedKeys.Contains(Keys.W)) this.Player.WorldPosition.Y -= 1;
            if (this.pressedKeys.Contains(Keys.D)) this.Player.WorldPosition.X += 1;
            if (this.pressedKeys.Contains(Keys.S)) this.Player.WorldPosition.Y += 1;
            if (this.pressedKeys.Contains(Keys.A)) this.Player.WorldPosition.X -= 1;
            if (this.pressedKeys.Contains(Keys.F))
            {
                var currentTile = this.Player.Tile.Number = this.world.Engine.Rng.Next(10);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            this.PaintGame(e.Graphics);
            this.PaintHud(e.Graphics);
        }

        private void PaintHud(Graphics canvas)
        {
            if (this.hudPainter == null) this.hudPainter = new HudPainter(canvas, this.Font);
            this.hudPainter.Reinitialize(canvas);

            this.hudPainter.DrawLine("WorldPosition: " + this.Player.WorldPosition.ToString(), Brushes.Black);
            this.hudPainter.DrawLine("ChunkPosition: " + this.Player.ChunkPosition.ToString(), Brushes.Black);
            this.hudPainter.DrawLine("ChunkLocalPosition: " + this.Player.ChunkLocalPosition.ToString(), Brushes.Black);
            this.hudPainter.DrawLine("TilePosition: " + this.Player.TilePosition.ToString(), Brushes.Black);
            this.hudPainter.DrawLine("TileLocalPosition: " + this.Player.TileLocalPosition.ToString(), Brushes.Black);
            this.hudPainter.DrawLine("", Brushes.Black);
            this.hudPainter.DrawLine("CenterScreen: " + this.debugSystem.Painting.CenterScreen, Brushes.Black);
            this.hudPainter.DrawLine("CurrentTileCornerOffset: " + this.debugSystem.Painting.CurrentTileCornerOffset, Brushes.Black);
            this.hudPainter.DrawLine("CurrentTileCorner: " + this.debugSystem.Painting.CurrentTileCorner, Brushes.Black);

        }

        private void PaintGame(Graphics canvas)
        {            
            var centerScreen = new Point(this.ClientSize.Width / 2, this.ClientSize.Height / 2);
            var screenTilesHalfWidth = (int)(Math.Ceiling((float)Math.Ceiling((float)this.ClientRectangle.Width / (float)Game.Tile.Size) + 2) / 2f);
            var screenTilesHalfHeight = (int)(Math.Ceiling((float)Math.Ceiling((float)this.ClientRectangle.Height / (float)Game.Tile.Size) + 2) / 2f);
            var topLeftCornerTilePosition = new Point(this.Player.TilePosition.X - screenTilesHalfWidth, this.Player.TilePosition.Y - screenTilesHalfHeight);
            var topLeftCornerPaintPosition = new Point((centerScreen.X-this.Player.TileLocalPosition.X )-(screenTilesHalfWidth*Game.Tile.Size), (centerScreen.Y - this.Player.TileLocalPosition.Y) - (screenTilesHalfHeight * Game.Tile.Size));

            for (int x = 0; x < (screenTilesHalfWidth * 2); x++)
            {
                for (int y = 0; y < (screenTilesHalfHeight * 2); y++)
                {
                    var currentTilePosition = new Game.Position(topLeftCornerTilePosition.X + x, topLeftCornerTilePosition.Y + y);
                    var currentTile = this.Player.Chunk.GetTile(currentTilePosition);
                    var currentTilePaintPosition = new Point(topLeftCornerPaintPosition.X + (x * Game.Tile.Size), topLeftCornerPaintPosition.Y + (y * Game.Tile.Size));
                    canvas.DrawRectangle(Pens.Black, new Rectangle(currentTilePaintPosition.X , currentTilePaintPosition.Y , Game.Tile.Size, Game.Tile.Size));
                    if (currentTile.Number > 0) canvas.DrawString(currentTile.Number.ToString(), this.Font, Brushes.Black, currentTilePaintPosition);
                }
            }
            //var currentTileCornerOffset = new Point((this.cameraTilePosition.X * this.tileSize) - this.cameraPosition.X, (this.cameraTilePosition.Y * this.tileSize) - this.cameraPosition.Y);
            //var currentTileCorner = new Point(centerScreen.X + (-this.cameraPosition.X + currentTileCornerOffset.X), centerScreen.Y + (-this.cameraPosition.Y + currentTileCornerOffset.Y));

            //for (int x = 0; x < Game.Chunk.Size; x++)
            //{
            //    for (int y = 0; y < Game.Chunk.Size; y++)
            //    {
            //        canvas.DrawRectangle(Pens.Black, new Rectangle(currentTileCorner.X + (x * this.tileSize), currentTileCorner.Y + (y * this.tileSize), this.tileSize, this.tileSize));
            //    }
            //}

            canvas.DrawLine(Pens.Red, new Point(centerScreen.X - 4, centerScreen.Y), new Point(centerScreen.X + 4, centerScreen.Y));
            canvas.DrawLine(Pens.Red, new Point(centerScreen.X, centerScreen.Y - 4), new Point(centerScreen.X, centerScreen.Y + 4));

            this.debugSystem.Painting.CenterScreen = centerScreen;
            //this.debugSystem.Painting.CurrentTileCornerOffset = currentTileCornerOffset;
            //this.debugSystem.Painting.CurrentTileCorner = currentTileCorner;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            this.pressedKeys.Add(e.KeyCode);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);
            this.pressedKeys.Remove(e.KeyCode);
        }
    }
}
