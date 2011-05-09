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
using System.Windows.Shapes;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class UserCredentials : Window
    {
        MainWindow parent;
        public UserCredentials(MainWindow parentWindow)
        {
            parent = parentWindow;
            InitializeComponent();
            Show();
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            parent.gmailUserName = txtUserName.Text;
            parent.gmailUserPassword = pbxPassword.Password;
            Close();
        }
    }
}
