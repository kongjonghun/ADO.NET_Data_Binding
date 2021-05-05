using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Npgsql;

namespace _3_SimpleDisconnected2
{
    class Program
    {
        static void Main(string[] args)
        {
            String strConn = "Host=localhost; port= 5433; Username=postgres; Password=8270; Database=gis";
            NpgsqlConnection conn = new NpgsqlConnection(strConn);

            String strSQL = "select name, address, x, y from public_bicycle";
            NpgsqlCommand cmd = new NpgsqlCommand(strSQL, conn);

            NpgsqlDataAdapter adapter = new NpgsqlDataAdapter();
            adapter.SelectCommand = cmd;

            DataSet dataset = new DataSet();

            //dataset에 Bicycle 테이블 생성하여 cmd 명령문 결과를 저장하라
            adapter.Fill(dataset, "Bicycle"); 

            //DataSet으로 부터 table을 가져와라
            DataTable bicycleTable = dataset.Tables["Bicycle"];

            foreach(DataRow row in bicycleTable.Rows)
            {
                Console.WriteLine("{0}\t{1}\t{2}\t{3}\t", row["name"], row["address"], row["x"], row["y"]);
            }
            Console.ReadLine();
        }
    }
}
