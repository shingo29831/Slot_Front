namespace GameMachine
{
    partial class UserSelectionScren
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
            panel1 = new Panel();
            selectAccountPlay = new Button();
            label1 = new Label();
            panel2 = new Panel();
            selectGustPlay = new Button();
            label2 = new Label();
            label3 = new Label();
            panel3 = new Panel();
            textBox1 = new TextBox();
            label4 = new Label();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            panel3.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(192, 255, 192);
            panel1.Controls.Add(selectAccountPlay);
            panel1.Controls.Add(label1);
            panel1.Location = new Point(225, 150);
            panel1.Name = "panel1";
            panel1.Size = new Size(325, 450);
            panel1.TabIndex = 0;
            // 
            // selectAccountPlay
            // 
            selectAccountPlay.Font = new Font("Yu Gothic UI", 20.25F, FontStyle.Regular, GraphicsUnit.Point);
            selectAccountPlay.Location = new Point(75, 275);
            selectAccountPlay.Name = "selectAccountPlay";
            selectAccountPlay.Size = new Size(175, 75);
            selectAccountPlay.TabIndex = 2;
            selectAccountPlay.Text = "Play";
            selectAccountPlay.UseVisualStyleBackColor = true;
            selectAccountPlay.Click += selectAccountPlay_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Yu Gothic UI", 27.75F, FontStyle.Regular, GraphicsUnit.Point);
            label1.Location = new Point(50, 100);
            label1.Name = "label1";
            label1.Size = new Size(238, 50);
            label1.TabIndex = 0;
            label1.Text = "アカウントプレイ";
            // 
            // panel2
            // 
            panel2.BackColor = Color.FromArgb(192, 255, 192);
            panel2.Controls.Add(selectGustPlay);
            panel2.Controls.Add(label2);
            panel2.Location = new Point(875, 150);
            panel2.Name = "panel2";
            panel2.Size = new Size(325, 450);
            panel2.TabIndex = 1;
            // 
            // selectGustPlay
            // 
            selectGustPlay.Font = new Font("Yu Gothic UI", 20.25F, FontStyle.Regular, GraphicsUnit.Point);
            selectGustPlay.Location = new Point(75, 275);
            selectGustPlay.Name = "selectGustPlay";
            selectGustPlay.Size = new Size(175, 75);
            selectGustPlay.TabIndex = 3;
            selectGustPlay.Text = "Play";
            selectGustPlay.UseVisualStyleBackColor = true;
            selectGustPlay.Click += selectGustPlay_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Yu Gothic UI", 27.75F, FontStyle.Regular, GraphicsUnit.Point);
            label2.Location = new Point(75, 100);
            label2.Name = "label2";
            label2.Size = new Size(185, 50);
            label2.TabIndex = 1;
            label2.Text = "ゲストプレイ";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Yu Gothic UI", 26.25F, FontStyle.Regular, GraphicsUnit.Point);
            label3.Location = new Point(500, 50);
            label3.Name = "label3";
            label3.Size = new Size(413, 47);
            label3.TabIndex = 2;
            label3.Text = "プレイ方法を選択してください";
            // 
            // panel3
            // 
            panel3.BackColor = Color.FromArgb(224, 224, 224);
            panel3.Controls.Add(textBox1);
            panel3.Controls.Add(label4);
            panel3.Location = new Point(1150, 650);
            panel3.Name = "panel3";
            panel3.Size = new Size(250, 75);
            panel3.TabIndex = 3;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(75, 25);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(150, 25);
            textBox1.TabIndex = 1;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Yu Gothic UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label4.Location = new Point(0, 25);
            label4.Name = "label4";
            label4.Size = new Size(58, 21);
            label4.TabIndex = 0;
            label4.Text = "コード：";
            // 
            // UserSelectionScren
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.White;
            Controls.Add(panel3);
            Controls.Add(label3);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Name = "UserSelectionScren";
            Size = new Size(1425, 750);
            Load += UserSelectionScren_Load;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel panel1;
        private Panel panel2;
        private Button selectAccountPlay;
        private Label label1;
        private Button selectGustPlay;
        private Label label2;
        private Label label3;
        private Panel panel3;
        private Label label4;
        private TextBox textBox1;
    }
}
