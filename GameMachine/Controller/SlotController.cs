using System;
using System.Drawing;
using System.Windows.Forms;
using System.Timers;
using static Constants;
using GameMachine.View;
using GameMachine.Model;
using GameMachine.Controller;
using System.Data;

namespace GameMachine
{
    public partial class SlotController : UserControl
    {
        private SlotView slotView;
        private SlotViewLamp slotViewLamp;

        private CreditView creditView;
        private CounterView counterView;


        //private ResultView resultView;
        private ResultController resultController;

        public static sbyte stopLeftCount, stopCenterCount, stopRightCount;

        // ランプパターン
        private static int patternCount = 1;

        // 停止ボタンカウントとフラグ
        private static sbyte btnCount = 3;
        private static bool startFlag = false;
        private static bool maxBetFlag = false;
        private static bool stopBtnEnabled = false;
        private static bool isDialogShown = false;
        private static bool firstIn = true;
        private static bool nextIf = false;
        private static Roles establishedRole = Roles.NONE;

        public PictureBox[] leftReelContainers = new PictureBox[4];
        public PictureBox[] centerReelContainers = new PictureBox[4];
        public PictureBox[] rightReelContainers = new PictureBox[4];
        public PictureBox[] pictureButtons = new PictureBox[9];

        // スペースボタンの押下カウント
        private int spaceBarPressCount = 0;

        private bool resultEnabled = false;

        private Control mainForm;
        

        public System.Timers.Timer reelTimer = new System.Timers.Timer { Interval = 16, AutoReset = true };

        public SlotController(CreditView creditView, CounterView counterView)
        {
            InitializeComponent();
            InitializeSlotView();
            InitializeSlotViewLamp();

            this.creditView = creditView;
            this.counterView = counterView;
            this.slotViewLamp = new SlotViewLamp(FlowerLeft, FlowerRight);
            this.resultController = new ResultController();


            

            this.KeyDown += SlotController_KeyDown;
            this.PreviewKeyDown += SlotController_PreviewKeyDown;  // フォーカスを設定するためのイベント

            
        }

        // コントロールにフォーカスを設定するためのイベント
        private void SlotController_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            e.IsInputKey = true; // 矢印キーなども認識させる
        }

        // コントロールをアクティブにするメソッド
        public void ActivateController()
        {
            this.Focus();
            creditView.ShowCreditDisp();
        }

        // キーボード入力処理
        static Object lockx = new Object();
        private void SlotController_KeyDown(object sender, KeyEventArgs e)
        {


            Reels rel = 0;
            int spaceflag = 0;
            int slotstart = 0;
            lock (e)
            {

                switch (e.KeyCode)
                {
                    case Keys.Z: // MAXBETボタン
                        slotstart = 1;
                        break;
                    case Keys.X: // レバー
                        slotstart = 2;
                        break;
                    case Keys.V: // LEFTBUTTON
                        if (/*sender == LeftStopBtn &&*/ startFlag == true && LeftStopBtn.Enabled && stopBtnEnabled)
                        {
                            rel = (Reels.LEFT);
                        }
                        break;
                    case Keys.B: // CENTERBUTTON
                        if (/*sender == CenterStopBtn && */ startFlag == true && CenterStopBtn.Enabled && stopBtnEnabled)
                        {
                            rel = (Reels.CENTER);

                        }
                        break;
                    case Keys.N: // RIGHTBUTTON
                        if (/*sender == RightStopBtn && */ startFlag == true && RightStopBtn.Enabled && stopBtnEnabled)
                        {
                            rel = (Reels.RIGHT);

                        }
                        break;
                    case Keys.Space: // スペースバー
                        spaceflag = 1;
                        break;
                }
            }
            if (slotstart == 1)
            {

                MaxBet_Click(sender, e);
                slotView.MaxBetChangeDown();
            }
            else if (slotstart == 2)
            {
                startLever_Click(sender, e);
                slotView.LeverDown();
            }
            if (rel != 0)
            {

                OnPushedStopBtn(rel);

            }
            if (spaceflag != 0)
            {
                HandleSpaceBarPress();
            }

        }

