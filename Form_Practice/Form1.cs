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

namespace Form_Practice
{
    public partial class Form1 : Form
    {
        NpgsqlConnection conn;
        NpgsqlCommand cmd;

        String strConn = "host=localhost; port=5433; username=postgres; password=8270; database=gis";
        String strSQL = "select * from public_bicycle";
        int numberOfRows;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            conn = new NpgsqlConnection(strConn);
            cmd = new NpgsqlCommand(strSQL, conn);
            conn.Open();

            DisplayData();
        }

        private void DisplayData()
        {
            lstData.Items.Clear();
            cmd.CommandText = strSQL;
            NpgsqlDataReader dataReader = cmd.ExecuteReader();
            String item = String.Empty;

            while (dataReader.Read())
            {
                item = String.Empty;
                for (int i = 0; i < dataReader.FieldCount -1; i++)
                {
                    item += dataReader.GetValue(i).ToString().Trim() + "\t";
                }
                lstData.Items.Add(item);
            }
            dataReader.Close();
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            String str = "5000, '공간정보공학 따릉이', '동대문구', '서울시립댈학교 21세기관 앞'";
            MessageBox.Show(str, "Adding data..");
            cmd.CommandText = "insert into public_bicycle (id, name, sgg, address) values(" + str + ")";
            try
            {
                numberOfRows = cmd.ExecuteNonQuery();
            }
            catch{}
            lblRows.Text = "Number of rows added = " + numberOfRows;
            DisplayData();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            MessageBox.Show("이름변경 : 공간정보공학 따릉이 -> 공간DB 따릉이");
            cmd.CommandText = "update pulbic_bicycle set name = '공간DB 따릉이' where id = 5000";
            try
            {
                numberOfRows = cmd.ExecuteNonQuery();
            }
            catch 
            {
            }
            lblRows.Text = "Number of rows updated = " + numberOfRows;
            DisplayData();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if(lstData.SelectedItems.Count == 0)
            {
                MessageBox.Show("Selected an item whit id = 5000");
                return;
            }
            string str = lstData.SelectedItem.ToString();
            int id = int.Parse(str.Substring(0, 4));
            if(id == 5000)
            {
                MessageBox.Show("선택된 따릉이 대여소 삭제");
                cmd.CommandText = "Deleted From public_bicycle where id = " + id;
                try
                {
                    numberOfRows = cmd.ExecuteNonQuery();
                }
                catch 
                {                    
                }
                lblRows.Text = "Number of Rows deleted =" + numberOfRows;
                DisplayData();
            }
            else
            {
                MessageBox.Show("slected an item with id = 5000");
            }
        }
    }
}
