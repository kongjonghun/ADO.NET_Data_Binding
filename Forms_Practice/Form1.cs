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

namespace Forms_Practice
{
    public partial class Form1 : Form
    {
        NpgsqlConnection conn;
        NpgsqlCommand cmd;
        NpgsqlDataAdapter adapter;

        DataSet dataSet;
        DataTable tableGu;
        DataTable tableDong;
        DataTable tableGuDem;
        DataTable tableDongDem;

        BindingSource bsGu;
        BindingSource bsDong;
        BindingSource bsGuDem;
        BindingSource bsDongDem;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string strConn = "host=localhost; port=5433; username=postgres; password=8270; database=gis";
            conn = new NpgsqlConnection(strConn);
            cmd = new NpgsqlCommand();
            cmd.Connection = conn;

            adapter = new NpgsqlDataAdapter();
            adapter.SelectCommand = cmd;

            dataSet = new DataSet();

            // Datable 생성
            cmd.CommandText = "select sid, sgg_nm from seoul_gu";
            adapter.Fill(dataSet, "Gu");
            tableGu = dataSet.Tables["Gu"];

            cmd.CommandText = "select id, sid, emd_nm from seoul_dong";
            adapter.Fill(dataSet, "Dong");
            tableDong = dataSet.Tables["Dong"];

            cmd.CommandText = "select y.sid, (ST_SummaryStats(ST_Union(ST_Clip(x.rast, ST_Transform(y.geom, 5186))))).* "
                + "from dem as x, seoul_gu as y group by y.sid";
            adapter.Fill(dataSet, "Gu_Dem");
            tableGuDem = dataSet.Tables["Gu_Dem"];

            cmd.CommandText = "select y.id, y.sid, (ST_SummaryStats(ST_Union(ST_Clip(x.rast, ST_Transform(y.geom, 5186))))).* "
                + "from dem as x, seoul_dong as y group by y.id";
            adapter.Fill(dataSet, "Dong_Dem");
            tableDongDem = dataSet.Tables["Dong_Dem"];

            // 관계 생성
            DataRelation gu_dong = new DataRelation("gu_dong", tableGu.Columns["sid"], tableDong.Columns["sid"]);
            dataSet.Relations.Add(gu_dong);
            DataRelation gu_dong_dem = new DataRelation("gu_dong_dem", tableGuDem.Columns["sid"], tableDongDem.Columns["sid"]);
            dataSet.Relations.Add(gu_dong_dem);

            // BindingSource 설정
            bsGu = new BindingSource();
            bsGu.DataSource = tableGu;
            bsDong = new BindingSource();
            bsDong.DataSource = bsGu;
            bsDong.DataMember = "gu_dong";

            bsGuDem = new BindingSource();
            bsGuDem.DataSource = tableGuDem;
            bsDongDem = new BindingSource();
            bsDongDem.DataSource = bsGuDem;
            bsDongDem.DataMember = "gu_dong_dem";

            // 컨트롤 바인딩
            cbGu.DataSource = bsGu;
            cbGu.DisplayMember = "sgg_nm";

            dgvDong.DataSource = bsDong;
            dgvDongDem.DataSource = bsDongDem;

            tbGuAvgDem.DataBindings.Add("Text", bsGuDem, "mean");
            tbGuMinDem.DataBindings.Add("Text", bsGuDem, "min");
            tbGuMaxDem.DataBindings.Add("Text", bsGuDem, "max");

            // 바인딩소스의 CurrentChanged
            bsGu.CurrentChanged += BsGu_CurrentChanged;
            bsGu.ResetBindings(false);

            bsDong.CurrentChanged += BsDong_CurrentChanged;
            bsDong.ResetBindings(false);

            bsDongDem.CurrentChanged += BsDongDem_CurrentChanged;
        }
        private void BsGu_CurrentChanged(object sender, EventArgs e)
        {
            DataRowView drv = bsGu.Current as DataRowView;
            string filterString = "sid = " + drv["sid"];
            bsGuDem.Filter = filterString;
        }       
        private void BsDong_CurrentChanged(object sender, EventArgs e)
        {
            bsDongDem.Position = bsDong.Position;
            if(bsDongDem.Current != null)
            {
                DataRowView drv = bsDongDem.Current as DataRowView;
                tbDongAvgDem.Text = drv["mean"].ToString();
                tbDongMinDem.Text = drv["min"].ToString();
                tbDongMaxDem.Text = drv["max"].ToString();
            }
        }
        private void BsDongDem_CurrentChanged(object sender, EventArgs e)
        {
            bsDong.Position = bsDongDem.Position;
        }

    }
}
