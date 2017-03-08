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
        public graph_window()
        {
            InitializeComponent();            
        }

        public void setFilename(string s)
        {
            filename = s;
        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }
        public void plot_data()
        {
            try {
                string[] lines = System.IO.File.ReadAllLines(filename);
          

            foreach (string line in lines)
            {
                try
                {
                    List<int> data = line.Split(',').Select(Int32.Parse).ToList();
                    
                    chart1.Series["Series1"].Points.AddXY((data[0]-2.7598883559)/( 204.52*3.2), (data[1]- 2.7598883559)*1000 /( 204.52*101 *1.3));       

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

