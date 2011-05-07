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
using System.Text.RegularExpressions;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int choiceIndex;
        List<string[]> totalVis = new List<string[]>();
        List<string[]> total = new List<string[]>();
        List<string> lstPoolOfChoices = new List<string>();

        public MainWindow()
        {
            InitializeComponent();
            buildRepoFolderList();
        }

        private void txtNames_TextChanged(object sender, TextChangedEventArgs e)
        {
            filterResults();
        }

        /// <summary>
        /// Creates list of folders in script repo
        /// </summary>
        /// <param name="Subfolder">Schedule or Booklist</param>
        /// <returns></returns>
        private List<string> buildRepoFolderList()
        {
            List<string> results = new List<string>();
            string[] statesFolders = Directory.GetDirectories(@"C:\Users\Brian\Documents\Current Projects\Schools");
            foreach (string state in statesFolders)
            {
                string[] schoolFolders = Directory.GetDirectories(state);
                foreach (string school in schoolFolders)
                {
                    results.Add(school);
                    DirectoryInfo schoolDir = new DirectoryInfo(school);
                    string schoolName = schoolDir.Name;
                    string[] outputArray = {school,schoolName};
                    total.Add(outputArray);
                    totalVis.Add(outputArray);
                }
            }
            return results;
        }

        /// <summary>
        /// Filters lstPoolOfChoices based on current entry. lstPoolOfChoices is iterated through when you scroll up/down.
        /// </summary>
        public void filterResults()
        {
            if (txtNames.Text.Length < 18)
            {
                choiceIndex = 0;
                List<string> results = new List<string>();
                IEnumerable<string> filteredResults;
                foreach (string[] arrayNick in totalVis)
                {
                    string nick = arrayNick[1];
                    results.Add(nick);
                }

                string txt = txtNames.Text;
                filteredResults = results.Where(x => x.ToUpperInvariant().Contains(txt.ToUpperInvariant()));
                lstPoolOfChoices.Clear();

                foreach (string result in filteredResults)
                {
                    lstPoolOfChoices.Add(result);
                }
                if (lstPoolOfChoices.Count == 1)
                {
                    txtNames.Text = lstPoolOfChoices.ElementAt(0);
                    txtNames.MaxLength = lstPoolOfChoices.ElementAt(0).ToString().Length;
                }
                if (lstPoolOfChoices.Count == 0)
                {
                    MessageBox.Show("No results found!");
                    txtNames.Text = "";
                }
            }
        }

        /// <summary>
        /// Handles cycling through all current matches to the input string
        /// </summary>
        /// <param name="direction">which direction to cycle through, up or down</param>
        private void cycleThroughFilter(string direction, ref TextBox txtNames)
        {
            if (lstPoolOfChoices.Count != 0)
            {
                if (direction == "up")
                {
                    if (choiceIndex >= (lstPoolOfChoices.Count() - 1))
                    {
                        txtNames.Text = lstPoolOfChoices.ElementAt(0);
                        txtNames.SelectAll();
                        choiceIndex = 0;
                    }
                    else
                    {
                        choiceIndex++;
                        txtNames.Text = lstPoolOfChoices.ElementAt(choiceIndex);
                        txtNames.SelectAll();
                    }
                }
                if (direction == "down")
                {
                    if (choiceIndex == 0)
                    {
                        txtNames.Text = lstPoolOfChoices.ElementAt(lstPoolOfChoices.Count() - 1);
                        txtNames.SelectAll();
                        choiceIndex = lstPoolOfChoices.Count() - 1;

                    }
                    else
                    {
                        choiceIndex--;
                        txtNames.Text = lstPoolOfChoices.ElementAt(choiceIndex);
                        txtNames.SelectAll();
                    }
                }
            }
        }

        private void txtNames_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            //Regular keydown events do not capture "up" and "down"
            if (e.Key == Key.Up || e.Key == Key.Down)
            {
                //Cycle matches
                if (e.Key == Key.Up)
                {
                    e.Handled = true;
                    cycleThroughFilter("up", ref txtNames);
                }
                //Cycle matches
                else if (e.Key == Key.Down)
                {
                    e.Handled = true;
                    cycleThroughFilter("down", ref txtNames);
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
