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
using System.Diagnostics;
using System.IO;

namespace motion_tools
{
    /// <summary>
    /// Interaction logic for keyframewin.xaml
    /// </summary>
    public partial class keyframewin : Window
    {
        public keyframewin()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            string sourceFile = textbox1.Text;
            string interFile1 = "c:\\Users\\PhucMinh\\Desktop\\Project Hanoi\\dance.mp4";
            string interFile2 = "c:\\Users\\PhucMinh\\Desktop\\Project Hanoi\\HisSummaryFTU.avi";
            string destFile = textbox2.Text;

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
                    bool syntaxok(string pathWname)
                    {
                        try
                        {
                            Directory.Exists(System.IO.Path.GetDirectoryName(destFile));
                        }
                        catch
                        {
                            return false;
                        }
                        return true;
                    }
                    if (syntaxok(destFile) == false)
                    {
                        MessageBox.Show("Directiry of edited video not found");
                    }
                    else
                    {
                        if (Directory.Exists(System.IO.Path.GetDirectoryName(destFile)) == false)
                        {
                            MessageBox.Show("Directory of edited video not found");
                        }
                        else
                        {
                            txt.Text = "Rendering";
                            int i = 0;
                            File.Copy(sourceFile, interFile1, true);
                            while (File.Exists(interFile1) == false) { }

                            Process process = new Process();
                            process.StartInfo.FileName = "python.exe";
                            process.StartInfo.WorkingDirectory = "c:\\Users\\PhucMinh\\Desktop\\Project Hanoi";
                            process.StartInfo.Arguments = "script.py";
                            process.Start();
                            bool IsFileLocked(FileInfo file)
                            {
                                FileStream stream = null;
                                try
                                {
                                    stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
                                }
                                catch (IOException)
                                {
                                    return true;
                                }
                                finally
                                {
                                    if (stream != null)
                                        stream.Close();
                                }
                                return false;
                            }
                            FileInfo fil = new FileInfo(interFile2);
                            while (IsFileLocked(fil) == true)
                            {
                                Console.WriteLine("processing" + i.ToString());
                                if (i % 3 == 0)
                                {
                                    txt.Text = "Rendering .";
                                    await Task.Delay(400);
                                }
                                if (i % 3 == 1)
                                {
                                    txt.Text = "Rendering . .";
                                    await Task.Delay(400);
                                }
                                if (i % 3 == 2)
                                {
                                    txt.Text = "Rendering . . .";
                                    await Task.Delay(400);
                                }
                                i = i + 1;
                            }

                            File.SetAttributes(interFile1, FileAttributes.Normal);
                            if (File.Exists(interFile1))
                            {
                                File.Delete(interFile1);
                            }
                            File.Copy(interFile2, destFile, true);
                            while (File.Exists(destFile) == false) { }

                            File.SetAttributes(interFile2, FileAttributes.Normal);
                            if (File.Exists(interFile2))
                            {
                                File.Delete(interFile2);
                            }
                            while (File.Exists(interFile1) == true || File.Exists(interFile2) == true) { }

                            textbox1.Text = "";
                            textbox2.Text = "";
                            if (i % 3 == 0)
                            {
                                txt.Text = "Rendering .";
                                await Task.Delay(400);
                            }
                            if (i % 3 == 1)
                            {
                                txt.Text = "Rendering . .";
                                await Task.Delay(400);
                            }
                            if (i % 3 == 2)
                            {
                                txt.Text = "Rendering . . .";
                                await Task.Delay(400);
                            }
                            i = i + 1;
                            txt.Text = "Rendering Complete";
                        }
                    }
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
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
                txt.Text = "Click on 'Keyframe'-button to edit video\nClick on 'Optical Flow'-button to edit video";
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "edited_video"; // Default file name
            dlg.DefaultExt = ".avi"; // Default file extension
            dlg.Filter = "Video files |*.avi; *.wmv; *.3g2; *.3gp; *.3gp2; *.3gpp; *.amv; *.asf;  *.bin; *.cue; *.divx; *.dv; *.flv; *.gxf; *.iso; *.m1v; *.m2v; *.m2t; *.m2ts; *.m4v; *.mkv; *.mov; *.mp2; *.mp2v; *.mp4; *.mp4v; *.mpa; *.mpe; *.mpeg; *.mpeg1; *.mpeg2; *.mpeg4; *.mpg; *.mpv2; *.mts; *.nsv; *.nuv; *.ogg; *.ogm; *.ogv; *.ogx; *.ps; *.rec; *.rm; *.rmvb; *.tod; *.ts; *.tts; *.vob; *.vro; *.webm; *.dat; "; // Filter files by extension

