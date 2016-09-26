using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Core.Objects;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using EFTest.demo1;

namespace EFTest
{
    public partial class CallSP : Form
    {
        public CallSP()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            // this example showing how to handle when SP return multiple data
            using (var db = new TestDBContext1())
            {
                var customers = await db.Database.SqlQuery<Customer>("AllCustomers").ToListAsync();
                //var customers = await db.Customer.SqlQuery("AllCustomers").ToListAsync();
                //dataGridView1.DataSource = customers;

                // we can use also firstasync if we are need to read first data from collection.
                //var customers = await db.Database.SqlQuery<Customer>("AllCustomers").FirstAsync();

                // we can use singleasync if we are sure that one row will be return
                //var customers = await db.Database.SqlQuery<Customer>("AllCustomers").SingleAsync();
            }
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            // this example showing how to handle when SP return single data based on parameter

            using (var db = new TestDBContext1())
            {
                SqlParameter param1 = new SqlParameter("@CustomerID",  Convert.ToInt32(3));
                var customers = await db.Database.SqlQuery<Customer>("AllCustomers @CustomerID",param1).ToListAsync();
            }
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            using (var db = new TestDBContext1())
            {
                SqlParameter param1 = new SqlParameter("@CustomerID", Convert.ToInt32(0));

                var outParam = new SqlParameter();
                outParam.ParameterName = "@Counter";
                outParam.SqlDbType = SqlDbType.Int;
                outParam.Direction = ParameterDirection.Output;

                var customers = await db.Database.SqlQuery<Customer>("DetailsANDCount @CustomerID, @Counter OUT", param1, outParam).ToListAsync();
                dataGridView1.DataSource = customers;

                MessageBox.Show("Total customer found " + outParam.Value);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            using (var db = new TestDBContext1())
            {
                db.Database.Initialize(force: false);
                // Create a SQL command to execute the sproc 
                var cmd = db.Database.Connection.CreateCommand();
                cmd.CommandText = "[dbo].[MultiResultSet]";

                try
                {

                    db.Database.Connection.Open();
                    // Run the sproc  
                    var reader = cmd.ExecuteReader();

                    // Read Blogs from the first result set 
                    var customers = ((IObjectContextAdapter) db)
                        .ObjectContext
                       //.Translate<Customer>(reader, "Customers", System.Data.Entity.Core.Objects.MergeOption.AppendOnly);
                        .Translate<Customer>(reader);


                    foreach (var item in customers)
                    {
                        Console.WriteLine(item.FirstName);
                    }

                    // Move to second result set and read Posts 
                    reader.NextResult();
                    var Addresses = ((IObjectContextAdapter) db)
                        .ObjectContext
                        //.Translate<Addresses>(reader, "Addresses", System.Data.Entity.Core.Objects.MergeOption.AppendOnly);
                        .Translate<Addresses>(reader);


                    foreach (var item in Addresses)
                    {
                        Console.WriteLine(item.Address1);
                    }
                }
                finally
                {
                    db.Database.Connection.Close();
                }
            }

        }
    }
}
