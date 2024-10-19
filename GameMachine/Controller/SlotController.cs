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

        //止まっているリールの数でボタンのカウントを初期化
        private int btnCount = 3;
        //スタートしていないときにボタンを押されないようにする
        private Boolean StartFlag = false;
        //マックスベットを押した後にレバーの動作をオンにするためのフラグ
        private Boolean MaxbetFlag = false;

        public SlotController()
        {
            InitializeComponent();

            // PictureBox配列を作成
            PictureBox[] leftReels = { LpB1, LpB2, LpB3, LpB4 };
            PictureBox[] centerReels = { CpB1, CpB2, CpB3, CpB4 };
            PictureBox[] rightReels = { RpB1, RpB2, RpB3, RpB4 };

            PictureBox[] pictureChange = { btnStart, btnstop1, btnstop2, btnstop3, btnstop3, MaxBet, Bet1, Bet2, Bet3 };

            // SlotView のインスタンスを作成
            slotView = new SlotView(leftReels, centerReels, rightReels, pictureChange);
        }

        private void UserGameScreen_Load(object sender, EventArgs e)
        {
            //シンボル表示初期位置
            slotView.initialPictureSet(1, 3, 20);//数値をずらすと始まる位置が変わる
        }

        private void stopBtns_Click(object sender, EventArgs e)
        {
            if (sender == btnstop1 && StartFlag == true){ 
                slotView.StopLeftReel(); 
                slotView.leftbtnChange();
                btnCount++;
                switch (btnCount)
                {
                    case 1:
                        slotView.betThirdOFF();
                        break;
                    case 2:
                        slotView.betSecondOFF();
                        break;
                    case 3:
                        slotView.betFirstOFF();
                        break;
                }
                btnstop1.Enabled = false;
            }else if (sender == btnstop2 && StartFlag == true)
            { 
                slotView.StopCenterReel(); 
                slotView.centerbtnChange();
                btnCount++;
                switch (btnCount)
                {
                    case 1:
                        slotView.betThirdOFF();
                        break;
                    case 2:
                        slotView.betSecondOFF();
                        break;
                    case 3:
                        slotView.betFirstOFF();
                        break;
                }
                btnstop2.Enabled = false;
            }
            else if (sender == btnstop3 && StartFlag == true)
            { 
                slotView.StopRightReel(); 
                slotView.rightbtnChange();
                btnCount++;
                switch (btnCount)
                {
                    case 1:
                        slotView.betThirdOFF();
                        break;
                    case 2:
                        slotView.betSecondOFF();
                        break;
                    case 3:
                        slotView.betFirstOFF();
                        break;
                }
                btnstop3.Enabled = false;
            }
            
        }

        //レバーが押されると回転スタート
        private void btnStart_Click(object sender, EventArgs e)
        {
            if (btnCount == 3 && MaxbetFlag)
            {
                btnstop1.Enabled = true;
                btnstop2.Enabled = true;
                btnstop3.Enabled = true;
                StartFlag = true;

                slotView.Start();
                slotView.Changereset();

                btnCount = 0;
            }
            
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

        //MAXBET
        private void MaxBet_Click(object sender, EventArgs e)
        {
            slotView.betFirstON();
            slotView.betSecondON();
            slotView.betThirdON();
            MaxbetFlag = true;

        }

        private void MaxBet_MouseUp(object sender, MouseEventArgs e)
        {
            slotView.maxbetChengeUp();
        }

        private void MaxBet_MouseDown(object sender, MouseEventArgs e)
        {
            slotView.maxbetChengeDown();
        }
    }
}
