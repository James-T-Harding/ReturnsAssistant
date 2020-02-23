using System;
using System.IO;
using System.Collections.Generic;

namespace Tax_Returns_Assistant
{
    public class BankReader : CSVReader
    {
        public BankReader(string path) : base(path) { }

        private readonly string 
            Balance = "Balance",
            AccNum = "Account Number",
            Date = "Transaction Date",
            TD = "Transaction Description",
            Dbt = "Debit Amount",
            Crd = "Credit Amount",
            TT = "Transaction Type";

        public override bool Validate(string[][] inp)
        {
            if (inp.Length < 2) return false;

            string[] Allnames = new string[] { Balance, AccNum, Date, TD, Dbt, Crd, TT };

            foreach (string s in Allnames)
                if (!Array.Exists(inp[0], x => x == s)) return false;

            return true;
        }

        public string GetAccNumber()
        {
            return GetColumn(AccNum)[0];
        }

        public string GetDebDifferernce()
        {
            string[] numsarr = GetColumn(Balance);

            decimal cval = Convert.ToDecimal(TotalColumn(Crd));
            decimal dval = Convert.ToDecimal(TotalColumn(Dbt));

            return Convert.ToString(cval-dval);
        }

        public decimal MaxBalance()
        {
            return GetMax(Balance);
        }

        public string[] MaxBalance(string[] titleheaders)
        {
            return GetMax(Balance,titleheaders);
        }

        public string[] TotalLine()
        {
            string[] DebStrings = new string[] { Date, Dbt, Crd, Balance };
            string[] Amounts = new string[] { "Tot", TotalColumn(Dbt), TotalColumn(Crd), GetDebDifferernce() };

            return CreateLine(DebStrings, Amounts);
        }

        public string[] GetTranTypes()
        {
            return GetFilteredList(TT);
        }

        public void AddDebFilter(string inp)
        {
            FilterOpts.Add(new StringFilter(GetColNum(TT),inp));
        }

        public void AddDateFilter(DateTime Sdate,DateTime Fdate)
        {
            FilterOpts.Add(new DateFilter(GetColNum(Date), Sdate, Fdate));
        }
    }
}
