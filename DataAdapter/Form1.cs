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

namespace DataAdapter
{
    public partial class MainForm : Form
    {   //멤버 변수 : 클래스 내부에서 공통으로 사용 가능
        NpgsqlConnection conn;
        NpgsqlCommand cmd;
        NpgsqlDataAdapter adapter;
        DataSet dataset; //선언
        DataTable table;
        
        public MainForm() //생성자
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

            //// 80% 데이터 불러오고 저장하기까지
            // Create a connection object 
            string strConn =
            "Host=localhost; port=5433; Username=postgres; Password=8270; Database=gis";
            conn = new NpgsqlConnection(strConn);

            // create a SqlCommand object
            string strSQL = " SELECT id, name, address FROM public_bicycle";
            cmd = conn.CreateCommand();
            cmd.CommandText = strSQL;

            // conn.Open(); => Not required when using adapter's Fill() method
            // Create a adapter object and set its SelectCommand
            // property to the command object
            adapter = new NpgsqlDataAdapter();
            adapter.SelectCommand = cmd;

            // Create the dataset object
            // and fill the dataset object using the Fill() method
            dataset = new DataSet();
            adapter.Fill(dataset, "Bicycle");
            table = dataset.Tables["Bicycle"];

            // 리스트 박스에 데이터 가시화
            DisplayData(); 
        }

        private void DisplayData()
        {
            // Clear the listbox
            lstData.Items.Clear();
            string item = String.Empty;
           
            // Enumerate cached data
            foreach (DataRow row in table.Rows)
            {
                item = String.Empty;
                //if( (row.RowState & DataRowState.Deleted) != 0 )
                // 데이터셋에서 row의 상태
                // 현재 row가 지워졌다면 id값을 가져와라
                if (row.RowState == DataRowState.Deleted)
                {
                    //DataRowVersion.Original : 삭제 이전의 원래의 값
                    string id = row["id", DataRowVersion.Original].ToString();
                    lstData.Items.Add("***deleted***: " + id);
                    continue;
                }
                for (int curCol = 0; curCol < table.Columns.Count; curCol++)
                {
                    item += row[curCol].ToString().Trim() + "\t";
                }
                // row[curCol] : 해당 컬럼의 값 row[컬럼명]
                // row[0]부터 n-1까지 컬럼의 값 가져오기

                item += string.Format("({0})", row.RowState);
                if (row.HasErrors) item += "(***" + row.RowError + "***)";
                lstData.Items.Add(item);

                ////////중요!!
                //// DataTable, DataRow 익숙해지기!!
                //// foreach (DataRow row in table.Rows)
                //// 여기서 row의각 컬럼 값에 접근하기
                // for (int curCol = 0; curCol < table.Columns.Count; curCol++)
                // { item += row[curCol].ToString().Trim() + "\t"; }
                // row[i] <- 해당되는 컬럼의 값 or row["컬럼명"]
            }
        }
        private void btnInsert_Click(object sender, EventArgs e) 
        {
            // Ask table for an empty DataRow
            // 새로운 데이터 row 생성
            DataRow row = table.NewRow();

            // Fill DataRow
            row["id"] = 5000;
            row["name"] = "공간정보공학 따릉이";
            row["address"] = "시립대 21세기관 앞";

            // Add DataRow to the table
            table.Rows.Add(row);
            DataTable tableChanges = table.GetChanges(DataRowState.Added);
            if (tableChanges != null)
            {
                foreach (DataRow addedRow in tableChanges.Rows)
                {
                    MessageBox.Show("***added*** id : " + addedRow["id"]);
                }
            }
            // 추가된 row을 다시 보여주기 위해
            DisplayData();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            // Get selection index from listbox
            int index = lstData.SelectedIndex;
            // 선택한것이 없으면 끝남
            if (index == -1) return;

            // Get row from datatable
            DataRow row = table.Rows[index];
            // 5000아니라면 끝내
            if (row["id"].ToString() != "5000") return;

            /// 공강정보 따릉이 -> 공간디비 따릉이
            row["name"] = "공간디비 따릉이";

            DataTable tableChanges = table.GetChanges(DataRowState.Modified);
            if (tableChanges != null)
            {
                foreach (DataRow changedRow in tableChanges.Rows)
                {
                    MessageBox.Show("***modified*** id : " + changedRow["id"]);
                }
            }
            DisplayData();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            // Get selection index from listbox
            int index = lstData.SelectedIndex;
            if (index == -1) return;
            // Get row from datatable
            DataRow row = table.Rows[index];
            if (row["id"].ToString() != "5000") return;
            // Remove the row from the datatable
           //table.Rows.Remove(row);); // Remove a row from the collection
            // Mark the row as deleted
            row.Delete();
            DataTable tableChanges = table.GetChanges(DataRowState.Deleted);
            if (tableChanges != null)
            {
                foreach (DataRow deletedRow in tableChanges.Rows)
                {
                    MessageBox.Show("***deleted*** id: " +
                    deletedRow["id"
                    , DataRowVersion.Original]);
                }
            }
            DisplayData();
        }
        private void btnCommit_Click(object sender, EventArgs e)
        {
            // 어댑터를 사용한 DB 업데이트
            // 커맨드빌더를 생성
            // 커맨드 빌더가 데이터셋 안의 테이블 상태를 체크하여 자동으로 쿼리문 생성(Update,Delete, Insert)
            ////// 중요!!
            /// 데이터셋을 db에 반영하는 것 : Builder
            NpgsqlCommandBuilder builder = new NpgsqlCommandBuilder(adapter);

            // Commit changes back to the data source
            try
            {
                adapter.Update(table);
                //Same as the following
                //adapter.Update(dataset, "Bicycle");
            }
            catch (NpgsqlException ex)
            {
                MessageBox.Show(ex.Message, "Error(s) Commiting Changes");
            }
            DisplayData();
        }

        private void btnReload_Click(object sender, EventArgs e)
        {
            dataset.Reset();
            adapter.Fill(dataset, "Bicycle");
            table = dataset.Tables["Bicycle"];
            DisplayData();
        }

    }
}


