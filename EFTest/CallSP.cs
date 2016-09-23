using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
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
                SqlParameter param1 = new SqlParameter("@CustomerID", 3);
                var customers = await db.Database.SqlQuery<Customer>("AllCustomers @CustomerID",param1).ToListAsync();
            }
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            using (var db = new TestDBContext1())
            {
                SqlParameter param1 = new SqlParameter("@CustomerID", 0);

                var outParam = new SqlParameter();
                outParam.ParameterName = "Counter";
                outParam.SqlDbType = SqlDbType.Int;
                outParam.Direction = ParameterDirection.Output;

                var customers = await db.Database.SqlQuery<Customer>("DetailsANDCount @CustomerID, @Counter OUT", param1, outParam).ToListAsync();
            }
        }
    }
}
