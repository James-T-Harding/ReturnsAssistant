using System;
using System.IO;
using System.Windows.Forms;
using System.Globalization;

namespace Tax_Returns_Assistant
{
    public partial class Main : Form
    {
        BankReader Reader;
        FilterSettings Settings = new FilterSettings();

        const string FileNotLoadedMessage = "File is not loaded. Banking values can be loaded using the 'Import Account' option.";
        const string BadFormatMessage = "File format is invalid. Input file must be a CSV aquired from an online bank with a compatible format.";
        const string NoValsFoundMessage = "No values found within specified range.";

        public Main()
        {
            InitializeComponent();

            listView1.View = View.Details;
            listView1.Items.Clear();
        }

        private void SaveVals()
        {
            saveFileDialog1.Filter = "Text Files | *.txt";

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    StreamWriter writer = new StreamWriter(saveFileDialog1.FileName);

                    using (writer)
                    {
                        foreach (string s in listView1.Items)
                            writer.WriteLine(s.TrimEnd());
                    }
                }
                catch (Exception e) { MessageBox.Show(e.Message); }
            }
        }

        private void importAccountToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Banking Spreadsheet | *.csv";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string filename = openFileDialog1.FileName;

                try
                {
                    Reader = new BankReader(filename);
                    toolStripStatusLabel1.Text = filename;

                    Settings.ShowDebTypes(Reader.GetTranTypes());
                    Settings.Clear();
                }
                catch (ArgumentException) { MessageBox.Show(BadFormatMessage); }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Reader != null)
            {
                string[] maxvals = Reader.MaxBalance(new string[] { "Transaction Date", "Balance" });

                if (maxvals != null)
                {
                    decimal maxval = Convert.ToDecimal(maxvals[1]);
                    string[] printvals = new string[] { maxvals[0], maxval.ToString("c") };

                    ListViewItem item = new ListViewItem(Reader.GetAccNumber());

                    foreach (string s in printvals)
                        item.SubItems.Add(s);

                    try
                    {
                        decimal convval = Convert.ToDecimal(textBox1.Text);
                        string usval = (convval * maxval).ToString("c", CultureInfo.CreateSpecificCulture("en-US"));

                        item.SubItems.Add(usval);
                    }
                    catch (FormatException) { };

                    listView1.Items.Add(item);
                }
                else
                    MessageBox.Show(NoValsFoundMessage);
            }
            else MessageBox.Show(FileNotLoadedMessage);
        }
    

        private void exportResultsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveVals();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }

        private void viewStatementToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            if (Reader != null)
            {
                Statement_Displayer Statement = new Statement_Displayer(Reader);
                Statement.ShowDialog();
            }
            else MessageBox.Show(FileNotLoadedMessage);
        }

        private void fIlterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Settings.ShowDialog();

            Reader.ClearFilters();

            if (Settings.Sdate != Settings.Edate)
                Reader.AddDateFilter(Settings.Sdate, Settings.Edate);

            if (!String.IsNullOrEmpty(Settings.DebString))
                Reader.AddDebFilter(Settings.DebString);
        }
    }
}
