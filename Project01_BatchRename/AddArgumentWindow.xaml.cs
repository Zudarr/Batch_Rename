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

namespace Project01_BatchRename
{
    /// <summary>
    /// Interaction logic for AddArgumentWindow.xaml
    /// </summary>
    public partial class AddArgumentWindow : Window
    {
        public string NewArgument { get; set; }
        public AddArgumentWindow()
        {
            InitializeComponent();
        }

        private void confirmButton_Click(object sender, RoutedEventArgs e)
        {
            NewArgument = argumentTextBox.Text;
            DialogResult = true;
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
