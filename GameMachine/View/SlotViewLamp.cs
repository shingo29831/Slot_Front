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
        private bool isLampOn = false;      // 両方点滅時のトグルフラグ
        private bool isLeftLampOn = true;   // 片方点滅時のトグルフラグ

        // 点滅モードフラグ
        private bool isBothFlashing = false;  // 両方点滅か片方ずつかを判定するフラグ

        public SlotViewLamp(PictureBox flowerLeft, PictureBox flowerRight)
        {
            FlowerLeft = flowerLeft;
            FlowerRight = flowerRight;

            // タイマーの初期化
            flashTimer = new System.Timers.Timer(500);  // 500ミリ秒間隔
            flashTimer.Elapsed += flashTimerTick;      // イベントハンドラを追加
            flashTimer.AutoReset = true;               // タイマーが繰り返し発生するよう設定
        }

        // 点滅動作
        private void flashTimerTick(object sender, ElapsedEventArgs e)
        {
            if (isBothFlashing)
            {
                // 両方点滅
                if (isLampOn)
                {
                    LampOff();  // 両方消灯
                }
                else
                {
                    BothFlowerLamp();  // 両方点灯
                }

                isLampOn = !isLampOn;
            }
            else
            {
                // 片方ずつ点滅
                if (isLeftLampOn)
                {
                    LeftFlowerLamp();  // 左だけ点灯
                    RightFlowerLampOff();  // 右は消灯
                }
                else
                {
                    RightFlowerLamp();  // 右だけ点灯
                    LeftFlowerLampOff();  // 左は消灯
                }

                isLeftLampOn = !isLeftLampOn;
            }
        }

        // 両方ランプを点滅させる（低速）
        public void StartLampFlashSlow()
        {
            flashTimer.Interval = 500;
            isBothFlashing = true;  // 両方点滅モード
            flashTimer.Start();
        }

        // 両方ランプを高速点滅させる
        public void StartLampFlashFast()
        {
            flashTimer.Interval = 200;
            isBothFlashing = true;  // 両方点滅モード
            flashTimer.Start();
        }

        // 片方ずつランプを点滅させる（低速）
        public void StartAlternatingLampFlashSlow()
        {
            flashTimer.Interval = 500;
            isBothFlashing = false;  // 片方ずつ点滅モード
            flashTimer.Start();
        }

        // 片方ずつランプを高速点滅させる
        public void StartAlternatingLampFlashFast()
        {
            flashTimer.Interval = 200;
            isBothFlashing = false;  // 片方ずつ点滅モード
            flashTimer.Start();
        }

        // ランプの点滅を停止
        public void StopLampFlash()
        {
            flashTimer.Stop();
            LampOff();  // 両方消灯して終了
        }

        //消灯処理
        public void LampOff() { FlowerLeft.Image = Properties.Resources.FlowerOFF; FlowerRight.Image = Properties.Resources.FlowerOFF; }
        //両方点灯
        public void BothFlowerLamp() { FlowerLeft.Image = Properties.Resources.FlowerON; FlowerRight.Image = Properties.Resources.FlowerON; }
        //右だけ点灯
        public void RightFlowerLamp() { FlowerLeft.Image = Properties.Resources.FlowerOFF; FlowerRight.Image = Properties.Resources.FlowerON; }
        //左だけ点灯
        public void LeftFlowerLamp() { FlowerLeft.Image = Properties.Resources.FlowerON; FlowerRight.Image = Properties.Resources.FlowerOFF; }

        public void RightFlowerLampOff() { FlowerRight.Image = Properties.Resources.FlowerOFF; }

        public void LeftFlowerLampOff() { FlowerLeft.Image = Properties.Resources.FlowerOFF; }

    }
}
