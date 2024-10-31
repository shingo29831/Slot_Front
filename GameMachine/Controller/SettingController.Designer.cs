namespace GameMachine.Controller
{
    partial class SettingController
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
            InCredit = new Button();
            SettingComplete = new Button();
            Cancel = new Button();
            SuspendLayout();
            // 
            // InCredit
            // 
            InCredit.Font = new Font("Yu Gothic UI", 15F, FontStyle.Regular, GraphicsUnit.Point);
            InCredit.Location = new Point(300, 275);
            InCredit.Name = "InCredit";
            InCredit.Size = new Size(275, 150);
            InCredit.TabIndex = 0;
            InCredit.Text = "入金";
            InCredit.UseVisualStyleBackColor = true;
            InCredit.Click += InCredit_Click;
            // 
            // SettingComplete
            // 
            SettingComplete.Font = new Font("Yu Gothic UI", 15F, FontStyle.Regular, GraphicsUnit.Point);
            SettingComplete.Location = new Point(700, 275);
            SettingComplete.Name = "SettingComplete";
            SettingComplete.Size = new Size(275, 150);
            SettingComplete.TabIndex = 1;
            SettingComplete.Text = "台設定";
            SettingComplete.UseVisualStyleBackColor = true;
            SettingComplete.Click += SlotSetting_Click;
            // 
            // Cancel
            // 
            Cancel.Location = new Point(300, 475);
            Cancel.Name = "Cancel";
            Cancel.Size = new Size(675, 60);
            Cancel.TabIndex = 2;
            Cancel.Text = "キャンセル";
            Cancel.UseVisualStyleBackColor = true;
            Cancel.Click += Cancel_Click;
            // 
            // SettingController
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            Controls.Add(Cancel);
            Controls.Add(SettingComplete);
            Controls.Add(InCredit);
            Margin = new Padding(0);
            Name = "SettingController";
            Size = new Size(1275, 700);
            ResumeLayout(false);
        }

        #endregion

        private Button InCredit;
        private Button SettingComplete;
        private Button Cancel;
    }
}
