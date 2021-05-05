using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//[STEP 1] Include namespaces for the data provider.
using Npgsql;
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
/// 1. strSQL = "Select * From public_bicycle";
///    NpgsqlCommand cmd = new NpgsqlCommand(strSQL, conn);
///    
/// 2. strSQL = "Select * From public_bicycle";
///    NpgsqlCommand cmd = conn.CreateCommand();
///    cmd.CommandText = strSQL;
///
/// 3. NpgsqlCommand cmd = new NpgsqlCommand();
///    cmd.CommandText = strSQL;
///    cmd.Connection = conn;
namespace _1_SimpleConnected2
{
    class Program
    {
        static void Main(string[] args)
        {
            // [STEP 2] Create a connection            
            string strConn = "host=localhost; port=5433; username=postgres; password=8270; database=gis";
            NpgsqlConnection conn = new NpgsqlConnection(strConn);

            // [STEP 3] open the connection            
            conn.Open();

            // [STEP 4] Create a command object.
            // specify the query string
            string strSQL = "Select * From public_bicycle ";           
            NpgsqlCommand cmd = new NpgsqlCommand(strSQL, conn);

            // [STEP 5] Obtain a data reader via ExecuteReader().            
            NpgsqlDataReader myDataReader = cmd.ExecuteReader();

            // [STEP 6] Process the data.            
            while (myDataReader.Read())
            {
                Console.WriteLine("{0}\t{1}\t{2}",
                myDataReader["name"].ToString().Trim(),
                myDataReader["address"].ToString().Trim(),
                myDataReader["x"].ToString().Trim() + "," + myDataReader["y"].ToString().Trim());
            }        
            // [STEP 7] Close the reader first and then the connection
            myDataReader.Close();
            conn.Close();
            Console.Write("Hit return... ");
            Console.ReadLine();
        }
    }
}
   