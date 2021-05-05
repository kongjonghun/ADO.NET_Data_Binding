using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// 1. Data Provider 추가
using Npgsql;
// Data Set 활용 목적
using System.Data;

namespace SimpleDisconnected
{
    class Program
    {
        static void Main(string[] args)
        {
            // 2.DB 연결자
            NpgsqlConnection conn = new NpgsqlConnection();
            conn.ConnectionString ="Host=localhost; port=5433; Username=postgres; Password=8270; Database=gis";

            // 3. 쿼리 전달자
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.CommandText = "SELECT name, address, x, y FROM public_bicycle";
            cmd.Connection = conn;

            //4. Data Adapter 생성
            NpgsqlDataAdapter myAdapter = new NpgsqlDataAdapter();
            myAdapter.SelectCommand = cmd;

            //5. Data Set 생성
            DataSet ds = new DataSet();
            myAdapter.Fill(ds, "Bicycle");
            // Bicycle 테이블 생성하여 cmd 명령문 결과를 저장하라

            //6.
            DataTable bicycleTable = ds.Tables["Bicycle"];

            foreach(DataRow row in bicycleTable.Rows)
            {
                Console.WriteLine("{0}\t{1}\t{2}\t{3}", row["name"], row["address"], row["x"], row["y"]);
            }
        }
              
    }
}
