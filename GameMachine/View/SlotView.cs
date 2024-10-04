using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Timers;  // System.Timers を使用
using static Constants;

namespace GameMachine
{
    public class SlotView
    {
        // 高精度タイマー（リールの回転速度を制御する）
        private System.Timers.Timer reelTimer;

        // 各リールの停止フラグ
        private bool leftReelStop = false;
        private bool centerReelStop = false;
        private bool rightReelStop = false;

        // リールのシンボル配置を保持する配列
        public static Symbols[] leftOrder;
        public static Symbols[] centerOrder;
        public static Symbols[] rightOrder;

        // リールの現在
        public int leftcount = 0;
        public int centercount = 0;
        public int rightcount = 0;

        // 各リールに対応する
        private PictureBox[] leftReel;
        private PictureBox[] centerReel;
        private PictureBox[] rightReel;

        // 画面上のレバーやボタンの画像を保持する PictureBox 配列
        private PictureBox[] PChange;

        // 列挙型 Symbols をキーとしたシンボル画像辞書
        private Dictionary<Symbols, Image> symbolImages;

        // コンストラクタ：リール回転に必要な要素を初期化
        public SlotView(System.Timers.Timer timer, PictureBox[] left, PictureBox[] center, PictureBox[] right, PictureBox[] pictureChange)
        {
            // タイマーの設定
            reelTimer = timer;
            reelTimer.Elapsed += ReelTimerTick;  // タイマーのイベントハンドラを設定
            reelTimer.AutoReset = true;          // 自動で繰り返し実行
            reelTimer.Interval = 1;             // タイマーの間隔を設定（50ミリ秒）

            // 各リールの PictureBox 配列を初期化
            leftReel = left;
            centerReel = center;
            rightReel = right;

            // 各リールの回転順序を設定
            leftOrder = ReelOrder.leftReelOrder;
            centerOrder = ReelOrder.centerReelOrder;
            rightOrder = ReelOrder.rightReelOrder;

            // レバーやボタンの PictureBox 配列を設定
            PChange = pictureChange;

            // 各シンボルに対応する画像を設定する辞書
            symbolImages = new Dictionary<Symbols, Image>
            {
                { Symbols.BELL, Properties.Resources.bell },
                { Symbols.REPLAY, Properties.Resources.REPLAY },
                { Symbols.WATERMELON, Properties.Resources.watermelon },
                { Symbols.CHERRY, Properties.Resources.cherry },
                { Symbols.SEVEN, Properties.Resources.seven },
                { Symbols.BAR, Properties.Resources.bar }
            };
        }

        // タイマーのスタートメソッド（リールの回転開始）
        public void Start()
        {
            reelTimer.Start();  // タイマーをスタート
            // リール停止フラグを初期化（全てのリールを回転状態に設定）
            leftReelStop = false;
            centerReelStop = false;
            rightReelStop = false;
        }

        // 各リールの停止メソッド（ボタン押下時に呼ばれる）
        public void StopLeftReel()
        {
            while(true)
            {
                if (leftReel[0].Top==25|| leftReel[0].Top == 200 || leftReel[0].Top == 375 || leftReel[0].Top == 550)
                {
                    leftReelStop = true;
                    break;
                }
                else
                {
                    for(int i = 0; i < 4; i++)
                    {
                        leftReel[i].Top += 25; 
                    }
                }
            }
                   
        }    // 左リールを停止
        public void StopCenterReel() {
            while (true)
            {
                if (centerReel[0].Top == 25 || centerReel[0].Top == 200 || centerReel[0].Top == 375 || centerReel[0].Top == 550)
                {
                    centerReelStop = true;
                    break;
                }
                else
                {
                    for (int i = 0; i < 4; i++)
                    {
                        centerReel[i].Top += 25;
                    }
                }
            }
        }  // 中央リールを停止
        public void StopRightReel() {
            while (true)
            {
                if (rightReel[0].Top == 25 || rightReel[0].Top == 200 || rightReel[0].Top == 375 || rightReel[0].Top == 550)
                {
                    rightReelStop = true;
                    break;
                }
                else
                {
                    for (int i = 0; i < 4; i++)
                    {
                        rightReel[i].Top += 25;
                    }
                }
            }
        }   // 右リールを停止

        // リールのシンボルに対応する画像を設定する（初期画像を設定）
        private void SetReelImages(PictureBox[] reel, Symbols[] order, int standardorder)
        {
            standardorder--;  // 指定された基準インデックスを調整（0ベースに変更）

            // 各リール画像に対応するシンボルを設定
            for (int i = 0; i < reel.Length; i++)
            {
                // 辞書を用いてキー対応するシンボル画像
                if (symbolImages.ContainsKey(ReelOrder.leftReelOrder[standardorder]))
                {
                    reel[i].Image = symbolImages[ReelOrder.leftReelOrder[standardorder]];

                    // 配列の最後まで達したら先頭に戻る（循環）
                    standardorder = (standardorder + 1) % ReelOrder.leftReelOrder.Length;
                }
            }
        }

        // 初期設定関数（ゲーム開始時のリール画像を設定）
        public void initialPictureSet(int standardOrderLeft, int standardOrderCenter, int standardOrderRight)
        {
            // 各リールの現在位置を設定
            leftcount = standardOrderLeft;
            centercount = standardOrderCenter;
            rightcount = standardOrderRight;

            // 左リールの画像設定
            SetReelImages(leftReel, leftOrder, leftcount);
            // 中央リールの画像設定
            SetReelImages(centerReel, centerOrder, centercount);
            // 右リールの画像設定
            SetReelImages(rightReel, rightOrder, rightcount);
        }

        // タイマーのイベントハンドラ（定期的に呼び出される）
        private void ReelTimerTick(object sender, ElapsedEventArgs e)
        {
            // UI スレッドで実行するために Invoke を使用
            if (leftReel[0].InvokeRequired)
            {
                leftReel[0].Invoke(new Action(() =>
                {
                    // 各リールの回転処理
                    if (!leftReelStop) { MoveReel(leftReel, ref leftcount, leftOrder); }
                    if (!centerReelStop) { MoveReel(centerReel, ref centercount, centerOrder); }
                    if (!rightReelStop) { MoveReel(rightReel, ref rightcount, rightOrder); }
                }));
            }
        }

        // リールを動かす（各リールの回転アニメーション処理）
        private void MoveReel(PictureBox[] reels, ref int count, Symbols[] order)
        {
            foreach (var reel in reels)
            {
                reel.Top +=25;  // リールの速度を調整
                if (reel.Top > 550)  // リールが画面外に出たら位置をリセット
                {
                    reel.Top = -reel.Height;
                    UpdateImage(reel, ref count, order); // 画像を次のシンボルに更新
                }
            }
        }

        // リールの画像を更新する（シンボル順序に従って画像を変更）
        private void UpdateImage(PictureBox reel, ref int count, Symbols[] order)
        {
            if (symbolImages.ContainsKey(order[count]))
            {
                reel.Image = symbolImages[order[count]]; // 新しい画像を設定
                count = (count + 1) % order.Length;  // インデックスを循環
            }
        }

        // レバー・ボタンの画像変更（操作時のボタン画像を変更）
        public void leverUp() { PChange[0].Image = Properties.Resources.LeverOFF; }
        public void leverDown() { PChange[0].Image = Properties.Resources.LeverON; }
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
