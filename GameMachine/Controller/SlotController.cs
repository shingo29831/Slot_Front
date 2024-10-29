﻿using System;
using System.Drawing;
using System.Windows.Forms;
using System.Timers;
using static Constants;
using GameMachine.View;
using GameMachine.Model;

namespace GameMachine
{
    public partial class SlotController : UserControl
    {
        private SlotView slotView;
        private SlotViewLamp slotViewLamp;

        private CreditView creditView;
        private CounterView counterView;

        public static sbyte stopLeftCount, stopCenterCount, stopRightCount;

        // ランプパターン
        private static int patternCount = 1;

        // 停止ボタンカウントとフラグ
        private static sbyte btnCount = 3;
        private sbyte stopCount = 0;
        private sbyte rotatedcount = 0;
        private static bool acquisition = true;
        private static bool startFlag = false;
        private static bool maxBetFlag = false;
        private static Roles establishedRole = Roles.NONE;

        public PictureBox[] leftReels = new PictureBox[4];
        public PictureBox[] centerReels = new PictureBox[4];
        public PictureBox[] rightReels = new PictureBox[4];
        public PictureBox[] pictureButtons = new PictureBox[9];

        private Control mainForm;

        public SlotController(CreditView creditView, CounterView counterView)
        {
            InitializeComponent();
            InitializeSlotView();
            InitializeSlotViewLamp();

            this.creditView = creditView;
            this.counterView = counterView;

        }

        public void SlotViewLoad(SlotView slotView)
        {
            this.slotView = slotView;
        }

        public void InitializeSlotView()
        {
            leftReels[0] = LpB1;
            leftReels[1] = LpB2;
            leftReels[2] = LpB3;
            leftReels[3] = LpB4;

            centerReels[0] = CpB1;
            centerReels[1] = CpB2;
            centerReels[2] = CpB3;
            centerReels[3] = CpB4;

            rightReels[0] = RpB1;
            rightReels[1] = RpB2;
            rightReels[2] = RpB3;
            rightReels[3] = RpB4;

            pictureButtons[0] = startLever; 
            pictureButtons[1] = LeftStopBtn; 
            pictureButtons[2] = CenterStopBtn; 
            pictureButtons[3] = RightStopBtn;
            pictureButtons[4] = RightStopBtn; 
            pictureButtons[5] = MaxBet;
            pictureButtons[6] = Bet1; 
            pictureButtons[7] = Bet2;
            pictureButtons[8] = Bet3 ;

        }

        private void InitializeSlotViewLamp()
        {
            slotViewLamp = new SlotViewLamp(FlowerLeft, FlowerRight);
        }

        private void UserGameScreen_Load(object sender, EventArgs e)
        {
            mainForm = this.Parent as StartUp;


            
            creditView.ShowCreditDisp();


            LeftStopBtn.Enabled = false;
            CenterStopBtn.Enabled = false;
            RightStopBtn.Enabled = false;
        }

        private void GameReelLoad(Control slotView)
        {

        }

        //停止処理
        private void stopBtns_Click(object sender, EventArgs e)
        {
            if (sender == LeftStopBtn && startFlag == true)
            {
                sbyte reelPosition = slotView.GetReelPosition(Reels.LEFT);
                OnPushedStopBtn(Reels.LEFT, reelPosition); 
            }
            else if (sender == CenterStopBtn && startFlag == true)
            {
                sbyte reelPosition = slotView.GetReelPosition(Reels.CENTER);
                OnPushedStopBtn(Reels.CENTER,reelPosition); 

            }
            else if (sender == RightStopBtn && startFlag == true)
            {
                sbyte reelPosition = slotView.GetReelPosition(Reels.RIGHT);
                OnPushedStopBtn(Reels.RIGHT, reelPosition); 

            }

        }


        private void startLever_Click(object sender, EventArgs e)
        {
            if (maxBetFlag && AnyStopBtnEnabled() == false)
            {
                EnableStopButtons();
                StartReels();
            }
        }

