namespace GameMachine
{
    partial class CreditController
    {
        /// <summary> 
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region コンポーネント デザイナーで生成されたコード

        /// <summary> 
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を 
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            creditTxb = new TextBox();
            countTxb = new TextBox();
            payoutTxb = new TextBox();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            SuspendLayout();
            // 
            // creditTxb
            // 
            creditTxb.Font = new Font("Yu Gothic UI Semibold", 48F, FontStyle.Bold, GraphicsUnit.Point);
            creditTxb.Location = new Point(500, 42);
            creditTxb.Margin = new Padding(4, 5, 4, 5);
            creditTxb.Multiline = true;
            creditTxb.Name = "creditTxb";
            creditTxb.Size = new Size(213, 164);
            creditTxb.TabIndex = 0;
            creditTxb.Text = "0";
            creditTxb.TextAlign = HorizontalAlignment.Right;
            // 
            // countTxb
            // 
            countTxb.Font = new Font("Yu Gothic UI Semibold", 48F, FontStyle.Bold, GraphicsUnit.Point);
            countTxb.Location = new Point(786, 42);
            countTxb.Margin = new Padding(4, 5, 4, 5);
            countTxb.Multiline = true;
            countTxb.Name = "countTxb";
            countTxb.Size = new Size(284, 164);
            countTxb.TabIndex = 1;
            countTxb.Text = "0";
            countTxb.TextAlign = HorizontalAlignment.Right;
            // 
            // payoutTxb
            // 
            payoutTxb.Font = new Font("Yu Gothic UI Semibold", 48F, FontStyle.Bold, GraphicsUnit.Point);
            payoutTxb.Location = new Point(1143, 42);
            payoutTxb.Margin = new Padding(4, 5, 4, 5);
            payoutTxb.Multiline = true;
            payoutTxb.Name = "payoutTxb";
            payoutTxb.Size = new Size(213, 164);
            payoutTxb.TabIndex = 2;
            payoutTxb.Text = "0";
            payoutTxb.TextAlign = HorizontalAlignment.Right;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Yu Gothic UI", 14.25F, FontStyle.Italic, GraphicsUnit.Point);
            label1.Location = new Point(464, 0);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(110, 40);
            label1.TabIndex = 3;
            label1.Text = "CREDIT";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Yu Gothic UI", 14.25F, FontStyle.Italic, GraphicsUnit.Point);
            label2.Location = new Point(750, 0);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new Size(114, 40);
            label2.TabIndex = 4;
            label2.Text = "COUNT";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Yu Gothic UI", 14.25F, FontStyle.Italic, GraphicsUnit.Point);
            label3.Location = new Point(1107, 0);
            label3.Margin = new Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new Size(131, 40);
            label3.TabIndex = 5;
            label3.Text = "PAY OUT";
            // 
            // CreditController
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(payoutTxb);
            Controls.Add(countTxb);
            Controls.Add(creditTxb);
            Margin = new Padding(4, 5, 4, 5);
            Name = "CreditController";
            Size = new Size(1821, 248);
            Load += CreditDisplay_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label label1;
        private Label label2;
        private Label label3;
        public TextBox creditTxb;
        public TextBox countTxb;
        public TextBox payoutTxb;
    }
}
