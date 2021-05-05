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

namespace Master_Details
{
    public partial class Form1 : Form
    {
        // 각 메소드에서 공통으로 사용할 변수 선언
        // PostgreSQL과 연결하기 위해 Npgsql에서 제공하는 객체들
        NpgsqlConnection conn;
        NpgsqlCommand cmd;
        NpgsqlDataAdapter adapter;

        // 데이터셋 선언
        // 데이터테이블은 데이터셋.Tabels 안에 저장된 각각의 테이블을 참조하기 위한 변수
        DataSet dataSet;
        DataTable tblGu;
        DataTable tblDong;

        // 데이터테이블과 컨트롤 사이의 연결고리 역할을 하는 BindingSource 선언
        // BindingSource를 이용하면 컨트롤에서 기본키-외래키 관계를 통한 필터링이 자동으로 구현
        // 테이블의 수만큼 BindingSource 정의
        // !! 중요!!!
        BindingSource bsGu;
        BindingSource bsDong;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
            // 1. DB와의 연결
            // 연결 객체 초기화
            string strConn = "Host=localhost; port=5433; Username=postgres; Password=8270; Database=gis";
            conn = new NpgsqlConnection(strConn);

            // 명령 객체 초기화
            cmd = new NpgsqlCommand();
            cmd.Connection = conn;

            // 어댑터 및 데이터셋 초기화
            adapter = new NpgsqlDataAdapter();
            adapter.SelectCommand = cmd;
            dataSet = new DataSet();

            // 명령 전달 및 테이블 생성
            // !!! 중요!!! 데이터셋에 N개의 테이블을 형성 
            cmd.CommandText = "select sid, sgg_nm from seoul_gu";
            adapter.Fill(dataSet, "Gu");
            tblGu = dataSet.Tables["Gu"];

            cmd.CommandText = "select id, emd_nm, sid, area from seoul_dong";
            adapter.Fill(dataSet, "Dong");
            tblDong = dataSet.Tables["Dong"];

            // 관계 생성 및 데이터셋에 추가
            // !!!중요!!!
            // new DataRelation("관계명",부모테이블 기본키, 자식테이블 외래키)
            DataRelation gu_dong = new DataRelation("gu_dong", tblGu.Columns["sid"], tblDong.Columns["sid"]);
            // 데이터셋에 관계 추가
            dataSet.Relations.Add(gu_dong);

            // BindingSource 초기화 및 컨트롤로의 연결
            // DataBinding : 컨트롤과 데이터소스간 연결
            bsGu = new BindingSource();
            bsGu.DataSource = tblGu;
            //!!!중요!!! 핵심1. 자식 테이블의 BS는 부모 테이블의 BS 할당
            bsDong = new BindingSource();
            bsDong.DataSource = bsGu;
            //!!!중요!!! 핵심2. 자식 BS의 데이터멤버로 데이터셋의 관계명 할당
            bsDong.DataMember = "gu_dong";  // 관계에 해당하는 데이터들만 필터링

            //컨트롤에 바인딩소스 할당
            dgvGu.DataSource = bsGu;
            dgvDong.DataSource = bsDong;
        }
    }
}
