using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using RC.Gmail;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string gmailUserName;
        public string gmailUserPassword;

        int choiceIndex;
        KeyValuePair<string,string> selectedItem = new KeyValuePair<string,string>();
        Dictionary<string, string> totalFolderList = new Dictionary<string,string>();
        Dictionary<string, string> filteredList = new Dictionary<string, string>();

        public MainWindow()
        {
            InitializeComponent();
            txtNames.Focus();
            buildRepoFolderList();
        }

        /// <summary>
        /// Creates list of folders in script repo
        /// </summary>
        /// <param name="Subfolder">Schedule or Booklist</param>
        /// <returns></returns>
        private void buildRepoFolderList()
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
                    totalFolderList.Add(school, schoolDir.Name);
                }
            }
        }

        /// <summary>
        /// Filters lstPoolOfChoices based on current entry. lstPoolOfChoices is iterated through when you scroll up/down.
        /// </summary>
        public void filterResults()
        {
            if (txtNames.Text.Length < 18)
            {
                choiceIndex = 0;
                IEnumerable<KeyValuePair<string, string>> filteredResults;

                string txt = txtNames.Text;
                filteredResults = totalFolderList.Where(x => x.Key.ToUpperInvariant().Contains(txt.ToUpperInvariant()));
                filteredList.Clear();

                foreach (KeyValuePair<string, string> result in filteredResults)
                {
                    filteredList.Add(result.Key, result.Value);
                }
                if (filteredList.Count == 1)
                {
                    setSelectedItem();
                }
                if (filteredList.Count == 0)
                {
                    setSelectedItem();
                    MessageBox.Show("No results found!");
                }
            }
        }

        private void setSelectedItem()
        {
            if (filteredList.Count != 0)
                selectedItem = new KeyValuePair<string, string>(filteredList.Keys.ElementAt(choiceIndex), filteredList.Values.ElementAt(choiceIndex));
            else
                selectedItem = new KeyValuePair<string, string>("", "");

            txtNames.Text = selectedItem.Value;
            txtNames.MaxLength = selectedItem.Value.Length;
        }

        /// <summary>
        /// Handles cycling through all current matches to the input string
        /// </summary>
        /// <param name="direction">which direction to cycle through, up or down</param>
        private void cycleThroughFilter(string direction)
        {
            if (filteredList.Count != 0)
            {
                if (direction == "up")
                {
                    if (choiceIndex >= (filteredList.Count() - 1))
                    {
                        choiceIndex = 0;
                        setSelectedItem();
                        txtNames.SelectAll();
                    }
                    else
                    {
                        choiceIndex++;
                        setSelectedItem();
                        txtNames.SelectAll();
                    }
                }
                if (direction == "down")
                {
                    if (choiceIndex == 0)
                    {
                        choiceIndex = filteredList.Count() - 1;
                        setSelectedItem();
                        txtNames.SelectAll();
                    }
                    else
                    {
                        choiceIndex--;
                        setSelectedItem();
                        txtNames.SelectAll();
                    }
                }
            }
        }

        private void generateLetter()
        {
            DirectoryInfo di = new DirectoryInfo(selectedItem.Key);

            Dictionary<string, string> partsOfLetter = new Dictionary<string, string>(7) { 
                { "Signature", null }, 
                { "Attachment", null }, 
                { "Postscript", null }, 
                { "Close", null }, 
                { "Body", null }, 
                { "Greeting", null }, 
                { "Heading", null } 
            };

            //Use School overrides first
            partsOfLetter = findLetterComponents(partsOfLetter, di.FullName);

            //Use State overrides next
            partsOfLetter = findLetterComponents(partsOfLetter, di.Parent.FullName);

            //Use defaults otherwise
            partsOfLetter = findLetterComponents(partsOfLetter, di.Parent.Parent.FullName);

            txtGenerated.Text = (partsOfLetter["Signature"] + partsOfLetter["Attachment"] +
                partsOfLetter["Postscript"] + partsOfLetter["Close"] + partsOfLetter["Body"] +
                partsOfLetter["Greeting"] + partsOfLetter["Heading"]);
        }

        /// <summary>
        /// Searches a directory for a list of text files and returns their contents into a dictionary
        /// </summary>
        /// <param name="partsOfLetter">Dictionary of keys to search for</param>
        /// <param name="pathToSearch">Directory to search</param>
        /// <returns></returns>
        private static Dictionary<string, string> findLetterComponents(Dictionary<string, string> partsOfLetter, string pathToSearch)
        {
            Dictionary<string, string> partsToUpdate = new Dictionary<string, string>(partsOfLetter);
            foreach (KeyValuePair<string, string> part in partsOfLetter)
            {
                if (part.Value == null)
                {
                    if (Directory.EnumerateFiles(pathToSearch + "//", part.Key + "*").Count() != 0)
                    {
                        partsToUpdate[part.Key] = File.ReadAllText(pathToSearch + "//" + part.Key + ".txt") + "\n\n";
                    }
                }
            }
            return partsToUpdate;
        }



        #region Event Handlers
        private void btnGenerate_Click(object sender, RoutedEventArgs e)
        {
            generateLetter();
            txtNames.SelectAll();
        }

        private void txtNames_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            filterResults();
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
                    cycleThroughFilter("up");
                }
                //Cycle matches
                else if (e.Key == Key.Down)
                {
                    e.Handled = true;
                    cycleThroughFilter("down");
                }
            }
        }

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            if (gmailUserName == null || gmailUserPassword == null)
            {
                UserCredentials uc = new UserCredentials(this);
            }


        }
        #endregion
    }
}
