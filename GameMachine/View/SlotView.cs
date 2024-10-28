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
        private static sbyte leftCount, centerCount, rightCount;
        private readonly PictureBox[] leftReel, centerReel, rightReel, PChange;
        private readonly Dictionary<Symbols, Bitmap> symbolImages;
        private SlotController slotController;
        //定数
        private const int AMOUNT_OF_MOVEMENT = 25, SWITCHING_POINT = 550, INTERVAL = 16;

        public SlotView(PictureBox[] left, PictureBox[] center, PictureBox[] right, PictureBox[] pictureChange)
        {
            reelTimer = new System.Timers.Timer(INTERVAL) { AutoReset = true };
            reelTimer.Elapsed += ReelTimerTick;

            leftReel = left;
            centerReel = center;
            rightReel = right;
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

        public void Start()
        {
            reelTimer.Start();
            leftReelStop = centerReelStop = rightReelStop = false;
        }

        //ストップ処理
        public async Task StopLeftReel(sbyte stopcount)
        {
            // 座標300〜500に存在する要素数を取得
            
            while (true)
            {
                int countInRange = CountElementsInRange(leftReel);
                if (leftReel[0].Top == 25 || leftReel[0].Top == 200 || leftReel[0].Top == 375 || leftReel[0].Top == 550 && (sbyte)leftReel[countInRange].Tag == stopcount)
                {
                    leftReelStop = true;
                    break;
                }
                await Task.Delay(INTERVAL);
            }
        }

        public async Task StopCenterReel(sbyte stopcount)
        {
            // 座標300〜500に存在する要素数を取得
            
            while (true)
            {
                int countInRange = CountElementsInRange(centerReel);
                if (centerReel[0].Top == 25 || centerReel[0].Top == 200 || centerReel[0].Top == 375 || centerReel[0].Top == 550 && (sbyte)centerReel[countInRange].Tag == stopcount)
                {
                    centerReelStop = true;
                    break;
                }
                await Task.Delay(INTERVAL);
            }
        }

        public async Task StopRightReel(sbyte stopcount)
        {
            // 座標300〜500に存在する要素数を取得
            
            while (true)
            {
                int countInRange = CountElementsInRange(rightReel);
                if (rightReel[0].Top == 25 || rightReel[0].Top == 200 || rightReel[0].Top == 375 || rightReel[0].Top == 550 && (sbyte)rightReel[countInRange].Tag == stopcount)
                {
                    rightReelStop = true;
                    break;
                }
                await Task.Delay(INTERVAL);
            }
        }

        private int CountElementsInRange(PictureBox[] reel)
        {
            return reel.Count(item => item.Top >= 350 && item.Top <= 525);
        }

        public sbyte GetCurrentPosition(Reels reelname)
        {
            if (Reels.LEFT == reelname)
            {
                return (sbyte)leftReel[(sbyte)CountElementsInRange(leftReel)].Tag;
            }
            if (Reels.CENTER == reelname)
            {
                return (sbyte)centerReel[(sbyte)CountElementsInRange(centerReel)].Tag;
            }
            if (Reels.RIGHT == reelname)
            {
                return (sbyte)rightReel[(sbyte)CountElementsInRange(rightReel)].Tag;
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
            leftCount = standardOrderLeft;
            centerCount = standardOrderCenter;
            rightCount = standardOrderRight;

            InitialSetReelImages(leftReel, leftOrder, leftCount);
            InitialSetReelImages(centerReel, centerOrder, centerCount);
            InitialSetReelImages(rightReel, rightOrder, rightCount);
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

        private async void ReelTimerTick(object sender, ElapsedEventArgs e)
        {
            if (leftReel[0].InvokeRequired)
            {
                leftReel[0].Invoke(new Action(() =>
                {
                    if (!leftReelStop) MoveReel(leftReel, ref leftCount, leftOrder);
                    if (!centerReelStop) MoveReel(centerReel, ref centerCount, centerOrder);
                    if (!rightReelStop) MoveReel(rightReel, ref rightCount, rightOrder);
                }));
            }
            await Task.Delay(16);
        }

        private void MoveReel(PictureBox[] reels, ref sbyte count, Symbols[] order)
        {
            foreach (var reel in reels)
            {
                reel.Top += AMOUNT_OF_MOVEMENT;
                if (reel.Top > SWITCHING_POINT)
                {
                    UpdateImage(reel, ref count, order);
                    reel.Top = -reel.Height;
                }
            }
        }

        // レバー、ボタン、シンボルの画像管理

        private void UpdateImage(PictureBox reel, ref sbyte count, Symbols[] order)
        {
            reel.Image = symbolImages[order[count]];
            count = (sbyte)((count + 1) % order.Length);
            reel.Tag = count;
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
    }
}
