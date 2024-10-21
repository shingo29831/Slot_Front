namespace GameMachine.InitialSettingView
{
    partial class AccountLinkingController
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
            EmailTxb = new TextBox();
            PasswordTxb = new TextBox();
            label1 = new Label();
            label2 = new Label();
            RegistrationBtn = new Button();
            SuspendLayout();
            // 
            // EmailTxb
            // 
            EmailTxb.Font = new Font("Yu Gothic UI", 20.25F, FontStyle.Regular, GraphicsUnit.Point);
            EmailTxb.ImeMode = ImeMode.Off;
            EmailTxb.Location = new Point(375, 225);
            EmailTxb.Multiline = true;
            EmailTxb.Name = "EmailTxb";
            EmailTxb.Size = new Size(550, 50);
            EmailTxb.TabIndex = 0;
            // 
            // PasswordTxb
            // 
            PasswordTxb.Font = new Font("Yu Gothic UI", 20.25F, FontStyle.Regular, GraphicsUnit.Point);
            PasswordTxb.Location = new Point(375, 400);
            PasswordTxb.Multiline = true;
            PasswordTxb.Name = "PasswordTxb";
            PasswordTxb.Size = new Size(550, 50);
            PasswordTxb.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Yu Gothic UI Semibold", 20.25F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point);
            label1.Location = new Point(325, 175);
            label1.Name = "label1";
            label1.Size = new Size(94, 37);
            label1.TabIndex = 2;
            label1.Text = "E-mail";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Yu Gothic UI Semibold", 20.25F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point);
            label2.Location = new Point(325, 350);
            label2.Name = "label2";
            label2.Size = new Size(132, 37);
            label2.TabIndex = 3;
            label2.Text = "Password";
            // 
            // RegistrationBtn
            // 
            RegistrationBtn.BackColor = Color.FromArgb(0, 192, 0);
            RegistrationBtn.Font = new Font("Yu Gothic UI", 24F, FontStyle.Regular, GraphicsUnit.Point);
            RegistrationBtn.Location = new Point(575, 525);
            RegistrationBtn.Name = "RegistrationBtn";
            RegistrationBtn.Size = new Size(150, 75);
            RegistrationBtn.TabIndex = 4;
            RegistrationBtn.Text = "登録";
            RegistrationBtn.UseVisualStyleBackColor = false;
            RegistrationBtn.Click += RegistrationBtn_Click;
            // 
            // AccountLinkingController
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            Controls.Add(RegistrationBtn);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(PasswordTxb);
            Controls.Add(EmailTxb);
            Name = "AccountLinkingController";
            Size = new Size(1275, 700);
            Load += AccountLinkingScreen_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox EmailTxb;
        private TextBox PasswordTxb;
        private Label label1;
        private Label label2;
        private Button RegistrationBtn;
    }
}
