﻿namespace GameMachine.Controller
{
    partial class WaitLogoutController
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
            label1 = new Label();
            SuspendLayout();
            // 
            // label1
            // 
            label1.Font = new Font("Yu Gothic UI", 30F, FontStyle.Regular, GraphicsUnit.Point);
            label1.Location = new Point(275, 250);
            label1.Name = "label1";
            label1.Size = new Size(775, 175);
            label1.TabIndex = 0;
            label1.Text = "ログアウト処理が終わるまで少々お待ちください";
            // 
            // WaitLogoutController
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            Controls.Add(label1);
            Margin = new Padding(0);
            Name = "WaitLogoutController";
            Size = new Size(1275, 700);
            ResumeLayout(false);
        }

        #endregion

        private Label label1;
    }
}
