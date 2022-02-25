using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiteDB;
using System.IO;
using Windows.Storage;

namespace Agualytics.Manager.DataAccess
{
    /// <summary>
    /// This static class contains data parsing and database operations features.
    /// </summary>
    public static class Parser
    {
        public static string DBPath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "database.db");

        /// <summary>
        /// Returns entire collection of a customer by ID card number.
        /// </summary>
        /// <param name="DNIParam">DNI</param>
        /// <returns><c>Client</c></returns>
        public static Client GetThings(string DNIParam)
        {
            using var db = new LiteDatabase(DBPath);
            var clients = db.GetCollection<Client>("clientes");
            var lobueno = clients.FindOne(Query.EQ("DNI", DNIParam));

            return lobueno;
        }

        /// <summary>
        /// Inserts a new <c>Client</c> object collection in database.
        /// </summary>
        /// <param name="client">Customer</param>
        public static void InsertThings(Client client)
        {
            using var db = new LiteDatabase(DBPath);
            //db.DropCollection("clientes");
            var col = db.GetCollection<Client>("clientes");
            col.Insert(client);
            col.EnsureIndex(x => x.DNI);
        }

        /// <summary>
        /// Updates a <c>Client</c> collection in database. All properties can be modified.
        /// </summary>
        /// <param name="clt"></param>
        public static void UpdateThings(Client clt)
        {
            using var db = new LiteDatabase(DBPath);
            var clients = db.GetCollection<Client>("clientes");
            var updt = clients.FindOne(Query.EQ("DNI", clt.DNI));

            updt.Name = clt.Name;
            updt.DNI = clt.DNI;
            updt.Address = clt.Address;
            updt.Tlf = clt.Tlf;
            updt.Email = clt.Email;
            updt.IBAN = clt.IBAN;

            for (int i = 0; i < clt.Lands.Count; i++)
            {
                updt.Lands[i].RefCat = clt.Lands[i].RefCat;
                updt.Lands[i].OwnerName = clt.Lands[i].OwnerName;
                updt.Lands[i].OwnerDNI = clt.Lands[i].OwnerDNI;
                updt.Lands[i].OwnerAddress = clt.Lands[i].OwnerAddress;
                updt.Lands[i].Address = clt.Lands[i].Address;
                updt.Lands[i].Endowment = clt.Lands[i].Endowment;
                updt.Lands[i].Latitude = clt.Lands[i].Latitude;
                updt.Lands[i].Longitude = clt.Lands[i].Longitude;
                updt.Lands[i].Consumptions = clt.Lands[i].Consumptions;
                for (int a = 0; a < clt.Lands[i].Consumptions.Count; a++)
                {
                    updt.Lands[i].Consumptions[a].DateFrom = clt.Lands[i].Consumptions[a].DateFrom;
                    updt.Lands[i].Consumptions[a].DateTo = clt.Lands[i].Consumptions[a].DateTo;
                    updt.Lands[i].Consumptions[a].ConsumedLiters = clt.Lands[i].Consumptions[a].ConsumedLiters;

                    updt.Lands[i].Consumptions[a].Invoice.DateExp = clt.Lands[i].Consumptions[a].Invoice.DateExp;
                    updt.Lands[i].Consumptions[a].Invoice.DateFrom = clt.Lands[i].Consumptions[a].Invoice.DateFrom;
                    updt.Lands[i].Consumptions[a].Invoice.DateTo = clt.Lands[i].Consumptions[a].Invoice.DateTo;
                    updt.Lands[i].Consumptions[a].Invoice.ConsumedLiters = clt.Lands[i].Consumptions[a].Invoice.ConsumedLiters;
                    updt.Lands[i].Consumptions[a].Invoice.LiterPrice = clt.Lands[i].Consumptions[a].Invoice.LiterPrice;
                    updt.Lands[i].Consumptions[a].Invoice.Tax = clt.Lands[i].Consumptions[a].Invoice.Tax;
                }
            }

            clients.Update(updt);
        }

        /// <summary>
        /// Static subclass focused on <c>Land</c> object inside <c>Client/c> object operations. Such as adding, removing or updating a Land.
        /// </summary>
        public static class Lands
        {
            /// <summary>
            /// Adds a new <c>Land</c> object to a <c>Client</c> object collection in database.
            /// </summary>
            /// <param name="clt">Customer</param>
            /// <param name="lnd">New land</param>
            public static void Add(Client clt, Land lnd)
            {
                using var db = new LiteDatabase(DBPath);
                var clients = db.GetCollection<Client>("clientes");
                var updt = clients.FindOne(Query.EQ("DNI", clt.DNI));

                try
                {
                    updt.Lands.Add(lnd);
                    clients.Update(updt);
                }
                catch
                {
                    throw;
                }
            }

