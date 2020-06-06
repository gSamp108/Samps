using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Simulation
{
    public partial class formMain : Form
    {
        private bool showHelpMenu;
        private Universe universe;
        private int universeMenuPosition;
        private Cluster cluster;
        private int clusterMenuPosition;
        private World world;
        private int worldMenuPosition;
        private Region region;
        private int regionMenuPosition;

        public formMain()
        {
            this.InitializeComponent();
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            this.Invalidate();
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);
            if (e.KeyCode == Keys.F1) this.showHelpMenu = !this.showHelpMenu;
            if (e.KeyCode == Keys.F2) this.universe = new Universe();
            if (e.KeyCode == Keys.Up) this.MoveCurrentMenuIndex(-1);
            if (e.KeyCode == Keys.Down) this.MoveCurrentMenuIndex(1);
            if (e.KeyCode == Keys.Left) this.MoveIntoNextMenu();
            if (e.KeyCode == Keys.Right) this.MoveBackToLastMenu();
            this.Invalidate();
        }

        private void MoveBackToLastMenu()
        {
        }

        private void MoveIntoNextMenu()
        {
            if (this.universe == null) return;
            else if (this.cluster == null) this.universeMenuPosition = (this.universeMenuPosition + amount).Clamp(0, this.universe.Clusters.Count - 1);
            else if (this.world == null) this.clusterMenuPosition = (this.clusterMenuPosition + amount).Clamp(0, this.cluster.Worlds.Count - 1);
            else if (this.region == null) this.worldMenuPosition = (this.worldMenuPosition + amount).Clamp(0, this.world.Regions.Count - 1);
            else this.regionMenuPosition = 0;
        }

        private void MoveCurrentMenuIndex(int amount)
        {
            if (this.universe == null) return;
            else if (this.cluster == null) this.universeMenuPosition = (this.universeMenuPosition + amount).Clamp(0, this.universe.Clusters.Count - 1);
            else if (this.world == null) this.clusterMenuPosition = (this.clusterMenuPosition + amount).Clamp(0, this.cluster.Worlds.Count - 1);
            else if (this.region == null) this.worldMenuPosition = (this.worldMenuPosition + amount).Clamp(0, this.world.Regions.Count - 1);
            else this.regionMenuPosition = 0;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (this.showHelpMenu) this.PaintHelpMenu(e.Graphics);
            else this.PaintStandardDisplay(e.Graphics);
            e.Graphics.DrawString("F1 - HELP", this.Font, Brushes.White, new Point(this.ClientRectangle.Width - 100, 10));

        }

        private void PaintStandardDisplay(Graphics canvas)
        {
            var textCanvas = new GraphicsTextHelper(canvas, this.Font, Brushes.White, new Point(10, 10), 12);
            if (this.universe == null) this.PaintEmptyDisplay(canvas, textCanvas);
            else if (this.cluster == null) this.PaintUniverseMenu(canvas, textCanvas);
            else if (this.world == null) this.PaintClusterMenu(canvas, textCanvas);
            else if (this.region == null) this.PaintWorldMenu(canvas, textCanvas);
            else this.PaintRegionMenu(canvas, textCanvas);
        }

        private void PaintRegionMenu(Graphics canvas, GraphicsTextHelper textCanvas)
        {
            throw new NotImplementedException();
        }

        private void PaintWorldMenu(Graphics canvas, GraphicsTextHelper textCanvas)
        {
            throw new NotImplementedException();
        }

        private void PaintClusterMenu(Graphics canvas, GraphicsTextHelper textCanvas)
        {
            throw new NotImplementedException();
        }

        private void PaintUniverseMenu(Graphics canvas, GraphicsTextHelper textCanvas)
        {
            var index = 0;
            foreach(var cluster in this.universe.Clusters.All<Cluster>())
            {
                if (index == this.universeMenuPosition) textCanvas.Draw(">>> " + cluster.ToString(), Brushes.Cyan);
                else textCanvas.Draw("    " + cluster.ToString());
                index += 1;
            }
        }

        private void PaintEmptyDisplay(Graphics canvas, GraphicsTextHelper textCanvas)
        {
            textCanvas.Draw("Nothing Loaded");
        }

        private void PaintHelpMenu(Graphics canvas)
        {
            var textCanvas = new GraphicsTextHelper(canvas, this.Font, Brushes.White, new Point(10, 10), 12);
            textCanvas.Draw("F2 - Generate New Universe");
        }
    }
}
