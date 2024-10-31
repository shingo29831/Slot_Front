namespace GameMachine.Controller
{
    partial class InCreditController
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
            InCredit_Label = new Label();
            Yen_Label = new Label();
            textBox1 = new TextBox();
            OK_Button = new Button();
            Cancel_Button = new Button();
            SuspendLayout();
            // 
            // InCredit_Label
            // 
            InCredit_Label.Font = new Font("Yu Gothic UI", 20F, FontStyle.Regular, GraphicsUnit.Point);
            InCredit_Label.Location = new Point(432, 303);
            InCredit_Label.Margin = new Padding(0);
            InCredit_Label.Name = "InCredit_Label";
            InCredit_Label.Size = new Size(100, 50);
            InCredit_Label.TabIndex = 0;
            InCredit_Label.Text = "入金額";
            // 
            // Yen_Label
            // 
            Yen_Label.Font = new Font("Yu Gothic UI", 20F, FontStyle.Regular, GraphicsUnit.Point);
            Yen_Label.Location = new Point(843, 303);
            Yen_Label.Margin = new Padding(0);
            Yen_Label.Name = "Yen_Label";
            Yen_Label.Size = new Size(43, 50);
            Yen_Label.TabIndex = 1;
            Yen_Label.Text = "円";
            // 
            // textBox1
            // 
            textBox1.Font = new Font("Yu Gothic UI", 20F, FontStyle.Regular, GraphicsUnit.Point);
            textBox1.Location = new Point(665, 300);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(175, 43);
            textBox1.TabIndex = 2;
            textBox1.TextAlign = HorizontalAlignment.Right;
            textBox1.KeyPress += ValueCheck_KeyPress;
            // 
            // OK_Button
            // 
            OK_Button.Font = new Font("Yu Gothic UI", 15F, FontStyle.Regular, GraphicsUnit.Point);
            OK_Button.Location = new Point(432, 446);
            OK_Button.Name = "OK_Button";
            OK_Button.Size = new Size(175, 50);
            OK_Button.TabIndex = 3;
            OK_Button.Text = "OK";
            OK_Button.UseVisualStyleBackColor = true;
            OK_Button.Click += OK_Button_Click;
            // 
            // Cancel_Button
            // 
            Cancel_Button.Font = new Font("Yu Gothic UI", 15F, FontStyle.Regular, GraphicsUnit.Point);
            Cancel_Button.Location = new Point(665, 446);
            Cancel_Button.Name = "Cancel_Button";
            Cancel_Button.Size = new Size(175, 50);
            Cancel_Button.TabIndex = 4;
            Cancel_Button.Text = "Cancel";
            Cancel_Button.UseVisualStyleBackColor = true;
            Cancel_Button.Click += Cancel_Button_Click;
            // 
            // InCreditController
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            Controls.Add(Cancel_Button);
            Controls.Add(OK_Button);
            Controls.Add(textBox1);
            Controls.Add(Yen_Label);
            Controls.Add(InCredit_Label);
            Margin = new Padding(0);
            Name = "InCreditController";
            Size = new Size(1275, 700);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label InCredit_Label;
        private Label Yen_Label;
        private TextBox textBox1;
        private Button OK_Button;
        private Button Cancel_Button;
    }
}
