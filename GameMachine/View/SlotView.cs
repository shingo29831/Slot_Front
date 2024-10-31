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
using GameMachine.View;

namespace GameMachine
{
    public class SlotView
    {

        private bool leftReelStop, centerReelStop, rightReelStop;
        private readonly Symbols[] leftOrder, centerOrder, rightOrder;
        private static sbyte leftReelPosition, centerReelPosition, rightReelPosition;
        private readonly Dictionary<Symbols, Bitmap> symbolImages;
        private readonly SlotController slotController;
        private readonly ResultController resultController;
        //定数
        private static readonly int SWITCHING_POINT = 550, INTERVAL = 16;//16;
        private static readonly int CONTAINER_INTERVAL = 175;
        private static readonly int GAP = 25;
        private static readonly int MOVE = 36;

        private sbyte leftStopReelPosition = NONE;
        private sbyte centerStopReelPosition = NONE;
        private sbyte rightStopReelPosition = NONE;

        private int leftNextMove = MOVE;
        private int centerNextMove = MOVE;
        private int rightNextMove = MOVE;

        private bool leftNextStop = false;
        private bool centerNextStop = false;
        private bool rightNextStop = false;

        private System.Timers.Timer reelTimer = new System.Timers.Timer { Interval = INTERVAL, AutoReset = true };
        public SlotView(SlotController slotController)
        {


            this.slotController = slotController;
            this.resultController = new ResultController(); // インスタンス化を追加

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


        //全てのリールが停止しているか
        public bool AllStopReel()
        {
            
            if (leftReelStop && centerReelStop && rightReelStop)
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

            InitialSetReelImages(slotController.leftReelContainers, leftOrder, ref leftReelPosition);
            InitialSetReelImages(slotController.centerReelContainers, centerOrder, ref centerReelPosition);
            InitialSetReelImages(slotController.rightReelContainers, rightOrder, ref rightReelPosition);
        }

        //画像初期表示
        private void InitialSetReelImages(PictureBox[] reel, Symbols[] order, ref sbyte index)
        {
            int position = 375; ;
            for (int i = 0; i < reel.Length; i++)
            {
                reel[i].Image = symbolImages[order[index]];
                reel[i].Top = position;
                reel[i].Tag = (sbyte)index;
                index = (sbyte)((index + 1) % order.Length);
                position -= CONTAINER_INTERVAL;
            }

        }

        //
        ////初期設定ここまで////
        //

        //定期動作させる処理
        private void ReelTimerTick()
        {
            try
            {
                slotController.Invoke((MethodInvoker)(() => UpdateReel()));
            }
            catch (Exception e)
            {
                MessageBox.Show("ゲームシステムにエラーが発生しました\nスタッフを呼んでください\n最終残高:" + Game.GetHasCoin());
            }
            
        }

        //リールを動いている判定にする
        public void MovingReel()
        {
            leftReelStop = false;
            centerReelStop = false;
            rightReelStop = false;

            leftNextMove = 0;
            centerNextMove = 0;
            rightNextMove = 0;

            leftNextStop = false;
            centerNextStop = false;
            rightNextStop = false;


            leftStopReelPosition = NONE;
            centerStopReelPosition = NONE;
            rightStopReelPosition = NONE;
        }

        public void UpdateReel()
        {
            if (!AllStopReel() || !AllCorrect())
            {

                PreStopMoveReels();

                MoveReels();

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
                if ((sbyte)symbol.Tag == leftStopReelPosition && (375 - symbol.Top <= MOVE && symbol.Top > 200))
                {
                    leftNextMove = 375 - symbol.Top;
                    leftNextStop = true;
                }
            }

            foreach (PictureBox symbol in slotController.centerReelContainers)
            {
                if ((sbyte)symbol.Tag == centerStopReelPosition && (375 - symbol.Top <= MOVE && symbol.Top > 200))
                {
                    centerNextMove = 375 - symbol.Top;
                    centerNextStop = true;
                }
            }

            foreach (PictureBox symbol in slotController.rightReelContainers)
            {
                if ((sbyte)symbol.Tag == rightStopReelPosition && (375 - symbol.Top <= MOVE && symbol.Top > 200))
                {
                    rightNextMove = 375 - symbol.Top;
                    rightNextStop = true;
                }
            }
        }




        private void PreStopMoveReels()
        {
            if (leftNextStop && !leftReelStop) PreStopMoveReel(Reels.LEFT);
            if (centerNextStop && !centerReelStop) PreStopMoveReel(Reels.CENTER);
            if (rightNextStop && !rightReelStop) PreStopMoveReel(Reels.RIGHT);

        }


        private void PreStopMoveReel(Reels selectReel)
        {
            PictureBox[] symbolContainers = new PictureBox[4];

            switch (selectReel)
            {
                case Reels.LEFT:
                    symbolContainers = slotController.leftReelContainers;
                    leftReelStop = true;
                    break;
                case Reels.CENTER:
                    symbolContainers = slotController.centerReelContainers;
                    centerReelStop = true;
                    break;
                case Reels.RIGHT:
                    symbolContainers = slotController.rightReelContainers;
                    rightReelStop = true;
                    break;
            }

            sbyte bottomIndex = GetBottomContainerIndex(symbolContainers);
            int position = 375;
            for (sbyte i = 0; i < symbolContainers.Length; i++)
            {
                sbyte index = (sbyte)((bottomIndex + i) % symbolContainers.Length);
                symbolContainers[index].Top = position - (CONTAINER_INTERVAL * i);
            }
            slotController.SetStopBtnEnabled();
        }


        private void MoveReels()
        {
            if (!leftNextStop) MoveReel(Reels.LEFT, ref leftReelPosition, slotController.leftReelContainers);
            if (!centerNextStop) MoveReel(Reels.CENTER, ref centerReelPosition, slotController.centerReelContainers);
            if (!rightNextStop) MoveReel(Reels.RIGHT, ref rightReelPosition, slotController.rightReelContainers);
        }

        //画像遷移
        private void MoveReel(Reels selectReel, ref sbyte reelPosition, PictureBox[] symbolContainers)
        {
            Symbols[] order = new Symbols[21];
            bool stopFlag = false;


            switch (selectReel)
            {
                case Reels.LEFT:
                    symbolContainers = slotController.leftReelContainers;
                    order = leftOrder;
                    break;
                case Reels.CENTER:
                    symbolContainers = slotController.centerReelContainers;
                    order = centerOrder;
                    break;
                case Reels.RIGHT:
                    symbolContainers = slotController.rightReelContainers;
                    order = rightOrder;
                    break;
            }
            foreach (var symbolContainer in symbolContainers)
            {
                if (!stopFlag || !GetIsCorrect(symbolContainer))
                {
                    symbolContainer.Top += MOVE; //シンボルの画像位置を移動
                }


                if (symbolContainer.Top >= SWITCHING_POINT - MOVE)  //画面範囲外に到達した時
                {
                    UpdateImage(symbolContainer, ref reelPosition, order); //次のシンボルの画像に代入、またTagに要素番号を代入

                    symbolContainer.Top = GetTopContainer(symbolContainers).Top - CONTAINER_INTERVAL; //textBoxの位置を上に移動
                }
            }


        }



        //シンボルの画像切り替え
        private void UpdateImage(PictureBox symbolContainer, ref sbyte reelPosition, Symbols[] order)
        {
            reelPosition = (sbyte)(reelPosition % 21);
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


        //上段にあるコンテナ
        public PictureBox GetTopContainer(PictureBox[] symbolContainers)
        {
            int minTop = 0;
            int top = 0;
            for (int i = 0; i < symbolContainers.Length; i++)
            {
                if (symbolContainers[i].Top > minTop)
                {
                    minTop = symbolContainers[i].Top;
                    top = i;
                }
            }
            top = (top + symbolContainers.Length - 1) % symbolContainers.Length;
            return symbolContainers[top];
        }


        //抽選を開始する地点のリールの要素番号を取得
        public sbyte GetLotteryPosition(PictureBox[] symbolContainers)
        {
            int maxTop = 0;
            int bottom = 0;
            for (int i = 0; i < symbolContainers.Length; i++)
            {
                if (symbolContainers[i].Top > maxTop)
                {
                    maxTop = symbolContainers[i].Top;
                    bottom = i;
                }
            }
            sbyte lotteryPositionIndex = (sbyte)((bottom + 1) % symbolContainers.Length);
            return (sbyte)symbolContainers[lotteryPositionIndex].Tag;
        }



        private sbyte GetBottomContainerIndex(PictureBox[] symbolContainers)
        {
            int maxTop = 0;
            sbyte bottom = 0;
            for (sbyte i = 0; i < symbolContainers.Length; i++)
            {
                if (symbolContainers[i].Top > maxTop)
                {
                    maxTop = symbolContainers[i].Top;
                    bottom = i;
                }
            }
            return bottom;
        }



        //停止位置をセット
        public void SetStopReelPosition(Reels selectReel, sbyte reelPosition)
        {
            switch (selectReel)
            {
                case Reels.LEFT:

                    this.leftStopReelPosition = reelPosition;
                    break;
                case Reels.CENTER:
                    this.centerStopReelPosition = reelPosition;
                    break;
                case Reels.RIGHT:
                    this.rightStopReelPosition = reelPosition;
                    break;
            }
        }


        //全てのシンボルの停止座標が正しいか判定
        public bool AllCorrect()
        {
            if (GetReelCorrect(Reels.LEFT) && GetReelCorrect(Reels.CENTER) && GetReelCorrect(Reels.RIGHT))
            {
                return true;
            }
            return false;
        }

        //それぞれのリールがシンボルの停止座標が正しいか判定
        public bool GetReelCorrect(Reels selectReel)
        {
            bool reelCorrect = true;
            PictureBox[] symbolContainers = new PictureBox[5];
            switch (selectReel)
            {
                case Reels.LEFT:
                    symbolContainers = slotController.leftReelContainers;
                    break;
                case Reels.CENTER:
                    symbolContainers = slotController.centerReelContainers;
                    break;
                case Reels.RIGHT:
                    symbolContainers = slotController.rightReelContainers;
                    break;
            }

            foreach (PictureBox symbolContainer in symbolContainers)
            {
                reelCorrect &= GetIsCorrect(symbolContainer);
            }

            return reelCorrect;

        }


        //停止位置として正しいか判定する
        public bool GetIsCorrect(PictureBox symbolContainer)
        {
            if ((symbolContainer.Top < 0 && symbolContainer.Top > (-1 * symbolContainer.Height)) || ((symbolContainer.Top - GAP) % CONTAINER_INTERVAL) != 0)
            {
                return false;
            }
            if ((symbolContainer.Top < 0 && symbolContainer.Top <= (-1 * symbolContainer.Height)) || (symbolContainer.Top >= 0 && (symbolContainer.Top - GAP) % CONTAINER_INTERVAL == 0)) //画面範囲外か0以上ならINTERVALの間隔で配置されているか
            {
                return true;
            }
            return false;
        }

        

    }
}
