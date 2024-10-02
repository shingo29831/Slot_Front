using System;
using static Model.Setting;
using static Constants;
using static Model.Game;


namespace GameMachine
{
    public partial class TestForm : Form
    {
        int leftNowReelPosition = GetNowReelPosition(Reels.LEFT);
        int[] leftReelOrder = GetReelOrder(Reels.LEFT);
        int[] centerReelOrder = GetReelOrder(Reels.CENTER);
        int[] rightReelOrder = GetReelOrder(Reels.RIGHT);


        int bonusState = NONE;

        int role = NONE;
        int leftPosition = NONE;
        int centerPosition = NONE;
        int rightPosition = NONE;

        int stopBtnCount = 0;

        String uniqueID = "";
        public TestForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            makeTableID();
            txtbox1.Text = getTableID();


            dispReelsSymbols(Reels.LEFT);
            dispReelsSymbols(Reels.CENTER);
            dispReelsSymbols(Reels.RIGHT);


        }


        private void button1_Click(object sender, EventArgs e)
        {
            if (stopBtnCount == 0)
            {
                role = HitRoleLottery();
                role = Role.STRONG_CHERRY;
                lblArray.Text = "ROLE:" + changeToName(role);
                leftPosition = NONE;
                centerPosition = NONE;
                rightPosition = NONE;
                timer1.Enabled = true;
            }
            


        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private String changeToName(int roleNum)
        {
            String value = "";
            switch (roleNum)
            {
                case 0:
                    value = "NONE";
                    break;
                case 1:
                    value = "BELL";
                    break;
                case 2:
                    value = "REPLAY";
                    break;
                case 3:
                    value = "WATERMELON";
                    break;
                case 4:
                    value = "WEAK_CHERRY";
                    break;
                case 5:
                    value = "STRONG_CHERRY";
                    break;
                case 6:
                    value = "VERY_STRONG_CHERRY";
                    break;
                case 7:
                    value = "REACH";
                    break;
                case 8:
                    value = "REG";
                    break;
                case 9:
                    value = "BIG";
                    break;
            }
            return value;
        }

        //íËêîÇ≈ëIë
        private void dispReelsSymbols(Reels selectReel)
        {
            switch (selectReel)
            {
                case Reels.LEFT:
                    leftReelBot.Text = leftReelOrder[GetDispSymbol(Reels.LEFT, Position.BOTTOM)].ToString();
                    leftReelMid.Text = leftReelOrder[GetDispSymbol(Reels.LEFT, Position.MIDDLE)].ToString();
                    leftReelTop.Text = leftReelOrder[GetDispSymbol(Reels.LEFT, Position.TOP)].ToString();
                    break;

                case Reels.CENTER:
                    centerReelBot.Text = centerReelOrder[GetDispSymbol(Reels.CENTER, Position.BOTTOM)].ToString();
                    centerReelMid.Text = centerReelOrder[GetDispSymbol(Reels.CENTER, Position.MIDDLE)].ToString();
                    centerReelTop.Text = centerReelOrder[GetDispSymbol(Reels.CENTER, Position.TOP)].ToString();
                    break;

                case Reels.RIGHT:
                    rightReelBot.Text = rightReelOrder[GetDispSymbol(Reels.RIGHT, Position.BOTTOM)].ToString();
                    rightReelMid.Text = rightReelOrder[GetDispSymbol(Reels.RIGHT, Position.MIDDLE)].ToString();
                    rightReelTop.Text = rightReelOrder[GetDispSymbol(Reels.RIGHT, Position.TOP)].ToString();
                    break;
            }
        }

        private void leftStop_Click(object sender, EventArgs e)
        {
            leftPosition = GetFirstReelPosition(Reels.LEFT, role);

            lblArray.Text += "   Reels.LEFT:" + leftPosition.ToString() + " , " + GetNowReelPosition(Reels.LEFT);
            stopBtnCount++;
            if (stopBtnCount == 3)
            {
                stopBtnCount = 0;
            }
        }

        private void centerStop_Click(object sender, EventArgs e)
        {
            centerPosition = GetFirstReelPosition(Reels.CENTER, role);


            lblArray.Text += "   Reels.CENTER:" + centerPosition.ToString() + " , " + GetNowReelPosition(Reels.CENTER);
            stopBtnCount++;
            if (stopBtnCount == 3)
            {
                stopBtnCount = 0;
            }
        }

        private void rightStop_Click(object sender, EventArgs e)
        {
            rightPosition = GetFirstReelPosition(Reels.RIGHT, role);


            lblArray.Text += "   Reels.RIGHT:" + rightPosition.ToString() + " , " + GetNowReelPosition(Reels.RIGHT);
            stopBtnCount++;

            if (stopBtnCount == 3)
            {
                stopBtnCount = 0;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (stopBtnCount < 3)
            {
                UpReelPosition(Reels.LEFT, leftPosition);
                dispReelsSymbols(Reels.LEFT);


                UpReelPosition(Reels.CENTER, centerPosition);
                dispReelsSymbols(Reels.CENTER);

                UpReelPosition(Reels.RIGHT, rightPosition);
                dispReelsSymbols(Reels.RIGHT);
            }

        }

        
    }
}