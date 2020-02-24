using System;
using System.IO;
using System.Globalization;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Defunct
{
    public class BankReader
    {
        string[][] Allvals;

        public void SetVals(string path)
        {
            string[] alllines;

            alllines = File.ReadAllLines(path);

            int length = alllines.Length;
            Allvals = new string[length][];

            for (int i = 0; i < length; i++)
                Allvals[i] = alllines[i].Split(',');
        }

        private double GetMaxVal(out string maxdate)
        {
            string[] numsarr = FindValsOf("Balance");
            string[] datearr = FindValsOf("Transaction Date");
            double[] numarr = new double[numsarr.Length];
            
            for (int i = 0; i < numsarr.Length; i++)
                numarr[i] = Convert.ToDouble(numsarr[i]);

            int valpos=0;

            for (int i =1;i<numarr.Length;i++)
                if (numarr[i] > numarr[valpos]) valpos = i;

            maxdate = datearr[valpos];

            return numarr[valpos];         
        }

        public string[] ListMaxBalance()
        {
            double maxval = GetMaxVal(out string date);

            string val = maxval.ToString("c");
            string accnum = GetAccNumber();

            return new string[] { accnum, date, val };
        }

        public string[] ListMaxBalance(double convrate)
        {
            double maxval = GetMaxVal(out string date);

            string val = maxval.ToString("c");
            string usval = (convrate * maxval).ToString("c", CultureInfo.CreateSpecificCulture("en-US"));
            string accnum = GetAccNumber();

            return new string[] { accnum, date, val, usval };

            
        }

        public string[][] GetVals()
        {
            return Allvals;
        }

        public string GetAccNumber()
        {
            return Allvals[1][FindColumnOf("Account Number")];
        }

        private int FindColumnOf(string title)
        {
            for (int i =0;i<Allvals.Length;i++)
            {
                if (Allvals[0][i].Trim() == title)
                    return i;
            }
            return -1;
        }

        private string[] FindValsOf(string title)
        {
            int OutArrLength = Allvals.Length-1;
            string[] retarr = new string[OutArrLength];

            int column = FindColumnOf(title);

            if (column !=-1)
            {
                for (int j = 0; j < OutArrLength; j++)
                    retarr[j] = Allvals[j + 1][column];

                return retarr;
            }

            return null;
        }
    }
}
