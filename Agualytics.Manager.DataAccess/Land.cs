using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Agualytics.Manager.DataAccess
{
    public class Land
    {
        public string RefCat { get; set; }
        public string OwnerName { get; set; }
        public string OwnerDNI { get; set; }
        public string OwnerAddress { get; set; }
        public string Address { get; set; }
        public int Endowment { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public List<Consumption> Consumptions { get; set; }
    }
}
