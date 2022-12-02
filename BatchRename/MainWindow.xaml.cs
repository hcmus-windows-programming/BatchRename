﻿using Microsoft.Win32;
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
            public string name;
            public string dir;
            public string extension;
            public string Name
            {
                get => name; set{
                    name = value;
                    RaiseEvent();
                }
            }
            public string Extension
            {
                get => extension; set{
                    extension = value;
                    RaiseEvent();
                }
            }
            public string Dir
            {
                get => dir; set{
                    dir = value;
                    RaiseEvent();
                }
            }
            public event PropertyChangedEventHandler PropertyChanged;

            void RaiseEvent([CallerMemberName] string propertyName = ""){
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        BindingList<Object> Objects = new BindingList<Object>();

        private void addFileButton_Click(object sender, RoutedEventArgs e)
        {
            CommonOpenFileDialog dialog = new CommonOpenFileDialog();
            dialog.Multiselect = true;
            dialog.IsFolderPicker = false;

            if (Objects.Count != 0 && Objects[0].Extension == "")
                Objects.Clear();
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok){
                foreach (string sFileName in dialog.FileNames){
                    Objects.Add(new Object { Name = Path.GetFileName(sFileName), Dir = Path.GetDirectoryName(sFileName) + "\\", Extension = Path.GetExtension(sFileName) });
                    for (int i = 0; i < Objects.Count; i++){
                        for (int j = i + 1; j < Objects.Count; j++){
                            if (Objects[i].Name == Objects[j].Name)
                                Objects.Remove(Objects[j]);
                        }
                    }

                    sourceListView.Items.Clear();
                    foreach (Object obj in Objects){
                        sourceListView.Items.Add(obj);
                    }
                }
            }
        }
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
                    for (int i = 0; i < Objects.Count; i++){
                        for (int j = i + 1; j < Objects.Count; j++){
                            if (Objects[i].Name == Objects[j].Name)
                                Objects.Remove(Objects[j]);
                        }
                    }

                    sourceListView.Items.Clear();
                    foreach (Object obj in Objects){
                        sourceListView.Items.Add(obj);
                    }
                }
            }
        }
    }
}