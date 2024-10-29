using GameMachine.InitialSettingView;
using GameMachine.View;
using System;
using System.Windows.Forms;

namespace GameMachine
{
    public partial  class StartUp : Form
    {
        private static SelectionController userSelectionScreen = new SelectionController(); // ユーザー選択画面のコントローラー
        
        private static AccountLinkingController accountLinkingScreen = new AccountLinkingController(); // アカウントリンク画面のコントローラー



        


        //集計（カウンター）画面
        private static CounterController counterDisplay = new CounterController();        // カウンター表示コントローラー
        private static CounterView counterView = new CounterView(counterDisplay);


        //スロット画面（クレジット画面とカウンターViewも付加）
        private static CreditController creditDisplay = new CreditController();          // クレジット表示コントローラー
        public static CreditView creditView = new CreditView(creditDisplay);
        private static SlotController userGameScreen = new SlotController(creditView,counterView);       // スロットゲーム画面のコントローラー
        public static SlotView slotView = new SlotView(userGameScreen);

        public StartUp()
        {
            InitializeComponent();

            // 起動時にフルスクリーン
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            this.KeyPreview = true;

            // DPI スケーリング対応設定
            this.AutoScaleDimensions = new SizeF(96F, 96F);
            this.AutoScaleMode = AutoScaleMode.Dpi;

            // キーイベントの設定
            this.KeyDown += new KeyEventHandler(StartUp_KeyDown);


            slotView = new SlotView(userGameScreen);
        }

        private void StartUp_Load(object sender, EventArgs e)
        {
            

            //ユーザーコントロール インスタンス
            //counterDisplay = new CounterController();        // カウンター表示画面
            //userSelectionScreen = new SelectionController(); // ユーザー選択画面
            //accountLinkingScreen = new AccountLinkingController(); // アカウントリンク画面
            //creditDisplay = new CreditController();          // クレジット表示画面
            //userGameScreen = new SlotController(creditView);           // スロットゲーム画面



            //creditView = new CreditView(creditDisplay); //ビューインスタンス
            //counterView = new CounterView(counterDisplay);



            // 各ユーザーコントロールの初期サイズと位置を設定
            InitializeControlSettings();

            ///////////////////////////
            // 初期画面表示メソッド  //
            ///////////////////////////
            ShowUserSelectionScreen();

            counterView.SwitchCounterUpdate();
        }

        private void InitializeControlSettings()
        {
            //リサイズの時に調整されるように設定
            SetControlProperties(counterDisplay, new Size(1920, 1080), new Point(0, 0)); // カウンター表示
            SetControlProperties(userSelectionScreen, new Size(1275, 700), new Point(325, 200)); // ユーザー選択
            SetControlProperties(userGameScreen, new Size(1275, 875), new Point(325, 200)); // スロットゲーム
            SetControlProperties(accountLinkingScreen, new Size(1275, 700), new Point(325, 200)); // アカウントリンク
            SetControlProperties(creditDisplay, new Size(1275, 149), new Point(325, 930)); // クレジット表示
        }

        private void SetControlProperties(Control control, Size size, Point location)
        {
            control.Size = size;      // コントロールのサイズを設定
            control.Location = location; // コントロールの位置を設定
            control.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right; // リサイズ対応
            this.Controls.Add(control); // フォームに追加
        }

        private void ShowUserControl(Control controlToShow)
        {
            //表示処理
            foreach (Control control in this.Controls)
            {
                control.Visible = false; // 他のコントロールを非表示にする
            }

            controlToShow.Visible = true; // 対象のコントロールを表示
            controlToShow.BringToFront(); // 対象のコントロールを前面に表示
        }

        public void ShowUserSelectionScreen()
        {
            // counterDisplay と userSelectionScreen を表示する
            counterDisplay.Visible = true;      // カウンター表示を有効化
            userSelectionScreen.Visible = true; // ユーザー選択画面を有効化

            // それぞれ前面に表示
            counterDisplay.BringToFront();
            userSelectionScreen.BringToFront();
        }

        public void ShowUserGameScreen()
        {
            ShowUserControl(userGameScreen); // ゲーム画面を表示
            counterDisplay.Visible = true;      // カウンター表示を有効化
            creditDisplay.Visible = true;    // クレジット表示を有効にする
            creditDisplay.BringToFront();    // クレジット表示を前面に移動

            userGameScreen.SlotViewLoad(slotView); //slotViewを渡す
            slotView.InitialPictureSet(1, 3, 20); // 初期シンボル表示位置

            //たぶん表示
            slotView.LeftBtnChange();
            slotView.CenterBtnChange();
            slotView.RightBtnChange();
        }

        public void ShowAccountLinkingScreen()
        {
            ShowUserControl(accountLinkingScreen); // アカウントリンク画面を表示
            counterDisplay.Visible = true;      // カウンター表示を有効化
            creditDisplay.BringToFront();    // クレジット表示を前面に移動
        }



        private void StartUp_KeyDown(object sender, KeyEventArgs e)
        {
            // Esc キーが押されたら終了確認のダイアログを表示
            if (e.KeyCode == Keys.Escape)
            {
                DialogResult result = MessageBox.Show("アプリケーションを終了しますか？", "ゲーム", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    this.Close(); // アプリケーションを終了
                }
            }
        }
    }
}
