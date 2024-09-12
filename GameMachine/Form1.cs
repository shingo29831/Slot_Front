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
            role = Constants.Role.BIG;
            int position = Game.GetFarstReelPosition(Constants.SelectReel.LEFT, role);

            int nowReelPosition = Game.GetNowReelPosition(Constants.SelectReel.LEFT);
            int reelPosition = NONE;
            int[] reelOrder = Game.GetReelOrder(Constants.SelectReel.LEFT); //�I�����ꂽ���[���̃V���{���z����Q�Ɠn������
            int[] symbolCandidate = { NONE, NONE, NONE, NONE, NONE, NONE, NONE };//�X�g�b�v�{�^�������������_�ŕ\�����銊��4�܂߂�����v�f�ԍ���
            int[] symbolsAccordingRole = { NONE, NONE };//role��B���ł���V���{�����i�[����
            int[] stopCandidate = { NONE, NONE };
            int searchReelPosition = nowReelPosition;


            Random rnd = new Random();

            lblArray.Text = "";

            //���݂̃��[���̃|�W�V�����ŉ�������7�����Ƃ��đ��
            for (int i = 0; i < symbolCandidate.Length; i++)
            {
                symbolCandidate[i] = searchReelPosition;
                searchReelPosition = Game.CalcReelPosition(searchReelPosition, 1);
            }



            //���𐬗�������V���{�������A4�ȉ��̃��[���͂��̂܂܃V���{���Ƃ��đ���AREG��BIG��7�Ƒ����Ƃ���BAR����
            if (role <= 4)
            {
                symbolsAccordingRole[0] = role;
            }
            else if (role == 6 || role == 7)
            {
                symbolsAccordingRole[0] = Constants.Symbol.SEVEN;
                symbolsAccordingRole[1] = Constants.Symbol.BAR;
            }

            

            //REG��BIG�̏ꍇ7�͑����7�����Ȃ������ꍇ��7��Ԃ��A���̖��͑����
            for (int j = symbolCandidate.Length - 1; j >= 0; j--) //���ƂȂ�V���{���̐����s�A���݈ʒu���牓����
            {
                if (reelOrder[symbolCandidate[j]] == symbolsAccordingRole[0]) //�~�܂����̃V���{���ƃ��[���𐬗�������V���{�����r
                {
                    stopCandidate[0] = reelOrder[symbolCandidate[j]]; //��~���ɑ��
                    reelPosition = symbolCandidate[j];
                    lblArray.Text += "Candi[" + j + "]:" + reelPosition + " ";
                }
                else if (reelOrder[symbolCandidate[j]] == symbolsAccordingRole[1])
                {
                    stopCandidate[1] = symbolCandidate[j];
                }
            }



            if (reelPosition == NONE && stopCandidate[1] != NONE) //���[���̑���~��₪�Ȃ������ꍇ�ɑ�����������
            {
                reelPosition = stopCandidate[1];
            }
            else if (reelPosition == NONE) //���,����~��₪�Ȃ������ꍇ�A���݂̈ʒu����
            {
                reelPosition = nowReelPosition;
            }
            if (reelPosition == Game.CalcReelPosition(nowReelPosition, 6)) //�I�����ꂽ�V���{�������̂Ȃ��ł����Ƃ��������A�ړI�̃V���{���������ɗ��Ȃ��悤�Ɏ~�܂���2�߂�
            {
                reelPosition = Game.CalcReelPosition(reelPosition, -2);
            }
            else if (reelPosition != nowReelPosition) //��{�I�ɑI�΂ꂽ�V���{�����I�������ɗ��邽�ߐ^�񒆂ɂ���悤�Ɉ�߂�
            {
                reelPosition = Game.CalcReelPosition(reelPosition, -1);
            }

            position = reelPosition;


            lblArray.Text += "   " + changeToName(role) + " , " + position.ToString() + " , " + Game.GetNowReelPosition(Constants.SelectReel.LEFT);
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
                case 6:
                    value = "REGULAR";
                    break;
                case 7:
                    value = "BIG";
                    break;
            }
            return value;
        }


    }
}