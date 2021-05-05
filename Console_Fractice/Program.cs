using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using System.Data;
/// <summary>
/// Connection
/// 1. String strConn = "Host=localhost; port = 5433; Username=postgres; Password=8270; Database=gis";
///    NpgsqlConnection conn = new NpgsqlConnection(strConn);
///    
/// 2. NpgsqlConnection conn = new NpgsqlConnection();
///    String strConn = "Host=localhost; port = 5433; Username=postgres; Password=8270; Database=gis"; 
///    conn.ConnectionString = strConn
///    
/// Command
/// 1. String strSQL = "Select * From public_bicycle";
///    NpgsqlCommand cmd = new NpgsqlCommand(strSQL, conn);
///    
/// 2. String strSQL = "Select * From public_bicycle";
///    NpgsqlCommand cmd = conn.CreateCommand();
///    cmd.CommandText = strSQL;
///
/// 3. NpgsqlCommand cmd = new NpgsqlCommand();
///    cmd.CommandText = strSQL;
///    cmd.Connection = conn;
/// </summary>
namespace Console_Fractice
{
    class Program
    { 
        static void Main(string[] args)
        {
            String strConn = "host=localhost; port=5433; username=postgres; password=8270; database=gis";
            NpgsqlConnection conn = new NpgsqlConnection(strConn);

            String strSQL = "select name, address, x, y from public_bicycle";
            NpgsqlCommand cmd = new NpgsqlCommand(strSQL, conn);

            NpgsqlDataAdapter adapter = new NpgsqlDataAdapter();
            adapter.SelectCommand = cmd;

            DataSet ds = new DataSet();
            adapter.Fill(ds, "Bicycle");

            DataTable dataTable = ds.Tables["Bicycle"];

            foreach (DataRow row in dataTable.Rows)
            {
                Console.WriteLine("{0}\t{1}\t{2}\t{3}\t", row["name"], row["address"], row["X"], row["y"]);
            }
         
        }                   
    }
}
