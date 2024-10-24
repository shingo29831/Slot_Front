namespace GameMachine
{
    partial class TestForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            txtbox1 = new TextBox();
            button1 = new Button();
            timer1 = new System.Windows.Forms.Timer(components);
            leftReelTop = new Label();
            centerReelTop = new Label();
            rightReelTop = new Label();
            leftReelMid = new Label();
            centerReelMid = new Label();
            rightReelMid = new Label();
            leftReelBot = new Label();
            centerReelBot = new Label();
            rightReelBot = new Label();
            lblArray = new Label();
            button2 = new Button();
            leftStop = new Button();
            button4 = new Button();
            centerStop = new Button();
            rightStop = new Button();
            SuspendLayout();
            // 
            // txtbox1
            // 
            txtbox1.Location = new Point(43, 12);
            txtbox1.Name = "txtbox1";
            txtbox1.Size = new Size(291, 27);
            txtbox1.TabIndex = 0;
            // 
            // button1
            // 
            button1.Location = new Point(615, 325);
            button1.Name = "button1";
            button1.Size = new Size(94, 29);
            button1.TabIndex = 1;
            button1.Text = "button1";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // timer1
            // 
            timer1.Interval = 35;
            timer1.Tick += timer1_Tick;
            // 
            // leftReelTop
            // 
            leftReelTop.BackColor = Color.White;
            leftReelTop.Location = new Point(121, 87);
            leftReelTop.Name = "leftReelTop";
            leftReelTop.Size = new Size(120, 51);
            leftReelTop.TabIndex = 2;
            leftReelTop.Text = "NONE";
            leftReelTop.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // centerReelTop
            // 
            centerReelTop.BackColor = Color.White;
            centerReelTop.Location = new Point(306, 87);
            centerReelTop.Name = "centerReelTop";
            centerReelTop.Size = new Size(120, 51);
            centerReelTop.TabIndex = 3;
            centerReelTop.Text = "NONE";
            centerReelTop.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // rightReelTop
            // 
            rightReelTop.BackColor = Color.White;
            rightReelTop.Location = new Point(474, 87);
            rightReelTop.Name = "rightReelTop";
            rightReelTop.Size = new Size(120, 51);
            rightReelTop.TabIndex = 4;
            rightReelTop.Text = "NONE";
            rightReelTop.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // leftReelMid
            // 
            leftReelMid.BackColor = Color.White;
            leftReelMid.Location = new Point(121, 179);
            leftReelMid.Name = "leftReelMid";
            leftReelMid.Size = new Size(120, 51);
            leftReelMid.TabIndex = 5;
            leftReelMid.Text = "NONE";
            leftReelMid.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // centerReelMid
            // 
            centerReelMid.BackColor = Color.White;
            centerReelMid.Location = new Point(306, 179);
            centerReelMid.Name = "centerReelMid";
            centerReelMid.Size = new Size(120, 51);
            centerReelMid.TabIndex = 6;
            centerReelMid.Text = "NONE";
            centerReelMid.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // rightReelMid
            // 
            rightReelMid.BackColor = Color.White;
            rightReelMid.Location = new Point(474, 179);
            rightReelMid.Name = "rightReelMid";
            rightReelMid.Size = new Size(120, 51);
            rightReelMid.TabIndex = 7;
            rightReelMid.Text = "NONE";
            rightReelMid.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // leftReelBot
            // 
            leftReelBot.BackColor = Color.White;
            leftReelBot.Location = new Point(121, 259);
            leftReelBot.Name = "leftReelBot";
            leftReelBot.Size = new Size(120, 51);
            leftReelBot.TabIndex = 8;
            leftReelBot.Text = "NONE";
            leftReelBot.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // centerReelBot
            // 
            centerReelBot.BackColor = Color.White;
            centerReelBot.Location = new Point(306, 259);
            centerReelBot.Name = "centerReelBot";
            centerReelBot.Size = new Size(120, 51);
            centerReelBot.TabIndex = 9;
            centerReelBot.Text = "NONE";
            centerReelBot.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // rightReelBot
            // 
            rightReelBot.BackColor = Color.White;
            rightReelBot.Location = new Point(474, 259);
            rightReelBot.Name = "rightReelBot";
            rightReelBot.Size = new Size(120, 51);
            rightReelBot.TabIndex = 10;
            rightReelBot.Text = "NONE";
            rightReelBot.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblArray
            // 
            lblArray.BackColor = Color.White;
            lblArray.Location = new Point(78, 45);
            lblArray.Name = "lblArray";
            lblArray.Size = new Size(689, 25);
            lblArray.TabIndex = 11;
            lblArray.Text = "NONE";
            // 
            // button2
            // 
            button2.Location = new Point(615, 248);
            button2.Name = "button2";
            button2.Size = new Size(94, 29);
            button2.TabIndex = 12;
            button2.Text = "button2";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // leftStop
            // 
            leftStop.Font = new Font("UD デジタル 教科書体 NK-B", 7.8F, FontStyle.Bold, GraphicsUnit.Point);
            leftStop.Location = new Point(121, 339);
            leftStop.Name = "leftStop";
            leftStop.Size = new Size(53, 51);
            leftStop.TabIndex = 13;
            leftStop.Text = "STOP";
            leftStop.UseVisualStyleBackColor = true;
            leftStop.Click += leftStop_Click;
            // 
            // button4
            // 
            button4.Location = new Point(283, 339);
            button4.Name = "button4";
            button4.Size = new Size(0, 0);
            button4.TabIndex = 14;
            button4.Text = "button4";
            button4.UseVisualStyleBackColor = true;
            // 
            // centerStop
            // 
            centerStop.Font = new Font("UD デジタル 教科書体 NK-B", 7.8F, FontStyle.Bold, GraphicsUnit.Point);
            centerStop.Location = new Point(304, 339);
            centerStop.Name = "centerStop";
            centerStop.Size = new Size(53, 51);
            centerStop.TabIndex = 15;
            centerStop.Text = "STOP";
            centerStop.UseVisualStyleBackColor = true;
            centerStop.Click += centerStop_Click;
            // 
            // rightStop
            // 
            rightStop.Font = new Font("UD デジタル 教科書体 NK-B", 7.8F, FontStyle.Bold, GraphicsUnit.Point);
            rightStop.Location = new Point(472, 339);
            rightStop.Name = "rightStop";
            rightStop.Size = new Size(53, 51);
            rightStop.TabIndex = 16;
            rightStop.Text = "STOP";
            rightStop.UseVisualStyleBackColor = true;
            rightStop.Click += rightStop_Click;
            // 
            // TestForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 451);
            Controls.Add(rightStop);
            Controls.Add(centerStop);
            Controls.Add(button4);
            Controls.Add(leftStop);
            Controls.Add(button2);
            Controls.Add(lblArray);
            Controls.Add(rightReelBot);
            Controls.Add(centerReelBot);
            Controls.Add(leftReelBot);
            Controls.Add(rightReelMid);
            Controls.Add(centerReelMid);
            Controls.Add(leftReelMid);
            Controls.Add(rightReelTop);
            Controls.Add(centerReelTop);
            Controls.Add(leftReelTop);
            Controls.Add(button1);
            Controls.Add(txtbox1);
            Name = "TestForm";
            Text = "Form1";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtbox1;
        private Button button1;
        public System.Windows.Forms.Timer timer1;
        private Label leftReelTop;
        private Label centerReelTop;
        private Label rightReelTop;
        private Label leftReelMid;
        private Label centerReelMid;
        private Label rightReelMid;
        private Label leftReelBot;
        private Label centerReelBot;
        private Label rightReelBot;
        private Label lblArray;
        private Button button2;
        private Button leftStop;
        private Button button4;
        private Button centerStop;
        private Button rightStop;
    }
}