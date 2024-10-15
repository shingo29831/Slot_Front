using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
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

        // リールの現在位置（シンボルのインデックスを保持）
        public int leftcount = 0;
        public int centercount = 0;
        public int rightcount = 0;

        // 各リールに対応する PictureBox 配列
        private PictureBox[] leftReel;
        private PictureBox[] centerReel;
        private PictureBox[] rightReel;

        // 画面上のレバーやボタンの画像を保持する PictureBox 配列
        private PictureBox[] PChange;

        // 列挙型 Symbols をキーとしたシンボル画像辞書（各シンボルに対応する画像を設定）
        private Dictionary<Symbols, Image> symbolImages;

        // コンストラクタ：リール回転に必要な要素を初期化
        public SlotView(PictureBox[] left, PictureBox[] center, PictureBox[] right, PictureBox[] pictureChange)
        {
            // タイマーの初期化
            reelTimer = new System.Timers.Timer(16);  // 16ミリ秒の間隔でタイマーを設定
            reelTimer.Elapsed += ReelTimerTick;        // イベントハンドラを追加
            reelTimer.AutoReset = true;                // タイマーが繰り返し発生するよう設定
            reelTimer.Interval = 16;                   // 再度設定（念のため）

            // 各リール用の PictureBox 配列を設定
            leftReel = left;
            centerReel = center;
            rightReel = right;

            // リールの回転順序を設定（シンボルの配列）
            leftOrder = ReelOrder.leftReelOrder;
            centerOrder = ReelOrder.centerReelOrder;
            rightOrder = ReelOrder.rightReelOrder;

            // レバーやボタンの PictureBox 配列を設定
            PChange = pictureChange;

            // 各シンボルに対応する画像を設定する辞書を初期化
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
            reelTimer.Start();          // タイマーをスタート
            leftReelStop = false;      // 左リールの停止フラグをリセット
            centerReelStop = false;    // 中央リールの停止フラグをリセット
            rightReelStop = false;     // 右リールの停止フラグをリセット
        }

        // 各リールの停止メソッド（ボタン押下時に呼ばれる）—非同期処理を使って停止
        public async Task StopLeftReel()
        {
            // 左リールが指定位置に停止するまでループ
            while (true)
            {
                if (leftReel[0].Top == 25 || leftReel[0].Top == 200 || leftReel[0].Top == 375 || leftReel[0].Top == 550)
                {
                    leftReelStop = true;  // 停止フラグを設定
                    break;
                }
                else
                {
                    // リールの位置を調整（シンボルを 25 ピクセルずつ移動）
                    for (int i = 0; i < leftReel.Length; i++)
                    {
                        leftReel[i].Top += 25;
                    }
                }
                await Task.Delay(16);  // 非同期で16ミリ秒待機（UIフリーズを防ぐ）
            }
        }

        public async Task StopCenterReel()
        {
            // 中央リールが指定位置に停止するまでループ
            while (true)
            {
                if (centerReel[0].Top == 25 || centerReel[0].Top == 200 || centerReel[0].Top == 375 || centerReel[0].Top == 550)
                {
                    centerReelStop = true;  // 停止フラグを設定
                    break;
                }
                else
                {
                    for (int i = 0; i < centerReel.Length; i++)
                    {
                        centerReel[i].Top += 25;
                    }
                }
                await Task.Delay(16);  // 非同期で16ミリ秒待機
            }
        }

        public async Task StopRightReel()
        {
            // 右リールが指定位置に停止するまでループ
            while (true)
            {
                if (rightReel[0].Top == 25 || rightReel[0].Top == 200 || rightReel[0].Top == 375 || rightReel[0].Top == 550)
                {
                    rightReelStop = true;  // 停止フラグを設定
                    break;
                }
                else
                {
                    for (int i = 0; i < rightReel.Length; i++)
                    {
                        rightReel[i].Top += 25;
                    }
                }
                await Task.Delay(16);  // 非同期で16ミリ秒待機
            }
        }

        // リールに初期画像を設定するメソッド
        private void SetReelImages(PictureBox[] reel, Symbols[] order, int standardorder)
        {
            standardorder--;  // 指定された基準インデックスを調整（0ベースに変更）

            // 各リール画像に対応するシンボルを設定
            for (int i = 0; i < reel.Length; i++)
            {
                if (symbolImages.ContainsKey(order[standardorder]))
                {
                    reel[i].Image = symbolImages[order[standardorder]];
                    standardorder = (standardorder + 1) % order.Length;  // 循環
                }
            }
        }

        // ゲーム開始時の初期画像設定
        public void initialPictureSet(int standardOrderLeft, int standardOrderCenter, int standardOrderRight)
        {
            leftcount = standardOrderLeft;
            centercount = standardOrderCenter;
            rightcount = standardOrderRight;

            SetReelImages(leftReel, leftOrder, leftcount);       // 左リールの画像設定
            SetReelImages(centerReel, centerOrder, centercount); // 中央リールの画像設定
            SetReelImages(rightReel, rightOrder, rightcount);    // 右リールの画像設定
        }

        // タイマーイベントハンドラ（16msごとに呼ばれる）
        private async void ReelTimerTick(object sender, ElapsedEventArgs e)
        {
            // UI スレッドで実行するために Invoke を使用
            if (leftReel[0].InvokeRequired)
            {
                leftReel[0].Invoke(new Action(() =>
                {
                    if (!leftReelStop) { MoveReel(leftReel, ref leftcount, leftOrder); }         // 左リールの回転処理
                    if (!centerReelStop) { MoveReel(centerReel, ref centercount, centerOrder); } // 中央リールの回転処理
                    if (!rightReelStop) { MoveReel(rightReel, ref rightcount, rightOrder); }    // 右リールの回転処理
                }));
            }
            await Task.Delay(16); // 必要に応じて非同期処理を追加
        }

        // リールを動かす（各リールの回転アニメーション処理）
        private void MoveReel(PictureBox[] reels, ref int count, Symbols[] order)
        {
            foreach (var reel in reels)
            {
                reel.Top += 25;  // リールの速度を調整
                if (reel.Top > 550)  // リールが画面外に出たら位置をリセット
                {
                    UpdateImage(reel, ref count, order); // 画像を次のシンボルに更新
                    reel.Top = -reel.Height;
                }
            }
        }

        // リールの画像を更新する（シンボル順序に従って画像を変更）
        private void UpdateImage(PictureBox reel, ref int count, Symbols[] order)
        {
            // 現在表示されている画像と次の画像が同じ場合は更新しない
            if (reel.Image == symbolImages[order[count]])
                return;

            if (symbolImages.ContainsKey(order[count]))
            {
                reel.Image = symbolImages[order[count]]; // 新しい画像を設定
                count = (count + 1) % order.Length;      // インデックスを循環
            }
        }

        // レバー・ボタンの画像変更（操作時のボタン画像を変更）
        public void leverUp() { PChange[0].Image = Properties.Resources.LeverOFF; }
        public void leverDown() { PChange[0].Image = Properties.Resources.LeverON; }
        public void leftbtnChange() { PChange[1].Image = Properties.Resources.LeftButtonON; }
        public void centerbtnChange() { PChange[2].Image = Properties.Resources.CenterButtonON; }
        public void rightbtnChange() { PChange[3].Image = Properties.Resources.RightButtonON; }
        public void maxbetChengeUp() { PChange[4].Image = Properties.Resources.MAXBETOFF; }
        public void maxbetChengeDown() { PChange[4].Image = Properties.Resources.MAXBETON; }
        public void betChenge()
        {
            PChange[5].Image = Properties.Resources.OneON;
            PChange[6].Image = Properties.Resources.TwoON;
            PChange[7].Image = Properties.Resources.ThreeON;
        }
        public void Changereset()
        {
            PChange[1].Image = Properties.Resources.LeftButtonOFF;
            PChange[2].Image = Properties.Resources.CenterButtonOFF;
            PChange[3].Image = Properties.Resources.RightButtonOFF;
        }
    }
}
