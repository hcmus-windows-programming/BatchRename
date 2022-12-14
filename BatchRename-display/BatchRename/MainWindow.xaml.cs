//using Microsoft.Win32;
//using System;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Data;
//using System.Windows.Documents;
//using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Media.Imaging;
//using System.Windows.Navigation;
//using System.Windows.Shapes;
//using Microsoft.WindowsAPICodePack.Dialogs;
//using Path = System.IO.Path;
//using System.ComponentModel;
//using System.Runtime.CompilerServices;
//using System.Diagnostics;
//using System.Reflection;
//using Contract;

//namespace BatchRename
//{
//	/// <summary>
//	/// Interaction logic for MainWindow.xaml
//	/// </summary>
//	public partial class MainWindow : Window
//	{
//		public MainWindow()
//		{
//			InitializeComponent();
//		}
//		public class Object : INotifyPropertyChanged
//		{
//			public string name;
//			public string dir;
//			public string extension;
//			public string Name
//			{
//				get => name; set
//				{
//					name = value;
//					RaiseEvent();
//				}
//			}
//			public string Extension
//			{
//				get => extension; set
//				{
//					extension = value;
//					RaiseEvent();
//				}
//			}
//			public string Dir
//			{
//				get => dir; set
//				{
//					dir = value;
//					RaiseEvent();
//				}
//			}
//			public event PropertyChangedEventHandler PropertyChanged;

//			void RaiseEvent([CallerMemberName] string propertyName = "")
//			{
//				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
//			}
//		}

//		ObservableCollection<Object> Objects = new ObservableCollection<Object>();
//		ObservableCollection<IRule> _selectedRules = new ObservableCollection<IRule>();
//		List<IRule> _activeRules = new List<IRule>();

//		private void addFileButton_Click(object sender, RoutedEventArgs e)
//		{
//			CommonOpenFileDialog dialog = new CommonOpenFileDialog();
//			dialog.Multiselect = true;
//			dialog.IsFolderPicker = false;

//			if (Objects.Count != 0 && Objects[0].Extension == "")
//				Objects.Clear();
//			if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
//			{
//				foreach (string sFileName in dialog.FileNames)
//				{
//					Objects.Add(new Object { Name = Path.GetFileName(sFileName), Dir = Path.GetDirectoryName(sFileName) + "\\", Extension = Path.GetExtension(sFileName) });
//					for (int i = 0; i < Objects.Count; i++)
//					{
//						for (int j = i + 1; j < Objects.Count; j++)
//						{
//							if (Objects[i].Name == Objects[j].Name)
//								Objects.Remove(Objects[j]);
//						}
//					}

//					sourceListView.ItemsSource = Objects;
//				}
//			}
//		}
//		private void addFolderButton_Click(object sender, RoutedEventArgs e)
//		{
//			CommonOpenFileDialog dialog = new CommonOpenFileDialog();
//			dialog.Multiselect = true;
//			dialog.IsFolderPicker = true;

//			if (Objects.Count != 0 && Objects[0].Extension != "")
//				Objects.Clear();
//			if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
//			{
//				foreach (string sFileName in dialog.FileNames)
//				{
//					Objects.Add(new Object { Name = Path.GetFileName(sFileName), Dir = Path.GetDirectoryName(sFileName) + "\\", Extension = Path.GetExtension(sFileName) });
//					for (int i = 0; i < Objects.Count; i++)
//					{
//						for (int j = i + 1; j < Objects.Count; j++)
//						{
//							if (Objects[i].Name == Objects[j].Name)
//								Objects.Remove(Objects[j]);
//						}
//					}

//					sourceListView.Items.Clear();
//					foreach (Object obj in Objects)
//					{
//						sourceListView.Items.Add(obj);
//					}
//				}
//			}
//		}

//		private void playBatchButton_Click(object sender, RoutedEventArgs e)
//		{

//		}

//		private void addMethod_Click(object sender, RoutedEventArgs e)
//		{
//			ContextMenu? cm = FindResource("renamingRulesContextMenu") as ContextMenu;
//			cm!.PlacementTarget = sender as Button;
//			cm.IsOpen = true;
//		}
//        List<IRule> rules = new List<IRule>();

//        private void menuItemRenamingRulesContextMenu_Click(object sender, RoutedEventArgs e)
//		{
//			var menuItem = sender as MenuItem;

//			if (menuItem != null)
//			{
//				foreach(var rule in rules) {
//                    _activeRules.Add(rule);
//                    _selectedRules.Add(rule);
//                    selectedRules.ItemsSource = _selectedRules;

//                    var converter = (PreviewRenameConverter)FindResource("PreviewRenameConverter");
//                    converter.Rules = _activeRules;

