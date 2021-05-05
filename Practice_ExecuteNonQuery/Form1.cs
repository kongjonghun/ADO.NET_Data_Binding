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

namespace Practice_ExecuteNonQuery
{
    public partial class MainForm : Form
    {
        String strConn = "Host=localhost; port=5433; Username=postgres; Password=8270; Database = gis";
        String strSQL = "select * from public_bicycle";
        NpgsqlConnection conn;
        NpgsqlCommand cmd;
        int numbeOfRows;
        public MainForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }
        private void btnLoad_Click(object sender, EventArgs e)
        {
            conn = new NpgsqlConnection(strConn);
            cmd = conn.CreateCommand();
            cmd.CommandText = strSQL;
            conn.Open();

            DisplayData();
        }

        private void DisplayData()
        {
            lstData.Items.Clear();

            cmd.CommandText = strSQL;
            NpgsqlDataReader reader = cmd.ExecuteReader();
            String item = String.Empty;
            while (reader.Read())
            {
                item = String.Empty;
                for (int i = 0; i < reader.FieldCount -1; i++)
                {
                    item += reader.GetValue(i).ToString().Trim() + "\t";
                }
                lstData.Items.Add(item);
            }
            reader.Close();
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            String str = "5000, '공간정보공학 따릉이', '동대문구', '서울시립대학교 21세기관 앞'";
            MessageBox.Show(str, "Adding data...");

            cmd.CommandText = "Insert into public_bicycle (id, name, sgg, address) values(" + str + ")";
            try
            {
                numbeOfRows = cmd.ExecuteNonQuery();
            }
            catch { }
            lblRows.Text = "Number Of rows added = " + numbeOfRows;

            DisplayData();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if(lstData.SelectedItems.Count == 0)
            {
                MessageBox.Show("select an item with id = 5000");
                return;
            }
            String str = lstData.SelectedItem.ToString();
            int id = int.Parse(str.Substring(0, 4));
            if(id == 5000)
            {
                MessageBox.Show("선택된 따릉이 대여소 삭제");
                cmd.CommandText = "delete from public_bicycle where id =" + id;
                try{
                    numbeOfRows = cmd.ExecuteNonQuery();
                }
                catch { }
                lblRows.Text = "Number of rows deleted =" + numbeOfRows;

                DisplayData();
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            MessageBox.Show("이름 변경 : 공간정보공학 따릉이 -> 공간DB 따릉이");
            cmd.CommandText = "update public_bicycle set name = '공간DB 따릉이' where id = 5000";
            try
            {
                numbeOfRows =  cmd.ExecuteNonQuery();
            }
            catch { }
            lblRows.Text = "Number of rows updated = " + numbeOfRows;
            DisplayData();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(conn != null)
            {
                conn.Close();
            }
        }
    }
}
