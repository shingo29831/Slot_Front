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
            creditTxb.Location = new Point(200, 25);
            creditTxb.Multiline = true;
            creditTxb.Name = "creditTxb";
            creditTxb.Size = new Size(175, 125);
            creditTxb.TabIndex = 0;
            // 
            // countTxb
            // 
            countTxb.Location = new Point(525, 25);
            countTxb.Multiline = true;
            countTxb.Name = "countTxb";
            countTxb.Size = new Size(225, 125);
            countTxb.TabIndex = 1;
            // 
            // payoutTxb
            // 
            payoutTxb.Location = new Point(900, 25);
            payoutTxb.Multiline = true;
            payoutTxb.Name = "payoutTxb";
            payoutTxb.Size = new Size(175, 125);
            payoutTxb.TabIndex = 2;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Yu Gothic UI", 14.25F, FontStyle.Italic, GraphicsUnit.Point);
            label1.Location = new Point(175, 0);
            label1.Name = "label1";
            label1.Size = new Size(73, 25);
            label1.TabIndex = 3;
            label1.Text = "CREDIT";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Yu Gothic UI", 14.25F, FontStyle.Italic, GraphicsUnit.Point);
            label2.Location = new Point(525, 0);
            label2.Name = "label2";
            label2.Size = new Size(75, 25);
            label2.TabIndex = 4;
            label2.Text = "COUNT";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Yu Gothic UI", 14.25F, FontStyle.Italic, GraphicsUnit.Point);
            label3.Location = new Point(875, 0);
            label3.Name = "label3";
            label3.Size = new Size(86, 25);
            label3.TabIndex = 5;
            label3.Text = "PAY OUT";
            // 
            // CreditDisplay
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(payoutTxb);
            Controls.Add(countTxb);
            Controls.Add(creditTxb);
            Name = "CreditDisplay";
            Size = new Size(1275, 175);
            Load += CreditDisplay_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox creditTxb;
        private TextBox countTxb;
        private TextBox payoutTxb;
        private Label label1;
        private Label label2;
        private Label label3;
    }
}
