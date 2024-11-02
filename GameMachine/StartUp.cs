using GameMachine.Controller;
using GameMachine.InitialSettingView;
using GameMachine.Model;
using GameMachine.View;
using Microsoft.VisualBasic.Devices;
using System;
using System.Windows.Forms;
using static Constants;
using Networks;

namespace GameMachine
{
    public partial class StartUp : Form
    {
        private static bool inOnline = true;
        public static bool GetOnline { get => inOnline; }

        private static SelectionController userSelectionScreen = new SelectionController(); // ユーザー選択画面のコントローラー

        private static AccountLinkingController accountLinkingScreen = new AccountLinkingController(); // アカウントリンク画面のコントローラー

        private static WaitLinkController waitLinkScreen = new WaitLinkController(); //待機画面のコントローラー

        private static WaitLogoutController waitLogoutController = new WaitLogoutController(); //ログアウト待機画面のコントローラー

        private static ResultController resultScreen = new ResultController();//リザルト画面のコントローラー

        private static GameEndController gameEndScreen = new GameEndController();//ゲーム終了画面

        private static SettingController settingController = new SettingController();   //設定画面のコントローラー

        private static InCreditController inCreditController = new InCreditController(); // ローカル入金画面のコントローラー

        private static SlotSettingController slotSettingController = new SlotSettingController();//台設定変更画面のコントローラー

        //集計（カウンター）画面
        private static CounterController counterDisplay = new CounterController();        // カウンター表示コントローラー
        private static CounterView counterView = new CounterView(counterDisplay);


        //スロット画面（クレジット画面とカウンターViewも付加）
        private static CreditController creditDisplay = new CreditController();          // クレジット表示コントローラー
        public static CreditView creditView = new CreditView(creditDisplay);
        private static SlotController userGameScreen = new SlotController(creditView, counterView);       // スロットゲーム画面のコントローラー
        public static SlotView slotView = new SlotView(userGameScreen);

        static Account_SYS? account_ = null;

        static Network_sys? ns = null;

        private bool exitFlg = false;

        public static Account_SYS Account { get => account_; set => account_ = value; }

        public static Network_sys Network { get => ns; set => ns = value; }


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
            SetSlotExpected();

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
            SetControlProperties(resultScreen, new Size(1275, 730), new Point(325, 200)); // リザルト表示
            SetControlProperties(waitLinkScreen, new Size(1275, 875), new Point(325, 200));//待機画面
            SetControlProperties(waitLogoutController, new Size(1275, 875), new Point(325, 200));//ログアウト待ち画面
            SetControlProperties(gameEndScreen, new Size(1275, 730), new Point(325, 200));//ゲーム終了画面
            SetControlProperties(settingController, new Size(1275, 875), new Point(325, 200));//設定画面
            SetControlProperties(inCreditController, new Size(1275, 875), new Point(325, 200));//入金画面
            SetControlProperties(slotSettingController, new Size(1275, 875), new Point(325, 200));//台設定画面
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

            //ボタン表示切り替え
            slotView.BtnChange(Reels.LEFT);
            slotView.BtnChange(Reels.CENTER);
            slotView.BtnChange(Reels.RIGHT);

          
        }

        public void ShowAccountLinkingScreen()
        {
            ShowUserControl(accountLinkingScreen); // アカウントリンク画面を表示
            counterDisplay.Visible = true;      // カウンター表示を有効化
            creditDisplay.BringToFront();    // クレジット表示を前面に移動
        }


        public void ShowResultScreen()
        {
            ShowUserControl(resultScreen);
            resultScreen.BringToFront();
        }

        public void HideResultScreen()
        {
            if (resultScreen != null)
            {
                resultScreen.Visible = false;
            }
        }

        public void ShowGameEndScreen()
        {
            ShowUserControl(gameEndScreen);
            counterDisplay.Visible = true;      // カウンター表示を有効化
            creditDisplay.Visible = true;    // クレジット表示を有効化
            creditDisplay.BringToFront();    // クレジット表示を前面に移動
            gameEndScreen.Credit.Text = Game.GetHasCoin().ToString();
        }

        public void ShowWaitLinkScreen()
        {
            ShowUserControl(waitLinkScreen);    //待機画面表示
            counterDisplay.Visible = true;      //カウンター画面表示
            waitLinkScreen.BringToFront();      //待機画面を前面に移動
            waitLinkScreen.WaitLink();          //待機処理開始
        }
        public async void ShowWaitLogoutScreen()
        {
            ShowUserControl(waitLogoutController);  //ログアウト待機画面表示
            counterDisplay.Visible = true;          //カウンターが画面を表示(横の背景を表示するため
            waitLogoutController.BringToFront();    //待機画面を前面に移動
            waitLogoutController.ActivateController();
            if (inOnline)
            {
                await waitLogoutController.WaitLogout();
                Game.ResetAll();
                userGameScreen.ResetAll();
                creditView.ResetAll();

                account_ = null;

                exitFlg = false;
                //待機処理開始
            }
            else
            {
                Game.ResetAll();
                userGameScreen.ResetAll();
                creditView.ResetAll();

                account_ = null;

                exitFlg = false;
                await waitLogoutController.EndLocalGame();
            }
           
    }

        public void ShowSettingScreen()
        {
            ShowUserControl(settingController); //設定画面表示
            counterDisplay.Visible = true;      //カウンター画面を表示
            settingController.BringToFront();   //設定画面を前面に移動
        }

        public void ShowInCreditScreen()
        {
            ShowUserControl(inCreditController);    //ローカル入金画面表示
            counterDisplay.Visible = true;          //カウンター画面を表示
            inCreditController.BringToFront();      //ローカル入金画面を前面に移動
            inCreditController.TextBoxFocus();
        }

        public void ShowSlotSettingScreen()
        {
            ShowUserControl(slotSettingController); //台設定画面表示
            counterDisplay.Visible = true;          //カウンター画面表示
            slotSettingController.BringToFront();   //台設定画面を前面に移動
        }


        public void ShowCreditDisp()
        {
            creditView.ShowCreditDisp();
        }


        private void StartUp_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void StartUp_KeyUp(object sender, KeyEventArgs e)
        {
        }

        async private void SetSlotExpected()
        {
            int slotExpected;
            while (!exitFlg)
            {
                if (Network != null)
                {
                    slotExpected = await Networks.Table.get_probability(Network);
                    Model.Setting.SetExpected(Convert.ToByte(slotExpected));
                }
                await Task.Delay(30000);
            }
            this.Close();
        }

        public void SetExitFlg(bool flg)
        {
            exitFlg = flg;
        }


        public static void SetInOnline(bool online)
        {
            inOnline = online;
        }
    }
}
