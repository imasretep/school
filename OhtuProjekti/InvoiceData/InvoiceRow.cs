using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vt_systems.InvoiceData
{
    public class InvoiceRow
    {
        public string? Name { get; set; }
        public int Qty { get; set; }
        public double Time { get; set; }
        public double UnitPrice { get; set; }
        public double WholePrice { get; set; }
    }
}