            // Show save file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                // Save document
                string filename = dlg.FileName;
                textbox2.Text = filename;
                textbox2.Focus();
                textbox2.CaretIndex = textbox2.Text.Length;
                txt.Text = "Click on 'Keyframe'-button to edit video\nClick on 'Optical Flow'-button to edit video";
            }
        }

        private void textbox1_TextChanged(object sender, RoutedEventArgs e)
        {
            textbox1.Foreground = new SolidColorBrush(Colors.Black);
            txt.Text = "Click on 'Keyframe'-button to edit video\nClick on 'Optical Flow'-button to edit video";
        }

        private void textbox2_TextChanged(object sender, TextChangedEventArgs e)
        {
            textbox2.Foreground = new SolidColorBrush(Colors.Black);
            txt.Text = "Click on 'Keyframe'-button to edit video\nClick on 'Optical Flow'-button to edit video";
        }


        private async void Button_Click_3(object sender, RoutedEventArgs e)
        {
            string sourceFile = textbox1.Text;
            string interFile1 = "c:\\Users\\PhucMinh\\Desktop\\Project Hanoi\\dance.mp4";
            string interFile2 = "c:\\Users\\PhucMinh\\Desktop\\Project Hanoi\\HisSummaryFTU.avi";
            string destFile = textbox2.Text;

            if (File.Exists(sourceFile) == false)
            {
                MessageBox.Show("Chosen video not found");
            }
            else
            {
                string path = sourceFile;
                long length = new FileInfo(path).Length;
                if (length > 2000000)
                {
                    MessageBox.Show("The file you have selected is too large");
                }
                else
                {
                    bool syntaxok(string pathWname)
                    {
                        try
                        {
                            Directory.Exists(System.IO.Path.GetDirectoryName(destFile));
                        }
                        catch
                        {
                            return false;
                        }
                        return true;
                    }
                    if (syntaxok(destFile) == false)
                    {
                        MessageBox.Show("Directory of edited video not found");
                    }
                    else
                    {
                        if (Directory.Exists(System.IO.Path.GetDirectoryName(destFile)) == false)
                        {
                            MessageBox.Show("Directory of edited video not found");
                        }
                        else
                        {
                            txt.Text = "Rendering";
                            int i = 0;
                            File.Copy(sourceFile, interFile1, true);
                            while (File.Exists(interFile1) == false) { }

                            Process process = new Process();
                            process.StartInfo.FileName = "python.exe";
                            process.StartInfo.WorkingDirectory = "c:\\Users\\PhucMinh\\Desktop\\Project Hanoi";
                            process.StartInfo.Arguments = "scriptoptflow.py";
                            process.Start();
                            bool IsFileLocked(FileInfo file)
                            {
                                FileStream stream = null;
                                try
                                {
                                    stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
                                }
                                catch (IOException)
                                {
                                    return true;
                                }
                                finally
                                {
                                    if (stream != null)
                                        stream.Close();
                                }
                                return false;
                            }
                            FileInfo fil = new FileInfo(interFile2);
                            while (IsFileLocked(fil) == true)
                            {
                                Console.WriteLine("processing" + i.ToString());
                                if (i % 3 == 0)
                                {
                                    txt.Text = "Rendering .";
                                    await Task.Delay(400);
                                }
                                if (i % 3 == 1)
                                {
                                    txt.Text = "Rendering . .";
                                    await Task.Delay(400);
                                }
                                if (i % 3 == 2)
                                {
                                    txt.Text = "Rendering . . .";
                                    await Task.Delay(400);
                                }
                                i = i + 1;
                            }

                            File.SetAttributes(interFile1, FileAttributes.Normal);
                            if (File.Exists(interFile1))
                            {
                                File.Delete(interFile1);
                            }
                            File.Copy(interFile2, destFile, true);
                            while (File.Exists(destFile) == false) { }

                            File.SetAttributes(interFile2, FileAttributes.Normal);
                            if (File.Exists(interFile2))
                            {
                                File.Delete(interFile2);
                            }
                            while (File.Exists(interFile1) == true || File.Exists(interFile2) == true) { }

                            textbox1.Text = "";
                            textbox2.Text = "";
                            if (i % 3 == 0)
                            {
                                txt.Text = "Rendering .";
                                await Task.Delay(400);
                            }
                            if (i % 3 == 1)
                            {
                                txt.Text = "Rendering . .";
                                await Task.Delay(400);
                            }
                            if (i % 3 == 2)
                            {
                                txt.Text = "Rendering . . .";
                                await Task.Delay(400);
                            }
                            i = i + 1;
                            txt.Text = "Rendering Complete";
                        }
                    }
                }
            }
        }
    }
}
