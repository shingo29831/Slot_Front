using System;
using System.Windows.Forms;

namespace GameMachine
{
    public partial class Form1 : Form
    {
        private UserSelectionScren userSelectionScren; // フィールドとして定義
        private UserGameScreen userGameScreen;         // フィールドとして定義
        private CounterDisplay counterDisplay;         // フィールドとして定義

        public Form1()
        {
            InitializeComponent();
            // 起動時フルスクリーン
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;

            // キーイベントをすべてのフォームで受け取る処理
            this.KeyPreview = true;

#nullable disable
            this.KeyDown += new KeyEventHandler(Form1_KeyDown);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // ユーザーコントロールをインスタンス化
            counterDisplay = new CounterDisplay();
            userSelectionScren = new UserSelectionScren();
            userGameScreen = new UserGameScreen();

            // counterDisplay のサイズや位置を設定
            counterDisplay.Size = new Size(1920, 1080);
            counterDisplay.Location = new Point(0, 0);

            // userSelectionScren のサイズや位置を設定
            userSelectionScren.Size = new Size(1425, 750);
            userSelectionScren.Location = new Point(250, 200);

            // userGameScreen のサイズや位置を設定
            userGameScreen.Size = new Size(1425, 750);
            userGameScreen.Location = new Point(250, 200);

            // フォームに追加
            this.Controls.Add(counterDisplay);
            this.Controls.Add(userSelectionScren);

            userSelectionScren.BringToFront(); // 初期表示は userSelectionScren
        }

        public void ShowUserGameScreen()
        {
            // UserSelectionScren を非表示にして UserGameScreen を表示する
            this.Controls.Remove(userSelectionScren);
            this.Controls.Add(userGameScreen);
            userGameScreen.BringToFront();
        }

        public void ShowUserSelectionScren()
        {
            // UserGameScreen を非表示にして UserSelectionScren を表示する
            this.Controls.Remove(userGameScreen);
            this.Controls.Add(userSelectionScren);
            userSelectionScren.BringToFront();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            // アプリケーション終了処理
            if (e.KeyCode == Keys.Escape)
            {
                DialogResult result = MessageBox.Show("アプリケーションを終了しますか？", "ゲーム", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    this.Close();
                }
            }
        }
    }
}
