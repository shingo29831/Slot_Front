using System;
using System.Drawing;
using System.Windows.Forms;
using GameMachine.FakeModel;

namespace GameMachine
{
    //#############################################//
    //                                             //
    //              　　　未完成          　　　　 //
    //                                             //
    //#############################################//
    public partial class SlotController : UserControl
    {
        //インスタンス生成
        //view
        private SlotView slotView;

        //データ取得
        int[] leftReelOrder = SlotModel.leftReelOrder;
        int[] centerReelOrder = SlotModel.centerReelOrder;
        int[] rightReelOrder = SlotModel.rightReelOrder;

        public SlotController()
        {
            InitializeComponent();

            // PictureBox配列を作成
            PictureBox[] leftReels = { LpB1, LpB2, LpB3, LpB4 };
            PictureBox[] centerReels = { CpB1, CpB2, CpB3, CpB4 };
            PictureBox[] rightReels = { RpB1, RpB2, RpB3, RpB4 };

            // SlotViewのインスタンスを作成
            slotView = new SlotView(reelTimer, leftReels, centerReels, rightReels, leftReelOrder, centerReelOrder, rightReelOrder);

        }

        private void UserGameScreen_Load(object sender, EventArgs e)
        {
            //最初に表示される画像
            slotView.initialPictureSet();
        }

        //ストップの動作
        private void stopBtns_Click(object sender, EventArgs e){ }

        //スタートの動作
        private void btnStart_Click(object sender, EventArgs e)
        {
            // スタートボタンが押されたときにスロットを開始
            slotView.Start();
            
        }

        private void reelTimer_Tick(object sender, EventArgs e)
        {
            // SlotViewのタイマーイベントを呼び出す
            slotView.reelTimerTick(sender, e);
        }
    }
}