        private void EnableStopButtons()
        {
            LeftStopBtn.Enabled = true;
            CenterStopBtn.Enabled = true;
            RightStopBtn.Enabled = true;
        }

        private void StartReels()
        {
            startFlag = true;
            slotView.Start();
            slotView.ResetChange();
            btnCount = 0;
        }

        //全てのストップボタンが動いているかどうか
        private bool AllStopBtnEnabled()
        {
            if (LeftStopBtn.Enabled && CenterStopBtn.Enabled && RightStopBtn.Enabled)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        //いずれかのストップボタンが動いているかどうか
        private bool AnyStopBtnEnabled()
        {
            if(LeftStopBtn.Enabled || CenterStopBtn.Enabled || RightStopBtn.Enabled)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void startLever_MouseUp(object sender, MouseEventArgs e) => slotView.LeverUp();
        private void startLever_MouseDown(object sender, MouseEventArgs e) => slotView.LeverDown();

        private void MaxBet_Click(object sender, EventArgs e)
        {
            if(maxBetFlag == false && establishedRole != Roles.REPLAY){
                OnPushedMaxBet();
            }
        }

        private void MaxBet_MouseUp(object sender, MouseEventArgs e) => slotView.MaxBetChangeUp();
        private void MaxBet_MouseDown(object sender, MouseEventArgs e) => slotView.MaxBetChangeDown();





        //マックスベットが押された時の処理
        private void OnPushedMaxBet()
        {
            ShowTextBox();
            int hasCoin = Game.GetHasCoin();
            bool inBonus = Game.GetInBonus();
            establishedRole = Game.GetEstablishedRole();
            if (((hasCoin >= 3 && inBonus == false) || (hasCoin >= 2 && inBonus == true) || establishedRole == Roles.REPLAY) && AnyStopBtnEnabled() == false)
            {
                maxBetFlag = true;
                slotView.BetOn(false);
                Game.CalcCoinCollection(); //コイン回収
                Game.SetEstablishedRole(Roles.NONE); //現在の役をなしに設定
                Game.HitRolesLottery(); //役の抽選
                Game.BonusLottery(); //ボーナスの抽選(レア役がでた時のみ)
                Game.ResetReelsMoving(); //全てのリールを動いているフラグにする
                creditView.ShowCreditDisp();
            }

            
        }

        //ストップボタンが押された時の処理
        private void OnPushedStopBtn(Reels selectReel, in sbyte reelPosition)
        {
            switch (selectReel)
            {
                case Reels.LEFT:
                    if(LeftStopBtn.Enabled == false)
                    {
                        //LeftCurrentPositionLabel.Text = slotView.GetCurrentPosition(Reels.LEFT).ToString();
                    }
                    break;
                case Reels.CENTER:
                    if (CenterStopBtn.Enabled)
                    {
                        //CenterCurrentPositionLabel.Text = slotView.GetCurrentPosition(Reels.CENTER).ToString();
                    }
                    break;
                case Reels.RIGHT:
                    if (RightStopBtn.Enabled)
                    {
                        //RightCurrentPositionLabel.Text = slotView.GetCurrentPosition(Reels.RIGHT).ToString();
                    }
                    break;

            }

            

            btnCount++;
            Game.SetNowReelPosition(selectReel, reelPosition); //現在のリールの位置を設定
            stopCount = Game.CalcNextReelPosition(selectReel); //現在のリールの位置を元に計算　こいつの返り値を代入してViewに反映すること);
            Game.SetReelMoving(selectReel, false); //選択したリールを停止
            //三つ目のリールが停止した時
            if (btnCount == 3)
            {
                Game.HitEstablishedRoles(); //達成された役を探索
                Game.CalcCoinReturned(); //達成された役を元にコインを還元
                Game.SwitchingBonus(); //ボーナスの状態を(達成したボーナスに突入・停止・次のボーナスに)移行

                creditView.ShowCreditDisp();


                rotatedcount++;
                slotView.BetOff();
                //追加
                if (rotatedcount == 1)
                {
                    acquisition = false;
                }
                maxBetFlag = false;

                Counter.CountUpCounterData(establishedRole);
                counterView.SwitchCounterUpdate(); //集計の表示を更新
                if(establishedRole == Roles.REPLAY)
                {
                    OnPushedMaxBet();
                }
            }



            switch (selectReel)
            {
                case Reels.LEFT:
                    //LeftCurrentPositionLabel.Text = slotView.GetCurrentPosition(Reels.LEFT).ToString();
                    LeftStopPositionLabel.Text = stopCount.ToString();
                    slotView.StopLeftReel(stopCount);
                    slotView.LeftBtnChange();
                    LeftStopBtn.Enabled = false;
                    break;


                case Reels.CENTER:
                    //CenterCurrentPositionLabel.Text = slotView.GetCurrentPosition(Reels.CENTER).ToString();
                    CenterStopPositionLabel.Text = stopCount.ToString();
                    slotView.StopCenterReel(stopCount);
                    slotView.CenterBtnChange();
                    CenterStopBtn.Enabled = false;
                    break;


                case Reels.RIGHT:
                    //RightCurrentPositionLabel.Text = slotView.GetCurrentPosition(Reels.RIGHT).ToString();
                    RightStopPositionLabel.Text = stopCount.ToString();
                    slotView.StopRightReel(stopCount);
                    slotView.RightBtnChange();
                    RightStopBtn.Enabled = false;
                    break;

            }


        }







        //テストコード
        private void TriggerLampPattern()
        {
            slotViewLamp.StopLampFlash();
            switch (patternCount)
            {
                case 1:
                    slotViewLamp.BothFlowerLamp();
                    break;
                case 2:
                    slotViewLamp.RightFlowerLamp();
                    break;
                case 3:
                    slotViewLamp.LeftFlowerLamp();
                    break;
                case 4:
                    slotViewLamp.StartLampFlashSlow();
                    break;
                case 5:
                    slotViewLamp.StartLampFlashFast();
                    break;
                case 6:
                    slotViewLamp.StartAlternatingLampFlashSlow();
                    break;
            }
        }


        //test
        private void ShowTextBox()
        {
            MessageBox.Show("達成した役" + Game.GetEstablishedRole().ToString()
                    + "\n  LEFFT:" + SymbolChangeToName(Constants.ReelOrder.LEFT_REEL_ORDER[Game.GetNextReelPosition(Reels.LEFT)]) + "," + Game.GetNextReelPosition(Reels.LEFT)
                    + "\n  CENTER:" + SymbolChangeToName(Constants.ReelOrder.CENTER_REEL_ORDER[Game.GetNextReelPosition(Reels.CENTER)]) + "," + Game.GetNextReelPosition(Reels.CENTER)
                    + "\n  RIGHT:" + SymbolChangeToName(Constants.ReelOrder.RIGHT_REEL_ORDER[Game.GetNextReelPosition(Reels.RIGHT)]) + "," + Game.GetNextReelPosition(Reels.RIGHT));
        }

        //test
        private String SymbolChangeToName(Symbols symbol)
        {
            String value = "";
            switch (symbol)
            {
                case Symbols.NONE:
                    value = "NONE";
                    break;
                case Symbols.BELL:
                    value = "BELL";
                    break;
                case Symbols.REPLAY:
                    value = "REPLAY";
                    break;
                case Symbols.WATERMELON:
                    value = "WATERMELON";
                    break;
                case Symbols.CHERRY:
                    value = "CHERRY";
                    break;
                case Symbols.BAR:
                    value = "BAR";
                    break;
                case Symbols.SEVEN:
                    value = "SEVEN";
                    break;
                case Symbols.REACH:
                    value = "REACH";
                    break;
            }
            return value;
        }
    }
}
