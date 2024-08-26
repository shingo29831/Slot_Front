using System.Threading;
using System.Windows.Forms;
using GameMachine.SlotSystemController;

namespace GameMachine.SlotView
{
    // スロットを止める処理
    internal class LaneButton
    {
        private Button button;
        private int reelNumber;
        private SlotController slotController;

        public LaneButton(Button button, int reelNumber, SlotController slotController)
        {
            this.button = button;
            this.reelNumber = reelNumber;
            this.slotController = slotController;

            // ボタンのクリックイベントにメソッドを登録
            button.Click += (sender, e) => OnButtonClick();
        }

        private void OnButtonClick()
        {
            button.Enabled = false;
            slotController.StopSpecificReel(reelNumber); // 対応するリールを停止
        }

        public void EnableButton()
        {
            button.Enabled = true;
        }
    }
}
