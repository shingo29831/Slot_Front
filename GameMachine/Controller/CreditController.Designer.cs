﻿namespace GameMachine
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
            creditTxb.Location = new Point(350, 25);
            creditTxb.Multiline = true;
            creditTxb.Name = "creditTxb";
            creditTxb.Size = new Size(150, 100);
            creditTxb.TabIndex = 0;
            creditTxb.Text = "0";
            creditTxb.TextAlign = HorizontalAlignment.Right;
            // 
            // countTxb
            // 
            countTxb.Font = new Font("Yu Gothic UI Semibold", 48F, FontStyle.Bold, GraphicsUnit.Point);
            countTxb.Location = new Point(550, 25);
            countTxb.Multiline = true;
            countTxb.Name = "countTxb";
            countTxb.Size = new Size(200, 100);
            countTxb.TabIndex = 1;
            countTxb.Text = "0";
            countTxb.TextAlign = HorizontalAlignment.Right;
            // 
            // payoutTxb
            // 
            payoutTxb.Font = new Font("Yu Gothic UI Semibold", 48F, FontStyle.Bold, GraphicsUnit.Point);
            payoutTxb.Location = new Point(800, 25);
            payoutTxb.Multiline = true;
            payoutTxb.Name = "payoutTxb";
            payoutTxb.Size = new Size(150, 100);
            payoutTxb.TabIndex = 2;
            payoutTxb.Text = "0";
            payoutTxb.TextAlign = HorizontalAlignment.Right;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Yu Gothic UI", 14.25F, FontStyle.Italic, GraphicsUnit.Point);
            label1.Location = new Point(325, 0);
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
            label3.Location = new Point(775, 0);
            label3.Name = "label3";
            label3.Size = new Size(86, 25);
            label3.TabIndex = 5;
            label3.Text = "PAY OUT";
            // 
            // CreditController
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
            Name = "CreditController";
            Size = new Size(1275, 149);
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
