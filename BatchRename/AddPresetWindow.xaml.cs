using System;
using System.Collections.Generic;
using System.IO;
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

namespace BatchRename
{
    /// <summary>
    /// Interaction logic for AddPresetWindow.xaml
    /// </summary>
    public partial class AddPresetWindow : Window
    {
        public string newPreset { get; set; }
        public string[] newRules { get; set; }
        public AddPresetWindow(string PresetPath)
        {
            InitializeComponent();
            newPreset = PresetPath;
            DataContext = newPreset;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            textbox.Text = File.ReadAllText(newPreset);
          
        }

        private void Okbutton_Click(object sender, RoutedEventArgs e)
        {
            newRules = textbox.Text.Split('\n');
            DialogResult = true;
        }
    }
}
