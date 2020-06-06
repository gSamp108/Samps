using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Simulation.UI
{
    public sealed class TextMenu
    {
        public event Action GoBack = delegate { };
        public event Action<object> GoForward = delegate { };

        public Point Position;
        public bool Visible = true;

        private List<object> contents = new List<object>();
        private int selectedIndex;

        public TextMenu(Point position)
        {
            this.Position = position;
        }

        public void AddItem(object item)
        {
            this.contents.Add(item);
        }
        public void RemoveItem(object item)
        {
            this.contents.Remove(item);
            if (this.selectedIndex >= this.contents.Count) this.selectedIndex = this.contents.Count - 1;
        }
        
        public void ReceiveInput(KeyEventArgs keyEvent)
        {
            if (keyEvent.KeyCode == Keys.W) this.MoveSelection(-1);
            if (keyEvent.KeyCode == Keys.S) this.MoveSelection(1);
            if (keyEvent.KeyCode == Keys.D) this.TryGoForward();
            if (keyEvent.KeyCode == Keys.A) this.GoBack();
        }

        private void TryGoForward()
        {
            if (this.contents.Count > 0) this.GoForward(this.contents[this.selectedIndex]);
        }

        private void MoveSelection(int amount)
        {
            this.selectedIndex += amount;
            if (this.selectedIndex < 0) this.selectedIndex = 0;
            if (this.selectedIndex >= this.contents.Count) this.selectedIndex = this.contents.Count - 1;
        }

        public void Paint(Graphics canvas, Font font)
        {
            var textCanvas = new GraphicsTextHelper(canvas, font, Brushes.White, new Point(this.Position.X + 10, this.Position.Y + 10), 12);
            for (var i = 0; i < this.contents.Count; i++)
            {
                var item = this.contents[i];
                if (i == this.selectedIndex) textCanvas.Draw(">>> " + item.ToString(), Brushes.Cyan);
                else textCanvas.Draw("    " + item.ToString());
            }
        }
    }
}