        private void SlotController_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Z: // MAXBETボタン
                    slotView.MaxBetChangeUp();
                    MaxBet_Click(sender, e);
                    break;
                case Keys.X: // レバー
                    slotView.LeverUp();
                    startLever_Click(sender, e);
                    break;
                    break;
                case Keys.Escape: // エスケープキー
                    if (e.KeyCode == Keys.Escape && !isDialogShown)
                    {
                        isDialogShown = true;
                        Thread.Sleep(200);
                        if (MessageBox.Show("ゲーム終了しますか", "ゲーム", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                        {
                            var mainForm = this.Parent as StartUp;
                            if (mainForm != null)
                            {
                                mainForm.ShowGameEndScreen();
                            }
                        }
                        isDialogShown = false;
                    }
                    break;
                case Keys.V: // LEFTBUTTON
                    stopBtns_Click(LeftStopBtn, e);
                    break;
                case Keys.B: // CENTERBUTTON
                    stopBtns_Click(CenterStopBtn, e);
                    break;
                case Keys.N: // RIGHTBUTTON
                    stopBtns_Click(RightStopBtn, e);
                    break;
                case Keys.Space: // スペースバー
                    HandleSpaceBarPress();
                    break;
                    break;
            }
        }
        // スペースキーが押されたときの処理
        private void HandleSpaceBarPress()
        {
            // スペースが押された回数をカウント
            spaceBarPressCount++;

            // 左から順にボタンを押す
            if (spaceBarPressCount == 1)
            {
                stopBtns_Click(LeftStopBtn, null);
            }
            else if (spaceBarPressCount == 2)
            {
                stopBtns_Click(CenterStopBtn, null);
            }
            else if (spaceBarPressCount == 3)
            {
                stopBtns_Click(RightStopBtn, null);
            }
            else
            {
                // 3回押したらリセット
                spaceBarPressCount = 0;
            }
        }

        public void SlotViewLoad(SlotView slotView)
        {
            this.slotView = slotView;
            // コントロールがロードされたらフォーカスを設定
            ActivateController();
        }

        public void InitializeSlotView()
        {
            leftReelContainers[0] = LpB1;
            leftReelContainers[1] = LpB2;
            leftReelContainers[2] = LpB3;
            leftReelContainers[3] = LpB4;

            centerReelContainers[0] = CpB1;
            centerReelContainers[1] = CpB2;
            centerReelContainers[2] = CpB3;
            centerReelContainers[3] = CpB4;

            rightReelContainers[0] = RpB1;
            rightReelContainers[1] = RpB2;
            rightReelContainers[2] = RpB3;
            rightReelContainers[3] = RpB4;

            pictureButtons[0] = startLever;
            pictureButtons[1] = LeftStopBtn;
            pictureButtons[2] = CenterStopBtn;
            pictureButtons[3] = RightStopBtn;
            pictureButtons[4] = RightStopBtn;
            pictureButtons[5] = MaxBet;
            pictureButtons[6] = Bet1;
            pictureButtons[7] = Bet2;
            pictureButtons[8] = Bet3;
        }

        private void InitializeSlotViewLamp()
        {
            slotViewLamp = new SlotViewLamp(FlowerLeft, FlowerRight);
        }

        private void UserGameScreen_Load(object sender, EventArgs e)
        {
            mainForm = this.Parent as StartUp;


            LeftStopBtn.Enabled = false;
            CenterStopBtn.Enabled = false;
            RightStopBtn.Enabled = false;
        }


