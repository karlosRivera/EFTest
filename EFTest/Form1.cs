using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Entity;
using System.Configuration;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure;
using System.Reflection;
using System.Data.EntityClient;
using System.Data.Objects;
using System.Globalization;
using System.Data.Common;
using System.Data.SqlClient;

namespace EFTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            //AppDomain.CurrentDomain.SetData("DataDirectory", @"c:\users\tridip.bbakolkata\documents\visual studio 2013\Projects\EFTest\EFTest");
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            /*
            using (var db = new TestDBContext())
            {
                var customer = new Customer
                {
                    FirstName = "Test Customer1",
                    LastName = "Test Customer1",
                    Addresses = new List<Addresses>
                    {
                        new Addresses
                        {
                            Address1 = "test add1",
                            Address2 = "test add2",
                            IsDefault=true,
                            Contacts =  new List<Contacts>
                            {
                               new Contacts {  Phone = "1111111", Fax = "1-1111111",IsDefault=true, SerialNo=1 },
                               new Contacts {  Phone = "2222222", Fax = "1-2222222",IsDefault=false, SerialNo=2  }
                            }
                        },
                        new Addresses
                        {
                            Address1 = "test add3",
                            Address2 = "test add3",
                            IsDefault=false,
                            Contacts =  new List<Contacts>
                            {
                               new Contacts {  Phone = "33333333", Fax = "1-33333333",IsDefault=false, SerialNo=1 },
                               new Contacts {  Phone = "33333333", Fax = "1-33333333",IsDefault=true, SerialNo=2  }
                            }
                        }

                    }
                };

                db.Customer.Add(customer);
                db.SaveChanges();

                int id = customer.CustomerID;
            }
             */

            using (var db = new TestDBContext())
            {
                //db.Customer.Where(e=> e.)
                var customer = new Customer
                {
                    FirstName = "Test Customer1",
                    LastName = "Test Customer1",

                };

                db.Customer.Add(customer);
                db.SaveChanges();
                int CustomerID = customer.CustomerID;

                Addresses oAdr = new Addresses();
                oAdr.Address1 = "test add1";
                oAdr.Address2 = "test add2";
                oAdr.IsDefault = true;
                oAdr.CustomerID = CustomerID;
                oAdr.SerialNo = 1;
                db.Addresses.Add(oAdr);
                db.SaveChanges();
                int AddressID = oAdr.AddressID;

                Contacts oContacts = new Contacts();
                oContacts.Phone = "1111111";
                oContacts.Fax = "1-1111111";
                oContacts.SerialNo = 1;
                oContacts.IsDefault = true;
                oContacts.AddressID = AddressID;
                db.Contacts.Add(oContacts);
                db.SaveChanges();

                oContacts = new Contacts();
                oContacts.Phone = "222222222";
                oContacts.Fax = "2-1111111";
                oContacts.SerialNo = 2;
                oContacts.IsDefault = false;
                oContacts.AddressID = AddressID;
                db.Contacts.Add(oContacts);
                db.SaveChanges();

                oAdr = new Addresses();
                oAdr.Address1 = "test add3";
                oAdr.Address2 = "test add3";
                oAdr.SerialNo = 2;
                oAdr.IsDefault = false;
                oAdr.CustomerID = CustomerID;
                db.Addresses.Add(oAdr);
                db.SaveChanges();
                AddressID = oAdr.AddressID;

                oContacts = new Contacts();
                oContacts.Phone = "33333333";
                oContacts.Fax = "3-1111111";
                oContacts.IsDefault = true;
                oContacts.SerialNo = 1;
                oContacts.AddressID = AddressID;
                db.Contacts.Add(oContacts);
                db.SaveChanges();

                oContacts = new Contacts();
                oContacts.Phone = "444444444";
                oContacts.Fax = "4-1111111";
                oContacts.SerialNo = 2;
                oContacts.IsDefault = false;
                oContacts.AddressID = AddressID;
                db.Contacts.Add(oContacts);
                db.SaveChanges();

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // for update operation

            using (var db = new TestDBContext())
            {
                var existingCustomer = db.Customer
                .Include(a => a.Addresses.Select(x=> x.Contacts))
                .FirstOrDefault(p => p.CustomerID == 5);

                existingCustomer.FirstName = "Test Customer122";

                foreach (var Custaddress in existingCustomer.Addresses.Where(a => a.AddressID == 5))
                {
                    Custaddress.Address1 = "test add1-22";
                    foreach (var CustContacts in Custaddress.Contacts.Where(a => a.ContactID == 5))
                    {
                        CustContacts.Phone = "1111111-22";
                    }
                }

                db.SaveChanges();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //Addresses CurrentAddress = null;
            //Contacts CurrentContacts = null;

            using (var db = new TestDBContext())
            {
                //db.Database.Log = s => MyLogger.Log("EFApp", s);

                var existingCustomer = db.Customer
                .Include(a => a.Addresses.Select(x => x.Contacts))
                .FirstOrDefault(p => p.CustomerID == 5);

                existingCustomer.FirstName = "Test Customer123";

                existingCustomer.Addresses.Where(a => a.AddressID == 5).ToList().ForEach(r => db.Addresses.Remove(r));
                existingCustomer.Addresses.Where(a => a.AddressID == 5).SelectMany(ad => ad.Contacts).Where(c=> c.ContactID==5).ToList().ForEach(r => db.Contacts.Remove(r));

                Addresses oAdrModel = new Addresses();
                oAdrModel.Address1 = "test xxx";
                oAdrModel.Address2 = "test xxx";
                oAdrModel.SerialNo = 3;
                oAdrModel.IsDefault = true;
                oAdrModel.CustomerID = 5;
                db.Addresses.Add(oAdrModel);
                db.SaveChanges();
                int CurAddressID = oAdrModel.AddressID;

                Contacts ContactModel = new Contacts();
                ContactModel.Phone = "XX-1111111-33";
                ContactModel.Fax = "XX-1-1111111";
                ContactModel.SerialNo = 4;
                ContactModel.IsDefault = true;
                ContactModel.AddressID = CurAddressID;
                db.Contacts.Add(ContactModel);

                db.SaveChanges();
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            var db = new TestDBContext();
            //var bsCustomer = db.Customer.Where(e => e.CustomerID == 2).Select(p => new { p.Blog.Name, p.Title }).ToList();
            //var bsCustomer = db.Customer.Where(c => c.CustomerID == 2).Include(a => a.Addresses.Where(b => b.IsDefault == true)).Include(c=>c.Addresses.)

            var bsCustomer = (from cu in db.Customer
                                          where (cu.CustomerID == 2)
                                          select new
                                          {
                                              cu,
                                              Addresses = from ad in cu.Addresses
                                                          where (ad.IsDefault == true)
                                                          from ct in ad.Contacts
                                                          select ad,

                                          }).ToList();
#region
            //var bsCustomer1 = (from c in db.Customer
            //                             where (c.CustomerID == 2)
            //                             select new 
            //                             {
            //                                 CustomerID = c.CustomerID,
            //                                 FirstName = c.FirstName,
            //                                 LastName = c.LastName,
            //                                 Addresses = (from ad in c.Addresses
            //                                              where (ad.IsDefault == true)
            //                                              from cts in ad.Contacts
            //                                              where (cts != null)
            //                                              select ad).ToList(),
            //                             }).ToList();

           // var bsCustomer1 = (from c in db.Customer
           //                    where (c.CustomerID == 2)
           //                    select new
           //                    {
           //                        CustomerID = c.CustomerID,
           //                        FirstName = c.FirstName,
           //                        LastName = c.LastName,
           //                        Addresses = (from ad in c.Addresses
           //                                     where (ad.IsDefault == true)
           //                                     from cts in ad.Contacts
           //                                     where (cts != null)
           //                                     select ad).ToList(),
           //                    }).ToList()
           //                     .Select(x => new Customer
           //                     {
           //                         CustomerID = x.CustomerID,
           //                         FirstName = x.FirstName,
           //                         LastName = x.LastName,
           //                         Addresses=x.Addresses,
           //                     }).ToList();

           //var cc= bsCustomer1[0].FirstName;
           //var cc1 = bsCustomer1[0].Addresses.Select(x => x.Address1).SingleOrDefault();

            //var bsCustomer1 = (from c in db.Customer
            //                   where (c.CustomerID == 2)
            //                   select new
            //                   {
            //                       CustomerID = c.CustomerID,
            //                       FirstName = c.FirstName,
            //                       LastName = c.LastName,
            //                       Addresses = (from ad in c.Addresses
            //                                    where (ad.IsDefault == true)
            //                                    from cts in ad.Contacts
            //                                    where (cts != null && cts.IsDefault == true)
            //                                    select ad).ToList(),
            //                   }).ToList()
            //        .Select(x => new CustomerBase
            //        {
            //            CustomerID = x.CustomerID,
            //            FirstName = x.FirstName,
            //            LastName = x.LastName,
            //            Address1 = x.Addresses.Select(a => a.Address1).SingleOrDefault(),
            //            Address2 = x.Addresses.Select(a => a.Address2).SingleOrDefault(),
            //            Phone = x.Addresses.Select(c => c.Contacts.Select(cd => cd.Phone).SingleOrDefault()),
            //            Fax = x.Addresses.Select(c => c.Contacts.Select(cd => cd.Fax).SingleOrDefault())
            //        }).ToList();

            //var cc = bsCustomer1[0].FirstName;
            //var cc1 = bsCustomer1[0].Addresses.Select(x => x.Address1).SingleOrDefault();
#endregion

        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (var db = new TestDBContext())
            {
                var listMyViews = db.vwCustomers.ToList();
                var listMyViews1 = db.vwMyCustomers.ToList();
            }
        }



 
    }

    public class CustomerBase
    {
        public int CustomerID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [NotMapped]
        public string Address1 { get; set; }

        [NotMapped]
        public string Address2 { get; set; }

        [NotMapped]
        public string Phone { get; set; }

        [NotMapped]
        public string Fax { get; set; }

    }

    public class Customer : CustomerBase
    {
        public virtual List<Addresses> Addresses { get; set; }
    }

    public class Addresses
    {
        [Key]
        public int AddressID { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public bool IsDefault { get; set; }
        public int SerialNo { get; set; }
        public virtual List<Contacts> Contacts { get; set; }

        public int CustomerID { get; set; }
        //[ForeignKey("CustomerID")]
        public virtual Customer Customer { get; set; }
    }

    public class Contacts
    {
        [Key]
        public int ContactID { get; set; }

        public string Phone { get; set; }
        public string Fax { get; set; }
        public bool IsDefault { get; set; }
        public int SerialNo { get; set; }
        public int AddressID { get; set; }

        //[ForeignKey("AddressID")]
        public virtual Addresses Customer { get; set; } 

    }

    public partial class vwCustomer
    {
        [Key]
        public int CustomerID { get; set; }

        public string FirstName { get; set; }
    }

    public partial class vwMyCustomers
    {
        [Key]
        public int CustomerID { get; set; }

        public string FirstName { get; set; }
    }

    //public class vwCustomerConfiguration : EntityTypeConfiguration<vwCustomer>
    //{
    //    public vwCustomerConfiguration()
    //    {
    //        this.HasKey(t => t.CustomerID);
    //        this.ToTable("vwCustomers");
    //    }
    //}

    public class TestDBContext : DbContext
    {
        public TestDBContext()
            : base("name=TestDBContext")
        {
            //((IObjectContextAdapter)this).ObjectContext.SavingChanges += new EventHandler(objContext_SavingChanges);
        }

        //public void objContext_SavingChanges(object sender, EventArgs e)
        //{
        //    var commandText = new StringBuilder();

        //    var conn = sender.GetType()
        //         .GetProperties(BindingFlags.Public | BindingFlags.Instance)
        //         .Where(p => p.Name == "Connection")
        //         .Select(p => p.GetValue(sender, null))
        //         .SingleOrDefault();
        //    var entityConn = (EntityConnection)conn;

        //    var objStateManager = (ObjectStateManager)sender.GetType()
        //          .GetProperty("ObjectStateManager", BindingFlags.Instance | BindingFlags.Public)
        //          .GetValue(sender, null);

        //    var workspace = entityConn.GetMetadataWorkspace();

        //    var translatorT =
        //        sender.GetType().Assembly.GetType("System.Data.Mapping.Update.Internal.UpdateTranslator");

        //    var translator = Activator.CreateInstance(translatorT, BindingFlags.Instance |
        //        BindingFlags.NonPublic, null, new object[] {objStateManager,workspace,
        //    entityConn,entityConn.ConnectionTimeout }, CultureInfo.InvariantCulture);

        //    var produceCommands = translator.GetType().GetMethod(
        //        "ProduceCommands", BindingFlags.NonPublic | BindingFlags.Instance);

        //    var commands = (IEnumerable<object>)produceCommands.Invoke(translator, null);

        //    foreach (var cmd in commands)
        //    {
        //        var identifierValues = new Dictionary<int, object>();
        //        var dcmd =
        //            (DbCommand)cmd.GetType()
        //               .GetMethod("CreateCommand", BindingFlags.Instance | BindingFlags.NonPublic)
        //               .Invoke(cmd, new[] { translator, identifierValues });

        //        foreach (DbParameter param in dcmd.Parameters)
        //        {
        //            var sqlParam = (SqlParameter)param;

        //            commandText.AppendLine(String.Format("declare {0} {1} {2}",
        //                                                    sqlParam.ParameterName,
        //                                                    sqlParam.SqlDbType.ToString().ToLower(),
        //                                                    sqlParam.Size > 0 ? "(" + sqlParam.Size + ")" : ""));

        //            commandText.AppendLine(String.Format("set {0} = '{1}'", sqlParam.ParameterName, sqlParam.SqlValue));
        //        }

        //        commandText.AppendLine();
        //        commandText.AppendLine(dcmd.CommandText);
        //        commandText.AppendLine("go");
        //        commandText.AppendLine();
        //    }

        //    System.Diagnostics.Debug.Write(commandText.ToString());
        //}

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Configurations.Add(new vwCustomerConfiguration());
            Database.SetInitializer<TestDBContext>(null);
        }

        public DbSet<Customer> Customer { get; set; }
        public DbSet<Addresses> Addresses { get; set; }
        public DbSet<Contacts> Contacts { get; set; }
        public virtual DbSet<vwCustomer> vwCustomers { get; set; }
        public DbSet<vwMyCustomers> vwMyCustomers { get; set; }
    }

    public class MyLogger
    {
        public static void Log(string component, string message)
        {
            Console.WriteLine("Component: {0} Message: {1} ", component, message);
        }
    }
}
