using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Timers;
using static Constants;
using GameMachine.Model;

namespace GameMachine
{
    public class SlotView
    {
        private readonly System.Timers.Timer reelTimer;
        private bool leftReelStop, centerReelStop, rightReelStop;
        private static Symbols[] leftOrder, centerOrder, rightOrder;
        private static sbyte leftReelPosition, centerReelPosition, rightReelPosition;
        private  PictureBox[] leftReelContainers, centerReelContainers, rightReelContainers, PChange;
        private readonly Dictionary<Symbols, Bitmap> symbolImages;
        private SlotController slotController;
        //定数
        private readonly int AMOUNT_OF_MOVEMENT = 25, SWITCHING_POINT = 550, INTERVAL = 16;

        private static sbyte leftStopReelPosition = 0;
        private static sbyte centerStopReelPosition = 0;
        private static sbyte rightStopReelPosition = 0;

        public SlotView(PictureBox[] left, PictureBox[] center, PictureBox[] right, PictureBox[] pictureChange)
        {
            reelTimer = new System.Timers.Timer(INTERVAL) { AutoReset = true };
            reelTimer.Elapsed += ReelTimerTick;

            leftReelContainers = left;
            centerReelContainers = center;
            rightReelContainers = right;
            PChange = pictureChange;

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
        }

        //リールを動かす
        public void Start()
        {
            reelTimer.Start();
            leftReelStop = centerReelStop = rightReelStop = false;
        }

        //ストップ処理
        public void StopLeftReel(sbyte stopreelPosition)
        {
            SetStopReelPosition(Reels.LEFT, stopreelPosition);
            
        }

        public void StopCenterReel(sbyte stopreelPosition)
        {
            SetStopReelPosition(Reels.CENTER,stopreelPosition);
            
        }

        //async
        public void StopRightReel(sbyte stopreelPosition)
        {
            // 座標300〜500に存在する要素数を取得

            SetStopReelPosition(Reels.RIGHT, stopreelPosition);
            
        }

        private int ReelPositionElementsInRange(PictureBox[] reel)
        {
            return reel.Count(item => item.Top >= 350 && item.Top <= 525);
        }

        public sbyte GetCurrentPosition(Reels reelname)
        {
            if (Reels.LEFT == reelname)
            {
                return (sbyte)leftReelContainers[(sbyte)ReelPositionElementsInRange(leftReelContainers)].Tag;
            }
            if (Reels.CENTER == reelname)
            {
                return (sbyte)centerReelContainers[(sbyte)ReelPositionElementsInRange(centerReelContainers)].Tag;
            }
            if (Reels.RIGHT == reelname)
            {
                return (sbyte)rightReelContainers[(sbyte)ReelPositionElementsInRange(rightReelContainers)].Tag;
            }
            else
            {
                return 0;
            }
        }

        //
        ////初期設定////
        //

        public void InitialPictureSet(sbyte standardOrderLeft, sbyte standardOrderCenter, sbyte standardOrderRight)
        {
            leftReelPosition = standardOrderLeft;
            centerReelPosition = standardOrderCenter;
            rightReelPosition = standardOrderRight;

            InitialSetReelImages(leftReelContainers, leftOrder, leftReelPosition);
            InitialSetReelImages(centerReelContainers, centerOrder, centerReelPosition);
            InitialSetReelImages(rightReelContainers, rightOrder, rightReelPosition);
        }

        //画像切り替え
        private void InitialSetReelImages(PictureBox[] reel, Symbols[] order, int startIndex)
        {
            startIndex--;
            for (int i = 0; i < reel.Length; i++)
            {
                reel[i].Image = symbolImages[order[startIndex]];
                startIndex = (startIndex + 1) % order.Length;
            }
        }

        //
        ////初期設定ここまで////
        //

        //async
        private void ReelTimerTick(object sender, ElapsedEventArgs e)
        {
            if (!leftReelStop) MoveReel(Reels.LEFT, ref leftReelPosition);
            if (!centerReelStop) MoveReel(Reels.CENTER, ref centerReelPosition);
            if (!rightReelStop) MoveReel(Reels.RIGHT, ref rightReelPosition);

            
        }


