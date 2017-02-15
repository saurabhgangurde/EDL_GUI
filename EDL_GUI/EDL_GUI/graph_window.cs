using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EDL_GUI
{
    public partial class graph_window : Form
    {

        string filename;
        public graph_window(string s)
        {
            InitializeComponent();
            filename = s;
            plot_data();
        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }
        private void plot_data()
        {
            try {
                string[] lines = System.IO.File.ReadAllLines(filename);
          

            foreach (string line in lines)
            {
                try
                {
                    List<int> data = line.Split(',').Select(Int32.Parse).ToList();
                    
                    chart1.Series["Series1"].Points.AddXY(data[0], data[1]);       

                }
                catch (Exception e)
                {

                }


            }
            }
            catch { }
            chart1.Series["Series1"].Color = Color.Red;

        }

        private void graph_window_Load(object sender, EventArgs e)
        {

        }

        private void chart1_Click_1(object sender, EventArgs e)
        {

        }
    }
}

