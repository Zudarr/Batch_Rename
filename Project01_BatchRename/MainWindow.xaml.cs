using Contract;
using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Windows;
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

        //Dict dùng để ghi lại các luật đang hiện hành và tham số của nó (nếu có)
        //Cần cái này vì khi save preset ko thể truy cập được vào argument (nếu có) của luật
        Dictionary<string, string> dictOfNameAndArg = new Dictionary<string, string>();
        string currentPresetPath = "";
        ObservableCollection<SFile> fileList = new ObservableCollection<SFile>();
        ObservableCollection<IRule> rulesList = new ObservableCollection<IRule>();
        RuleFactory factory = RuleFactory.Instance();

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            fileNameList.ItemsSource = fileList;
            ruleComboBox.ItemsSource = factory._prototypes;
            ruleListView.ItemsSource = rulesList;
        }

        private void PreviewTrigger()
        {
            foreach(var file in fileList)
            {
                var previewName = file.Name.Trim();
                if (file.IsChecked)
                {
                    foreach(var rule in rulesList)
                    {
                        if (rule.IsChecked)
                        {
                            previewName = rule.Rename(previewName);
                        }
                    }

                    if (previewName.Length > 255)
                    {
                        MessageBox.Show($"ERROR: Length of {previewName} exceed 255 characters");
                        continue;
                    }

                    Regex pattern = new Regex(@"[<>:/\\?*|""]");
                    var match = pattern.Match(previewName);
                    if(match.Success)
                    {
                        MessageBox.Show($"ERROR: {previewName} contains {match} characters");
                        continue;
                    }
                }
                file.PreviewName = previewName;
            }
        }

        private List<string> DirectorySearch(string path)
        {
            List<string> files = new List<string>();
            try
            {
                foreach (string f in Directory.GetFiles(path))
                {
                    files.Add(f);
                }
                foreach (string d in Directory.GetDirectories(path))
                {
                    files.AddRange(DirectorySearch(d));
                }
            }
            catch (System.Exception excpt)
            {
                MessageBox.Show(excpt.Message);
            }

            return files;
        }

        private void FileExplorerButton_Click(object sender, RoutedEventArgs e)
        {
            var chooseFileScreen = new CommonOpenFileDialog();
            chooseFileScreen.Multiselect = true;

            if (chooseFileScreen.ShowDialog() == CommonFileDialogResult.Ok)
            {
                foreach (var fileName in chooseFileScreen.FileNames)
                {
                    var nameExist = fileList.SingleOrDefault(
                        f => Path.GetFullPath(f.FullName) == Path.GetFullPath(fileName)
                        );
                    if (nameExist == null)
                    {
                        var newFile = new SFile()
                        {
                            Path = Path.GetDirectoryName(fileName),
                            Name = Path.GetFileName(fileName),
                            PreviewName = Path.GetFileName(fileName),
                            Type = "File",
                            IsChecked = true
                        };
                        fileList.Add(newFile);
                    }
                }
            }
            PreviewTrigger();
        }

        private void FileFromFolderExplorerButton_Click(object sender, RoutedEventArgs e)
        {
            var chooseFileScreen = new CommonOpenFileDialog();
            chooseFileScreen.IsFolderPicker = true;
            chooseFileScreen.Multiselect = true;

            if (chooseFileScreen.ShowDialog() == CommonFileDialogResult.Ok)
            {
                foreach (var fileName in chooseFileScreen.FileNames)
                {
                    var fileInDir = DirectorySearch(fileName);
                    foreach (var name in fileInDir)
                    {
                        var nameExist = fileList.SingleOrDefault(
                            f => Path.GetFullPath(f.FullName) == Path.GetFullPath(name)
                        );

                        if (nameExist == null)
                        {
                            var newFile = new SFile()
                            {
                                Path = Path.GetDirectoryName(name),
                                Name = Path.GetFileName(name),
                                PreviewName = Path.GetFileName(name),
                                Type = "File",
                                IsChecked = true
                            };
                            fileList.Add(newFile);
                        }
                    }
                }
            }
            PreviewTrigger();
        }

        private void FolderExplorerButton_Click(object sender, RoutedEventArgs e)
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
                            PreviewName = Path.GetFileName(folderName),
                            Type = "Folder",
                            IsChecked = true
                        };
                        fileList.Add(newFile);
                    }
                }
            }
            PreviewTrigger();
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
                fileList.Clear();
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

        private void clearCopyPathButton_Click(object sender, RoutedEventArgs e)
        {
            CopyToTextBlock.Text = "";
        }

        private void IsSelectedCheckBox_CheckChanged(object sender, RoutedEventArgs e)
        {
            PreviewTrigger();
        }

        private void addRuleToList_Click(object sender, RoutedEventArgs e)
        {
            if (ruleComboBox.SelectedItem == null)
            {
                return;
            }

            var element = (KeyValuePair<string, IRule>)ruleComboBox.SelectedItem;
            var rule = element.Value;

            foreach(var temp in rulesList)
            {
                if (temp.Name == rule.Name)
                {
                    return;
                }
            }

            var argument = string.Empty;
            var ruleInfo = new Dictionary<string, string>()
            {
                {"Name",rule.Name },
                {"Argument",argument }
            };

            if (rule.IsRequireArgument)
            {
                var screen = new AddArgumentWindow();
                if (screen.ShowDialog() == true)
                {
                    argument = screen.NewArgument;
                    ruleInfo["Argument"] = argument;
                }
                else
                {
                    return;
                }
            }

            dictOfNameAndArg.Add(rule.Name, argument);

            rule = factory.Parse(ruleInfo);
            rulesList.Add(rule);

            PreviewTrigger();
        }

        private void clearRuleList_Click(object sender, RoutedEventArgs e)
        {
            rulesList.Clear();
            dictOfNameAndArg.Clear();
            PreviewTrigger();
        }

        private void moveRuleUp_Click(object sender, RoutedEventArgs e)
        {
            var index = ruleListView.SelectedIndex;
            if (index > 0)
            {
                var temp = rulesList[index];
                rulesList.RemoveAt(index);
                rulesList.Insert(index - 1, temp);
            }
            PreviewTrigger();
        }

        private void moveRuleDown_Click(object sender, RoutedEventArgs e)
        {
            var index = ruleListView.SelectedIndex;
            if (index < rulesList.Count - 1)
            {
                var temp = rulesList[index];
                rulesList.RemoveAt(index);
                rulesList.Insert(index + 1, temp);
            }
            PreviewTrigger();
        }

        private void deleteRule_Click(object sender, RoutedEventArgs e)
        {
            var index = ruleListView.SelectedIndex;
            dictOfNameAndArg.Remove(rulesList[index].Name);
            rulesList.RemoveAt(index);
            PreviewTrigger();
        }

        private void savePresetAs_Click(object sender, RoutedEventArgs e)
        {
            if (rulesList.Count == 0)
            {
                return;
            }

            var saveDialog = new SaveFileDialog();
            saveDialog.Filter = "JSON file|*.json";

            if (saveDialog.ShowDialog() == true)
            {
                string filePath = saveDialog.FileName;
                currentPresetPath = filePath;
                currentPresetLabel.Content = Path.GetFileName(filePath);

                var outputRuleList = new List<string>();
                foreach (IRule rule in rulesList)
                {
                    var temp = new Dictionary<string, string>
                    {
                        { "Name", rule.Name },
                        {"Argument", dictOfNameAndArg[rule.Name] }
                    };

                    var jsonString = JsonSerializer.Serialize(temp);
                    outputRuleList.Add(jsonString);
                }

                File.WriteAllLines(filePath, outputRuleList);
            }
        }

        private void savePreset_Click(object sender, RoutedEventArgs e)
        {
            if (rulesList.Count == 0)
            {
                return;
            }

            if (currentPresetPath == "")
            {
                savePresetAs_Click(sender, e);
                return;
            }

            string filePath = currentPresetPath;
            var outputRuleList = new List<string>();
            foreach (IRule rule in rulesList)
            {
                var temp = new Dictionary<string, string>
                {
                    { "Name", rule.Name },
                    {"Argument", dictOfNameAndArg[rule.Name] }
                };

                var jsonString = JsonSerializer.Serialize(temp);
                outputRuleList.Add(jsonString);
            }
            File.WriteAllLines(filePath, outputRuleList);
        }

        private void ChoosePresetButton_Click(object sender, RoutedEventArgs e)
        {
            rulesList.Clear();
            dictOfNameAndArg.Clear();
            var openDialog = new OpenFileDialog();
            openDialog.Filter = "JSON file|*.json";
            openDialog.Multiselect = false;

            if (openDialog.ShowDialog() == true)
            {
                currentPresetPath = openDialog.FileName;
                currentPresetLabel.Content = Path.GetFileName(currentPresetPath);
                
                var ruleInpreset = File.ReadAllLines(currentPresetPath);
                foreach(var line in ruleInpreset)
                {
                    var ruleInfo = JsonSerializer.Deserialize<Dictionary<string, string>>(line);

                    dictOfNameAndArg.Add(ruleInfo["Name"], ruleInfo["Argument"]);

                    var rule = factory.Parse(ruleInfo);
                    rulesList.Add(rule);
                }
            }
            PreviewTrigger();
        }

        private void fileNameList_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                foreach (var fileName in files)
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
                            PreviewName = Path.GetFileName(fileName),
                            IsChecked = true
                        };

                        if(File.Exists(fileName))
                        {
                            newFile.Type = "File";
                        }
                        else
                        {
                            newFile.Type = "Folder";
                        }

                        fileList.Add(newFile);
                    }
                }
            }
            PreviewTrigger();
        }

        private void ruleListView_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                rulesList.Clear();
                dictOfNameAndArg.Clear();

                currentPresetPath = files[0];
                currentPresetLabel.Content = Path.GetFileName(currentPresetPath);

                var ruleInpreset = File.ReadAllLines(currentPresetPath);
                foreach (var line in ruleInpreset)
                {
                    var ruleInfo = JsonSerializer.Deserialize<Dictionary<string, string>>(line);

                    dictOfNameAndArg.Add(ruleInfo["Name"], ruleInfo["Argument"]);

                    var rule = factory.Parse(ruleInfo);
                    rulesList.Add(rule);
                }
            }
            PreviewTrigger();
        }
    }
}
