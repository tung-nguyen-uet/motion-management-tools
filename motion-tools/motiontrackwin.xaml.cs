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
using System.Windows.Shapes;
using System.Diagnostics;
using System.IO;

namespace motion_tools
{
    /// <summary>
    /// Interaction logic for motiontrackwin.xaml
    /// </summary>
    public partial class motiontrackwin : Window
    {
        public motiontrackwin()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (cbcam.IsChecked == true)
            {
                Process process = new Process();
                process.StartInfo.FileName = "opentld.exe";
                process.StartInfo.WorkingDirectory = "c:\\Users\\PhucMinh\\Desktop\\Project Hanoi\\OpenTLD";
                process.StartInfo.Arguments = "-s";
                process.Start();
            }
            if (cbvid.IsChecked == true)
            {
                string sourceFile = textbox1.Text;
                string interFile1 = "c:\\Users\\PhucMinh\\Desktop\\Project Hanoi\\videoplayer\\dance.mp4";
                if (File.Exists(sourceFile) == false)
                {
                    MessageBox.Show("Chosen video not found");
                }
                else
                {
                    string path = sourceFile;
                    long length = new FileInfo(path).Length;
                    if (length > 10000000)
                    {
                        MessageBox.Show("The file you have selected is too large");
                    }
                    else
                    {
                        File.Copy(sourceFile, interFile1, true);
                        while (File.Exists(interFile1) == false) { }
                        Process process = new Process();
                        process.StartInfo.FileName = "python.exe";
                        process.StartInfo.WorkingDirectory = "c:\\Users\\PhucMinh\\Desktop\\Project Hanoi\\videoplayer";
                        process.StartInfo.Arguments = "videotrack.py";
                        process.Start();

                        bool IsProcessOpen(string name)
                        {
                            foreach (Process clsProcess in Process.GetProcesses())
                            {
                                if (clsProcess.ProcessName.Contains(name))
                                {
                                    return true;
                                }
                            }
                            return false;
                        }
                        while (IsProcessOpen("python") == true)
                        {
                            await Task.Delay(1000);
                        }

                        File.SetAttributes(interFile1, FileAttributes.Normal);
                        if (File.Exists(interFile1))
                        {
                            File.Delete(interFile1);
                        }
                        while (File.Exists(interFile1) == true) { }
                    }
                }
                string[] lines = System.IO.File.ReadAllLines("c:\\Users\\PhucMinh\\Desktop\\Project Hanoi\\videoplayer\\test.txt");
                for (int k = 0; k < 4; k = k + 1)
                {
                    Console.WriteLine(lines[k]);
                }
            }
            if (cbcam.IsChecked == false && cbvid.IsChecked == false)
            {
                MessageBox.Show("Choose input");
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog 
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and default file extension 
            dlg.DefaultExt = ".avi";
            dlg.Filter = "Video files |*.wmv; *.3g2; *.3gp; *.3gp2; *.3gpp; *.amv; *.asf;  *.avi; *.bin; *.cue; *.divx; *.dv; *.flv; *.gxf; *.iso; *.m1v; *.m2v; *.m2t; *.m2ts; *.m4v; *.mkv; *.mov; *.mp2; *.mp2v; *.mp4; *.mp4v; *.mpa; *.mpe; *.mpeg; *.mpeg1; *.mpeg2; *.mpeg4; *.mpg; *.mpv2; *.mts; *.nsv; *.nuv; *.ogg; *.ogm; *.ogv; *.ogx; *.ps; *.rec; *.rm; *.rmvb; *.tod; *.ts; *.tts; *.vob; *.vro; *.webm; *.dat; ";

            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();

            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                string filename = dlg.FileName;
                textbox1.Text = filename;
                textbox1.Focus();
                textbox1.CaretIndex = textbox1.Text.Length;
            }
        }

        private void textbox1_TextChanged(object sender, RoutedEventArgs e)
        {
            textbox1.Foreground = new SolidColorBrush(Colors.Black);
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            textblock1.Visibility = Visibility.Visible;
            textbox1.Visibility = Visibility.Visible;
            selectbut.Visibility = Visibility.Visible;
            cbcam.IsChecked = false;
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            textblock1.Visibility = Visibility.Hidden;
            textbox1.Visibility = Visibility.Hidden;
            selectbut.Visibility = Visibility.Hidden;
        }

        private void CheckBox_Checked_1(object sender, RoutedEventArgs e)
        {
            textblock1.Visibility = Visibility.Hidden;
            textbox1.Visibility = Visibility.Hidden;
            selectbut.Visibility = Visibility.Hidden;
            cbvid.IsChecked = false;
        }
    }
}
