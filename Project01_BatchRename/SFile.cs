using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Project01_BatchRename
{
    public class SFile:INotifyPropertyChanged
    {
        public string Path { get; set; }
        public string Name { get; set; }
        public string FullName => $"{Path}/{Name}";
        public string NameAfterChanged { get; set; }
        public string Type { get; set; }
        public bool IsChecked { get; set; }

        public bool Rename(string newPath)
        {
            try
            {
                if (Type == "File")
                {
                    if(newPath == Path) //path mới giống path path cũ, nghĩa là ghi đè, path ko đổi
                    {
                        File.Move(FullName, $"{newPath}/{NameAfterChanged}");
                    }
                    else //copy sang một path mới
                    {
                        File.Copy(FullName, $"{newPath}/{NameAfterChanged}");
                    }
                }
                else
                {
                    if (newPath == Path)
                    {
                        Directory.Move(FullName, $"{newPath}/{NameAfterChanged}");
                    }
                    //copy folder sang path mới nghĩa là copy tất cả file và folder bên trong nó đi theo
                    else
                    {
                        Directory.CreateDirectory(newPath);
                        //Now Create all of the directories
                        foreach (string dirPath in Directory.GetDirectories(FullName, "*", SearchOption.AllDirectories))
                        {
                            Directory.CreateDirectory(dirPath.Replace(FullName, $"{newPath}/{NameAfterChanged}"));
                        }

                        //Copy all the files & Replaces any files with the same name
                        foreach (string newPathToCopy in Directory.GetFiles(FullName, "*.*", SearchOption.AllDirectories))
                        {
                            File.Copy(newPathToCopy, newPathToCopy.Replace(FullName, $"{newPath}/{NameAfterChanged}"), true);
                        }

                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