//                    var temp = new ObservableCollection<Object>();

//                    foreach (var file in Objects)
//                    {
//                        temp.Add(file);
//                    }

//                    Objects = temp;
//                }

//			}
//		}

//		private void Window_Loaded(object sender, RoutedEventArgs e)
//		{
//            //var exeFolder = AppDomain.CurrentDomain.BaseDirectory;
//            //var folderInfo = new DirectoryInfo(exeFolder);
//            //var dllFiles = folderInfo.GetFiles("BatchRename*RuleLib.dll");

//            //foreach (var dll in dllFiles)
//            //{
//            //	Assembly assembly = Assembly.LoadFrom(dll.FullName);
//            //	Type[] types = assembly.GetTypes();

//            //	foreach (Type type in types)
//            //	{
//            //		if (type.IsClass)
//            //		{
//            //			if (typeof(IRule).IsAssignableFrom(type))
//            //			{
//            //				_rules.Add((Activator.CreateInstance(type) as IRule)!);
//            //			}
//            //		}
//            //	}
//            //}
//            var exeFolder = AppDomain.CurrentDomain.BaseDirectory;
//            var folderInfo = new DirectoryInfo(exeFolder);
//            var dllFiles = folderInfo.GetFiles("*.dll");

//            foreach (var file in dllFiles)
//            {
//                var assembly = Assembly.LoadFrom(file.FullName);
//                var types = assembly.GetTypes();
//                IRule? rule = null;
//                foreach (var type in types)
//                {
//                    if (type.IsClass && typeof(IRule).IsAssignableFrom(type))
//                    {
//                        rule = (IRule)Activator.CreateInstance(type)!;
//                        RuleFactory.Register(rule);
//                    }
//                }
//            }
//            string presetPath = "HrRules.txt";
//            var rulesData = File.ReadAllLines(presetPath);
//            foreach (var line in rulesData)
//            {
//                var rule = RuleFactory.Instance().Parse(line);

//                if (rule != null)
//                {
//                    rules.Add(rule);
//                }
//            }
//            foreach (var rule in rules)
//			{
//				ContextMenu? contextMenu = FindResource("renamingRulesContextMenu") as ContextMenu;
//				var menuItem = new MenuItem();
//				menuItem.Header = rule?.Label!;
//				menuItem.Click += menuItemRenamingRulesContextMenu_Click;
//				contextMenu!.Items.Add(menuItem);
//			}
//		}

//		private void removeRule_Click(object sender, RoutedEventArgs e)
//		{

