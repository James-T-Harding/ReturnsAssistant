using System;
using System.IO;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Tax_Returns_Assistant
{
    public partial class Statement_Displayer : Form
    {
        public Statement_Displayer(BankReader Reader)
        {
            InitializeComponent();
            ListBoxShow(Reader);
        }

        public void ListBoxShow(BankReader Reader)
        {
            Text = String.Format("Statement for Account {0:G}.", Reader.GetAccNumber());
            listView1.View = View.Details;

            foreach (string s in Reader.GetTitles())
                listView1.Columns.Add(s, -2, HorizontalAlignment.Left);

            string[][] AllLines = Reader.GetVals();
            Array.Reverse(AllLines);

            foreach (string[] CLine in AllLines)
                listView1.Items.Add(Arr2Item(CLine));

            listView1.Items.Add(string.Empty);
            listView1.Items.Add(Arr2Item(Reader.TotalLine()));
        }

        ListViewItem Arr2Item(string[] inp)
        {
            ListViewItem listitem = new ListViewItem(inp[0]);

            for (int j = 1; j < inp.Length; j++)
                listitem.SubItems.Add(inp[j]);

            return listitem;
        }      
    }
}
