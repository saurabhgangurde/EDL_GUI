using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO.Ports;
using System.Threading;

namespace EDL_GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window

    {
        public SerialPort _serialport;
        public Thread _serialThread;
        static bool _continue = true;
        public String filename = "";
        graph_window graph_window= new graph_window();


        public MainWindow()
        {
            InitializeComponent();
            Load_COMPortComboBox();

            _serialport = new SerialPort();
            _serialport.PortName = "COM1";
            _serialThread = new Thread(ReadAndUpdate);
            _serialport.BaudRate = 9600;

        }

        private void Load_COMPortComboBox()
        {
            COMPortComboBox.Items.Clear();
            ComboBoxItem item1;
            COMPortComboBox.Text = "select COM port:";
            item1 = new ComboBoxItem();
            foreach (string s in SerialPort.GetPortNames())
            {
                item1.Content = s;
                COMPortComboBox.Items.Add(item1);
            }
        }


        private void startButton_Click(object sender, RoutedEventArgs e)
        {
            _serialThread = new Thread(ReadAndUpdate); //reinitialising thread
            DateTime current = DateTime.Now;
            String current_str = current.Date.ToString().Replace(" ", "");
            current_str = current.ToString().Replace(":", "-");
            filename = current_str + ".txt";
            _continue = true;

            try
            {
                _serialport.Open();
                _serialThread.Start(); //starts thread to take serial input and update gui
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            textBlock.Text = "Start button";
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            _continue = false;
 
            textBlock.Text = "Stop button";
        }

        private void COMPortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem port = (ComboBoxItem)COMPortComboBox.SelectedItem;
            try {
                if (port.Content.ToString() != "")
                {
                    _serialport.PortName = port.Content.ToString();
                }
            }
            catch { }
            MessageBox.Show("Port changed to"+ _serialport.PortName);
        }

        private void refreshButton_Click(object sender, RoutedEventArgs e)
        {
            Load_COMPortComboBox();
        }

        public void ReadAndUpdate()
        {

            while (_continue)
            {
                Thread.Sleep(10);
                try
                {
                    string message = _serialport.ReadLine();
                    //dispature used when a variable is updated by two threads
                    Dispatcher.Invoke(() =>
                    {
                        textBlock.Text = message;
                        
                    });

                    write_in_file(message);
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }

 
            }

            _serialport.Close();
            _serialThread.Abort();  //aborts previous thread
       
            
        
        }


        private void write_in_file(string s)
        {
            DateTime current = DateTime.Now;

            using (System.IO.StreamWriter file =
            new System.IO.StreamWriter(filename, true))
            {
                file.WriteLine(s);
            }
        }

        private void browseButton_Click(object sender, RoutedEventArgs e)
        {

            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = "Document"; // Default file name 
            dlg.DefaultExt = ".txt"; // Default file extension 
            dlg.Filter = "Text documents (.txt)|*.txt"; // Filter files by extension 
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results 
            if (result == true)
            {
                // Open document 
                filename = dlg.FileName;
                MessageBox.Show(filename);
            }
           graph_window.setFilename(filename) ;
            plotFileName.Text = filename;
            //System.Windows.Forms.Integration.WindowsFormsHost host =
            //               new System.Windows.Forms.Integration.WindowsFormsHost();
        }

        private void plotButton_Click(object sender, RoutedEventArgs e)
        {
            graph_window.plot_data();
            graph_window.Show();
        }
    }
}
