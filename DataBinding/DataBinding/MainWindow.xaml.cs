using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace DataBinding
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        School school = new School();

        public MainWindow()
        {
            InitializeComponent();

            DataContext = school;
            school.SchoolId = 206;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            school.SchoolId = 500;
        }
    }

    public class School : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged members
        public event PropertyChangedEventHandler PropertyChanged;
        protected void Notify(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

        public string Priority;
        public string UgPopulation;
        public string ActiveUsers;
        public string State;
        public string Edu_Type;
        public string Edu_Time;
        public string Team
        {
            get { return _team; }
            set
            {
                if (value != _team)
                {
                    _team = value;
                    Notify("Team");
                }
            }
        }
        public int SchoolId
        {
            get { return _schoolId; }
            set
            {
                if (value != _schoolId)
                {
                    _schoolId = value;
                    Notify("SchoolId");
                }
            }
        }
        private string _team;
        private int _schoolId;

        public School() { }

        public School(int schoolId)
        {
            SchoolId = schoolId;
        }

        public School(int schoolId, string priority, string ugPopulation, string activeUsers, string state, string edu_type, string edu_time, string team)
        {
            SchoolId = schoolId;
            Priority = priority;
            UgPopulation = ugPopulation;
            ActiveUsers = activeUsers;
            State = state;
            Edu_Type = edu_type;
            Edu_Time = edu_time;
            Team = team;
        }
    }
}
