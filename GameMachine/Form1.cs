using GameMachine.InitialSettingView;
using System;
using System.Windows.Forms;

namespace GameMachine
{
    public partial class Form1 : Form
    {
        private SelectionController userSelectionScren; // フィールドとして定義
        private SlotController userGameScreen;         // フィールドとして定義
        private AccountLinkingController accountLinkingScreen; //定義
        private CounterController counterDisplay;         // フィールドとして定義
        private CreditController creditDisplay;

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
            counterDisplay = new CounterController();
            userSelectionScren = new SelectionController();
            userGameScreen = new SlotController();
            accountLinkingScreen = new AccountLinkingController();
            creditDisplay = new CreditController();

            // counterDisplay のサイズや位置を設定
            counterDisplay.Size = new Size(1920, 1080);
            counterDisplay.Location = new Point(0, 0);

            // userSelectionScren のサイズや位置を設定
            userSelectionScren.Size = new Size(1275, 700);
            userSelectionScren.Location = new Point(325, 200);

            // userGameScreen のサイズや位置を設定
            userGameScreen.Size = new Size(1275, 875);
            userGameScreen.Location = new Point(325, 200);

            // accountLinkingScreen のサイズや位置を設定
            accountLinkingScreen.Size = new Size(1275, 875);
            accountLinkingScreen.Location = new Point(325, 200);

            // creditDisplayn のサイズや位置を設定
            creditDisplay.Size = new Size(1275, 175);
            creditDisplay.Location = new Point(325, 900);

            // フォームに追加
            this.Controls.Add(counterDisplay);
            this.Controls.Add(userSelectionScren);

            userSelectionScren.BringToFront(); // 初期表示は userSelectionScren
        }

        public void ShowUserGameScreen()
        {
            // UserSelectionScren を非表示にして UserGameScreen と　creditDisplayを表示する
            this.Controls.Remove(userSelectionScren);
            this.Controls.Add(userGameScreen);
            this.Controls.Add(creditDisplay);
            userGameScreen.BringToFront();
            creditDisplay.BringToFront();
        }

        public void ShowUserSelectionScren()
        {
            // UserGameScreen を非表示にして UserSelectionScren を表示する
            if (accountLinkingScreen.Visible)
            {
                // userScreen1 が表示されている場合の処理
                this.Controls.Remove(accountLinkingScreen);
                this.Controls.Add(userSelectionScren);
                userSelectionScren.BringToFront();
            }
            else if (userGameScreen.Visible)
            {
                // userScreen2 が表示されている場合の処理
                this.Controls.Remove(userGameScreen);
                this.Controls.Add(userSelectionScren);
                userSelectionScren.BringToFront();
            }
        }

        public void ShowAccountLinkingScreen()
        {
            //UserSelectionScren を非表示にして AccountLinkingScreen を表示する
            this.Controls.Remove(userSelectionScren);
            this.Controls.Add(accountLinkingScreen);
            accountLinkingScreen.BringToFront();
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
