using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agualytics.Manager.DataAccess
{
    /// <summary>
    /// A consumption object, that belongs to an specific land. It has the base data to forge an invoice.
    /// </summary>
    public class Consumption
    {
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public float ConsumedLiters { get; set; }
        public Invoice Invoice { get; set; }
    }
}
