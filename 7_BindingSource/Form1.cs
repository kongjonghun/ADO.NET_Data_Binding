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

namespace _7_BindingSource
{
    public partial class Form1 : Form
    {
        NpgsqlConnection conn;
        NpgsqlCommand cmd;
        NpgsqlDataAdapter adapter;

        DataSet dataSet;
        DataTable dtGu;
        DataTable dtDong;
        DataTable dtGuDem;
        DataTable dtDongDem;

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
            string strConn ="Host=localhost; port=5433; Username=postgres; Password=8270; Database=gis";
            conn = new NpgsqlConnection(strConn);
            cmd = new NpgsqlCommand();
            cmd.Connection = conn;

            adapter = new NpgsqlDataAdapter();
            adapter.SelectCommand = cmd;
            dataSet = new DataSet();

            // geom컬럼은 호출 x
            cmd.CommandText = "select sid, sgg_nm from seoul_gu";
            adapter.Fill(dataSet, "Gu");
            dtGu = dataSet.Tables["Gu"];

            cmd.CommandText = "select id, emd_nm, sid from seoul_dong";
            adapter.Fill(dataSet, "Dong");
            dtDong = dataSet.Tables["Dong"];

            // DEM 테이블 추가
            // 긴 SQL문을 전달할 때는 띄어쓰기, 컴마 등을 반드시 유의
            cmd.CommandText =
            "select y.sid, (ST_SummaryStats(ST_Union(ST_Clip(x.rast, ST_Transform(y.geom, 5186))))).* " +
            "from dem as x, seoul_gu as y group by y.sid";
            adapter.Fill(dataSet, "Gu_DEM");
            dtGuDem = dataSet.Tables["Gu_DEM"];

            cmd.CommandText =
            "select y.id, (ST_SummaryStats(ST_Union(ST_Clip(x.rast, ST_Transform(y.geom, 5186))))).*, y.sid " +
            "from dem as x, seoul_dong as y group by y.id";
            adapter.Fill(dataSet, "Dong_DEM");
            dtDongDem = dataSet.Tables["Dong_DEM"];

            // 관계 생성 파트
            DataRelation rel1 = new DataRelation("gu_dong", dtGu.Columns["sid"], dtDong.Columns["sid"]);
            dataSet.Relations.Add(rel1);
            DataRelation rel2 = new DataRelation("DEMs", dtGuDem.Columns["sid"], dtDongDem.Columns["sid"]);
            dataSet.Relations.Add(rel2);

            // BindingSource 설정
            // 구 - 동
            bsGu = new BindingSource();
            bsGu.DataSource = dtGu;
            bsDong = new BindingSource();
            bsDong.DataSource = bsGu;
            bsDong.DataMember = "gu_dong";

            // 구DEM - 동DEM
            bsGuDem = new BindingSource();
            bsGuDem.DataSource = dtGuDem;
            bsDongDem = new BindingSource();
            bsDongDem.DataSource = bsGuDem;
            bsDongDem.DataMember = "DEMs";

            // 컨트롤과의 바인딩
            cbGu.DataSource = bsGu;
            cbGu.DisplayMember = "sgg_nm";

            dgvDong.DataSource = bsDong;
            dgvDongDem.DataSource = bsDongDem;

            tbGuAvgDem.DataBindings.Add("Text", bsGuDem, "mean");
            tbGuMinDem.DataBindings.Add("Text", bsGuDem, "min");
            tbGuMaxDem.DataBindings.Add("Text", bsGuDem, "max");

            // 바인딩소스의 CurrentChanged 이벤트 추가
            bsGu.CurrentChanged += BsGu_CurrentChanged;
            // CurrentChanged : 바인딩소스이 가지고 있는 이벤트
            // 바인딩소스에서 선택된(활성화된) 데이터의 변화가 있을 때 += 오른쪽 메소드 실행: 구콤보박스 선택           
            bsGu.ResetBindings(false);
            // ResetBindings : bs를 refresh 기능 : 현재 컨트롤에 할당된 데이터를 적용
            bsDong.CurrentChanged += BsDong_CurrentChanged;
            bsDong.ResetBindings(false);
            bsDongDem.CurrentChanged += BsDongDem_CurrentChanged;
        }
        private void BsGu_CurrentChanged(object sender, EventArgs e)
        {
            // 바인딩소스에서 현재 선택된 객체를 참조, DataRowView
            // bs.Current : 바인딩소스에서 현재 선택된 객체(Current 프로퍼티)
            // 선택된 객체의 데이터 타입 : DataRowView
            DataRowView drv = bsGu.Current as DataRowView;
            // 바인딩소스 필터링
            // 자치구 콤보박스 선택시, 자치구 DEM TextBox 변화
            // Filter : 텍스트 형태로 필터링 조건 삽입            
            // string filterString = string.Format("sid = {0}", drv["sid"]);
            string filterString = "sid =" + drv["sid"].ToString();
            bsGuDem.Filter = filterString;            
        }
        private void BsDong_CurrentChanged(object sender, EventArgs e)
        {
            // Position : 바인딩소스에서 데이터의 인덱스
            // 법정동 현재 위치 = 법정동DEM 위치
            bsDongDem.Position = bsDong.Position;
            if (bsDongDem.Current != null)
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
