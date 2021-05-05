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
/// <summary>
/// 클래스의 멤버
/// 변수와 메소드
/// 접근한정자(Public, Private, Protected...), Static(정적 / 인스턴스 멤버)
/// </summary>


namespace _4_DataAdapter2
{
    public partial class DataAdapter : Form
    {
        NpgsqlConnection conn;
        NpgsqlCommand cmd;
        NpgsqlDataAdapter adapter;
        DataSet dataset;
        DataTable table; // 선언

        public DataAdapter() // Form 생성자 메소드
        {
            InitializeComponent();
        }
        // Form이 Load되면, ListBox에 테이블 가시화
        // Adapter로 DataSet에 따릉이 테이블 추가하고, ListBox에 dataRow 삽입
        private void DataAdapter_Load(object sender, EventArgs e)
        {
            String strConn = "Host=localhost; port=5433; Username=postgres; Password=8270; Database=gis";
            conn = new NpgsqlConnection(strConn);
            String strSQL = "select id, name, address from public_bicycle";
            cmd = new NpgsqlCommand(strSQL, conn); 

            adapter = new NpgsqlDataAdapter();
            adapter.SelectCommand = cmd;
            // 데이터셋 초기화
            dataset = new DataSet();

            // 데이터셋에 테이블 생성
            adapter.Fill(dataset, "Bicycle");
            table = dataset.Tables["Bicycle"];

            DisplayData();
        }

        private void DisplayData()
        {
            lstData.Items.Clear();
            string item = String.Empty;

            // Enumerate cached data
            foreach (DataRow row in table.Rows)
            {
                item = String.Empty;
            // if( (row.RowState & DataRowState.Deleted) != 0 )
            // RowState : RowState데이터셋에서 row의 상태
            // 현재 row가 지워졌다면 id값을 가져와라 
            if (row.RowState == DataRowState.Deleted)
            {
                //DataRowVersion.Original : 삭제 이전의 원래의 값(그중 id컬럼)
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
            // { 
            //    item += row[curCol].ToString().Trim() + "\t"; 
            // }
            // row[curCol] <- 해당되는 컬럼의 값 or row["컬럼명"]
            }
        }
        private void btnInsert_Click(object sender, EventArgs e)
        {            
            // 새로운 데이터 row 생성 / 비어있는 행으로 된 DataRow 개체 생성
            DataRow row = table.NewRow();

            // Fill DataRow
            row["id"] = 5000;
            row["name"] = "공간정보공학 따릉이";
            row["address"] = "시립대 21세기관 앞";

            // 테이블에 row 추가
            table.Rows.Add(row);
            // table.GetChanges(DataRowState.Added) : Added된 행들만
            DataTable tableChanges = table.GetChanges(DataRowState.Added);
            // 추가된게 있으면
            if (tableChanges != null)
            {   // 추가된 행에서 id값만 보여주기
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
            // 선택한 행 index
            int index = lstData.SelectedIndex;
            // 선택한것이 없으면 끝남
            if (index == -1) return;

            // Get row from datatable
            DataRow row = table.Rows[index];
            // 5000아니라면 끝내
            if (row["id"].ToString() != "5000") 
                return;

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
            int index = lstData.SelectedIndex;
            if (index == -1) return;
          
            DataRow row = table.Rows[index];
            if (row["id"].ToString() != "5000") 
                return;
            // Remove the row from the datatable
            // table.Rows.Remove(row);); // Remove a row from the collection            
            row.Delete();

            DataTable tableChanges = table.GetChanges(DataRowState.Deleted);
            if (tableChanges != null)
            {
                foreach (DataRow deletedRow in tableChanges.Rows)
                {
                    MessageBox.Show("***deleted*** id: " + deletedRow["id", DataRowVersion.Original]);
                }
            }
            DisplayData();
        }

        private void btnCommit_Click(object sender, EventArgs e)
        {
            // 어댑터를 사용한 DB 업데이트
            // CommandBuilder를 생성
            // 커맨드 빌더가 데이터셋 안의 테이블 상태를 체크하여 자동으로 쿼리문 생성(Update,Delete, Insert)
            ////// 중요!!
            /// 데이터셋을 db에 반영하는 것 : CommandBuilder
            NpgsqlCommandBuilder builder = new NpgsqlCommandBuilder(adapter);

            // Commit changes back to the data source
            try
            {
                adapter.Update(table);                
                // = adapter.Update(dataset, "Bicycle");
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

