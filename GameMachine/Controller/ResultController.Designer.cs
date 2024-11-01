namespace GameMachine.Controller
{
    partial class ResultController
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
            components = new System.ComponentModel.Container();
            ResultPictureBox = new PictureBox();
            label1 = new Label();
            label2 = new Label();
            EndResultDisplayTimer = new System.Windows.Forms.Timer(components);
            ((System.ComponentModel.ISupportInitialize)ResultPictureBox).BeginInit();
            SuspendLayout();
            // 
            // ResultPictureBox
            // 
            ResultPictureBox.Image = Properties.Resources.AmusementPark_1;
            ResultPictureBox.Location = new Point(0, 0);
            ResultPictureBox.Margin = new Padding(0);
            ResultPictureBox.Name = "ResultPictureBox";
            ResultPictureBox.Size = new Size(1275, 725);
            ResultPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            ResultPictureBox.TabIndex = 0;
            ResultPictureBox.TabStop = false;
            ResultPictureBox.Click += ResultPictureBox_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Yu Gothic UI Semibold", 72F, FontStyle.Bold, GraphicsUnit.Point);
            label1.ForeColor = Color.Black;
            label1.Location = new Point(715, 224);
            label1.Name = "label1";
            label1.Size = new Size(338, 128);
            label1.TabIndex = 1;
            label1.Text = "TOTAL";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Yu Gothic UI Semibold", 72F, FontStyle.Bold, GraphicsUnit.Point);
            label2.ForeColor = Color.Black;
            label2.Location = new Point(846, 352);
            label2.Name = "label2";
            label2.Size = new Size(372, 128);
            label2.TabIndex = 2;
            label2.Text = "000000";
            // 
            // EndResultDisplayTimer
            // 
            EndResultDisplayTimer.Interval = 1000;
            EndResultDisplayTimer.Tick += EndResultDisplayTimer_Tick;
            // 
            // ResultController
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Transparent;
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(ResultPictureBox);
            ForeColor = SystemColors.ActiveBorder;
            Name = "ResultController";
            Size = new Size(1260, 860);
            Load += ResultController_Load;
            ((System.ComponentModel.ISupportInitialize)ResultPictureBox).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox ResultPictureBox;
        private Label label1;
        private Label label2;
        private System.Windows.Forms.Timer EndResultDisplayTimer;
    }
}
