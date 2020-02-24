using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tax_Returns_Assistant
{
    public abstract class FilterOpts
    {
        public abstract void Filter(ref string[][] Vals);
    }
}
