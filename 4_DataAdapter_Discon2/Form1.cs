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

namespace _4_DataAdapter_Discon2
{
    public partial class MainForm : Form
    {
        NpgsqlConnection conn;
        NpgsqlCommand cmd;
        NpgsqlDataAdapter adapter;
        DataSet dataset;
        DataTable table;

        public MainForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            String strConn = "Host=localhost; port=5433; Username=postgres; Password=8270; Database=gis";
            conn = new NpgsqlConnection(strConn);
            String strSQL = "select id, name, address from public_bicycle";
            cmd = new NpgsqlCommand(strSQL, conn);

            adapter = new NpgsqlDataAdapter();
            adapter.SelectCommand = cmd;
            dataset = new DataSet();

            adapter.Fill(dataset, "Bicycle");
            table = dataset.Tables["Bicycle"];

            DisplayData();
        }

        private void DisplayData()
        {
            lstData.Items.Clear();
            string item = String.Empty;

            foreach (DataRow row in table.Rows)
            {
                item = String.Empty;
                if(row.RowState == DataRowState.Deleted)
                {
                    string id = row["id", DataRowVersion.Original].ToString();
                    lstData.Items.Add("**deleted**" + id);
                    continue;
                }
                for (int i = 0; i < table.Columns.Count; i++)
                {
                    item += row[i].ToString().Trim() + "\t";
                }
                item += string.Format("({0})", row.RowState);
                if (row.HasErrors) item += "(***" + row.RowError + "***)";
                lstData.Items.Add(item);
            }
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            DataRow row = table.NewRow();

            row["id"] = 5000;
            row["name"] = "공간정보공학 따릉이";
            row["address"] = "시립대 21세기관 앞";

            table.Rows.Add(row);
            DataTable tableChanges = table.GetChanges(DataRowState.Added);
            if(tableChanges != null)
            {
                foreach (DataRow addedRow in tableChanges.Rows)
                {
                    MessageBox.Show("***added*** id :" + addedRow["id"]);
                }
            }
            DisplayData();
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            int index = lstData.SelectedIndex;
            if (index == -1) return;

            DataRow row = table.Rows[index];
            if (row["id"].ToString() != "5000") return;

            row.Delete();
            DataTable tableChanges = table.GetChanges(DataRowState.Deleted);
            if (tableChanges != null)
            {
                foreach (DataRow deletedRow in tableChanges.Rows)
                {
                    MessageBox.Show("***deleted*** id:" + deletedRow["id", DataRowVersion.Original]);
                }
                DisplayData();
            }
        }
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            int index = lstData.SelectedIndex;
            if (index == -1) return;

            DataRow row = table.Rows[index];
            if (row["id"].ToString() != "5000") return;

            row["name"] = "공간디비 따릉이";

            DataTable tableChanges = table.GetChanges(DataRowState.Modified);
            if (tableChanges != null)
            {
                foreach (DataRow changeRow in tableChanges.Rows)
                {
                    MessageBox.Show("***modified*** id:" + changeRow["id"]);
                }
                DisplayData();
            }
        }

        private void btnCommit_Click(object sender, EventArgs e)
        {
            NpgsqlCommandBuilder builder = new NpgsqlCommandBuilder(adapter);
            try
            {
                adapter.Update(table);
            }
            catch(NpgsqlException EX)
            {
                MessageBox.Show(EX.Message, "Error Commiting Changes");
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
