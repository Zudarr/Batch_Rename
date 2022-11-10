using Contract;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Path = System.IO.Path;

namespace Project01_BatchRename
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        ObservableCollection<SFile> fileList = new ObservableCollection<SFile>();
        ObservableCollection<IRule> rulesList = new ObservableCollection<IRule>();
        RuleFactory factory = RuleFactory.Instance();

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            fileNameList.ItemsSource = fileList;
            ruleComboBox.ItemsSource = factory._prototypes;
        }

        private void FileExplerButton_Click(object sender, RoutedEventArgs e)
        {
            var chooseFileScreen = new CommonOpenFileDialog();
            chooseFileScreen.Multiselect = true;

            if (chooseFileScreen.ShowDialog() == CommonFileDialogResult.Ok)
            {
                foreach (var fileName in chooseFileScreen.FileNames)
                {
                    var fileExist = fileList.SingleOrDefault(
                        f => Path.GetFullPath(f.FullName) == Path.GetFullPath(fileName)
                        );
                    if (fileExist == null)
                    {
                        var newFile = new SFile()
                        {
                            Path = Path.GetDirectoryName(fileName),
                            Name = Path.GetFileName(fileName),
                            NameAfterChanged = Path.GetFileName(fileName),
                            Type = "File",
                            IsChecked = true
                        };
                        fileList.Add(newFile);
                    }
                }
            }
        }

        private void FolderExplerButton_Click(object sender, RoutedEventArgs e)
        {
            var chooseFolderScreen = new CommonOpenFileDialog();
            chooseFolderScreen.IsFolderPicker = true;
            chooseFolderScreen.Multiselect = true;

            if (chooseFolderScreen.ShowDialog() == CommonFileDialogResult.Ok)
            {
                foreach (var folderName in chooseFolderScreen.FileNames)
                {
                    var fileExist = fileList.SingleOrDefault(
                        f => Path.GetFullPath(f.FullName) == Path.GetFullPath(folderName)
                        );
                    if (fileExist == null)
                    {
                        var newFile = new SFile()
                        {
                            Path = Path.GetDirectoryName(folderName),
                            Name = Path.GetFileName(folderName),
                            NameAfterChanged = Path.GetFileName(folderName),
                            Type = "Folder",
                            IsChecked = true
                        };
                        fileList.Add(newFile);
                    }
                }
            }
        }

        private void deleteFile_Click(object sender, RoutedEventArgs e)
        {
            int index = fileNameList.SelectedIndex;
            fileList.RemoveAt(index);
        }

        private void applyNameRule_Click(object sender, RoutedEventArgs e)
        {
            var isSuccess = false;
            foreach (var file in fileList)
            {
                isSuccess = file.Rename(CopyToTextBlock.Text != "" ? CopyToTextBlock.Text : file.Path);
            }
            if (isSuccess)
            {
                MessageBox.Show("Rename successfully");
            }
        }

        private void browseButton_Click(object sender, RoutedEventArgs e)
        {
            var browseCopyPath = new CommonOpenFileDialog();
            browseCopyPath.IsFolderPicker = true;
            if (browseCopyPath.ShowDialog() == CommonFileDialogResult.Ok)
            {
                CopyToTextBlock.Text = browseCopyPath.FileName;
            }
        }

        private void clearAllButton_Click(object sender, RoutedEventArgs e)
        {
            fileList.Clear();
        }

        private void IsSelectedCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            //chưa hoàn thiện
            var index = fileNameList.SelectedIndex;
            fileList[index].IsChecked = true;
        }

        private void IsSelectedCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            //chưa hoàn thiện
            var index = fileNameList.SelectedIndex;
            fileList[index].IsChecked = false;
        }
    }
}
