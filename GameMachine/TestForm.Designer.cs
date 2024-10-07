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
            txtbox1.Location = new Point(38, 9);
            txtbox1.Margin = new Padding(3, 2, 3, 2);
            txtbox1.Name = "txtbox1";
            txtbox1.Size = new Size(255, 23);
            txtbox1.TabIndex = 0;
            // 
            // button1
            // 
            button1.Location = new Point(538, 244);
            button1.Margin = new Padding(3, 2, 3, 2);
            button1.Name = "button1";
            button1.Size = new Size(82, 22);
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
            leftReelTop.Location = new Point(106, 65);
            leftReelTop.Name = "leftReelTop";
            leftReelTop.Size = new Size(105, 38);
            leftReelTop.TabIndex = 2;
            leftReelTop.Text = "NONE";
            leftReelTop.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // centerReelTop
            // 
            centerReelTop.BackColor = Color.White;
            centerReelTop.Location = new Point(268, 65);
            centerReelTop.Name = "centerReelTop";
            centerReelTop.Size = new Size(105, 38);
            centerReelTop.TabIndex = 3;
            centerReelTop.Text = "NONE";
            centerReelTop.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // rightReelTop
            // 
            rightReelTop.BackColor = Color.White;
            rightReelTop.Location = new Point(415, 65);
            rightReelTop.Name = "rightReelTop";
            rightReelTop.Size = new Size(105, 38);
            rightReelTop.TabIndex = 4;
            rightReelTop.Text = "NONE";
            rightReelTop.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // leftReelMid
            // 
            leftReelMid.BackColor = Color.White;
            leftReelMid.Location = new Point(106, 134);
            leftReelMid.Name = "leftReelMid";
            leftReelMid.Size = new Size(105, 38);
            leftReelMid.TabIndex = 5;
            leftReelMid.Text = "NONE";
            leftReelMid.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // centerReelMid
            // 
            centerReelMid.BackColor = Color.White;
            centerReelMid.Location = new Point(268, 134);
            centerReelMid.Name = "centerReelMid";
            centerReelMid.Size = new Size(105, 38);
            centerReelMid.TabIndex = 6;
            centerReelMid.Text = "NONE";
            centerReelMid.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // rightReelMid
            // 
            rightReelMid.BackColor = Color.White;
            rightReelMid.Location = new Point(415, 134);
            rightReelMid.Name = "rightReelMid";
            rightReelMid.Size = new Size(105, 38);
            rightReelMid.TabIndex = 7;
            rightReelMid.Text = "NONE";
            rightReelMid.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // leftReelBot
            // 
            leftReelBot.BackColor = Color.White;
            leftReelBot.Location = new Point(106, 194);
            leftReelBot.Name = "leftReelBot";
            leftReelBot.Size = new Size(105, 38);
            leftReelBot.TabIndex = 8;
            leftReelBot.Text = "NONE";
            leftReelBot.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // centerReelBot
            // 
            centerReelBot.BackColor = Color.White;
            centerReelBot.Location = new Point(268, 194);
            centerReelBot.Name = "centerReelBot";
            centerReelBot.Size = new Size(105, 38);
            centerReelBot.TabIndex = 9;
            centerReelBot.Text = "NONE";
            centerReelBot.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // rightReelBot
            // 
            rightReelBot.BackColor = Color.White;
            rightReelBot.Location = new Point(415, 194);
            rightReelBot.Name = "rightReelBot";
            rightReelBot.Size = new Size(105, 38);
            rightReelBot.TabIndex = 10;
            rightReelBot.Text = "NONE";
            rightReelBot.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblArray
            // 
            lblArray.BackColor = Color.White;
            lblArray.Location = new Point(68, 34);
            lblArray.Name = "lblArray";
            lblArray.Size = new Size(603, 19);
            lblArray.TabIndex = 11;
            lblArray.Text = "NONE";
            // 
            // button2
            // 
            button2.Location = new Point(538, 186);
            button2.Margin = new Padding(3, 2, 3, 2);
            button2.Name = "button2";
            button2.Size = new Size(82, 22);
            button2.TabIndex = 12;
            button2.Text = "button2";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // leftStop
            // 
            leftStop.Font = new Font("UD デジタル 教科書体 NK-B", 7.8F, FontStyle.Bold, GraphicsUnit.Point);
            leftStop.Location = new Point(106, 254);
            leftStop.Margin = new Padding(3, 2, 3, 2);
            leftStop.Name = "leftStop";
            leftStop.Size = new Size(46, 38);
            leftStop.TabIndex = 13;
            leftStop.Text = "STOP";
            leftStop.UseVisualStyleBackColor = true;
            leftStop.Click += leftStop_Click;
            // 
            // button4
            // 
            button4.Location = new Point(248, 254);
            button4.Margin = new Padding(3, 2, 3, 2);
            button4.Name = "button4";
            button4.Size = new Size(0, 0);
            button4.TabIndex = 14;
            button4.Text = "button4";
            button4.UseVisualStyleBackColor = true;
            // 
            // centerStop
            // 
            centerStop.Font = new Font("UD デジタル 教科書体 NK-B", 7.8F, FontStyle.Bold, GraphicsUnit.Point);
            centerStop.Location = new Point(266, 254);
            centerStop.Margin = new Padding(3, 2, 3, 2);
            centerStop.Name = "centerStop";
            centerStop.Size = new Size(46, 38);
            centerStop.TabIndex = 15;
            centerStop.Text = "STOP";
            centerStop.UseVisualStyleBackColor = true;
            centerStop.Click += centerStop_Click;
            // 
            // rightStop
            // 
            rightStop.Font = new Font("UD デジタル 教科書体 NK-B", 7.8F, FontStyle.Bold, GraphicsUnit.Point);
            rightStop.Location = new Point(413, 254);
            rightStop.Margin = new Padding(3, 2, 3, 2);
            rightStop.Name = "rightStop";
            rightStop.Size = new Size(46, 38);
            rightStop.TabIndex = 16;
            rightStop.Text = "STOP";
            rightStop.UseVisualStyleBackColor = true;
            rightStop.Click += rightStop_Click;
            // 
            // TestForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(700, 338);
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
            Margin = new Padding(3, 2, 3, 2);
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