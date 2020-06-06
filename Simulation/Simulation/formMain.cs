using Simulation.UI;
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
        private List<TextMenu> menus = new List<TextMenu>();

        private Universe universe;

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

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyUp(e);
            if (e.KeyCode == Keys.F1) this.showHelpMenu = !this.showHelpMenu;
            if (e.KeyCode == Keys.F2)
            {
                this.menus.Clear();
                this.universe = new Universe();
                var universeMenu = new TextMenu(new Point());
                universeMenu.GoForward += this.AllMenus_GoForward;
                foreach (var cluster in this.universe.Clusters.All<Cluster>())
                {
                    universeMenu.AddItem(cluster);
                }
                this.menus.Add(universeMenu);
            }
            if (this.menus.Count > 0) this.menus.Last().ReceiveInput(e);
            this.Invalidate();
        }

        private void AllMenus_GoForward(object selectedItem)
        {
            var menu = new TextMenu(new Point());
            menu.GoBack += this.AllMenus_GoBack;
            menu.GoForward += this.AllMenus_GoForward;

            if (selectedItem is Cluster cluster)
            {
                foreach (var world in cluster.Worlds)
                {
                    menu.AddItem(world);
                }
                this.menus.Add(menu);
            }
            else if (selectedItem is World world)
            {
                foreach (var region in world.Regions)
                {
                    menu.AddItem(region);
                }
                this.menus.Add(menu);
            }
            else if (selectedItem is Region region)
            {
                menu.AddItem("Minerals " + region.Minerals);
                menu.AddItem("Organics " + region.Organics);
                menu.AddItem("Radiologicals " + region.Radiologicals);
                this.menus.Add(menu);
            }

            this.Invalidate();
        }

        private void AllMenus_GoBack()
        {
            var currentMenu = this.menus.Last();
            this.menus.Remove(currentMenu);
            this.Invalidate();
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
            if (this.menus.Count > 0) this.menus.Last().Paint(canvas, this.Font);
            else canvas.DrawString("Nothing Loaded", this.Font, Brushes.White, new Point(10, 10));

            //var textCanvas = new GraphicsTextHelper(canvas, this.Font, Brushes.White, new Point(10, 10), 12);
            //if (this.universe == null) this.PaintEmptyDisplay(canvas, textCanvas);
            //else if (this.cluster == null) this.PaintUniverseMenu(canvas, textCanvas);
            //else if (this.world == null) this.PaintClusterMenu(canvas, textCanvas);
            //else if (this.region == null) this.PaintWorldMenu(canvas, textCanvas);
            //else this.PaintRegionMenu(canvas, textCanvas);
        }

        private void PaintHelpMenu(Graphics canvas)
        {
            var textCanvas = new GraphicsTextHelper(canvas, this.Font, Brushes.White, new Point(10, 10), 12);
            textCanvas.Draw("F2 - Generate New Universe");
        }
    }
}
