using System;
using Model;
using static Constants;
using static Model.Game;

namespace GameMachine
{
    public partial class Form1 : Form
    {
        public static readonly int BELL = 1;
        public static readonly int REPLAY = 2;
        public static readonly int WATERMELON = 3;
        public static readonly int CHERRY = 4;
        public static readonly int REACH = 5;
        public static readonly int REGULAR = 6;
        public static readonly int BIG = 7;

        public static readonly int LEFT = 1;
        public static readonly int CENTER = 2;
        public static readonly int RIGHT = 3;


        int leftNowReelPosition = GetNowReelPosition(SelectReel.LEFT);
        int[] leftReelOrder = GetReelOrder(SelectReel.LEFT);
        int[] centerReelOrder = GetReelOrder(SelectReel.CENTER);
        int[] rightReelOrder = GetReelOrder(SelectReel.RIGHT);

        //int[,] reachRows = new int[3, 2];

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
            Setting.maketableID();
            txtbox1.Text = Setting.getTableID();


            dispReelsSymbols(SelectReel.LEFT);
            dispReelsSymbols(SelectReel.CENTER);
            dispReelsSymbols(SelectReel.RIGHT);


        }


        private void button1_Click(object sender, EventArgs e)
        {
            if (stopBtnCount == 0)
            {
                role = HitRoleLottery();
                role = Role.BIG;
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
                    leftReelBot.Text = leftReelOrder[GetDispSymbol(SelectReel.LEFT, Position.BOTTOM)].ToString();
                    leftReelMid.Text = leftReelOrder[GetDispSymbol(SelectReel.LEFT, Position.MIDDLE)].ToString();
                    leftReelTop.Text = leftReelOrder[GetDispSymbol(SelectReel.LEFT, Position.TOP)].ToString();
                    break;

                case 2:
                    centerReelBot.Text = centerReelOrder[GetDispSymbol(SelectReel.CENTER, Position.BOTTOM)].ToString();
                    centerReelMid.Text = centerReelOrder[GetDispSymbol(SelectReel.CENTER, Position.MIDDLE)].ToString();
                    centerReelTop.Text = centerReelOrder[GetDispSymbol(SelectReel.CENTER, Position.TOP)].ToString();
                    break;

                case 3:
                    rightReelBot.Text = rightReelOrder[GetDispSymbol(SelectReel.RIGHT, Position.BOTTOM)].ToString();
                    rightReelMid.Text = rightReelOrder[GetDispSymbol(SelectReel.RIGHT, Position.MIDDLE)].ToString();
                    rightReelTop.Text = rightReelOrder[GetDispSymbol(SelectReel.RIGHT, Position.TOP)].ToString();
                    break;
            }
        }

        private void leftStop_Click(object sender, EventArgs e)
        {
            leftPosition = GetFarstReelPosition(SelectReel.LEFT, role);

            lblArray.Text += "   LEFT:" + leftPosition.ToString() + " , " + GetNowReelPosition(SelectReel.LEFT);
            stopBtnCount++;
            if (stopBtnCount == 3)
            {
                stopBtnCount = 0;
            }
        }

        private void centerStop_Click(object sender, EventArgs e)
        {
            centerPosition = GetFarstReelPosition(SelectReel.CENTER, role);


            lblArray.Text += "   CENTER:" + centerPosition.ToString() + " , " + GetNowReelPosition(SelectReel.CENTER);
            stopBtnCount++;
            if (stopBtnCount == 3)
            {
                stopBtnCount = 0;
            }
        }

        private void rightStop_Click(object sender, EventArgs e)
        {
            rightPosition = GetFarstReelPosition(SelectReel.RIGHT, role);


            lblArray.Text += "   RIGHT:" + rightPosition.ToString() + " , " + GetNowReelPosition(SelectReel.RIGHT);
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
                UpReelPosition(SelectReel.LEFT, leftPosition);
                dispReelsSymbols(SelectReel.LEFT);


                UpReelPosition(SelectReel.CENTER, centerPosition);
                dispReelsSymbols(SelectReel.CENTER);

                UpReelPosition(SelectReel.RIGHT, rightPosition);
                dispReelsSymbols(SelectReel.RIGHT);
            }

        }

        
    }
}