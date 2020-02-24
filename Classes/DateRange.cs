using System;

namespace Tax_Returns_Assistant
{
    public sealed class DateFilter : FilterOpts
    {
        public DateTime Sdate, Edate;
        public int colnum;

        public DateFilter(int colnum,DateTime Sdate,DateTime Edate)
        {
            this.colnum = colnum;

            this.Sdate = Sdate;
            this.Edate = Edate;
        }

        public override void Filter(ref string[][] vals)
        {
            vals = Array.FindAll(vals,
                x => Utility.ReadDate(x[colnum]) >= Sdate && Utility.ReadDate(x[colnum]) <= Edate);
        }

    }
}
