namespace GameMachine
{
    partial class UserGameScreen
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
            btnstop1 = new Button();
            btnstop2 = new Button();
            btnstop3 = new Button();
            picBox1 = new PictureBox();
            picBox2 = new PictureBox();
            picBox3 = new PictureBox();
            picBox4 = new PictureBox();
            picBox5 = new PictureBox();
            picBox6 = new PictureBox();
            picBox7 = new PictureBox();
            picBox8 = new PictureBox();
            picBox9 = new PictureBox();
            btnStart = new Button();
            ((System.ComponentModel.ISupportInitialize)picBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picBox2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picBox3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picBox4).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picBox5).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picBox6).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picBox7).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picBox8).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picBox9).BeginInit();
            SuspendLayout();
            // 
            // btnstop1
            // 
            btnstop1.Location = new Point(350, 575);
            btnstop1.Name = "btnstop1";
            btnstop1.Size = new Size(125, 75);
            btnstop1.TabIndex = 0;
            btnstop1.Text = "ストップ";
            btnstop1.UseVisualStyleBackColor = true;
            btnstop1.Click += stopBtns_Click;
            // 
            // btnstop2
            // 
            btnstop2.Location = new Point(625, 575);
            btnstop2.Name = "btnstop2";
            btnstop2.Size = new Size(125, 75);
            btnstop2.TabIndex = 1;
            btnstop2.Text = "ストップ";
            btnstop2.UseVisualStyleBackColor = true;
            btnstop2.Click += stopBtns_Click;
            // 
            // btnstop3
            // 
            btnstop3.Location = new Point(900, 575);
            btnstop3.Name = "btnstop3";
            btnstop3.Size = new Size(125, 75);
            btnstop3.TabIndex = 2;
            btnstop3.Text = "ストップ";
            btnstop3.UseVisualStyleBackColor = true;
            btnstop3.Click += stopBtns_Click;
            // 
            // picBox1
            // 
            picBox1.Location = new Point(325, 25);
            picBox1.Name = "picBox1";
            picBox1.Size = new Size(175, 175);
            picBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            picBox1.TabIndex = 3;
            picBox1.TabStop = false;
            // 
            // picBox2
            // 
            picBox2.Location = new Point(600, 25);
            picBox2.Name = "picBox2";
            picBox2.Size = new Size(175, 175);
            picBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            picBox2.TabIndex = 4;
            picBox2.TabStop = false;
            // 
            // picBox3
            // 
            picBox3.Location = new Point(875, 25);
            picBox3.Name = "picBox3";
            picBox3.Size = new Size(175, 175);
            picBox3.SizeMode = PictureBoxSizeMode.StretchImage;
            picBox3.TabIndex = 5;
            picBox3.TabStop = false;
            // 
            // picBox4
            // 
            picBox4.Location = new Point(325, 200);
            picBox4.Name = "picBox4";
            picBox4.Size = new Size(175, 175);
            picBox4.SizeMode = PictureBoxSizeMode.StretchImage;
            picBox4.TabIndex = 6;
            picBox4.TabStop = false;
            // 
            // picBox5
            // 
            picBox5.Location = new Point(600, 200);
            picBox5.Name = "picBox5";
            picBox5.Size = new Size(175, 175);
            picBox5.SizeMode = PictureBoxSizeMode.StretchImage;
            picBox5.TabIndex = 7;
            picBox5.TabStop = false;
            // 
            // picBox6
            // 
            picBox6.Location = new Point(875, 200);
            picBox6.Name = "picBox6";
            picBox6.Size = new Size(175, 175);
            picBox6.SizeMode = PictureBoxSizeMode.StretchImage;
            picBox6.TabIndex = 8;
            picBox6.TabStop = false;
            // 
            // picBox7
            // 
            picBox7.Location = new Point(325, 375);
            picBox7.Name = "picBox7";
            picBox7.Size = new Size(175, 175);
            picBox7.SizeMode = PictureBoxSizeMode.StretchImage;
            picBox7.TabIndex = 9;
            picBox7.TabStop = false;
            // 
            // picBox8
            // 
            picBox8.Location = new Point(600, 375);
            picBox8.Name = "picBox8";
            picBox8.Size = new Size(175, 175);
            picBox8.SizeMode = PictureBoxSizeMode.StretchImage;
            picBox8.TabIndex = 10;
            picBox8.TabStop = false;
            // 
            // picBox9
            // 
            picBox9.Location = new Point(875, 375);
            picBox9.Name = "picBox9";
            picBox9.Size = new Size(175, 175);
            picBox9.SizeMode = PictureBoxSizeMode.StretchImage;
            picBox9.TabIndex = 11;
            picBox9.TabStop = false;
            // 
            // btnStart
            // 
            btnStart.Location = new Point(1250, 225);
            btnStart.Name = "btnStart";
            btnStart.Size = new Size(75, 275);
            btnStart.TabIndex = 12;
            btnStart.Text = "スタート";
            btnStart.UseVisualStyleBackColor = true;
            btnStart.Click += btnStart_Click;
            // 
            // UserGameScreen
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(btnStart);
            Controls.Add(picBox9);
            Controls.Add(picBox8);
            Controls.Add(picBox7);
            Controls.Add(picBox6);
            Controls.Add(picBox5);
            Controls.Add(picBox4);
            Controls.Add(picBox3);
            Controls.Add(picBox2);
            Controls.Add(picBox1);
            Controls.Add(btnstop3);
            Controls.Add(btnstop2);
            Controls.Add(btnstop1);
            Name = "UserGameScreen";
            Size = new Size(1425, 750);
            Load += UserGameScreen_Load;
            ((System.ComponentModel.ISupportInitialize)picBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)picBox2).EndInit();
            ((System.ComponentModel.ISupportInitialize)picBox3).EndInit();
            ((System.ComponentModel.ISupportInitialize)picBox4).EndInit();
            ((System.ComponentModel.ISupportInitialize)picBox5).EndInit();
            ((System.ComponentModel.ISupportInitialize)picBox6).EndInit();
            ((System.ComponentModel.ISupportInitialize)picBox7).EndInit();
            ((System.ComponentModel.ISupportInitialize)picBox8).EndInit();
            ((System.ComponentModel.ISupportInitialize)picBox9).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Button btnstop1;
        private Button btnstop2;
        private Button btnstop3;
        private PictureBox picBox1;
        private PictureBox picBox2;
        private PictureBox picBox3;
        private PictureBox picBox4;
        private PictureBox picBox5;
        private PictureBox picBox6;
        private PictureBox picBox7;
        private PictureBox picBox8;
        private PictureBox picBox9;
    }
}
