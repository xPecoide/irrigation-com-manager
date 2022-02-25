using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiteDB;

namespace Agualytics.Manager.DataAccess
{
    /// <summary>
    /// Represents an invoice from a <code>Consumption</code> object. This class is used to hold required data to generate a PDF invoice.
    /// </summary>
    public class Invoice
    {
        [BsonId]
        public int Id { get; set; }
        public DateTime DateExp { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public float ConsumedLiters { get; set; }
        public float LiterPrice { get; set; }
        public int Tax { get; set; }
    }
}
