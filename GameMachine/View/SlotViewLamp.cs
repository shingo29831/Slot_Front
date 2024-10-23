using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace GameMachine.View
{
    class SlotViewLamp
    {
        PictureBox FlowerLeft;
        PictureBox FlowerRight;

        // 高精度タイマー（ランプの点滅速度を制御する）
        private System.Timers.Timer flashTimer;

        // ランプの状態をトグルするためのフラグ
        private bool isLampOn = false;

        public SlotViewLamp(PictureBox flowerLeft, PictureBox flowerRight)
        {
            FlowerLeft = flowerLeft;
            FlowerRight = flowerRight;

            // タイマーの初期化
            flashTimer = new System.Timers.Timer(500);  // 500ミリ秒間隔（0.5秒ごとに点滅）
            flashTimer.Elapsed += flashTimerTick;      // イベントハンドラを追加
            flashTimer.AutoReset = true;               // タイマーが繰り返し発生するよう設定
        }

        // 点滅動作
        private void flashTimerTick(object sender, ElapsedEventArgs e)
        {
            // ランプの状態をトグル
            if (isLampOn)
            {
                LampOff();  // 両方消灯
            }
            else
            {
                BothFlowerLamp();  // 両方点灯
            }

            // ランプの状態を切り替え
            isLampOn = !isLampOn;
        }

        // ランプを点滅させる
        //両方点滅低速
        public void StartLampFlashSlow()
        {
            flashTimer.Interval = 500;
            flashTimer.Start();
        }
        //両方高速点滅
        public void StartLampFlashFast()
        {
            flashTimer.Interval = 200;
            flashTimer.Start();
        }

        // ランプの点滅を停止
        //両方点滅停止
        public void StopLampFlash()
        {
            flashTimer.Stop();
            LampOff();  // ランプを消灯して終了
        }

        //消灯処理
        public void LampOff() { FlowerLeft.Image = Properties.Resources.FlowerOFF; FlowerRight.Image = Properties.Resources.FlowerOFF; }
        //両方点灯
        public void BothFlowerLamp() { FlowerLeft.Image = Properties.Resources.FlowerON; FlowerRight.Image = Properties.Resources.FlowerON; }
        //右だけ点灯
        public void RightFlowerLamp() { FlowerLeft.Image = Properties.Resources.FlowerOFF; FlowerRight.Image = Properties.Resources.FlowerON; }
        //左だけ点灯
        public void LeftFlowerLamp() { FlowerLeft.Image = Properties.Resources.FlowerON; FlowerRight.Image = Properties.Resources.FlowerOFF; }


    }
}
