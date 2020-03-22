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
    public partial class TextInputForm : Form
    {
        public string Result = string.Empty;
        public int IntResult
        {
            get
            {
                var result = 0;
                int.TryParse(this.Result, out result);
                return result;
            }
        }
        public TextInputForm()
        {
            InitializeComponent();
            this.textBox1.KeyUp += TextBox1_KeyUp;
            this.textBox1.Focus();
        }

        private void TextBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode== Keys.Enter)
            {
                this.DialogResult = DialogResult.OK;
                this.Result = this.textBox1.Text;
                this.Close();
            }
            else if (e.KeyCode== Keys.Escape)
            {
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
        }
    }
}
