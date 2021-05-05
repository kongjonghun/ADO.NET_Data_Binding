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

namespace Form_practice2
{
    public partial class Form1 : Form
    {
        NpgsqlConnection conn;
        NpgsqlCommand cmd;
        NpgsqlDataAdapter adapter;

        DataSet dataset;
        DataTable tableGu;
        DataTable tableDong;
        DataTable tableBicycle;

        BindingSource bsGu;
        BindingSource bsDong;
        BindingSource bsBicycle;

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

            dataset = new DataSet();

            cmd.CommandText = "select sid, sgg_nm from seoul_gu";
            adapter.Fill(dataset, "Gu");
            tableGu = dataset.Tables["Gu"];

            cmd.CommandText = "select id, sid, emd_nm, area from seoul_dong";
            adapter.Fill(dataset, "Dong");
            tableDong = dataset.Tables["Dong"];

            cmd.CommandText = "select dong_id, id, name, address, capa from dong_bicycle";
            adapter.Fill(dataset, "Bicycle");
            tableBicycle = dataset.Tables["Bicycle"];

            DataRelation gu_dong = new DataRelation("gu_dong", tableGu.Columns["sid"], tableDong.Columns["sid"]);
            dataset.Relations.Add(gu_dong);
            DataRelation dong_bicycle = new DataRelation("dong_bicycle", tableDong.Columns["id"], tableBicycle.Columns["dong_id"]);
            dataset.Relations.Add(dong_bicycle);

            DataColumn dongCount = new DataColumn();
            dongCount.ColumnName = "count_dong";
            dongCount.DataType = typeof(int);
            dongCount.Expression = "Count(Child(gu_dong).id)";
            tableGu.Columns.Add(dongCount);
           
            DataColumn guArea = new DataColumn();
            guArea.ColumnName = "area_gu";
            guArea.DataType = typeof(double);
            guArea.Expression = "Sum(Child(gu_dong).area)";
            tableGu.Columns.Add(guArea);

            DataColumn bicycleCount = new DataColumn();
            bicycleCount.ColumnName = "count_bicycle";
            bicycleCount.DataType = typeof(int);
            bicycleCount.Expression = "Count(Child(dong_bicycle).id)";
            tableDong.Columns.Add(bicycleCount);

            DataColumn bicycleCapa = new DataColumn();
            bicycleCapa.ColumnName = "sum_capa";
            bicycleCapa.DataType = typeof(double);
            bicycleCapa.Expression = "Sum(Child(dong_bicycle).capa)";
            tableDong.Columns.Add(bicycleCapa);

            //// Expression 응용     
            // 부모테이블 컬럼 <- 자식테이블 연산 결과(One <- many)
            DataColumn dongCount2 = new DataColumn();
            dongCount2.ColumnName = "count_dong2";
            dongCount2.DataType = typeof(double);
            dongCount2.Expression = "sum(Child(gu_dong).id)/sum(Child(gu_dong).area)";
            tableGu.Columns.Add(dongCount2);

            // 자식테이블 컬럼 <- 부모테이블 컬럼(One -> many)
            DataColumn dongCount3 = new DataColumn();
            dongCount3.ColumnName = "count_dong3";
            dongCount3.DataType = typeof(string);
            dongCount3.Expression = "Parent(gu_dong).sid +' : '+ Parent(gu_dong).sgg_nm";
            tableDong.Columns.Add(dongCount3);

            // 자신의 테이블 컬럼
            DataColumn dongCount4 = new DataColumn();
            dongCount4.ColumnName = "count_dong4";
            dongCount4.DataType = typeof(string);
            dongCount4.Expression = "emd_nm";
            tableDong.Columns.Add(dongCount4);
            //// Expression 응용 끝

            bsGu = new BindingSource();
            bsGu.DataSource = tableGu;
            bsDong = new BindingSource();
            bsDong.DataSource = bsGu;
            bsDong.DataMember = "gu_dong";
            bsBicycle = new BindingSource();
            bsBicycle.DataSource = bsDong;
            bsBicycle.DataMember = "dong_bicycle";

            // 콤보박스
            cbGu.DataSource = bsGu;
            cbGu.DisplayMember = "sgg_nm";
            // 데이터그리드뷰
            dgvDong.DataSource = bsDong;
            dgvBicycle.DataSource = bsBicycle;
            // 텍스트박스
            tbCountDong.DataBindings.Add("Text", bsGu, "count_dong");
            tbSumArea.DataBindings.Add("Text", bsGu, "area_gu");
            tbCountBicycle.DataBindings.Add("Text", bsDong, "count_bicycle");
            tbSumCapa.DataBindings.Add("Text", bsDong, "sum_capa");

            // Expression 응용
            tb1.DataBindings.Add("Text", bsGu, "count_dong2");
            tb2.DataBindings.Add("Text", bsDong, "count_dong3");
            tb3.DataBindings.Add("Text", bsDong, "count_dong4");
        }
    }
}
