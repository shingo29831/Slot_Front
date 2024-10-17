namespace GameMachine
{
    partial class CounterController
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
            BetweenBonusTbx = new TextBox();
            RegularBonusTxb = new TextBox();
            BigBonusTxb = new TextBox();
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
            pictureBox1.Image = Properties.Resources.image2;
            pictureBox1.Location = new Point(0, 0);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(325, 1100);
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            pictureBox2.BackColor = Color.FromArgb(255, 128, 128);
            pictureBox2.Image = Properties.Resources.image1;
            pictureBox2.Location = new Point(1600, 0);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(325, 1100);
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox2.TabIndex = 1;
            pictureBox2.TabStop = false;
            // 
            // panel1
            // 
            panel1.BackColor = SystemColors.ActiveBorder;
            panel1.Controls.Add(BetweenBonusTbx);
            panel1.Controls.Add(RegularBonusTxb);
            panel1.Controls.Add(BigBonusTxb);
            panel1.Controls.Add(label3);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(label1);
            panel1.Location = new Point(325, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(1275, 200);
            panel1.TabIndex = 2;
            // 
            // BetweenBonusTbx
            // 
            BetweenBonusTbx.Font = new Font("Yu Gothic UI", 48F, FontStyle.Regular, GraphicsUnit.Point);
            BetweenBonusTbx.Location = new Point(1000, 50);
            BetweenBonusTbx.Multiline = true;
            BetweenBonusTbx.Name = "BetweenBonusTbx";
            BetweenBonusTbx.Size = new Size(200, 100);
            BetweenBonusTbx.TabIndex = 12;
            BetweenBonusTbx.Text = "000";
            BetweenBonusTbx.TextAlign = HorizontalAlignment.Center;
            // 
            // RegularBonusTxb
            // 
            RegularBonusTxb.Font = new Font("Yu Gothic UI", 48F, FontStyle.Regular, GraphicsUnit.Point);
            RegularBonusTxb.Location = new Point(450, 50);
            RegularBonusTxb.Multiline = true;
            RegularBonusTxb.Name = "RegularBonusTxb";
            RegularBonusTxb.Size = new Size(175, 100);
            RegularBonusTxb.TabIndex = 11;
            RegularBonusTxb.Text = "000";
            RegularBonusTxb.TextAlign = HorizontalAlignment.Center;
            // 
            // BigBonusTxb
            // 
            BigBonusTxb.Font = new Font("Yu Gothic UI", 48F, FontStyle.Regular, GraphicsUnit.Point);
            BigBonusTxb.Location = new Point(100, 50);
            BigBonusTxb.Multiline = true;
            BigBonusTxb.Name = "BigBonusTxb";
            BigBonusTxb.Size = new Size(200, 100);
            BigBonusTxb.TabIndex = 10;
            BigBonusTxb.Text = "000";
            BigBonusTxb.TextAlign = HorizontalAlignment.Center;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("ＭＳ 明朝", 15.75F, FontStyle.Regular, GraphicsUnit.Point);
            label3.Location = new Point(1000, 25);
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
            panel2.Location = new Point(325, 900);
            panel2.Name = "panel2";
            panel2.Size = new Size(1275, 180);
            panel2.TabIndex = 3;
            // 
            // CounterController
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(panel2);
            Controls.Add(panel1);
            Controls.Add(pictureBox2);
            Controls.Add(pictureBox1);
            Name = "CounterController";
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
        private TextBox BetweenBonusTbx;
        private TextBox RegularBonusTxb;
        private TextBox BigBonusTxb;
        private Label label3;
        private Label label2;
        private Label label1;
    }
}
