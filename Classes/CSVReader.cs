using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tax_Returns_Assistant
{
    public class CSVReader
    {
        private string[][] FullSheet;
        protected List<FilterOpts> FilterOpts = new List<FilterOpts>();

        public CSVReader(string path)
        {
            string[] alllines = File.ReadAllLines(path);
            int length = alllines.Length;

            string[][] FileVals = new string[length][];

            for (int i = 0; i < length; i++)
                FileVals[i] = alllines[i].Split(',');

            if (Validate(FileVals))
                FullSheet = FileVals;
            else
                throw new ArgumentException();
        }

        public virtual bool Validate(string[][] vals)
        {
            return true;
        }

        public string[][] GetVals()
        {
            string[][] Values = new string[FullSheet.Length - 1][];
            Array.Copy(FullSheet, 1, Values, 0, Values.Length);

            foreach (FilterOpts f in FilterOpts)
                f.Filter(ref Values);

            return Values;
        }

        public string[] GetTitles()
        {
            return FullSheet[0];
        }

        public void ClearFilters()
        {
            FilterOpts = new List<FilterOpts>();
        }

        protected string[] GetFilteredList(string columnname)
        {
            List<string> TList = new List<string>();

            foreach (string s in GetColumn(columnname))
                if (!String.IsNullOrEmpty(s) & !TList.Contains(s)) TList.Add(s);

            return TList.ToArray();
        }

        protected string TotalColumn(string name)
        {
            decimal retval = 0;

            foreach (string s in GetColumn(name))
            {
                if (!String.IsNullOrEmpty(s))
                    retval += Convert.ToDecimal(s);
            }

            return Convert.ToString(retval);
        }

        protected string[] CreateLine(string[] ColsToAdd, string[] colvals)
        {
            string[] retline = new string[GetTitles().Length];

            if (ColsToAdd.Length == colvals.Length)
            {
                for (int i = 0; i < ColsToAdd.Length; i++)
                    retline[GetColNum(ColsToAdd[i])] = colvals[i];

                return retline;
            }

            return null;
        }

        protected decimal GetMax(string colname)
        {
            string[] column = GetColumn(colname);
            decimal maxval = colname[0];

            foreach (string s in GetColumn(colname))
            {
                decimal curval = Convert.ToDecimal(s);
                if (curval > maxval) maxval = curval;
            }

            return maxval;
        }

        protected string[] GetMax(string colname,string[] retcolnames)
        {
            string[][] Vals = GetVals();
            string[] curline = Vals[0];

            int colnum = GetColNum(colname);

            foreach (string[] S in Vals)
            {
                decimal curval = Convert.ToDecimal(curline[colnum]);
                decimal newval = Convert.ToDecimal(S[colnum]);

                if (newval > curval) curline = S;
            }

            return GetValsInLine(curline, retcolnames);
        }

        protected string[] GetColumn(string title)
        {
            List<string> retlist = new List<string>();
            int ColNum = GetColNum(title);

            if (ColNum != -1)
            {
                foreach (string[] line in GetVals())
                    retlist.Add(line[ColNum]);

                return retlist.ToArray();
            }
            return null;
        }

        protected int GetColNum(string title)
        {
            string[] titlearr = GetTitles();

            for (int i = 0; i < titlearr.Length; i++)
            {
                if (titlearr[i].Trim() == title)
                    return i;
            }
            return -1;
        }

        private string[] GetValsInLine(string[] line, string[] colnames)
        {
            string[] colvals = new string[colnames.Length];

            for (int i = 0; i < colnames.Length; i++)
                colvals[i] = line[GetColNum(colnames[i])];

            return colvals;
        }
    }
}
