using System;

namespace Tax_Returns_Assistant
{
    public sealed class StringFilter : FilterOpts
    {
        private string fstring;
        private int colnum;

        public StringFilter(int colnum,string fstring)
        {
            this.colnum = colnum;
            this.fstring = fstring;
        }

        public override void Filter(ref string[][] vals)
        {
            if (fstring != null)
                vals = Array.FindAll(vals, x => x[colnum] == fstring);
        }
    }
}
