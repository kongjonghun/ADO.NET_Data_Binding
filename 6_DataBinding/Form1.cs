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

namespace _6_DataBinding
{
    public partial class Form1 : Form
    {
        NpgsqlConnection conn;
        NpgsqlCommand cmd;
        NpgsqlDataAdapter adapter;

        DataSet dataSet;
        DataTable tblGu;
        DataTable tblDong;
        DataTable tblBicycle;

        BindingSource bsGu;
        BindingSource bsDong;
        BindingSource bsBicycle;

        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            string strConn = "Host=localhost; port=5433; Username=postgres; Password=8270; Database=gis";
            conn = new NpgsqlConnection(strConn);
            cmd = new NpgsqlCommand();
            cmd.Connection = conn;

            adapter = new NpgsqlDataAdapter();
            adapter.SelectCommand = cmd;

            dataSet = new DataSet();

            cmd.CommandText = "select sid, sgg_nm from seoul_gu";
            adapter.Fill(dataSet, "Gu");
            tblGu = dataSet.Tables["Gu"];

            cmd.CommandText = "select id, emd_nm, sid, area from seoul_dong";
            adapter.Fill(dataSet, "Dong");
            tblDong = dataSet.Tables["Dong"];

            cmd.CommandText = "select id, name, address, capa, dong_id from dong_bicycle";
            adapter.Fill(dataSet, "Bicycle");
            tblBicycle = dataSet.Tables["Bicycle"];

            // 관계 생성 파트
            // 일 대 다 : 다(외래키)
            DataRelation gu_dong = new DataRelation("gu_dong", tblGu.Columns["sid"], tblDong.Columns["sid"]);
            dataSet.Relations.Add(gu_dong);

            DataRelation dong_bicycle = new DataRelation("dong_bicycle", tblDong.Columns["id"], tblBicycle.Columns["dong_id"]);
            dataSet.Relations.Add(dong_bicycle);

            // Expression 컬럼 추가 파트
            // 관계를 기반으로 집계함수 적용
            DataColumn dongCount = new DataColumn();
            dongCount.ColumnName = "count_dong";
            dongCount.DataType = typeof(int);
            dongCount.Expression = "Count(Child(gu_dong).id)";
            tblGu.Columns.Add(dongCount);
          
            DataColumn guArea = new DataColumn();
            guArea.ColumnName = "area_gu";
            guArea.DataType = typeof(double);
            guArea.Expression = "Sum(Child(gu_dong).area)";
            tblGu.Columns.Add(guArea);

            DataColumn bicycleCount = new DataColumn();
            bicycleCount.ColumnName = "count_bicycle";
            bicycleCount.DataType = typeof(int);
            bicycleCount.Expression = "Count(Child(dong_bicycle).id)";
            tblDong.Columns.Add(bicycleCount);

            DataColumn bicycleCapa = new DataColumn();
            bicycleCapa.ColumnName = "sum_capa";
            bicycleCapa.DataType = typeof(double);
            bicycleCapa.Expression = "Sum(Child(dong_bicycle).capa)";
            tblDong.Columns.Add(bicycleCapa);

            // BindingSource 설정 (연속된 one-to-many)
            bsGu = new BindingSource();
            bsGu.DataSource = tblGu;
            bsDong = new BindingSource();
            bsDong.DataSource = bsGu;
            bsDong.DataMember = "gu_dong";
            bsBicycle = new BindingSource();
            bsBicycle.DataSource = bsDong;
            bsBicycle.DataMember = "dong_bicycle";

            // 컨트롤과의 데이터 바인딩

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
        }
    }
}
