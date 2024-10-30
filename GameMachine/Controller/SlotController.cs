using System;
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
        private static bool startFlag = false;
        private static bool maxBetFlag = false;
        private static bool stopBtnEnabled = false;
        private static Roles establishedRole = Roles.NONE;

        public PictureBox[] leftReelContainers = new PictureBox[4];
        public PictureBox[] centerReelContainers = new PictureBox[4];
        public PictureBox[] rightReelContainers = new PictureBox[4];
        public PictureBox[] pictureButtons = new PictureBox[9];

        private Control mainForm;

        public System.Timers.Timer reelTimer = new System.Timers.Timer { Interval = 16, AutoReset = true };

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
            pictureButtons[8] = Bet3 ;


            //reelTimer.Elapsed += async (sender, args) =>
            //{
            //    await ReelTimerTick();
            //};
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


        private void startLever_Click(object sender, EventArgs e)
        {
            if ((maxBetFlag || establishedRole == Roles.REPLAY) && AnyStopBtnEnabled() == false)
            {
                Game.SetEstablishedRole(Roles.NONE); //現在の役をなしに設定
                Game.HitRolesLottery(); //役の抽選
                Game.BonusLottery(); //ボーナスの抽選(レア役がでた時のみ)
                Game.ResetReelsMoving(); //全てのリールを動いているフラグにする
                EnableStopButtons();
                StartReels();
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

        public void Stop()
        {

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
            //debag();
            leelTags();
            if (maxBetFlag == false && establishedRole != Roles.REPLAY){
                OnPushedMaxBet();
            }
            if(establishedRole == Roles.REPLAY)
            {
                slotView.MaxBetChangeDown();
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
            if (((hasCoin >= 3 && inBonus == false) || (hasCoin >= 2 && inBonus == true) || establishedRole == Roles.REPLAY) && AnyStopBtnEnabled() == false)
            {
                maxBetFlag = true;
                slotView.BetOn(false);
                Game.CalcCoinCollection(); //コイン回収
                //Game.SetEstablishedRole(Roles.NONE); //現在の役をなしに設定
                //Game.HitRolesLottery(); //役の抽選
                //Game.BonusLottery(); //ボーナスの抽選(レア役がでた時のみ)
                //Game.ResetReelsMoving(); //全てのリールを動いているフラグにする
                creditView.ShowCreditDisp();
            }

            
        }

        public void SetDownMaxBetFlag()
        {
            maxBetFlag = false;
        }



        //ストップボタンが押された時の処理
        private void OnPushedStopBtn(Reels selectReel)
        {
            stopBtnEnabled = false;
            switch (selectReel)
            {
                case Reels.LEFT:
                    if(LeftStopBtn.Enabled)
                    {
                        LeftStopBtn.Enabled = false;
                        stopBtnEnabled = false;
                        //LeftCurrentPositionLabel.Text = slotView.GetCurrentPosition(Reels.LEFT).ToString();
                    }
                    break;
                case Reels.CENTER:
                    if (CenterStopBtn.Enabled)
                    {
                        CenterStopBtn.Enabled = false;
                        stopBtnEnabled = false;
                        //CenterCurrentPositionLabel.Text = slotView.GetCurrentPosition(Reels.CENTER).ToString();
                    }
                    break;
                case Reels.RIGHT:
                    if (RightStopBtn.Enabled)
                    {
                        RightStopBtn.Enabled = false;
                        stopBtnEnabled = false;
                        //RightCurrentPositionLabel.Text = slotView.GetCurrentPosition(Reels.RIGHT).ToString();
                    }
                    break;

            }

            

            btnCount++;
            sbyte reelPosition = slotView.GetBottomReelPosition(selectReel);
           //MessageBox.Show("reelposition"+ slotView.GetBottomReelPosition(selectReel).ToString());
            Game.SetNowReelPosition(selectReel, reelPosition); //現在のリールの位置を設定
            sbyte stopReelPosition = Game.CalcNextReelPosition(selectReel); //現在のリールの位置を元に計算　こいつの返り値を代入してViewに反映すること);
            slotView.SetStopReelPosition(selectReel,stopReelPosition);
            Game.SetReelMoving(selectReel, false); //選択したリールを停止
            //三つ目のリールが停止した時
            if (btnCount == 3)
            {
                slotView.MaxBetChangeUp();
                Game.HitEstablishedRoles(); //達成された役を探索
                establishedRole = Game.GetEstablishedRole();
                Game.CalcCoinReturned(); //達成された役を元にコインを還元
                Game.SwitchingBonus(); //ボーナスの状態を(達成したボーナスに突入・停止・次のボーナスに)移行

                creditView.ShowCreditDisp();


                slotView.BetOff();

                Counter.CountUpCounterData(establishedRole);
                counterView.SwitchCounterUpdate(); //集計の表示を更新
                if(establishedRole == Roles.REPLAY)
                {
                    OnPushedMaxBet();
                    slotView.MaxBetChangeDown();
                }
            }



            switch (selectReel)
            {
                case Reels.LEFT:
                    LeftStopPositionLabel.Text = stopReelPosition.ToString();
                    //slotView.SetStopReelPosition(selectReel, stopReelPosition);
                    slotView.BtnChange(selectReel);
                    LeftStopBtn.Enabled = false;
                    break;


                case Reels.CENTER:
                    CenterStopPositionLabel.Text = stopReelPosition.ToString();
                    //slotView.SetStopReelPosition(selectReel, stopReelPosition);
                    slotView.BtnChange(selectReel);
                    CenterStopBtn.Enabled = false;
                    break;


                case Reels.RIGHT:
                    RightStopPositionLabel.Text = stopReelPosition.ToString();
                    //slotView.SetStopReelPosition(selectReel, stopReelPosition);
                    slotView.BtnChange(selectReel);
                    RightStopBtn.Enabled = false;
                    break;

            }


        }


        public void SetStopBtnEnabled()
        {
            stopBtnEnabled = true;
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
           //MessageBox.Show("達成した役" + Game.GetEstablishedRole().ToString()
                    //+ "\n  LEFFT:" + SymbolChangeToName(Constants.ReelOrder.LEFT_REEL_ORDER[Game.GetNextReelPosition(Reels.LEFT)]) + "," + Game.GetNextReelPosition(Reels.LEFT)
                    //+ "\n  CENTER:" + SymbolChangeToName(Constants.ReelOrder.CENTER_REEL_ORDER[Game.GetNextReelPosition(Reels.CENTER)]) + "," + Game.GetNextReelPosition(Reels.CENTER)
                    //+ "\n  RIGHT:" + SymbolChangeToName(Constants.ReelOrder.RIGHT_REEL_ORDER[Game.GetNextReelPosition(Reels.RIGHT)]) + "," + Game.GetNextReelPosition(Reels.RIGHT));
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

        private void leelTags()
        {
            String valu = "";
            foreach(PictureBox container in leftReelContainers)
            {
                 valu += container.Name + ":" + container.Top.ToString() + " , ";
            }
            foreach (PictureBox container in centerReelContainers)
            {
                valu += container.Name + ":" + container.Tag.ToString() + " , ";
            }
            foreach (PictureBox container in rightReelContainers)
            {
                valu += container.Name + ":" + container.Tag.ToString() + " , ";
            }
            MessageBox.Show(valu);
        }


        public void debag()
        {
            MessageBox.Show(
                slotView.GetReelCorrect(Reels.LEFT).ToString() + slotView.GetReelCorrect(Reels.CENTER).ToString() + slotView.GetReelCorrect(Reels.RIGHT).ToString() +
            slotView.GetIsCorrect(leftReelContainers[0]).ToString() + //; // =slotView.GetIsCorrect( leftReelContainers[0];
            slotView.GetIsCorrect(leftReelContainers[1]).ToString() + //; // =slotView.GetIsCorrect( leftReelContainers[1];
            slotView.GetIsCorrect(leftReelContainers[2]).ToString() + //; // =slotView.GetIsCorrect( leftReelContainers[2];
            slotView.GetIsCorrect(leftReelContainers[3]).ToString() + //; // =slotView.GetIsCorrect( leftReelContainers[3];


            slotView.GetIsCorrect(centerReelContainers[0]).ToString() + // ; // =slotView.GetIsCorrect( centerReelContainers[0];
            slotView.GetIsCorrect(centerReelContainers[1]).ToString() + // ; // =slotView.GetIsCorrect( centerReelContainers[1];
            slotView.GetIsCorrect(centerReelContainers[2]).ToString() + // ; // =slotView.GetIsCorrect( centerReelContainers[2];
            slotView.GetIsCorrect(centerReelContainers[3]).ToString() + // ; // =slotView.GetIsCorrect( centerReelContainers[3];

            slotView.GetIsCorrect(rightReelContainers[0]).ToString() + // ; // =slotView.GetIsCorrect( rightReelContainers[0];
            slotView.GetIsCorrect(rightReelContainers[1]).ToString() + // ; // =slotView.GetIsCorrect( rightReelContainers[1];
            slotView.GetIsCorrect(rightReelContainers[2]).ToString() + // ; // =slotView.GetIsCorrect( rightReelContainers[2];
            slotView.GetIsCorrect(rightReelContainers[3]).ToString()); // ; // =slotView.GetIsCorrect( rightReelContainers[3];


        }
    }
}
