 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
namespace SimpleConnected
{    
    class Program
    {
        // Connection : 데이터 소스(DBMS)와의 연결
        // Command : 데이터 소스(DBMS)에 명령어 전달
        // Command의 ExecuteReader() : 명령어 수행
        // Read(): 수행된 결과 확인
        static NpgsqlConnection DB연결자 = new NpgsqlConnection();
        static NpgsqlCommand 쿼리전달자 = new NpgsqlCommand();
        static NpgsqlDataReader 쿼리결과;
        static int userCommand;

        static void Main(string[] args) // 코드의 진입점, Main 메소드 
        {
            // 2. 데이터베이스와의 연결 
            ConnectDatabase();

            // 3. 쿼리 전달 
            QueryToDatabase();

            // 4. 결과 보기 
            GetResultFromDatabase();

            DB연결자.Close();
            Console.ReadLine();
        }


        private static void ConnectDatabase()
        {

            DB연결자.ConnectionString =
                "Host=localhost; " +
                "port=5433; " +
                "Username=postgres;" +
                "Password=8270; " +
                "Database=gis"; // 디비 주소 할당 

            DB연결자.Open(); // 디비 연결 
        }

        private static void QueryToDatabase()
        {
            Console.WriteLine("조회(select): 1, 갱신(insert, update, delete): 2");
            Console.Write("커맨드를 입력하세요:   ");

            userCommand = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("쿼리를 입력하세요~");
            쿼리전달자.CommandText = Console.ReadLine();
            쿼리전달자.Connection = DB연결자;

            if (userCommand == 2)
                쿼리전달자.ExecuteNonQuery();
        }

        private static void GetResultFromDatabase()
        {
            if (userCommand == 2)
            {
                Console.WriteLine("갱신한 테이블에 대한 조회문을 입력하세요");
                쿼리전달자.CommandText = Console.ReadLine();
            }

            쿼리결과 = 쿼리전달자.ExecuteReader();

            while (쿼리결과.Read())
            {
                string item = "";
                for (int i = 0; i < 쿼리결과.FieldCount - 1; i++)
                {
                    item += 쿼리결과.GetValue(i).ToString() + "\t";
                }
                Console.WriteLine(item);
            }
            쿼리결과.Close();
        }

    }
}
