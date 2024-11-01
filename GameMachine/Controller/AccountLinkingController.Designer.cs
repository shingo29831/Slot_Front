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
            UserNameTxb = new TextBox();
            PasswordTxb = new TextBox();
            label1 = new Label();
            label2 = new Label();
            RegistrationBtn = new Button();
            Cancel_Button = new Button();
            SuspendLayout();
            // 
            // UserNameTxb
            // 
            UserNameTxb.Font = new Font("Yu Gothic UI", 20.25F, FontStyle.Regular, GraphicsUnit.Point);
            UserNameTxb.ImeMode = ImeMode.Off;
            UserNameTxb.Location = new Point(375, 225);
            UserNameTxb.Multiline = true;
            UserNameTxb.Name = "UserNameTxb";
            UserNameTxb.Size = new Size(550, 50);
            UserNameTxb.TabIndex = 0;
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
            label1.Location = new Point(325, 173);
            label1.Name = "label1";
            label1.Size = new Size(145, 37);
            label1.TabIndex = 2;
            label1.Text = "UserName";
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
            RegistrationBtn.Location = new Point(375, 533);
            RegistrationBtn.Name = "RegistrationBtn";
            RegistrationBtn.Size = new Size(220, 75);
            RegistrationBtn.TabIndex = 4;
            RegistrationBtn.Text = "Login";
            RegistrationBtn.UseVisualStyleBackColor = false;
            RegistrationBtn.Click += RegistrationBtn_Click;
            // 
            // Cancel_Button
            // 
            Cancel_Button.BackColor = Color.FromArgb(128, 128, 255);
            Cancel_Button.Font = new Font("Yu Gothic UI", 24F, FontStyle.Regular, GraphicsUnit.Point);
            Cancel_Button.ForeColor = Color.White;
            Cancel_Button.Location = new Point(705, 533);
            Cancel_Button.Name = "Cancel_Button";
            Cancel_Button.Size = new Size(220, 75);
            Cancel_Button.TabIndex = 5;
            Cancel_Button.Text = "Cancel";
            Cancel_Button.UseVisualStyleBackColor = false;
            Cancel_Button.Click += CancelButton_Click;
            // 
            // AccountLinkingController
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            Controls.Add(Cancel_Button);
            Controls.Add(RegistrationBtn);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(PasswordTxb);
            Controls.Add(UserNameTxb);
            Name = "AccountLinkingController";
            Size = new Size(1275, 700);
            Load += AccountLinkingScreen_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox UserNameTxb;
        private TextBox PasswordTxb;
        private Label label1;
        private Label label2;
        private Button RegistrationBtn;
        private Button Cancel_Button;
    }
}
