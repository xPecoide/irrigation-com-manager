using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiteDB;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace Agualytics.Manager.DataAccess
{
    public class Client
    {
        [BsonId]
        public int Id { get; set; }
        public string Name { get; set; }
        public string DNI { get; set; }
        public string Address { get; set; }
        public string Tlf { get; set; }
        public string Email { get; set; }
        public string IBAN { get; set; }
        public List<Land> Lands { get; set; }
    }
}
