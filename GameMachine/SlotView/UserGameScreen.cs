using System;
using System.Drawing;
using System.Windows.Forms;
using GameMachine.SlotView;
using GameMachine.SlotSystemController;

namespace GameMachine
{
    public partial class UserGameScreen : UserControl
    {
        private Lane lane1;
        private Lane lane2;
        private Lane lane3;
        private Lever lever;
        private LaneButton LaneButton;
        private SlotController slotController;
        private LaneButton[] laneButtons;

        public UserGameScreen()
        {
            InitializeComponent();

            lane1 = new Lane(new PictureBox[] { picBox1, picBox4, picBox7 }, new Bitmap[] {
                Properties.Resources.seven,
                Properties.Resources.bar,
                Properties.Resources.cherry,
                Properties.Resources.bell,
                Properties.Resources.watermelon
            });

            lane2 = new Lane(new PictureBox[] { picBox2, picBox5, picBox8 }, new Bitmap[] {
                Properties.Resources.cherry,
                Properties.Resources.watermelon,
                Properties.Resources.bar,
                Properties.Resources.bell,
                Properties.Resources.seven
            });

            lane3 = new Lane(new PictureBox[] { picBox3, picBox6, picBox9 }, new Bitmap[] {
                Properties.Resources.cherry,
                Properties.Resources.bar,
                Properties.Resources.watermelon,
                Properties.Resources.seven,
                Properties.Resources.bell
            });

            lever = new Lever(lane1, lane2, lane3);
            slotController = new SlotController(lever);

            laneButtons = new LaneButton[]
            {
                new LaneButton(btnstop1, 1, slotController),
                new LaneButton(btnstop2, 2, slotController),
                new LaneButton(btnstop3, 3, slotController)
            };

            this.Load += UserGameScreen_Load;
        }

        // UserGameScreen_Load メソッドの追加
        private void UserGameScreen_Load(object sender, EventArgs e)
        {
            lane1.StartSpin();
            lane2.StartSpin();
            lane3.StartSpin();
        }

        //ストップの動作をコントローラに
        private void stopBtns_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            btn.Enabled = false;

            if (btn == btnstop1) slotController.StopSpecificReel(1); // スロットコントローラ リール1
            else if (btn == btnstop2) slotController.StopSpecificReel(2); // スロットコントローラ リール2
            else if (btn == btnstop3) slotController.StopSpecificReel(3); // スロットコントローラ リール3
        }

        //スタートの動作をコントローラに
        private void btnStart_Click(object sender, EventArgs e)
        {
            btnStart.Enabled = false;
            foreach (var laneButton in laneButtons)
            {
                laneButton.EnableButton();
            }

            slotController.StopAllReels(); // 既存のタスクをキャンセル
            slotController.DownLever(); // リールを回転させる
        }
    }
}
