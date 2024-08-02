namespace GameMachine
{
    partial class Form1
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
            userControl11 = new UserControl1();
            userControl21 = new UserControl2();
            SuspendLayout();
            // 
            // userControl11
            // 
            userControl11.Location = new Point(0, 0);
            userControl11.Name = "userControl11";
            userControl11.Size = new Size(1920, 1080);
            userControl11.TabIndex = 0;
            // 
            // userControl21
            // 
            userControl21.Location = new Point(250, 200);
            userControl21.Name = "userControl21";
            userControl21.Size = new Size(1425, 750);
            userControl21.TabIndex = 1;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1904, 1041);
            Controls.Add(userControl21);
            Controls.Add(userControl11);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            KeyDown += Form1_KeyDown;
            ResumeLayout(false);
        }

        #endregion

        private UserControl1 userControl11;
        private UserControl2 userControl21;
    }
}