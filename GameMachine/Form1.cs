using System;
using static Model.Setting;
using static Constants;
using static Constants.Symbol;
using static Constants.SelectReel;
using static Model.Game;


namespace GameMachine
{
    public partial class Form1 : Form
    {
        int leftNowReelPosition = GetNowReelPosition(LEFT);
        int[] leftReelOrder = GetReelOrder(LEFT);
        int[] centerReelOrder = GetReelOrder(CENTER);
        int[] rightReelOrder = GetReelOrder(RIGHT);


        int bonusState = NONE;

        int role = NONE;
        int leftPosition = NONE;
        int centerPosition = NONE;
        int rightPosition = NONE;

        int stopBtnCount = 0;

        String uniqueID = "";
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            makeTableID();
            txtbox1.Text = getTableID();


            dispReelsSymbols(LEFT);
            dispReelsSymbols(CENTER);
            dispReelsSymbols(RIGHT);


        }


        private void button1_Click(object sender, EventArgs e)
        {
            if (stopBtnCount == 0)
            {
                role = HitRoleLottery();
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
        private void dispReelsSymbols(int selectReel)
        {
            switch (selectReel)
            {
                case 1:
                    leftReelBot.Text = leftReelOrder[GetDispSymbol(LEFT, Position.BOTTOM)].ToString();
                    leftReelMid.Text = leftReelOrder[GetDispSymbol(LEFT, Position.MIDDLE)].ToString();
                    leftReelTop.Text = leftReelOrder[GetDispSymbol(LEFT, Position.TOP)].ToString();
                    break;

                case 2:
                    centerReelBot.Text = centerReelOrder[GetDispSymbol(CENTER, Position.BOTTOM)].ToString();
                    centerReelMid.Text = centerReelOrder[GetDispSymbol(CENTER, Position.MIDDLE)].ToString();
                    centerReelTop.Text = centerReelOrder[GetDispSymbol(CENTER, Position.TOP)].ToString();
                    break;

                case 3:
                    rightReelBot.Text = rightReelOrder[GetDispSymbol(RIGHT, Position.BOTTOM)].ToString();
                    rightReelMid.Text = rightReelOrder[GetDispSymbol(RIGHT, Position.MIDDLE)].ToString();
                    rightReelTop.Text = rightReelOrder[GetDispSymbol(RIGHT, Position.TOP)].ToString();
                    break;
            }
        }

        private void leftStop_Click(object sender, EventArgs e)
        {
            leftPosition = GetFarstReelPosition(LEFT, role);

            lblArray.Text += "   LEFT:" + leftPosition.ToString() + " , " + GetNowReelPosition(LEFT);
            stopBtnCount++;
            if (stopBtnCount == 3)
            {
                stopBtnCount = 0;
            }
        }

        private void centerStop_Click(object sender, EventArgs e)
        {
            centerPosition = GetFarstReelPosition(CENTER, role);


            lblArray.Text += "   CENTER:" + centerPosition.ToString() + " , " + GetNowReelPosition(CENTER);
            stopBtnCount++;
            if (stopBtnCount == 3)
            {
                stopBtnCount = 0;
            }
        }

        private void rightStop_Click(object sender, EventArgs e)
        {
            rightPosition = GetFarstReelPosition(RIGHT, role);


            lblArray.Text += "   RIGHT:" + rightPosition.ToString() + " , " + GetNowReelPosition(RIGHT);
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
                UpReelPosition(LEFT, leftPosition);
                dispReelsSymbols(LEFT);


                UpReelPosition(CENTER, centerPosition);
                dispReelsSymbols(CENTER);

                UpReelPosition(RIGHT, rightPosition);
                dispReelsSymbols(RIGHT);
            }

        }

        
    }
}