using System;
using System.Security.Cryptography;
using System.Net.NetworkInformation;
using System.Text;
using System.Diagnostics;
using Model;
using static Constants;

namespace GameMachine
{
    public partial class Form1 : Form
    {
        public static readonly int NONE = -1;
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


        int leftNowReelPosition = Game.GetNowReelPosition(Constants.SelectReel.LEFT);
        int[] leftReelOrder = Game.GetReelOrder(Constants.SelectReel.LEFT);
        int[] centerReelOrder = Game.GetReelOrder(Constants.SelectReel.CENTER);
        int[] rightReelOrder = Game.GetReelOrder(Constants.SelectReel.RIGHT);

        int bonusState = Constants.Bonus.NONE;

        int role = Constants.Role.NONE;
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


            dispReelsSymbols(Constants.SelectReel.LEFT);
            dispReelsSymbols(Constants.SelectReel.CENTER);
            dispReelsSymbols(Constants.SelectReel.RIGHT);


        }


        private void button1_Click(object sender, EventArgs e)
        {
            if (stopBtnCount == 0)
            {
                role = Game.HitRoleLottery();
                lblArray.Text = "ROLE:" + changeToName(role);
                leftPosition = NONE;
                centerPosition = NONE;
                rightPosition = NONE;
                timer1.Enabled = true;
            }



        }

        private void button2_Click(object sender, EventArgs e)
        {
            role = Game.HitRoleLottery();



            lblArray.Text = "ROLE:" + changeToName(role);
        }

        private String changeToName(int symbolNum)
        {
            String value = "";
            switch (symbolNum)
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
                    leftReelBot.Text = leftReelOrder[Game.GetDispSymbol(Constants.SelectReel.LEFT, Constants.Position.BOTTOM)].ToString();
                    leftReelMid.Text = leftReelOrder[Game.GetDispSymbol(Constants.SelectReel.LEFT, Constants.Position.MIDDLE)].ToString();
                    leftReelTop.Text = leftReelOrder[Game.GetDispSymbol(Constants.SelectReel.LEFT, Constants.Position.TOP)].ToString();
                    break;

                case 2:
                    centerReelBot.Text = centerReelOrder[Game.GetDispSymbol(Constants.SelectReel.CENTER, Constants.Position.BOTTOM)].ToString();
                    centerReelMid.Text = centerReelOrder[Game.GetDispSymbol(Constants.SelectReel.CENTER, Constants.Position.MIDDLE)].ToString();
                    centerReelTop.Text = centerReelOrder[Game.GetDispSymbol(Constants.SelectReel.CENTER, Constants.Position.TOP)].ToString();
                    break;

                case 3:
                    rightReelBot.Text = rightReelOrder[Game.GetDispSymbol(Constants.SelectReel.RIGHT, Constants.Position.BOTTOM)].ToString();
                    rightReelMid.Text = rightReelOrder[Game.GetDispSymbol(Constants.SelectReel.RIGHT, Constants.Position.MIDDLE)].ToString();
                    rightReelTop.Text = rightReelOrder[Game.GetDispSymbol(Constants.SelectReel.RIGHT, Constants.Position.TOP)].ToString();
                    break;
            }
        }

        private void leftStop_Click(object sender, EventArgs e)
        {
            leftPosition = Game.GetFarstReelPosition(Constants.SelectReel.LEFT, role);


            lblArray.Text += "   LEFT:" + leftPosition.ToString() + " , " + Game.GetNowReelPosition(Constants.SelectReel.LEFT);
            stopBtnCount++;
            if (stopBtnCount == 3)
            {
                stopBtnCount = 0;
            }
        }

        private void centerStop_Click(object sender, EventArgs e)
        {
            centerPosition = Game.GetFarstReelPosition(Constants.SelectReel.CENTER, role);


            lblArray.Text += "   CENTER:" + centerPosition.ToString() + " , " + Game.GetNowReelPosition(Constants.SelectReel.CENTER);
            stopBtnCount++;
            if (stopBtnCount == 3)
            {
                stopBtnCount = 0;
            }
        }

        private void rightStop_Click(object sender, EventArgs e)
        {
            rightPosition = Game.GetFarstReelPosition(Constants.SelectReel.RIGHT, role);


            lblArray.Text += "   RIGHT:" + rightPosition.ToString() + " , " + Game.GetNowReelPosition(Constants.SelectReel.RIGHT);
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
                Game.UpReelPosition(Constants.SelectReel.LEFT, leftPosition);
                dispReelsSymbols(Constants.SelectReel.LEFT);


                Game.UpReelPosition(Constants.SelectReel.CENTER, centerPosition);
                dispReelsSymbols(Constants.SelectReel.CENTER);

                Game.UpReelPosition(Constants.SelectReel.RIGHT, rightPosition);
                dispReelsSymbols(Constants.SelectReel.RIGHT);
            }

        }
    }
}