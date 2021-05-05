using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
// 데이터베이스의 어떠한 테이블을 호출하고, 데이터를 갱신해보자. 
// 1. 데이터 프로바이더 추가 (특정 DBMS와의 연결을 지원하는) 
namespace SimpleConnected2
{
    class Program

    {

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

                "Password=postgres; " +

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
