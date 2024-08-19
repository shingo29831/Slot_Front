namespace GameMachine
{
    partial class UserControl1
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
            pictureBox1 = new PictureBox();
            pictureBox2 = new PictureBox();
            panel1 = new Panel();
            textBox3 = new TextBox();
            textBox2 = new TextBox();
            textBox1 = new TextBox();
            label3 = new Label();
            label2 = new Label();
            label1 = new Label();
            panel2 = new Panel();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = Color.FromArgb(255, 128, 128);
            pictureBox1.Location = new Point(0, 0);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(250, 950);
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            pictureBox2.BackColor = Color.FromArgb(255, 128, 128);
            pictureBox2.Location = new Point(1675, 0);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(250, 950);
            pictureBox2.TabIndex = 1;
            pictureBox2.TabStop = false;
            // 
            // panel1
            // 
            panel1.BackColor = SystemColors.ActiveBorder;
            panel1.Controls.Add(textBox3);
            panel1.Controls.Add(textBox2);
            panel1.Controls.Add(textBox1);
            panel1.Controls.Add(label3);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(label1);
            panel1.Location = new Point(250, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(1425, 200);
            panel1.TabIndex = 2;
            // 
            // textBox3
            // 
            textBox3.Font = new Font("Yu Gothic UI", 48F, FontStyle.Regular, GraphicsUnit.Point);
            textBox3.Location = new Point(1100, 50);
            textBox3.Multiline = true;
            textBox3.Name = "textBox3";
            textBox3.Size = new Size(200, 100);
            textBox3.TabIndex = 12;
            textBox3.Text = "000";
            textBox3.TextAlign = HorizontalAlignment.Center;
            // 
            // textBox2
            // 
            textBox2.Font = new Font("Yu Gothic UI", 48F, FontStyle.Regular, GraphicsUnit.Point);
            textBox2.Location = new Point(450, 50);
            textBox2.Multiline = true;
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(175, 100);
            textBox2.TabIndex = 11;
            textBox2.Text = "000";
            textBox2.TextAlign = HorizontalAlignment.Center;
            // 
            // textBox1
            // 
            textBox1.Font = new Font("Yu Gothic UI", 48F, FontStyle.Regular, GraphicsUnit.Point);
            textBox1.Location = new Point(100, 50);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(200, 100);
            textBox1.TabIndex = 10;
            textBox1.Text = "000";
            textBox1.TextAlign = HorizontalAlignment.Center;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("ＭＳ 明朝", 15.75F, FontStyle.Regular, GraphicsUnit.Point);
            label3.Location = new Point(1100, 25);
            label3.Name = "label3";
            label3.Size = new Size(98, 21);
            label3.TabIndex = 9;
            label3.Text = "遊戯回数";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("ＭＳ 明朝", 15.75F, FontStyle.Regular, GraphicsUnit.Point);
            label2.Location = new Point(450, 25);
            label2.Name = "label2";
            label2.Size = new Size(120, 21);
            label2.TabIndex = 8;
            label2.Text = "レギュラー";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("ＭＳ 明朝", 15.75F, FontStyle.Regular, GraphicsUnit.Point);
            label1.Location = new Point(100, 25);
            label1.Name = "label1";
            label1.Size = new Size(76, 21);
            label1.TabIndex = 7;
            label1.Text = "ビック";
            // 
            // panel2
            // 
            panel2.BackColor = SystemColors.ActiveBorder;
            panel2.Location = new Point(0, 950);
            panel2.Name = "panel2";
            panel2.Size = new Size(1920, 130);
            panel2.TabIndex = 3;
            // 
            // UserControl1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(panel2);
            Controls.Add(panel1);
            Controls.Add(pictureBox2);
            Controls.Add(pictureBox1);
            Name = "UserControl1";
            Size = new Size(1920, 1080);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private PictureBox pictureBox1;
        private PictureBox pictureBox2;
        private Panel panel1;
        private Panel panel2;
        private TextBox textBox3;
        private TextBox textBox2;
        private TextBox textBox1;
        private Label label3;
        private Label label2;
        private Label label1;
    }
}
