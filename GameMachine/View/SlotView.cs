using System;
using System.Drawing;
using System.Windows.Forms;

namespace GameMachine
{
    public class SlotView
    {
        private System.Windows.Forms.Timer reelTimer; // ここで明示的に指定

        public static int[] leftOrder;
        public static int[] centerOrder;
        public static int[] rightOrder;
        public int leftcount = 0;
        public int centercount = 0;
        public int rightcount = 0;
        private PictureBox[] leftReel;
        private PictureBox[] centerReel;
        private PictureBox[] rightReel;

        //コンストラクタ
        public SlotView(System.Windows.Forms.Timer timer, PictureBox[] left, PictureBox[] center, PictureBox[] right, int[] leftorder, int[] centerorder, int[] rightorder)
        {
            reelTimer = timer;
            leftReel = left;
            centerReel = center;
            rightReel = right;
            leftOrder = leftorder;//左リールのパターン
            centerOrder = centerorder;//中央リールのパターン
            rightOrder = rightorder;//右リールのパターン
        }

        //タイマーメソッド　スタート
        public void Start()
        {
            reelTimer.Enabled = true;
        }

        //配列ReelOrderに沿って表示 初期設定
        public void initialPictureSet()
        {
            for (int i=0; i<4; i++)
            {
                switch (leftOrder[i])
                {
                    case 1:
                        leftReel[i].Image = Properties.Resources.bell;
                        break;
                    case 2:
                        leftReel[i].Image = Properties.Resources.REPLAY;
                        break;
                    case 3:
                        leftReel[i].Image = Properties.Resources.watermelon;
                        break;
                    case 4:
                        leftReel[i].Image = Properties.Resources.cherry;
                        break;
                    case 5:
                        leftReel[i].Image = Properties.Resources.seven;
                        break;
                    case 6:
                        leftReel[i].Image = Properties.Resources.bar;
                        break;
                }
            }
            for (int i = 0; i < 4; i++)
            {
                switch (centerOrder[i])
                {
                    case 1:
                        centerReel[i].Image = Properties.Resources.bell;
                        break;
                    case 2:
                        centerReel[i].Image = Properties.Resources.REPLAY;
                        break;
                    case 3:
                        centerReel[i].Image = Properties.Resources.watermelon;
                        break;
                    case 4:
                        centerReel[i].Image = Properties.Resources.cherry;
                        break;
                    case 5:
                        centerReel[i].Image = Properties.Resources.seven;
                        break;
                    case 6:
                        centerReel[i].Image = Properties.Resources.bar;
                        break;
                }
            }
            for (int i = 0; i < 4; i++)
            {
                switch (rightOrder[i])
                {
                    case 1:
                        rightReel[i].Image = Properties.Resources.bell;
                        break;
                    case 2:
                        rightReel[i].Image = Properties.Resources.REPLAY;
                        break;
                    case 3:
                        rightReel[i].Image = Properties.Resources.watermelon;
                        break;
                    case 4:
                        rightReel[i].Image = Properties.Resources.cherry;
                        break;
                    case 5:
                        rightReel[i].Image = Properties.Resources.seven;
                        break;
                    case 6:
                        rightReel[i].Image = Properties.Resources.bar;
                        break;
                }
                leftcount = i;
            }
            leftcount++;
            centercount = leftcount;
            rightcount = centercount;
        }

        //タイマーメソッド
        public void reelTimerTick(object sender, EventArgs e)
        {
            int speed = 20;
            MoveReel(leftReel, speed);
            MoveReel(centerReel, speed);
            MoveReel(rightReel, speed);
        }

        //リールを動かす
        private void MoveReel(PictureBox[] reels, int speed)
        {
            foreach (var reel in reels)
            {
                reel.Top += speed;
                Position(reel);
            }
        }

        private void Position(PictureBox pb)
        {
            if (pb.Top > 580)
            {
                pb.Top = -pb.Height;
                //配列ReelOrderに沿って表示
                //左リール用
                if (leftReel.Contains(pb))//今の要素はどの位置のリールであるかを確認する
                {
                    switch (leftOrder[leftcount])
                    {
                        case 1:
                            pb.Image = Properties.Resources.bell;
                            leftcount++;
                            break;
                        case 2:
                            pb.Image = Properties.Resources.REPLAY;
                            leftcount++;
                            break;
                        case 3:
                            pb.Image = Properties.Resources.watermelon;
                            leftcount++;
                            break;
                        case 4:
                            pb.Image = Properties.Resources.cherry;
                            leftcount++;
                            break;
                        case 5:
                            pb.Image = Properties.Resources.seven;
                            leftcount++;
                            break;
                        case 6:
                            pb.Image = Properties.Resources.bar;
                            leftcount++;
                            break;
                    }
                }
                //中央リール用
                if (centerReel.Contains(pb))//今の要素はどの位置のリールであるかを確認する
                {
                    switch (centerOrder[centercount])
                    {
                        case 1:
                            pb.Image = Properties.Resources.bell;
                            centercount++;
                            break;
                        case 2:
                            pb.Image = Properties.Resources.REPLAY;
                            centercount++;
                            break;
                        case 3:
                            pb.Image = Properties.Resources.watermelon;
                            centercount++;
                            break;
                        case 4:
                            pb.Image = Properties.Resources.cherry;
                            centercount++;
                            break;
                        case 5:
                            pb.Image = Properties.Resources.seven;
                            centercount++;
                            break;
                        case 6:
                            pb.Image = Properties.Resources.bar;
                            centercount++;
                            break;
                    }
                }
                //右リール用
                if (rightReel.Contains(pb))//今の要素はどの位置のリールであるかを確認する
                {
                    switch (rightOrder[rightcount])
                    {
                        case 1:
                            pb.Image = Properties.Resources.bell;
                            rightcount++;
                            break;
                        case 2:
                            pb.Image = Properties.Resources.REPLAY;
                            rightcount++;
                            break;
                        case 3:
                            pb.Image = Properties.Resources.watermelon;
                            rightcount++;
                            break;
                        case 4:
                            pb.Image = Properties.Resources.cherry;
                            rightcount++;
                            break;
                        case 5:
                            pb.Image = Properties.Resources.seven;
                            rightcount++;
                            break;
                        case 6:
                            pb.Image = Properties.Resources.bar;
                            rightcount++;
                            break;
                    }
                }
            }
            if (leftcount>20)
            {
                leftcount = 0;
            }
            if (centercount > 20)
            {
                centercount = 0;
            }
            if (rightcount > 20)
            {
                rightcount = 0;
            }


        }
    }
}