        //停止処理
        private void stopBtns_Click(object sender, EventArgs e)
        {
            

            if (sender == LeftStopBtn && startFlag == true && LeftStopBtn.Enabled && stopBtnEnabled)
            {
                OnPushedStopBtn(Reels.LEFT);
            }
            else if (sender == CenterStopBtn && startFlag == true && CenterStopBtn.Enabled && stopBtnEnabled)
            {
                OnPushedStopBtn(Reels.CENTER);

            }
            else if (sender == RightStopBtn && startFlag == true && RightStopBtn.Enabled && stopBtnEnabled)
            {
                OnPushedStopBtn(Reels.RIGHT);

            }

        }


        

        private void EnableStopButtons()
        {
            LeftStopBtn.Enabled = true;
            CenterStopBtn.Enabled = true;
            RightStopBtn.Enabled = true;

            stopBtnEnabled = true;
        }




        private void StartReels()
        {
            slotView.MovingReel();
            startFlag = true;
            stopBtnEnabled = true;
            slotView.Start();
            slotView.ResetChange();
            btnCount = 0;
        }



        //いずれかのストップボタンが動いているかどうか
        private bool AnyStopBtnEnabled()
        {
            if (LeftStopBtn.Enabled || CenterStopBtn.Enabled || RightStopBtn.Enabled)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void startLever_Click(object sender, EventArgs e)
        {
            
            if ((maxBetFlag || establishedRole == Roles.REPLAY) && AnyStopBtnEnabled() == false)
            {
                Game.SetEstablishedRole(Roles.NONE); //現在の役をなしに設定
                Game.HitRolesLottery(); //役の抽選
                Game.BonusLottery(); //ボーナスの抽選(レア役がでた時のみ)

                //test
                //Game.hitBonusFlag = true;
                //Game.SelectBonusLottery();


                if (Game.hitBonusFlag)
                {
                    slotViewLamp.StartLampFlashFast();
                }




                Game.ResetReelsMoving(); //全てのリールを動いているフラグにする
                EnableStopButtons();
                StartReels();
            }

            if ((maxBetFlag || establishedRole == Roles.REPLAY) && AnyStopBtnEnabled() == false && Game.hitBonusFlag)
            {

            }
        }

        private void startLever_MouseUp(object sender, MouseEventArgs e) => slotView.LeverUp();
        private void startLever_MouseDown(object sender, MouseEventArgs e) => slotView.LeverDown();

        private void MaxBet_Click(object sender, EventArgs e)
        {
            //if (maxBetFlag == false && establishedRole != Roles.REPLAY && (Game.GetEstablishedRole() == Roles.BIG || Game.GetEstablishedRole() == Roles.REGULAR))
            //{
            //    //Game.SetNowBonus(establishedRole);
            //}
            if (maxBetFlag == false && establishedRole != Roles.REPLAY)
            {
                OnPushedMaxBet();
            }
            if (establishedRole == Roles.REPLAY)
            {
                slotView.MaxBetChangeDown();
            }
            
        }

        private void MaxBet_MouseUp(object sender, MouseEventArgs e)
        {
            if (establishedRole != Roles.REPLAY)
            {
                slotView.MaxBetChangeUp();
            }
        }
        private void MaxBet_MouseDown(object sender, MouseEventArgs e) => slotView.MaxBetChangeDown();




        //マックスベットが押された時の処理
        private void OnPushedMaxBet()
        {
            int hasCoin = Game.GetHasCoin();
            bool inBonus = Game.GetInBonus();
            if (((hasCoin >= 3 && inBonus == false) || (hasCoin >= 2 && inBonus == true) || establishedRole == Roles.REPLAY) && AnyStopBtnEnabled() == false)
            {
                maxBetFlag = true;
                slotView.BetOn(false);
                Game.CalcCoinCollection(); //コイン回収
                nextIf = true;
                creditView.ShowCreditDisp();
                creditView.SetPayOut(0);
            }
            if (nextIf && firstIn && Game.GetInBonus())
            {

                firstIn = false;
                Game.SetPreBonusCoin(Game.GetHasCoin());
            }
            if (nextIf && !Game.GetInBonus())
            {
                Game.SetPreBonusCoin(0);
                firstIn = true;
            }

            nextIf = false;



        }

        public void SetDownMaxBetFlag()
        {
            maxBetFlag = false;
        }



        //ストップボタンが押された時の処理
        private void OnPushedStopBtn(Reels selectReel)
        {
            stopBtnEnabled = false;
            
            sbyte reelPosition = 0;
            switch (selectReel)
            {
                case Reels.LEFT:
                    if (LeftStopBtn.Enabled)
                    {
                        LeftStopBtn.Enabled = false;
                        stopBtnEnabled = false;
                        reelPosition = slotView.GetLotteryPosition(leftReelContainers);
                    }
                    break;
                case Reels.CENTER:
                    if (CenterStopBtn.Enabled)
                    {
                        CenterStopBtn.Enabled = false;
                        stopBtnEnabled = false;
                        reelPosition = slotView.GetLotteryPosition(centerReelContainers);
                    }
                    break;
                case Reels.RIGHT:
                    if (RightStopBtn.Enabled)
                    {
                        RightStopBtn.Enabled = false;
                        stopBtnEnabled = false;
                        reelPosition = slotView.GetLotteryPosition(rightReelContainers);
                    }
                    break;

            }



            btnCount++;
            Game.SetNowReelPosition(selectReel, reelPosition); //現在のリールの位置を設定
            sbyte stopReelPosition = Game.CalcNextReelPosition(selectReel); //現在のリールの位置を元に計算　こいつの返り値を代入してViewに反映すること);
            slotView.SetStopReelPosition(selectReel, stopReelPosition);
            Game.SetReelMoving(selectReel, false); //選択したリールを停止
            //三つ目のリールが停止した時
            
            if (btnCount == 3)
            {
                slotView.MaxBetChangeUp();
                Game.HitEstablishedRoles(); //達成された役を探索
                establishedRole = Game.GetEstablishedRole();
                Game.CalcCoinReturned(); //達成された役を元にコインを還元
                creditView.ShowCreditDisp();
                
                Game.SwitchingBonus(); //ボーナスの状態を(達成したボーナスに突入・停止・次のボーナスに)移行

                

                


                slotView.BetOff();

                Counter.CountUpCounterData(establishedRole);
                counterView.SwitchCounterUpdate(); //集計の表示を更新

                resultController.resultView.ResultPictureSwitching(Game.GetSuggestionImage(Setting.GetExpected()));
                resultController.ResultsDisplay();

                startFlag = false;
            }
            if (btnCount == 3 && establishedRole == Roles.REPLAY)
            {
                OnPushedMaxBet();
                slotView.MaxBetChangeDown();
            }

            if (btnCount == 3 && !Game.GetInBonus())
            {
                slotViewLamp.StopLampFlash();
            }
            if (btnCount == 3 && Game.hitBonusFlag)
            {
                slotViewLamp.StartLampFlashFast();
            }



            //if (btnCount == 3 && Game.hitBonusFlag == false && Game.GetNowBonus() == Roles.BIG && Game.GetLastBonusCount() >= 150)
            //{
            //    slotViewLamp.LampOff();
            //}
            //if (btnCount == 3 && Game.hitBonusFlag == false && Game.GetNowBonus() == Roles.REGULAR && Game.GetLastBonusCount() >= 75)
            //{
            //    slotViewLamp.LampOff();
            //}




            switch (selectReel)
            {
                case Reels.LEFT:
                    slotView.BtnChange(selectReel);
                    LeftStopBtn.Enabled = false;
                    break;


                case Reels.CENTER:
                    slotView.BtnChange(selectReel);
                    CenterStopBtn.Enabled = false;
                    break;


                case Reels.RIGHT:
                    slotView.BtnChange(selectReel);
                    RightStopBtn.Enabled = false;
                    break;

            }


        }


        public void SetStopBtnEnabled()
        {
            stopBtnEnabled = true;
        }


    }
}
