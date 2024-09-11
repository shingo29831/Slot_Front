using System;
using System.Security.Cryptography;
using System.Net.NetworkInformation;
using System.Text;
using System.Diagnostics;
using Model;

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


        int leftNowReelPosition = Game.GetNowReelPosition(Constants.SelectReel.LEFT);
        int[] leftReelOrder = Game.GetReelOrder(Constants.SelectReel.LEFT);
        int[] centerReelOrder = Game.GetReelOrder(Constants.SelectReel.CENTER);
        int[] rightReelOrder = Game.GetReelOrder(Constants.SelectReel.LEFT);

        int bonusState = Constants.Bonus.NONE;

        int role = Constants.Role.NONE;


        String uniqueID = "";
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Setting.maketableID();
            txtbox1.Text = Setting.getTableID();




            leftReelBot.Text = leftReelOrder[leftNowReelPosition].ToString();
            leftReelMid.Text = leftReelOrder[leftNowReelPosition + 1].ToString();
            leftReelTop.Text = leftReelOrder[leftNowReelPosition + 2].ToString();


        }


        private void button1_Click(object sender, EventArgs e)
        {
            Game.UpReelPosition(Constants.SelectReel.LEFT);
            leftReelBot.Text = leftReelOrder[Game.GetDispSymbol(Constants.SelectReel.LEFT, Constants.Position.BOTTOM)].ToString();
            leftReelMid.Text = leftReelOrder[Game.GetDispSymbol(Constants.SelectReel.LEFT, Constants.Position.MIDDLE)].ToString();
            leftReelTop.Text = leftReelOrder[Game.GetDispSymbol(Constants.SelectReel.LEFT, Constants.Position.TOP)].ToString();


            Game.UpReelPosition(Constants.SelectReel.CENTER);
            centerReelBot.Text = centerReelOrder[Game.GetDispSymbol(Constants.SelectReel.CENTER, Constants.Position.BOTTOM)].ToString();
            centerReelMid.Text = centerReelOrder[Game.GetDispSymbol(Constants.SelectReel.CENTER, Constants.Position.MIDDLE)].ToString();
            centerReelTop.Text = centerReelOrder[Game.GetDispSymbol(Constants.SelectReel.CENTER, Constants.Position.TOP)].ToString();

            Game.UpReelPosition(Constants.SelectReel.RIGHT);
            rightReelBot.Text = rightReelOrder[Game.GetDispSymbol(Constants.SelectReel.RIGHT, Constants.Position.BOTTOM)].ToString();
            rightReelMid.Text = rightReelOrder[Game.GetDispSymbol(Constants.SelectReel.RIGHT, Constants.Position.MIDDLE)].ToString();
            rightReelTop.Text = rightReelOrder[Game.GetDispSymbol(Constants.SelectReel.RIGHT, Constants.Position.TOP)].ToString();


        }

        private void button2_Click(object sender, EventArgs e)
        {
            role = Game.HitRoleLottery();

            lblArray.Text = changeToName(role);
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
                    value = "CHERRY";
                    break;
                case 5:
                    value = "SEVEN";
                    break;
                case 6:
                    value = "BAR";
                    break;
            }
            return value;
        }


    }
}