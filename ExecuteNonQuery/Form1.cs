using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;
// Provider 추가

/// <summary>
/// C#  프로그램의 기본 구조
/// namespace
///     class
///         memebers(변수, 함수)
///         
///         variable(멤버변수, 필드)
/// 
///         Method
///             local variable(지역변수) : 메소드가 종료되면 사라짐
///             
/// 
///  따릉이 대여소 테이블을 호출하고, 데이터를 업데이트 해보자
/// </summary>
/// ExecuteReader : 데이터 읽기
/// ExecuteNonQuery : 데이터 변경
namespace ExecuteNonQuery
{
    public partial class MainForm : Form
    {
        NpgsqlConnection conn;
        NpgsqlCommand cmd;

        string strConn = "Host=localhost; port=5433; Username=postgres; Password=8270; Database=gis";
        string strSQL = " Select * From public_bicycle";   
        int numberOfRows;

        public MainForm()
        {
            InitializeComponent();
        }
        private void btnLoad_Click(object sender, EventArgs e)
        {
            //로드 버튼 클릭시, 따릉이 대여소 테이블 가져온다.
            conn = new NpgsqlConnection(strConn);
            
            cmd = conn.CreateCommand();
            cmd.CommandText = strSQL;
            conn.Open();

            DisplayData();
        }
        // 접근한정자 : Private : 해당 클래스내에서만 접근 가능하다
        private void DisplayData()
        {
            // 리스트 박스에 가져온 데이터를 추가하자
            // Clear the listbox
            lstData.Items.Clear();

            cmd.CommandText = strSQL;
            NpgsqlDataReader reader = cmd.ExecuteReader();
            string item = String.Empty; 

            while (reader.Read())
            {
                item = String.Empty; // String.Empty : 초기화
                // 한 행당 item이라는 문자열로 저장
                // 마지막 컬럼이 Geometry이기 때문에 제외
                for (int i = 0; i < reader.FieldCount - 1; i++)
                {
                    item += reader.GetValue(i).ToString().Trim() + "\t";
                    // item = "a b c d "
                }
                lstData.Items.Add(item);
            }
            reader.Close();
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            string str = "5000, '공간정보공학 따릉이', '동대문구', '서울시립대학교 21세기관 앞'";
            MessageBox.Show(str, "Adding data...");
            
            cmd.CommandText = "INSERT INTO public_bicycle ( id, name, sgg, address) VALUES (" + str + ")";
            try
            {       
                // ExecuteNonQuery : '영향받은 행의 수'를 리턴
                numberOfRows = cmd.ExecuteNonQuery();
            }
            catch { }
            lblRows.Text = "Number of rows added = " + numberOfRows;
            DisplayData();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (lstData.SelectedItems.Count == 0)
            {
                MessageBox.Show("Select an item with id = 5000");
                return; // 다음 명령어들이 스킵되고 메소드가 종료
            }
            string str = lstData.SelectedItem.ToString();
            int id = int.Parse(str.Substring(0, 4)); //Substring(0,4) 첫번째글자부터 4칸 가져와라 : id만 가져옴
            if (id == 5000)
            {
                MessageBox.Show("선택된 따릉이 대여소 삭제");
                cmd.CommandText = "DELETE FROM public_bicycle " + "WHERE id = " + id;
                try
                {
                    numberOfRows = cmd.ExecuteNonQuery();
                }
                catch { }
                lblRows.Text = "Number of rows deleted = " + numberOfRows;
                DisplayData();
            }
            else
                MessageBox.Show("Select an item with id = 5000");
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            MessageBox.Show("이름 변경: 공간정보공학 따릉이 -> 공간DB 따릉이");
            cmd.CommandText = "UPDATE public_bicycle " + "SET name = '공간DB 따릉이' " + "WHERE id = 5000";
            try
            {
                numberOfRows = cmd.ExecuteNonQuery();
            }
            catch { }
            lblRows.Text = "Number of rows updated = " + numberOfRows;
            DisplayData();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (conn != null)
                conn.Close();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }
    }
}
