using System;
using System.Drawing;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;

namespace GameMachine
{
    public class SlotView
    {
        private System.Windows.Forms.Timer reelTimer; // ここで明示的に指定

        private bool leftReelStop = false;
        private bool centerReelStop = false;
        private bool rightReelStop = false;

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

        //タイマー 　回転スタート
        public void Start()
        {
            reelTimer.Enabled = true;
            leftReelStop = false;
            centerReelStop = false;
            rightReelStop = false;
        }

        // リールの停止
        public void StopLeftReel()
        {
            leftReelStop = true;
        }

        public void StopCenterReel()
        {
            centerReelStop = true;
        }

        public void StopRightReel()
        {
            rightReelStop = true;
        }

        // ここでシンボルに対応する画像を管理してます
        Dictionary<int, Image> symbolImages = new Dictionary<int, Image>
        {
            { 1, Properties.Resources.bell },
            { 2, Properties.Resources.REPLAY },
            { 3, Properties.Resources.watermelon },
            { 4, Properties.Resources.cherry },
            { 5, Properties.Resources.seven },
            { 6, Properties.Resources.bar }
        };

        // リールのシンボルに対応する画像を設定する
        private void SetReelImages(PictureBox[] reel, int[] order, int standardorder)
        {
            //修正しないとダメ！！
            standardorder--;

            for (int i = 0; i < reel.Length; i++)
            {
                if (symbolImages.ContainsKey(order[standardorder]))
                {
                    reel[i].Image = symbolImages[order[standardorder]];
                    //配列の最後尾の場合先頭に戻る　それ以外は加算
                    if (standardorder==20)
                    {
                        standardorder = 0;
                    }
                    else
                    {
                        standardorder++;
                    }
                }
            }
        }

        // 初期設定関数
        public void initialPictureSet(int standardOrderLeft, int standardOrderCenter, int standardOrderRight)
        {

            leftcount = standardOrderLeft;
            centercount = standardOrderCenter;
            rightcount = standardOrderRight;

            // 左リールの画像設定　Reel情報,Order情報,基準情報
            SetReelImages(leftReel, leftOrder, leftcount);

            // 中央リールの画像設定
            SetReelImages(centerReel, centerOrder, centercount);

            // 右リールの画像設定
            SetReelImages(rightReel, rightOrder, rightcount);
        }

        //タイマーメソッド
        public void reelTimerTick(object sender, EventArgs e)
        {
            //ここで速度調節できます
            int speed = 20;
            //リール情報と速度
            if (!leftReelStop)
            {
                MoveReel(leftReel, speed);
            }

            if (!centerReelStop)
            {
                MoveReel(centerReel, speed);
            }

            if (!rightReelStop)
            {
                MoveReel(rightReel, speed);
            }
           
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

        //コード最適化　未　重複してるswitch文をなくす
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
        //リールのストップ位置補正
        public void stopCorrection()
        {

        }
    }
}
