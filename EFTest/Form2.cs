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

namespace EFTest.demo1
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            using (var db = new TestDBContext1())
            {
                var dbcust = db.Customer.ToList();
                var listMyViews = db.vwCustomers.ToList();
            }
        }
    }

    public class CustomerBase
    {
        public int CustomerID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Address1 { get; set; }
        public string Address2 { get; set; }

        public string Phone { get; set; }
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
        public virtual List<Contacts> Contacts { get; set; }

        public int CustomerID { get; set; }
        public virtual Customer Customer { get; set; }
    }

    public class Contacts
    {
        [Key]
        public int ContactID { get; set; }

        public string Phone { get; set; }
        public string Fax { get; set; }
        public bool IsDefault { get; set; }

        public int AddressID { get; set; }
        public virtual Addresses Customer { get; set; }

    }

    public partial class vwCustomer
    {
        [Key]
        public int CustomerID { get; set; }

        public string FirstName { get; set; }
    }

    public class vwCustomerConfiguration : EntityTypeConfiguration<vwCustomer>
    {
        public vwCustomerConfiguration()
        {
            this.HasKey(t => t.CustomerID);
            this.ToTable("vwCustomers");
        }
    }

    public class TestDBContext1 : DbContext
    {
        public TestDBContext1()
            : base("name=TestDBContext")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Configurations.Add(new vwCustomerConfiguration());
        }

        public DbSet<Customer> Customer { get; set; }
        public DbSet<Addresses> Addresses { get; set; }
        public DbSet<Contacts> Contacts { get; set; }
        public virtual DbSet<vwCustomer> vwCustomers { get; set; }
    }
}