//        }
//	}
//}
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
            //public string Name
            //{
            //    get => name; set
            //    {
            //        name = value;
            //        RaiseEvent();
            //    }
            //}
            //public string Extension
            //{
            //    get => extension; set
            //    {
            //        extension = value;
            //        RaiseEvent();
            //    }
            //}
            //public string Dir
            //{
            //    get => dir; set
            //    {
            //        dir = value;
            //        RaiseEvent();
            //    }
            //}
            public event PropertyChangedEventHandler PropertyChanged;

            //void RaiseEvent([CallerMemberName] string propertyName = "")
            //{
            //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            //}
        }

        ObservableCollection<Object> Objects = new ObservableCollection<Object>();
        ObservableCollection<IRule> _selectedRules = new ObservableCollection<IRule>();
        List<IRule> _activeRules = new List<IRule>();
        List<IRule> _rules = new List<IRule>();
        string[] rulesData { get; set; }

        private void addFileButton_Click(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.Multiselect = true;
            dialog.IsFolderPicker = false;

            if (Objects.Count != 0 && Objects[0].Extension == "")
                Objects.Clear();
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                foreach (string sFileName in dialog.FileNames)
                {
                    Objects.Add(new Object { Name = Path.GetFileName(sFileName), Dir = Path.GetDirectoryName(sFileName) + "\\", Extension = Path.GetExtension(sFileName) });
                    for (int i = 0; i < Objects.Count; i++)
                    {
                        for (int j = i + 1; j < Objects.Count; j++)
                        {
                            if (Objects[i].Name == Objects[j].Name)
                                Objects.Remove(Objects[j]);
                        }
                        _backup_name = Objects[i].Name;
                        if (_activeRules.Count != 0)
                        {
                            foreach (var r in _activeRules)
                            {
                                Objects[i].NewName = r.Rename(_backup_name);
                                _backup_name = Objects[i].NewName;
                            }
                        }
                    }
                    //sourceListView.ItemsSource = Objects;
                    //var converter = (PreviewRenameConverter)FindResource("PreviewRenameConverter");
                    //converter.Rules = _activeRules;
                    //var temp = new ObservableCollection<Object>();

                    //foreach (var file in Objects)
                    //{
                    //    temp.Add(file);
                    //}
                    //for (int i = 0; i < Objects.Count; i++)
                    //{
                    //    _backup_name = Objects[i].Name;
                    //    if (_activeRules.Count != 0)
                    //    {
                    //        foreach (var r in _activeRules)
                    //        {
                    //            Objects[i].NewName = r.Rename(_backup_name);
                    //            _backup_name = Objects[i].NewName;
                    //        }
                    //    }
                    //}
                    _backup_name = "";
                    //Objects = temp;                  

                }
            }
        }
        public string _backup_name { get; set; }
        private void addFolderButton_Click(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.Multiselect = true;
            dialog.IsFolderPicker = true;

            if (Objects.Count != 0 && Objects[0].Extension != "")
                Objects.Clear();
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                foreach (string sFileName in dialog.FileNames)
                {
                    Objects.Add(new Object { Name = Path.GetFileName(sFileName), Dir = Path.GetDirectoryName(sFileName) + "\\", Extension = Path.GetExtension(sFileName) });
                    for (int i = 0; i < Objects.Count; i++)
                    {
                        for (int j = i + 1; j < Objects.Count; j++)
                        {
                            if (Objects[i].Name == Objects[j].Name)
                                Objects.Remove(Objects[j]);
                        }

                    }

                    sourceListView.Items.Clear();
                    foreach (Object obj in Objects)
                    {
                        sourceListView.Items.Add(obj);
                    }
                }
            }
        }

        private void playBatchButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = MessageBox.Show($"Are you sure to rename all files?", "Warning", MessageBoxButton.YesNo);
            if (dialog == MessageBoxResult.Yes)
            {
                for (int i = 0; i < Objects.Count; i++)
                {
                    string old_name = Objects[i].Name;
                    string new_name = Objects[i].NewName;
                    string oldDirName = Objects[i].Dir + old_name;
                    string newDirName = Objects[i].Dir + new_name;
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

                    //var converter = (PreviewRenameConverter)FindResource("PreviewRenameConverter");
                    //converter.Rules = _activeRules;

                    //var temp = new ObservableCollection<Object>();

                    //foreach (var file in Objects)
                    //{
                    //    temp.Add(file);
                    //}

                    //Objects = temp;
                   
                    //sourceListView.ItemsSource = Objects;
                }
            }
            for (int i = 0; i < Objects.Count; i++)
            {
                _backup_name = Objects[i].Name;
                if (_activeRules.Count != 0)
                {
                    foreach (var r in _activeRules)
                    {
                        Objects[i].NewName = r.Rename(_backup_name);
                        _backup_name = Objects[i].NewName;
                    }
                }
            }
            _backup_name = "";
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //var exeFolder = AppDomain.CurrentDomain.BaseDirectory;
            //var folderInfo = new DirectoryInfo(exeFolder);
            //var dllFiles = folderInfo.GetFiles("BatchRename*RuleLib.dll");

            //foreach (var dll in dllFiles)
            //{
            //	Assembly assembly = Assembly.LoadFrom(dll.FullName);
            //	Type[] types = assembly.GetTypes();

            //	foreach (Type type in types)
            //	{
            //		if (type.IsClass)
            //		{
            //			if (typeof(IRule).IsAssignableFrom(type))
            //			{
            //				_rules.Add((Activator.CreateInstance(type) as IRule)!);
            //			}
            //		}
            //	}
            //}
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
           
            sourceListView.ItemsSource = Objects;
        }
        private void removeRule_Click(object sender, RoutedEventArgs e)
        {
            int index = selectedRules.SelectedIndex;
            _selectedRules.RemoveAt(index);
            _activeRules.RemoveAt(index);
            for (int i = 0; i < Objects.Count; i++)
            {
                _backup_name = Objects[i].Name;
                if (_activeRules.Count != 0)
                {
                    foreach (var r in _activeRules)
                    {
                        Objects[i].NewName = r.Rename(_backup_name);
                        _backup_name = Objects[i].NewName;
                    }
                }
                else
                {
                    Objects[i].NewName = "";
                }
            }
            _backup_name = "";
            //var converter = (PreviewRenameConverter)FindResource("PreviewRenameConverter");
            //converter.Rules = _activeRules;

            //var temp = new ObservableCollection<Object>();

            //foreach (var file in Objects)
            //{
            //    temp.Add(file);
            //}

            //Objects = temp;

        }

        private void addPresetButton_Click(object sender, RoutedEventArgs e)
        {

            _rules.Clear();
            string presetPath = "HrRules.txt";
           
            var screen = new AddPresetWindow(presetPath);
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
           
        }
    }
}