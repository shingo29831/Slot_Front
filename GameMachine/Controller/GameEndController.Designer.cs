namespace GameMachine.Controller
{
    partial class GameEndController
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
            EndButton = new Button();
            label1 = new Label();
            Credit = new Label();
            SuspendLayout();
            // 
            // EndButton
            // 
            EndButton.Font = new Font("Yu Gothic UI", 48F, FontStyle.Regular, GraphicsUnit.Point);
            EndButton.Location = new Point(414, 420);
            EndButton.Name = "EndButton";
            EndButton.Size = new Size(459, 124);
            EndButton.TabIndex = 0;
            EndButton.Text = "終了ボタン";
            EndButton.UseVisualStyleBackColor = true;
            EndButton.Click += EndButton_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Yu Gothic UI", 48F, FontStyle.Regular, GraphicsUnit.Point);
            label1.Location = new Point(566, 134);
            label1.Name = "label1";
            label1.Size = new Size(165, 86);
            label1.TabIndex = 1;
            label1.Text = "残高";
            // 
            // Credit
            // 
            Credit.AutoSize = true;
            Credit.Font = new Font("Yu Gothic UI", 48F, FontStyle.Regular, GraphicsUnit.Point);
            Credit.Location = new Point(414, 265);
            Credit.Name = "Credit";
            Credit.Size = new Size(317, 86);
            Credit.TabIndex = 2;
            Credit.Text = "00000000";
            // 
            // GameEndController
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(Credit);
            Controls.Add(label1);
            Controls.Add(EndButton);
            Margin = new Padding(0);
            Name = "GameEndController";
            Size = new Size(1275, 730);
            Load += GameEndController_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button EndButton;
        private Label label1;
        private Label Credit;
    }
}
