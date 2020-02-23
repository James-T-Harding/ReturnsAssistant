using System;
using System.Windows.Forms;

namespace Tax_Returns_Assistant
{
    public partial class FilterSettings : Form
    {
        public DateTime Sdate, Edate;
        public string DebString;

        public FilterSettings()
        {
            InitializeComponent();
            Clear();
        }

        private void monthCalendar1_DateChanged(object sender, DateRangeEventArgs e)
        {
            Sdate = monthCalendar1.SelectionStart;
        }

        private void monthCalendar2_DateChanged(object sender, DateRangeEventArgs e)
        {
            Edate = monthCalendar2.SelectionStart;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            DebString = comboBox1.Text;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Clear();
        }

        public void Clear()
        {
            SelectionRange selrange = new SelectionRange(DateTime.Today, DateTime.Today);
            monthCalendar1.SelectionRange = monthCalendar2.SelectionRange = selrange;

            Sdate = Edate = DateTime.Today;
            comboBox1.Text = DebString = string.Empty;
        }

        public void ShowDebTypes(string[] NameArr)
        {
            comboBox1.Items.Clear();

            foreach (string n in NameArr)
                comboBox1.Items.Add(n);
        }
    }
}
