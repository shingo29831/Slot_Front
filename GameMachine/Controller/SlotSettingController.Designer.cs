namespace GameMachine.Controller
{
    partial class SlotSettingController
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
            SlotSet_Label = new Label();
            comboBox1 = new ComboBox();
            Ok_Button = new Button();
            Cancel_Button = new Button();
            SuspendLayout();
            // 
            // SlotSet_Label
            // 
            SlotSet_Label.Font = new Font("Yu Gothic UI", 20F, FontStyle.Regular, GraphicsUnit.Point);
            SlotSet_Label.Location = new Point(550, 325);
            SlotSet_Label.Name = "SlotSet_Label";
            SlotSet_Label.Size = new Size(100, 40);
            SlotSet_Label.TabIndex = 0;
            SlotSet_Label.Text = "台設定";
            // 
            // comboBox1
            // 
            comboBox1.Font = new Font("Yu Gothic UI", 15F, FontStyle.Regular, GraphicsUnit.Point);
            comboBox1.FormattingEnabled = true;
            comboBox1.Items.AddRange(new object[] { "1", "2", "3", "4", "5", "6" });
            comboBox1.Location = new Point(656, 329);
            comboBox1.MaxDropDownItems = 6;
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(100, 36);
            comboBox1.TabIndex = 1;
            // 
            // Ok_Button
            // 
            Ok_Button.Font = new Font("Yu Gothic UI", 15F, FontStyle.Regular, GraphicsUnit.Point);
            Ok_Button.Location = new Point(476, 454);
            Ok_Button.Name = "Ok_Button";
            Ok_Button.Size = new Size(139, 56);
            Ok_Button.TabIndex = 2;
            Ok_Button.Text = "OK";
            Ok_Button.UseVisualStyleBackColor = true;
            Ok_Button.Click += OkButton_Click;
            // 
            // Cancel_Button
            // 
            Cancel_Button.Font = new Font("Yu Gothic UI", 15F, FontStyle.Regular, GraphicsUnit.Point);
            Cancel_Button.Location = new Point(656, 454);
            Cancel_Button.Name = "Cancel_Button";
            Cancel_Button.Size = new Size(139, 56);
            Cancel_Button.TabIndex = 3;
            Cancel_Button.Text = "Cancel";
            Cancel_Button.UseVisualStyleBackColor = true;
            Cancel_Button.Click += CancelButton_Click;
            // 
            // SlotSetting
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            Controls.Add(Cancel_Button);
            Controls.Add(Ok_Button);
            Controls.Add(comboBox1);
            Controls.Add(SlotSet_Label);
            Margin = new Padding(0);
            Name = "SlotSetting";
            Size = new Size(1275, 700);
            Load += SlotSetting_Load;
            ResumeLayout(false);
        }

        #endregion

        private Label SlotSet_Label;
        private ComboBox comboBox1;
        private Button Ok_Button;
        private Button Cancel_Button;
    }
}
