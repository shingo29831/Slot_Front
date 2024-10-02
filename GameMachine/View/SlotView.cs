using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Timers;  // System.Timers を使用

namespace GameMachine
{
    public class SlotView
    {
        private System.Timers.Timer reelTimer; // 高精度タイマー

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

        private PictureBox[] PChange;

        // コンストラクタ
        public SlotView(System.Timers.Timer timer, PictureBox[] left, PictureBox[] center, PictureBox[] right, PictureBox[] pictureChange, int[] leftorder, int[] centerorder, int[] rightorder)
        {
            reelTimer = timer;
            reelTimer.Elapsed += ReelTimerTick;  // イベントハンドラを設定
            reelTimer.AutoReset = true;          // 自動で繰り返し
            reelTimer.Interval = 50;             // タイマーの間隔を設定 (50ミリ秒)

            leftReel = left;
            centerReel = center;
            rightReel = right;
            leftOrder = leftorder;  // 左リールのパターン
            centerOrder = centerorder;  // 中央リールのパターン
            rightOrder = rightorder;  // 右リールのパターン

            PChange = pictureChange;
        }

        // タイマー　回転スタート
        public void Start()
        {
            reelTimer.Start();  // タイマーをスタート
            leftReelStop = false;
            centerReelStop = false;
            rightReelStop = false;
        }

        // リールの停止
        public void StopLeftReel() { leftReelStop = true; }
        public void StopCenterReel() { centerReelStop = true; }
        public void StopRightReel() { rightReelStop = true; }

        // ここでシンボルに対応する画像を管理しています
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
            // 修正しないとダメ！！
            standardorder--;

            for (int i = 0; i < reel.Length; i++)
            {
                if (symbolImages.ContainsKey(order[standardorder]))
                {
                    reel[i].Image = symbolImages[order[standardorder]];
                    // 配列の最後尾の場合先頭に戻る　それ以外は加算
                    standardorder = (standardorder + 1) % order.Length;
                }
            }
        }

        // 初期設定関数
        public void initialPictureSet(int standardOrderLeft, int standardOrderCenter, int standardOrderRight)
        {
            leftcount = standardOrderLeft;
            centercount = standardOrderCenter;
            rightcount = standardOrderRight;

            // 左リールの画像設定　Reel情報, Order情報, 基準情報
            SetReelImages(leftReel, leftOrder, leftcount);

            // 中央リールの画像設定
            SetReelImages(centerReel, centerOrder, centercount);

            // 右リールの画像設定
            SetReelImages(rightReel, rightOrder, rightcount);
        }

        // タイマーメソッド
        private void ReelTimerTick(object sender, ElapsedEventArgs e)
        {
            // UI スレッドで実行するために Invoke を使用
            if (leftReel[0].InvokeRequired)
            {
                leftReel[0].Invoke(new Action(() =>
                {
                    if (!leftReelStop) { MoveReel(leftReel, ref leftcount, leftOrder); }
                    if (!centerReelStop) { MoveReel(centerReel, ref centercount, centerOrder); }
                    if (!rightReelStop) { MoveReel(rightReel, ref rightcount, rightOrder); }
                }));
            }
        }

        // リールを動かす
        private void MoveReel(PictureBox[] reels, ref int count, int[] order)
        {
            foreach (var reel in reels)
            {
                reel.Top += 50;  // スピードを調整
                if (reel.Top > 540) //この座標の場所で上に戻す
                {
                    reel.Top = -reel.Height;
                    UpdateImage(reel, ref count, order);//上に戻すタイミングで画像を次のオーダーされている画像に切り替える
                }
            }
        }

        // リールの画像を更新する
        private void UpdateImage(PictureBox reel, ref int count, int[] order)
        {
            if (symbolImages.ContainsKey(order[count]))
            {
                reel.Image = symbolImages[order[count]];
                count = (count + 1) % order.Length; // カウントをリセット
            }
        }

        // 画像の切り替え
        public void leverUp() { PChange[0].Image = Properties.Resources.LeverOFF; }//レバーが上がったらOFFの画像を差し込み
        public void leverDown() { PChange[0].Image = Properties.Resources.LeverON; }//レバーが下がったらONの画像を差し込み
        public void leftbtnChange() { PChange[1].Image = Properties.Resources.LeftButtonON; }
        public void centerbtnChange() { PChange[2].Image = Properties.Resources.CenterButtonON; }
        public void rightbtnChange() { PChange[3].Image = Properties.Resources.RightButtonON; }
        public void Changereset()
        {
            PChange[1].Image = Properties.Resources.LeftButtonOFF;
            PChange[2].Image = Properties.Resources.CenterButtonOFF;
            PChange[3].Image = Properties.Resources.RightButtonOFF;
        }

    }
}
