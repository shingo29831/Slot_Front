using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Timers;
using static Constants;
using GameMachine.Model;
using GameMachine.Controller;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace GameMachine
{
    public class SlotView
    {

        private bool leftReelStop, centerReelStop, rightReelStop;
        private Symbols[] leftOrder, centerOrder, rightOrder;
        private static sbyte leftReelPosition, centerReelPosition, rightReelPosition;
        private readonly Dictionary<Symbols, Bitmap> symbolImages;
        private readonly SlotController slotController;
        private readonly ResultController resultController;
        bool previousBonusState = false; // 初期状態をボーナス外と仮定
        //定数
        private static readonly int AMOUNT_OF_MOVEMENT = 25, SWITCHING_POINT = 550, INTERVAL = 16;

        private static sbyte leftStopReelPosition = NONE;
        private static sbyte centerStopReelPosition = NONE;
        private static sbyte rightStopReelPosition = NONE;

        private System.Timers.Timer reelTimer = new System.Timers.Timer { Interval = INTERVAL, AutoReset = true };
        public SlotView(SlotController slotController)
        {
            
            //ReelTimerTick;

            this.slotController = slotController;

            this.resultController = new ResultController(); // インスタンス化を追加

            //this.leftReelContainers = slotController.leftReels;
            //this.centerReelContainers = slotController.centerReels;
            //this.rightReelContainers = slotController.rightReels;
            //this.slotController.pictureButtons = slotController.pictureButtons;

            leftOrder = ReelOrder.LEFT_REEL_ORDER;
            centerOrder = ReelOrder.CENTER_REEL_ORDER;
            rightOrder = ReelOrder.RIGHT_REEL_ORDER;

            symbolImages = new Dictionary<Symbols, Bitmap>
            {
                { Symbols.BELL, new Bitmap(Properties.Resources.bell) },
                { Symbols.REPLAY, new Bitmap(Properties.Resources.REPLAY) },
                { Symbols.WATERMELON, new Bitmap(Properties.Resources.watermelon) },
                { Symbols.CHERRY, new Bitmap(Properties.Resources.cherry) },
                { Symbols.SEVEN, new Bitmap(Properties.Resources.seven) },
                { Symbols.BAR, new Bitmap(Properties.Resources.bar) }
            };

            reelTimer.Elapsed += (sender, args) =>
            {
                ReelTimerTick();
            };
        }


        //リールを動かす
        public void Start()
        {
            reelTimer.Start();
            leftReelStop = centerReelStop = rightReelStop = false;
        }



        public bool AllStopReel()
        {
            if(leftReelStop && centerReelStop && rightReelStop)
            {
                return true;
            }
            return false;
        }

        

        //
        ////初期設定////
        //

        public void InitialPictureSet(sbyte standardOrderLeft, sbyte standardOrderCenter, sbyte standardOrderRight)
        {
            leftReelPosition = standardOrderLeft;
            centerReelPosition = standardOrderCenter;
            rightReelPosition = standardOrderRight;

            InitialSetReelImages(slotController.leftReelContainers, leftOrder, leftReelPosition);
            InitialSetReelImages(slotController.centerReelContainers, centerOrder, centerReelPosition);
            InitialSetReelImages(slotController.rightReelContainers, rightOrder, rightReelPosition);
        }

        //画像初期表示
        private void InitialSetReelImages(PictureBox[] reel, Symbols[] order, int startIndex)
        {
            startIndex--;
            for (int i = 0; i < reel.Length; i++)
            {
                reel[i].Image = symbolImages[order[startIndex]];
                reel[i].Tag = (sbyte)startIndex;
                startIndex = (startIndex + 1) % order.Length;
            }
        }

        //
        ////初期設定ここまで////
        //

        //定期動作させる処理
        private void ReelTimerTick()
        {
            slotController.Invoke((MethodInvoker)(() => UpdateReel()));
           
        }

        //リールを動いている判定にする
        public void MovingReel()
        {
            leftReelStop = false;
            centerReelStop = false;
            rightReelStop = false;

            leftStopReelPosition = NONE;
            centerStopReelPosition = NONE;
            rightStopReelPosition = NONE;
        }

        int cnt = 0;
        public void UpdateReel()
        {
            if (!leftReelStop | !centerReelStop | !rightReelStop)
            {
                if (!leftReelStop) MoveReel(Reels.LEFT, ref leftReelPosition,slotController.leftReelContainers);
                if (!centerReelStop) MoveReel(Reels.CENTER, ref centerReelPosition, slotController.centerReelContainers);
                if (!rightReelStop) MoveReel(Reels.RIGHT, ref rightReelPosition, slotController.rightReelContainers);
            }
            else
            {
                reelTimer.Stop();
                slotController.SetDownMaxBetFlag();
            }
            stopReels();
            
        }

        private void stopReels()
        {
            foreach (PictureBox symbol in slotController.leftReelContainers)
            {

                if ((sbyte)symbol.Tag == leftStopReelPosition && symbol.Top == 375 && !leftReelStop)
                {
                    slotController.SetStopBtnEnabled();
                    //MessageBox.Show("leftReelStop" + leftReelStop.ToString());
                    leftReelStop = true;

                }
            }

            foreach (PictureBox symbol in slotController.centerReelContainers)
            {
                if ((sbyte)symbol.Tag == centerStopReelPosition && symbol.Top == 375 && !centerReelStop)
                {
                    slotController.SetStopBtnEnabled();
                    //MessageBox.Show("centerReelStop" + centerReelStop.ToString());
                    centerReelStop = true;
                }
            }

            foreach (PictureBox symbol in slotController.rightReelContainers)
            {
                if ((sbyte)symbol.Tag == rightStopReelPosition && symbol.Top == 375 && !rightReelStop)
                {
                    slotController.SetStopBtnEnabled();
                    //MessageBox.Show("rightReelStop" + rightReelStop.ToString());
                    rightReelStop = true;
                }
            }
        }



        //画像遷移
        private void MoveReel(Reels selectReel, ref sbyte reelPosition ,PictureBox[] symbolContainers) { 
            //var symbolContainers = leftReelContainers;
            Symbols[] order = new Symbols[21];
            bool stopFlag = false;


            switch (selectReel)
            {
                case Reels.LEFT:
                    symbolContainers = slotController. leftReelContainers;
                    order = leftOrder;
                    break;
                case Reels.CENTER:
                    symbolContainers = slotController. centerReelContainers;
                    order = centerOrder;
                    break;
                case Reels.RIGHT:
                    symbolContainers = slotController.rightReelContainers;
                    order = rightOrder;
                    break;
            }
            foreach (var symbolContainer in symbolContainers)
            {
                if (symbolContainer.Tag == (object)GetStopReelPosition(selectReel) && symbolContainer.Top == 375)
                {
                    return;
                }
                symbolContainer.Top += AMOUNT_OF_MOVEMENT; //シンボルの画像位置を移動
                if (symbolContainer.Top > SWITCHING_POINT)  //画面範囲外に到達した時
                {
                    UpdateImage(symbolContainer, ref reelPosition, order); //次のシンボルの画像に代入、またTagに要素番号を代入
                    symbolContainer.Top = -symbolContainer.Height; //textBoxの位置を上に移動
                }
            }

            
        }



        //シンボルの画像切り替え

        private void UpdateImage(PictureBox symbolContainer, ref sbyte reelPosition, Symbols[] order)
        {
            symbolContainer.Image = symbolImages[order[reelPosition]]; //画像挿入
            symbolContainer.Tag = reelPosition; //リールの位置をタグ付け
            reelPosition = (sbyte)((reelPosition + 1) % order.Length); //次に代入するリールの位置に更新
        }


        //ボタン表示切り替え
        public void BtnChange(Reels selectReel)
        {
            switch (selectReel)
            {
                case Reels.LEFT:
                    LeftBtnChange();
                    break;
                case Reels.CENTER:
                    CenterBtnChange();
                    break;
                case Reels.RIGHT:
                    RightBtnChange();
                    break;
            }
        }


        public void LeverUp() => slotController.pictureButtons[0].Image = new Bitmap(Properties.Resources.LeverOFF);
        public void LeverDown() => slotController.pictureButtons[0].Image = new Bitmap(Properties.Resources.LeverON);
        public void LeftBtnChange() => slotController.pictureButtons[1].Image = new Bitmap(Properties.Resources.LeftButtonON);
        public void CenterBtnChange() => slotController.pictureButtons[2].Image = new Bitmap(Properties.Resources.CenterButtonON);
        public void RightBtnChange() => slotController.pictureButtons[3].Image = new Bitmap(Properties.Resources.RightButtonON);
        public void MaxBetChangeUp() => slotController.pictureButtons[5].Image = new Bitmap(Properties.Resources.MAXBETOFF);
        public void MaxBetChangeDown() => slotController.pictureButtons[5].Image = new Bitmap(Properties.Resources.MAXBETON);

        public void BetOn(bool isBonus)
        {
            slotController.pictureButtons[6].Image = new Bitmap(Properties.Resources.OneON);
            slotController.pictureButtons[7].Image = new Bitmap(Properties.Resources.TwoON);
            slotController.pictureButtons[8].Image = isBonus ? null : new Bitmap(Properties.Resources.ThreeON);
        }

        public void BetOff()
        {
            slotController.pictureButtons[6].Image = new Bitmap(Properties.Resources.OneOFF);
            slotController.pictureButtons[7].Image = new Bitmap(Properties.Resources.TwoOFF);
            slotController.pictureButtons[8].Image = new Bitmap(Properties.Resources.ThreeOFF);
        }

        public void ResetChange()
        {
            slotController.pictureButtons[1].Image = new Bitmap(Properties.Resources.LeftButtonOFF);
            slotController.pictureButtons[2].Image = new Bitmap(Properties.Resources.CenterButtonOFF);
            slotController.pictureButtons[3].Image = new Bitmap(Properties.Resources.RightButtonOFF);
        }



        //停止位置を代入された値を取得する
        private sbyte GetStopReelPosition(Reels selectReel)
        {
            switch (selectReel)
            {
                case Reels.LEFT:
                    return leftStopReelPosition;
                case Reels.CENTER:
                    return centerStopReelPosition;
                case Reels.RIGHT:
                    return rightStopReelPosition;
            }
            return 0;
        }

        //現在のリールの下段の位置を探索する
        public sbyte GetBottomReelPosition(Reels selectReel)
        {
            PictureBox[] symbolContainers = new PictureBox[4];
            switch (selectReel)
            {
                case Reels.LEFT:
                    symbolContainers = slotController.leftReelContainers;
                    break;
                case Reels.CENTER:
                    symbolContainers = slotController.centerReelContainers;
                    break;
                case Reels.RIGHT:
                    symbolContainers= slotController.rightReelContainers;
                    break;
            }
            for(int i = 0; i < symbolContainers.Length; i++)
            {
                if (symbolContainers[i].Top >= 200 && symbolContainers[i].Top < 375)
                {
                    return (sbyte)symbolContainers[i].Tag;
                }
            }
            return 0;
        }




        public void SetStopReelPosition(Reels selectReel, sbyte reelPosition)
        {
            switch (selectReel)
            {
                case Reels.LEFT:
                    leftStopReelPosition = reelPosition;
                    break;
                case Reels.CENTER:
                    centerStopReelPosition = reelPosition;
                    break;
                case Reels.RIGHT:
                    rightStopReelPosition = reelPosition;
                    break;
            }
        }

        public void Result()
        {
            resultController.ResultsDisplay();
            /*bool currentBonusState = Game.GetInBonus();

            // ボーナスが終了したときのみリザルト画面を表示
            if (previousBonusState && !currentBonusState)
            {
                resultController.ResultsDisplay();
            }

            previousBonusState = currentBonusState; //状態を更新*/
        }

    }
}
