using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Threading;
using System.Threading;

namespace FolderSubscription
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<string> RecentEvents;

        public MainWindow()
        {
            RecentEvents = new ObservableCollection<string>();
            createWatcher(@"C:\AutoDropFolder\Manual", false); 
            InitializeComponent();
        }

        private void createWatcher(string path,bool includeSubDirectories, string filter="*")
        {
            FileSystemWatcher watcher = new FileSystemWatcher(path, filter);
            watcher.IncludeSubdirectories = includeSubDirectories;
            watcher.Created += new FileSystemEventHandler(watcher_Created);
            watcher.Deleted += new FileSystemEventHandler(watcher_Deleted);
            watcher.Renamed += new RenamedEventHandler(watcher_Renamed);
            watcher.Changed += new FileSystemEventHandler(watcher_Changed);
            watcher.EnableRaisingEvents = true;
        }

        private void addToRecentEvents(string message)
        {
            RecentEvents.Add(message);
        }

        void watcher_Changed(object sender, FileSystemEventArgs e)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action<string>(addToRecentEvents), e.Name + " Changed");
            
        }

        void watcher_Renamed(object sender, RenamedEventArgs e)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action<string>(addToRecentEvents), e.Name + " Renamed");
        }

        void watcher_Deleted(object sender, FileSystemEventArgs e)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action<string>(addToRecentEvents), e.Name + " Deleted");    
        }

        void watcher_Created(object sender, FileSystemEventArgs e)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action<string>(addToRecentEvents), e.Name + " Created");
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            listBox1.ItemsSource = RecentEvents;
        }
    }
}
