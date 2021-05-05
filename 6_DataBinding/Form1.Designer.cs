namespace _6_DataBinding
{
    partial class Form1
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.cbGu = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dgvDong = new System.Windows.Forms.DataGridView();
            this.dgvBicycle = new System.Windows.Forms.DataGridView();
            this.label2 = new System.Windows.Forms.Label();
            this.tbCountDong = new System.Windows.Forms.TextBox();
            this.tbSumArea = new System.Windows.Forms.TextBox();
            this.tbCountBicycle = new System.Windows.Forms.TextBox();
            this.tbSumCapa = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDong)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBicycle)).BeginInit();
            this.SuspendLayout();
            // 
            // cbGu
            // 
            this.cbGu.FormattingEnabled = true;
            this.cbGu.Location = new System.Drawing.Point(162, 44);
            this.cbGu.Name = "cbGu";
            this.cbGu.Size = new System.Drawing.Size(121, 23);
            this.cbGu.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(44, 47);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(112, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "서울시  구 선택";
            // 
            // dgvDong
            // 
            this.dgvDong.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvDong.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDong.Location = new System.Drawing.Point(47, 110);
            this.dgvDong.Name = "dgvDong";
            this.dgvDong.RowHeadersWidth = 51;
            this.dgvDong.RowTemplate.Height = 27;
            this.dgvDong.Size = new System.Drawing.Size(528, 179);
            this.dgvDong.TabIndex = 2;
            // 
            // dgvBicycle
            // 
            this.dgvBicycle.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvBicycle.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBicycle.Location = new System.Drawing.Point(47, 323);
            this.dgvBicycle.Name = "dgvBicycle";
            this.dgvBicycle.RowHeadersWidth = 51;
            this.dgvBicycle.RowTemplate.Height = 27;
            this.dgvBicycle.Size = new System.Drawing.Size(528, 179);
            this.dgvBicycle.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(44, 92);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(102, 15);
            this.label2.TabIndex = 4;
            this.label2.Text = "법정동 리스트";
            // 
            // tbCountDong
            // 
            this.tbCountDong.Location = new System.Drawing.Point(612, 150);
            this.tbCountDong.Name = "tbCountDong";
            this.tbCountDong.Size = new System.Drawing.Size(146, 25);
            this.tbCountDong.TabIndex = 5;
            // 
            // tbSumArea
            // 
            this.tbSumArea.Location = new System.Drawing.Point(612, 235);
            this.tbSumArea.Name = "tbSumArea";
            this.tbSumArea.Size = new System.Drawing.Size(146, 25);
            this.tbSumArea.TabIndex = 6;
            // 
            // tbCountBicycle
            // 
            this.tbCountBicycle.Location = new System.Drawing.Point(612, 354);
            this.tbCountBicycle.Name = "tbCountBicycle";
            this.tbCountBicycle.Size = new System.Drawing.Size(146, 25);
            this.tbCountBicycle.TabIndex = 7;
            // 
            // tbSumCapa
            // 
            this.tbSumCapa.Location = new System.Drawing.Point(612, 448);
            this.tbSumCapa.Name = "tbSumCapa";
            this.tbSumCapa.Size = new System.Drawing.Size(146, 25);
            this.tbSumCapa.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(609, 121);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 15);
            this.label3.TabIndex = 9;
            this.label3.Text = "법정동 수";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(609, 205);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(107, 15);
            this.label4.TabIndex = 10;
            this.label4.Text = "자치구 총 면적";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(609, 323);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(122, 15);
            this.label5.TabIndex = 11;
            this.label5.Text = "따릉이 대여소 수";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(609, 420);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(137, 15);
            this.label6.TabIndex = 12;
            this.label6.Text = "따릉이 거치대 총합";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(807, 524);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tbSumCapa);
            this.Controls.Add(this.tbCountBicycle);
            this.Controls.Add(this.tbSumArea);
            this.Controls.Add(this.tbCountDong);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dgvBicycle);
            this.Controls.Add(this.dgvDong);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbGu);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDong)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBicycle)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbGu;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dgvDong;
        private System.Windows.Forms.DataGridView dgvBicycle;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbCountDong;
        private System.Windows.Forms.TextBox tbSumArea;
        private System.Windows.Forms.TextBox tbCountBicycle;
        private System.Windows.Forms.TextBox tbSumCapa;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
    }
}

