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
using Microsoft.Win32;
using System.Windows.Forms;
using System.IO;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<string> OrigImageFilePaths = new List<string>();
        List<string> ImageFilePaths = new List<string>();
        List<BitmapImage> BitmapImages = new List<BitmapImage>();
        List<string> FinalImageFilePaths = new List<string>();
        List<BitmapImage> FinalBitmapImages = new List<BitmapImage>();
        string saveFolderPath = null;

        public MainWindow()
        {
            InitializeComponent();
            LsImageGallery.DataContext = BitmapImages;
            FinalImagesList.DataContext = FinalBitmapImages;
        }

        private void Image_MouseUp(object sender, MouseButtonEventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            DialogResult dr = fbd.ShowDialog();
            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                string[] filepaths = Directory.GetFiles(fbd.SelectedPath, "*.jpg", SearchOption.AllDirectories);
                foreach (string filepath in filepaths)
                {
                    bool foundFilePath = false;
                    foreach (string filepath2 in OrigImageFilePaths)
                    {
                        if (filepath2 == filepath)
                        {
                            foundFilePath = true;
                            break;
                        }
                    }
                    if (!foundFilePath)
                    {
                        OrigImageFilePaths.Add(filepath);
                        ImageFilePaths.Add(filepath);
                        BitmapImage img = new BitmapImage();
                        img.BeginInit();
                        img.UriSource = new Uri(filepath);
                        img.EndInit();
                        BitmapImages.Add(img);
                    }
                }
            }
            LsImageGallery.Items.Refresh();
        }

        private void Image_MouseUp_1(object sender, MouseButtonEventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            DialogResult dr = fbd.ShowDialog();
            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                saveFolderPath = fbd.SelectedPath;
            }
        }

        private void Image_MouseUp_2(object sender, MouseButtonEventArgs e)
        {
            if (saveFolderPath != null && FinalImagesList.Items.Count > 0)
            {
                string fileNameAppendString = "A";
                for (int i = 0; i < FinalImagesList.Items.Count; i++)
                {
                    File.Copy(FinalImageFilePaths[i], saveFolderPath + "\\" + fileNameAppendString + ".jpg");
                    string test = ((char)(fileNameAppendString.Substring(fileNameAppendString.Length - 1, 1).ToCharArray()[0] + 1)).ToString();
                    if (test == "[")
                    {
                        fileNameAppendString += "A";
                    }
                    else
                    {
                        fileNameAppendString = fileNameAppendString.Substring(0, fileNameAppendString.Length - 1) + test;
                    }
                }
            }
        }

        private void Image_MouseUp_3(object sender, MouseButtonEventArgs e)
        {
            if (LsImageGallery.SelectedItem != null)
            {
                string tmpPath = ((BitmapImage)LsImageGallery.SelectedItem).UriSource.AbsolutePath;
                tmpPath = tmpPath.Replace("%20", " ").Replace("/", "\\");
                for (int i = 0; i < ImageFilePaths.Count; i++)
                {
                    if (ImageFilePaths[i] == tmpPath)
                    {
                        bool found = false;
                        foreach (string f in FinalImageFilePaths)
                        {
                            if (tmpPath == f)
                            {
                                found = true;
                                break;
                            }
                        }
                        if (!found)
                        {
                            FinalImageFilePaths.Add(tmpPath);
                            FinalBitmapImages.Add(BitmapImages[i]);
                            FinalImagesList.Items.Refresh();
                            ImageFilePaths.RemoveAt(i);
                            BitmapImages.RemoveAt(i);
                            LsImageGallery.Items.Refresh();
                        }
                        break;
                    }
                }
            }
        }

        private void Image_MouseUp_4(object sender, MouseButtonEventArgs e)
        {
            if (FinalImagesList.SelectedItem != null)
            {
                string tmpPath = ((BitmapImage)FinalImagesList.SelectedItem).UriSource.AbsolutePath;
                tmpPath = tmpPath.Replace("%20", " ").Replace("/", "\\");
                bool found = false;
                for (int i = 0; i < ImageFilePaths.Count; i++)
                {
                    if (ImageFilePaths[i] == tmpPath)
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    ImageFilePaths.Add(tmpPath);
                    BitmapImages.Add(FinalBitmapImages[FinalImagesList.SelectedIndex]);
                    LsImageGallery.Items.Refresh();
                }
                FinalImageFilePaths.RemoveAt(FinalImagesList.SelectedIndex);
                FinalBitmapImages.RemoveAt(FinalImagesList.SelectedIndex);
                FinalImagesList.Items.Refresh();
            }
        }

        private void Image_MouseUp_5(object sender, MouseButtonEventArgs e)
        {
            if (FinalImagesList.SelectedItem != null && FinalImagesList.SelectedIndex - 1 >= 0)
            {
                string tmpPath = FinalImageFilePaths[FinalImagesList.SelectedIndex];
                BitmapImage tmpBitmapImage = FinalBitmapImages[FinalImagesList.SelectedIndex];
                FinalImageFilePaths[FinalImagesList.SelectedIndex] = FinalImageFilePaths[FinalImagesList.SelectedIndex - 1];
                FinalImageFilePaths[FinalImagesList.SelectedIndex - 1] = tmpPath;
                FinalBitmapImages[FinalImagesList.SelectedIndex] = FinalBitmapImages[FinalImagesList.SelectedIndex - 1];
                FinalBitmapImages[FinalImagesList.SelectedIndex - 1] = tmpBitmapImage;
                FinalImagesList.Items.Refresh();
            }
        }

        private void Image_MouseUp_6(object sender, MouseButtonEventArgs e)
        {
            if (FinalImagesList.SelectedItem != null && FinalImagesList.SelectedIndex + 1 < FinalImagesList.Items.Count)
            {
                string tmpPath = FinalImageFilePaths[FinalImagesList.SelectedIndex];
                BitmapImage tmpBitmapImage = FinalBitmapImages[FinalImagesList.SelectedIndex];
                FinalImageFilePaths[FinalImagesList.SelectedIndex] = FinalImageFilePaths[FinalImagesList.SelectedIndex + 1];
                FinalImageFilePaths[FinalImagesList.SelectedIndex + 1] = tmpPath;
                FinalBitmapImages[FinalImagesList.SelectedIndex] = FinalBitmapImages[FinalImagesList.SelectedIndex + 1];
                FinalBitmapImages[FinalImagesList.SelectedIndex + 1] = tmpBitmapImage;
                FinalImagesList.Items.Refresh();
            }
        }

        private void Image_MouseUp_7(object sender, MouseButtonEventArgs e)
        {
            if(FinalImagesList.SelectedItem != null)
            {
                BitmapImage bmp2 = new BitmapImage();
                bmp2.BeginInit();
                bmp2.UriSource = new Uri(FinalImageFilePaths[FinalImagesList.SelectedIndex]);
                bmp2.Rotation = Rotation.Rotate270;
                bmp2.EndInit();
                FinalBitmapImages[FinalImagesList.SelectedIndex] = bmp2;
                FinalImagesList.Items.Refresh();
            }
        }

        private void Image_MouseUp_8(object sender, MouseButtonEventArgs e)
        {
            if (FinalImagesList.SelectedItem != null)
            {
                BitmapImage bmp2 = new BitmapImage();
                bmp2.BeginInit();
                bmp2.UriSource = new Uri(FinalImageFilePaths[FinalImagesList.SelectedIndex]);
                bmp2.Rotation = Rotation.Rotate90;
                bmp2.EndInit();
                FinalBitmapImages[FinalImagesList.SelectedIndex] = bmp2;
                FinalImagesList.Items.Refresh();
            }
        }

        private void Image_MouseUp_9(object sender, MouseButtonEventArgs e)
        {
            BitmapImage bmp2 = new BitmapImage();
            bmp2.BeginInit();
            bmp2.UriSource = new Uri(FinalImageFilePaths[FinalImagesList.SelectedIndex]);
            bmp2.EndInit();
            FinalBitmapImages[FinalImagesList.SelectedIndex] = bmp2;
            FinalImagesList.Items.Refresh();
        }

        private void Image_MouseUp_10(object sender, MouseButtonEventArgs e)
        {
            if(LsImageGallery.SelectedItem != null)
            {
                string filePath = ImageFilePaths[LsImageGallery.SelectedIndex];
                ImageFilePaths.RemoveAt(LsImageGallery.SelectedIndex);
                BitmapImages.RemoveAt(LsImageGallery.SelectedIndex);
                LsImageGallery.ItemsSource = BitmapImages;
                LsImageGallery.Items.Refresh();
            }
        }
    }
}