            /// <summary>
            /// Removes a <c>Land</c> object inside a <c>Client</c> object collection in database.
            /// </summary>
            /// <param name="clt">Customer</param>
            /// <param name="refcat">Land cadastral reference number</param>
            public static void Remove(Client clt, string refcat)
            {
                using var db = new LiteDatabase(DBPath);
                var clients = db.GetCollection<Client>("clientes");
                var updt = clients.FindOne(Query.EQ("DNI", clt.DNI));

                try
                {
                    var landToRemove = updt.Lands.Where(x => x.RefCat == refcat).FirstOrDefault();
                    updt.Lands.Remove(landToRemove);
                    clients.Update(updt);
                }
                catch
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Static subclass focused on <c>Consumption</c> object inside <c>Land/c> object operations. Such as adding, removing or updating a Consumption interval.
        /// </summary>
        public static class Consumptions
        {
            /// <summary>
            /// Adds a new <c>Consumption</c> object to a <c>Land</c> object of a customer in database.
            /// </summary>
            /// <param name="dni">Customer ID card number</param>
            /// <param name="refcat">Land cadastral reference number</param>
            /// <param name="cmp">Consumption interval</param>
            public static void Add(string dni, string refcat, Consumption cmp)
            {
                using var db = new LiteDatabase(DBPath);
                var clients = db.GetCollection<Client>("clientes");
                var updt = clients.FindOne(Query.EQ("DNI", dni));

                try
                {
                    var consumList = updt.Lands.Where(x => x.RefCat == refcat).Select(x => x.Consumptions).FirstOrDefault();
                    var consumDuplicated = consumList.Where(x => x.DateFrom == cmp.DateFrom).FirstOrDefault();

                    if (consumDuplicated == null)
                    {
                        consumList.Add(cmp);
                    }
                    else
                    {
                        throw new Exception("Ya existe una tabla de consumo para esa fecha.");
                    }

                    clients.Update(updt);
                }
                catch
                {
                    throw;
                }
            }

            /// <summary>
            /// Removes a <c>Consumption</c> object from a <c>Land</c> object of a customer in database.
            /// </summary>
            /// <param name="dni">Customer ID card number</param>
            /// <param name="refcat">Land cadastral reference number</param>
            /// <param name="cmpDateFrom">Consumption's interval start date</param>
            public static void Remove(string dni, string refcat, DateTime cmpDateFrom)
            {
                using var db = new LiteDatabase(DBPath);
                var clients = db.GetCollection<Client>("clientes");
                var updt = clients.FindOne(Query.EQ("DNI", dni));

                try
                {
                    var consumList = updt.Lands.Where(x => x.RefCat == refcat).Select(x => x.Consumptions).FirstOrDefault();
                    var consumToRemove = consumList.Where(x => x.DateFrom == cmpDateFrom).FirstOrDefault();
                    consumList.Remove(consumToRemove);
                }
                catch
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Simple return to test fileIO methods.
        /// </summary>
        /// <param name="path">Path to file</param>
        public static void DummyReturn(string path)
        {
            FileInfo file;
            try
            {
                file = new FileInfo(path);
                Console.WriteLine(file.FullName);
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Static class to import data in plenty of formats.
        /// </summary>
        public static class Import
        {
            public static void CSV(string path)
            {
                throw new NotImplementedException();
            }

            public static void XLS(string path)
            {
                throw new NotImplementedException();
            }

            public static void XML(string path)
            {
                throw new NotImplementedException();
            }

            public static void JSON(string path)
            {
                throw new NotImplementedException();
            }

            public static void Manual()
            {
                List<string> values = new();
                string[] parameters = { "name", "DNI", "address", "phone number", "e-mail", "IBAN", "cadastre ref", "land address" };
                foreach (string param in parameters)
                {
                    Console.Write("Write the " + param + ": ");
                    string val = Console.ReadLine();
                    values.Add(val);
                }

                string[] vals = values.ToArray();

                Client client = new()
                {
                    Name = vals[5]
                };

                InsertThings(client);

            }
        }
        /// <summary>
        /// Static class to export data in plenty of formats.
        /// </summary>
        public static class Export
        {
            public static void CSV(string path)
            {
                throw new NotImplementedException();
            }

            public static void XLS(string path)
            {
                throw new NotImplementedException();
            }

            public static void XML(string path)
            {
                throw new NotImplementedException();
            }

            public static void JSON(string path)
            {
                throw new NotImplementedException();
            }
        }
    }
}
