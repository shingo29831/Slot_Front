using System;
using System.Drawing;
using System.Windows.Forms;
using System.Timers;
using static Constants;

namespace GameMachine
{
    public partial class SlotController : UserControl
    {
        private SlotView slotView;

        // System.Timers.Timer に変更
        private System.Timers.Timer reelTimer;

        public SlotController()
        {
            InitializeComponent();

            // PictureBox配列を作成
            PictureBox[] leftReels = { LpB1, LpB2, LpB3, LpB4 };
            PictureBox[] centerReels = { CpB1, CpB2, CpB3, CpB4 };
            PictureBox[] rightReels = { RpB1, RpB2, RpB3, RpB4 };

            PictureBox[] pictureChange = { btnStart, btnstop1, btnstop2, btnstop3 };

            // System.Timers.Timer のインスタンスを作成
            reelTimer = new System.Timers.Timer(10); // 50ミリ秒の間隔
            reelTimer.AutoReset = true; // 自動リセットを有効に
            reelTimer.Enabled = false; // 必要なときに開始

            // SlotView のインスタンスを作成
            slotView = new SlotView(reelTimer, leftReels, centerReels, rightReels, pictureChange);
        }

        private void UserGameScreen_Load(object sender, EventArgs e)
        {
            //シンボル表示初期位置
            slotView.initialPictureSet(1, 3, 20);//数値をずらすと始まる位置が変わる
        }

        private void stopBtns_Click(object sender, EventArgs e)
        {
            if (sender == btnstop1) { slotView.StopLeftReel(); slotView.leftbtnChange(); }
            else if (sender == btnstop2) { slotView.StopCenterReel(); slotView.centerbtnChange(); }
            else if (sender == btnstop3) { slotView.StopRightReel(); slotView.rightbtnChange(); }
        }

        //レバーが押されると回転スタート
        private void btnStart_Click(object sender, EventArgs e)
        {
            slotView.Start();
            slotView.Changereset();
        }

        //レバーが上がったら画像を切り替える
        private void btnStart_MouseUp(object sender, MouseEventArgs e)
        {
            slotView.leverUp();
        }

        //レバーが下がったら画像を切り替える
        private void btnStart_MouseDown(object sender, MouseEventArgs e)
        {
            slotView.leverDown();
        }

        private void LpB2_Click(object sender, EventArgs e)
        {

        }
    }
}