        //画像の遷移
        private void MoveReel(Reels selectReel, ref sbyte reelPosition)
        {
            PictureBox[] symbolContainers = leftReelContainers;
            Symbols[] order = new Symbols[21];
            bool stopFlag = false;
            

            switch (selectReel)
            {
                case Reels.LEFT:
                    symbolContainers = leftReelContainers;
                    order = leftOrder;
                    break;
                case Reels.CENTER:
                    symbolContainers = centerReelContainers;
                    order = centerOrder;
                    break;
                case Reels.RIGHT:
                    symbolContainers = rightReelContainers;
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

                if (symbolContainer.Top > SWITCHING_POINT)
                {
                    UpdateImage(symbolContainer, ref reelPosition, order); //次のシンボルの画像に代入またTagに要素番号を代入
                    symbolContainer.Top = -symbolContainer.Height; //textBoxの位置を上に移動
                }
            }
            
            switch (selectReel)
            {
                case Reels.LEFT:
                    leftReelStop = true;
                    break;
                case Reels.CENTER:
                    centerReelStop = true;
                    break;
                case Reels.RIGHT:
                    rightReelStop = true;
                    break;
            }
        }

        // レバー、ボタン、シンボルの画像管理

        private void UpdateImage(PictureBox symbolContainer, ref sbyte reelPosition, Symbols[] order)
        {
            symbolContainer.Image = symbolImages[order[reelPosition]];
            reelPosition = (sbyte)((reelPosition + 1) % order.Length);
            symbolContainer.Tag = reelPosition;
        }

        public void LeverUp() => PChange[0].Image = new Bitmap(Properties.Resources.LeverOFF);
        public void LeverDown() => PChange[0].Image = new Bitmap(Properties.Resources.LeverON);
        public void LeftBtnChange() => PChange[1].Image = new Bitmap(Properties.Resources.LeftButtonON);
        public void CenterBtnChange() => PChange[2].Image = new Bitmap(Properties.Resources.CenterButtonON);
        public void RightBtnChange() => PChange[3].Image = new Bitmap(Properties.Resources.RightButtonON);
        public void MaxBetChangeUp() => PChange[5].Image = new Bitmap(Properties.Resources.MAXBETOFF);
        public void MaxBetChangeDown() => PChange[5].Image = new Bitmap(Properties.Resources.MAXBETON);

        public void BetOn(bool isBonus)
        {
            PChange[6].Image = new Bitmap(Properties.Resources.OneON);
            PChange[7].Image = new Bitmap(Properties.Resources.TwoON);
            PChange[8].Image = isBonus ? null : new Bitmap(Properties.Resources.ThreeON);
        }

        public void BetOff()
        {
            PChange[6].Image = new Bitmap(Properties.Resources.OneOFF);
            PChange[7].Image = new Bitmap(Properties.Resources.TwoOFF);
            PChange[8].Image = new Bitmap(Properties.Resources.ThreeOFF);
        }

        public void ResetChange()
        {
            PChange[1].Image = new Bitmap(Properties.Resources.LeftButtonOFF);
            PChange[2].Image = new Bitmap(Properties.Resources.CenterButtonOFF);
            PChange[3].Image = new Bitmap(Properties.Resources.RightButtonOFF);
        }


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

        private void SetStopReelPosition(Reels selectReel, sbyte reelPosition)
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
        
        public sbyte GetReelPosition(Reels selectReel)
        {
            PictureBox[] symbolContainers = new PictureBox[4];
            switch (selectReel)
            {
                case Reels.LEFT:
                    symbolContainers = leftReelContainers;
                 
                    break;
                case Reels.CENTER:
                    symbolContainers = centerReelContainers;
                  
                    break;
                case Reels.RIGHT:
                    symbolContainers = rightReelContainers;
                   
                    break;
            }
            foreach (var symbolContainer in symbolContainers)
            {

                if (symbolContainer.Top <= 200 && symbolContainer.Top > 375 )
                {
                    return (sbyte)symbolContainer.Tag;
                }
            }
            return 0;
        }
    }
}
