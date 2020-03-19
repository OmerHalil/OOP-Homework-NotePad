using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NotePad
{
    public partial class DovizWin : Form
    {
        private Dictionary<string, float> Doviz;
        public DovizWin(Dictionary<string, float> Doviz)
        {
            this.Doviz = Doviz;
            InitializeComponent();
        }

        private void Doviz_Load(object sender, EventArgs e)
        {
            float TRY = Doviz["TRY"];
            float val;
            foreach (string s in Doviz.Keys)
            {
                try
                {
                    val = Doviz[s];
                    val = TRY / val;
                    listBox1.Items.Add(s + "   " + val);
                }
                catch
                {

                }
                
            }
        }
    }
}
