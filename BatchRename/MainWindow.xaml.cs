using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.WindowsAPICodePack.Dialogs;
using Path = System.IO.Path;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Diagnostics;
using System.Reflection;
using Contract;

namespace BatchRename
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
        public class Object : INotifyPropertyChanged
        {
            public string Name { get; set; }
            public string Dir { get; set; }
            public string Extension { get; set; }
            public string NewName { get; set; }

            public event PropertyChangedEventHandler PropertyChanged;

        }

        ObservableCollection<Object> objects = new ObservableCollection<Object>();
        ObservableCollection<IRule> _selectedRules = new ObservableCollection<IRule>();
        List<IRule> _activeRules = new List<IRule>();
        List<IRule> _rules = new List<IRule>();
        string[] rulesData { get; set; }

        private void addFileButton_Click(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.Multiselect = true;
            dialog.IsFolderPicker = false;

            //if (objects.Count != 0 && objects[0].Extension == "")
            //    objects.Clear();
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                foreach (string sFileName in dialog.FileNames)
                {
                    objects.Add(new Object { Name = Path.GetFileName(sFileName), Dir = Path.GetDirectoryName(sFileName) + "\\", Extension = Path.GetExtension(sFileName) });
                    for (int i = 0; i < objects.Count; i++)
                    {
                        for (int j = i + 1; j < objects.Count; j++)
                        {
                            if (objects[i].Name == objects[j].Name)
                                objects.Remove(objects[j]);
                        }
                        _backup_name = objects[i].Name;
                        if (_activeRules.Count != 0)
                        {
                            foreach (var r in _activeRules)
                            {
                                objects[i].NewName = r.Rename(_backup_name);
                                _backup_name = objects[i].NewName;
                            }
                        }
                    }
                    _backup_name = "";

                }
            }
        }
        public string _backup_name { get; set; }
        private void addFolderButton_Click(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.Multiselect = true;
            dialog.IsFolderPicker = true;

            //if (objects.Count != 0 && objects[0].Extension != "")
            //    objects.Clear();
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                foreach (string sFileName in dialog.FileNames)
                {
                    objects.Add(new Object { Name = Path.GetFileName(sFileName), Dir = Path.GetDirectoryName(sFileName) + "\\", Extension = Path.GetExtension(sFileName) });
                    for (int i = 0; i < objects.Count; i++)
                    {
                        for (int j = i + 1; j < objects.Count; j++)
                        {
                            if (objects[i].Name == objects[j].Name)
                                objects.Remove(objects[j]);
                        }

                        _backup_name = objects[i].Name;
                        if (_activeRules.Count != 0)
                        {
                            foreach (var r in _activeRules)
                            {
                                objects[i].NewName = r.Rename(_backup_name);
                                _backup_name = objects[i].NewName;
                            }
                        }
                    }
                    _backup_name = "";
                }
            }
        }
        private void addAllFilesInFolder_Click(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.Multiselect = true;
            dialog.IsFolderPicker = true;

            //if (objects.Count != 0 && objects[0].Extension != "")
            //    objects.Clear();
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                foreach (string FileName in dialog.FileNames)
                {
                    string[] filePaths = Directory.GetFiles(FileName, "",
                                         SearchOption.AllDirectories);
                    foreach (string sFileName in filePaths)
                    {
                        objects.Add(new Object { Name = Path.GetFileName(sFileName), Dir = Path.GetDirectoryName(sFileName) + "\\", Extension = Path.GetExtension(sFileName) });
                        for (int i = 0; i < objects.Count; i++)
                        {
                            for (int j = i + 1; j < objects.Count; j++)
                            {
                                if (objects[i].Name == objects[j].Name)
                                    objects.Remove(objects[j]);
                            }

                            _backup_name = objects[i].Name;
                            if (_activeRules.Count != 0)
                            {
                                foreach (var r in _activeRules)
                                {
                                    objects[i].NewName = r.Rename(_backup_name);
                                    _backup_name = objects[i].NewName;
                                }
                            }
                        }
                        _backup_name = "";
                    }
                }
            }
        }

        private void playBatchButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = MessageBox.Show($"Are you sure to rename all files?", "Warning", MessageBoxButton.YesNo);
            if (dialog == MessageBoxResult.Yes)
            {
                for (int i = 0; i < objects.Count; i++)
                {
                    string old_name = objects[i].Name;
                    string new_name = objects[i].NewName;
                    string oldDirName = objects[i].Dir + old_name;
                    string newDirName = objects[i].Dir + new_name;
                    objects[i].Name = objects[i].NewName;
                    try
                    {
                        File.Move(oldDirName, newDirName);
                        MessageBox.Show("Successfully Renaming", "", MessageBoxButton.OK);
                    }
                    catch
                    {
                        MessageBox.Show("Unable to rename all files");
                    }
                }
            }
        }

        private void addMethod_Click(object sender, RoutedEventArgs e)
        {
            ContextMenu? cm = FindResource("renamingRulesContextMenu") as ContextMenu;
            cm!.PlacementTarget = sender as Button;
            cm.IsOpen = true;
        }


        private void menuItemRenamingRulesContextMenu_Click(object sender, RoutedEventArgs e)
        {
            var menuItem = sender as MenuItem;
            if (menuItem != null)
            {
                var seleted = menuItem.Header.ToString();
                IRule rule = _rules.Find(x => x.Label == seleted);
                if (!_selectedRules.Contains(rule))
                {
                    _activeRules.Add(rule);
                    _selectedRules.Add(rule);
                    selectedRules.ItemsSource = _selectedRules;
                }
            }
            for (int i = 0; i < objects.Count; i++)
            {
                _backup_name = objects[i].Name;
                if (_activeRules.Count != 0)
                {
                    foreach (var r in _activeRules)
                    {
                        objects[i].NewName = r.Rename(_backup_name);
                        _backup_name = objects[i].NewName;
                    }
                }
            }
            _backup_name = "";
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var exeFolder = AppDomain.CurrentDomain.BaseDirectory;
            var folderInfo = new DirectoryInfo(exeFolder);
            var dllFiles = folderInfo.GetFiles("*.dll");

            foreach (var file in dllFiles)
            {
                var assembly = Assembly.LoadFrom(file.FullName);
                var types = assembly.GetTypes();
                IRule? rule = null;
                foreach (var type in types)
                {
                    if (type.IsClass && typeof(IRule).IsAssignableFrom(type))
                    {
                        rule = (IRule)Activator.CreateInstance(type)!;
                        RuleFactory.Register(rule);
                    }
                }
            }
            string presetPath = "HrRules.txt";
            rulesData = File.ReadAllLines(presetPath);
            foreach (var line in rulesData)
            {
                var rule = RuleFactory.Instance().Parse(line);

                if (rule != null)
                {
                    _rules.Add(rule);
                }
            }
            foreach (var rule in _rules)
            {
                ContextMenu? contextMenu = FindResource("renamingRulesContextMenu") as ContextMenu;
                var menuItem = new MenuItem();
                menuItem.Header = rule?.Label!;
                menuItem.Click += menuItemRenamingRulesContextMenu_Click;
                contextMenu!.Items.Add(menuItem);
            }

            sourceListView.ItemsSource = objects;
        }
        private void removeRule_Click(object sender, RoutedEventArgs e)
        {
            int index = selectedRules.SelectedIndex;
            _selectedRules.RemoveAt(index);
            _activeRules.RemoveAt(index);
            for (int i = 0; i < objects.Count; i++)
            {
                _backup_name = objects[i].Name;
                if (_activeRules.Count != 0)
                {
                    foreach (var r in _activeRules)
                    {
                        objects[i].NewName = r.Rename(_backup_name);
                        _backup_name = objects[i].NewName;
                    }
                }
                else
                {
                    objects[i].NewName = "";
                }
            }
            _backup_name = "";
        }

        private void editRulesButton_Click(object sender, RoutedEventArgs e)
        {
            _rules.Clear();

            string presetPath = "HrRules.txt";

            var screen = new EditRulesWindow(presetPath);
            if (screen.ShowDialog() == true)
            {
                rulesData = screen.newRules;
            }
            foreach (var line in rulesData)
            {
                var rule = RuleFactory.Instance().Parse(line);
                if (rule != null)
                {
                    _rules.Add(rule);
                }
            }
            ContextMenu? contextMenu = FindResource("renamingRulesContextMenu") as ContextMenu;
            contextMenu?.Items.Clear();
            foreach (var rule in _rules)
            {
                var menuItem = new MenuItem();
                menuItem.Header = rule?.Label!;
                menuItem.Click += menuItemRenamingRulesContextMenu_Click;
                contextMenu!.Items.Add(menuItem);
            }

        }
        private void sourceListView_Drop(object sender, DragEventArgs e)
        {
            string[] files = e.Data.GetData(System.Windows.DataFormats.FileDrop, false) as string[];
            for (int i = 0; i < files.Count(); i++)
            {
                objects.Add(new Object
                {
                    Name = Path.GetFileName(files[i]),
                    Dir = Path.GetDirectoryName(files[i]) + "\\",
                    Extension = Path.GetExtension(files[i])
                });
            }

        }
        private void addPreset_Click(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.Multiselect = true;
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                foreach (string presetPath in dialog.FileNames)
                {
                    presetsDropDown.Items.Add(presetPath);
                    presetsDropDown.SelectedItem = presetPath;
                }
            }
        }

        private void savePresetButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "Text file(*.txt)|*.txt";
            string savePreset = "";
            for(int i=0;i < _activeRules.Count; i++)
            {
                savePreset += _activeRules[i].textPreset + '\n';
            }
            
            if(dlg.ShowDialog() == true)
            {
                File.WriteAllText(dlg.FileName, savePreset);
            }
        }

        private void presetsDropDown_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string presetPath = presetsDropDown.SelectedItem.ToString();
            var lines = File.ReadAllLines(presetPath);
            _selectedRules.Clear();
            _activeRules.Clear();
            foreach (var line in lines)
            {
                IRule rule = RuleFactory.Instance().Parse(line);
                _activeRules.Add(rule);
                _selectedRules.Add(rule);
                selectedRules.ItemsSource = _selectedRules;
            }

            for (int i = 0; i < objects.Count; i++)
            {
                _backup_name = objects[i].Name;
                if (_activeRules.Count != 0)
                {
                    foreach (var r in _activeRules)
                    {
                        objects[i].NewName = r.Rename(_backup_name);
                        _backup_name = objects[i].NewName;
                    }
                }
            }
            _backup_name = "";
        }
    
    }
}