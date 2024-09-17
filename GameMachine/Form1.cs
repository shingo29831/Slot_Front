using System;
using System.Security.Cryptography;
using System.Net.NetworkInformation;
using System.Text;
using System.Diagnostics;
using Model;
using static Constants;
using System.Reflection;

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


        int leftNowReelPosition = Game.GetNowReelPosition(SelectReel.LEFT);
        int[] leftReelOrder = Game.GetReelOrder(SelectReel.LEFT);
        int[] centerReelOrder = Game.GetReelOrder(SelectReel.CENTER);
        int[] rightReelOrder = Game.GetReelOrder(SelectReel.RIGHT);

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
            lblArray.Text = "";
            Game.GetReachRows(leftPosition,centerPosition,rightPosition);


            



            for (int i = 0; i < 3; i++)
            {
                lblArray.Text += "{";
                for(int j = 0; j < 2; j++)
                {
                    lblArray.Text += "{" + Game.reachRows[i,j].ToString() + "}";
                }
                lblArray.Text += "}";
            }


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
                    value = "BIG";
                    break;
            }
            return value;
        }

        //’è”‚Å‘I‘ð
        private void dispReelsSymbols(int selectReel)
        {
            switch (selectReel)
            {
                case 1:
                    leftReelBot.Text = leftReelOrder[Game.GetDispSymbol(SelectReel.LEFT, Position.BOTTOM)].ToString();
                    leftReelMid.Text = leftReelOrder[Game.GetDispSymbol(SelectReel.LEFT, Position.MIDDLE)].ToString();
                    leftReelTop.Text = leftReelOrder[Game.GetDispSymbol(SelectReel.LEFT, Position.TOP)].ToString();
                    break;

                case 2:
                    centerReelBot.Text = centerReelOrder[Game.GetDispSymbol(SelectReel.CENTER, Position.BOTTOM)].ToString();
                    centerReelMid.Text = centerReelOrder[Game.GetDispSymbol(SelectReel.CENTER, Position.MIDDLE)].ToString();
                    centerReelTop.Text = centerReelOrder[Game.GetDispSymbol(SelectReel.CENTER, Position.TOP)].ToString();
                    break;

                case 3:
                    rightReelBot.Text = rightReelOrder[Game.GetDispSymbol(SelectReel.RIGHT, Position.BOTTOM)].ToString();
                    rightReelMid.Text = rightReelOrder[Game.GetDispSymbol(SelectReel.RIGHT, Position.MIDDLE)].ToString();
                    rightReelTop.Text = rightReelOrder[Game.GetDispSymbol(SelectReel.RIGHT, Position.TOP)].ToString();
                    break;
            }
        }

        private void leftStop_Click(object sender, EventArgs e)
        {
            leftPosition = Game.GetFarstReelPosition(SelectReel.LEFT, role);


            lblArray.Text += "   LEFT:" + leftPosition.ToString() + " , " + Game.GetNowReelPosition(SelectReel.LEFT);
            stopBtnCount++;
            if (stopBtnCount == 3)
            {
                stopBtnCount = 0;
            }
        }

        private void centerStop_Click(object sender, EventArgs e)
        {
            centerPosition = Game.GetFarstReelPosition(SelectReel.CENTER, role);


            lblArray.Text += "   CENTER:" + centerPosition.ToString() + " , " + Game.GetNowReelPosition(SelectReel.CENTER);
            stopBtnCount++;
            if (stopBtnCount == 3)
            {
                stopBtnCount = 0;
            }
        }

        private void rightStop_Click(object sender, EventArgs e)
        {
            rightPosition = Game.GetFarstReelPosition(SelectReel.RIGHT, role);


            lblArray.Text += "   RIGHT:" + rightPosition.ToString() + " , " + Game.GetNowReelPosition(SelectReel.RIGHT);
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
                Game.UpReelPosition(SelectReel.LEFT, leftPosition);
                dispReelsSymbols(SelectReel.LEFT);


                Game.UpReelPosition(SelectReel.CENTER, centerPosition);
                dispReelsSymbols(SelectReel.CENTER);

                Game.UpReelPosition(SelectReel.RIGHT, rightPosition);
                dispReelsSymbols(SelectReel.RIGHT);
            }

        }
    }
}