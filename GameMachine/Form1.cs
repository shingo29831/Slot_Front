using System;
using System.Security.Cryptography;
using System.Net.NetworkInformation;
using System.Text;
using System.Diagnostics;
using Model;
using static Constants;
using static Model.Game;
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
                lblArray.Text = "ROLE:" + changeToName(role);
                leftPosition = NONE;
                centerPosition = NONE;
                rightPosition = NONE;
                timer1.Enabled = true;
            }



        }

        private void button2_Click(object sender, EventArgs e)
        {
            int selectReel = LEFT;
            int nowReelPosition = GetNowReelPosition(selectReel);
            int reelPosition = NONE;
            int[] reelOrder = GetReelOrder(selectReel); //�I�����ꂽ���[���̃V���{���z����Q�Ɠn������
            int[] symbolCandidate = { NONE, NONE, NONE, NONE, NONE, NONE, NONE }; //�X�g�b�v�{�^�������������_�ŕ\�����銊��4�܂߂�����v�f�ԍ���
            int[] symbolsAccordingRole = { NONE, NONE }; //role��B���ł���V���{�����i�[����
            int[] stopCandidate = { NONE, NONE }; //role��B���ł���V���{���̈ʒu���i�[���� �v�f�ԍ��P��REG��BIG��7��BAR�Ŏg�p
            int searchReelPosition = nowReelPosition;

            int maxExclusion = NONE; //���O�͈͂̍ő�l��-1(���[����菬�����l)�ɐݒ�
            int minExclusion = 21; //���O�͈͂̍ŏ��l�����[�����傫���l�ɐݒ�

            int oneBeforePosition = NONE;
            int twoBeforePosition = NONE;

            bool cherryFounded = false;
            bool isLeft = false;
            bool isPositionFounded = false;

            Random rnd = new Random();

            if (selectReel == LEFT)
            {
                isLeft = true; //���t�g���[���t���O��true
            }

            //���݂̃��[���̃|�W�V�����ŉ�������7�����Ƃ��đ��
            for (int i = 0; i < symbolCandidate.Length; i++)
            {
                if (reelOrder[CalcReelPosition(searchReelPosition, i)] == Symbol.CHERRY && isLeft && cherryFounded == false && (role < Role.WEAK_CHERRY || role > Role.VERY_STRONG_CHERRY))
                {
                    cherryFounded = true; //�`�F���[�����t���O��true
                    maxExclusion = searchReelPosition; //�`�F���[�V���{��������ʒu����⏜�O�͈͂̍ő�l�Ƃ��đ��
                    minExclusion = CalcReelPosition(searchReelPosition, -2); //�`�F���[�V���{������2������⏜�O�͈͂̍ŏ��l�Ƃ��đ��
                }
                symbolCandidate[i] = searchReelPosition;
                searchReelPosition = CalcReelPosition(searchReelPosition, 1);
            }



            if (isLeft && cherryFounded && maxExclusion < minExclusion)//�����`�F���[�ȊO�̎��A�ő�l�ƍŏ��l�����΂Ȃ��������
            {
                int tmp = maxExclusion;
                maxExclusion = minExclusion;
                minExclusion = tmp;
            }



            //���𐬗�������V���{�������A4�ȉ�(�{�[�i�X�ȊO)�̃��[���͂��̂܂܃V���{���Ƃ��đ���A���E�ŋ��`�F���[��CHERRY����REG��BIG��7�Ƒ����Ƃ���BAR����
            if (role <= 4)
            {
                symbolsAccordingRole[0] = role;
            }
            else if (role <= Role.OTHER_BONUS)
            {
                symbolsAccordingRole[0] = Symbol.CHERRY;
            }
            else if (role == Role.REGULAR || role == Role.BIG)
            {
                symbolsAccordingRole[0] = Symbol.SEVEN;
                symbolsAccordingRole[1] = Symbol.BAR;
            }




            //REG��BIG�̏ꍇ7�͑����7�����Ȃ������ꍇ��7��Ԃ��A���̖��͑����
            for (int j = symbolCandidate.Length - 1; j >= 0; j--) //���ƂȂ�V���{���̐����s�A���݈ʒu���牓����
            {
                if (reelOrder[symbolCandidate[j]] == symbolsAccordingRole[0] && //�~�܂����̃V���{���ƃ��[���𐬗�������V���{�����r
                    (symbolCandidate[j] > maxExclusion || symbolCandidate[j] < minExclusion)) //�܂����O�͈͊O�ł��邩��r 
                {
                    stopCandidate[0] = symbolCandidate[j]; //��~���ɑ��
                }
                else if (reelOrder[symbolCandidate[j]] == symbolsAccordingRole[1] &&
                    (symbolCandidate[j] > maxExclusion || symbolCandidate[j] < minExclusion)) //�܂����O�͈͊O�ł��邩��r
                {
                    stopCandidate[1] = symbolCandidate[j];
                }
            }
            oneBeforePosition = CalcReelPosition(stopCandidate[0], -1); //�ЂƂO�̃|�W�V��������
            twoBeforePosition = CalcReelPosition(stopCandidate[0], -2); //�ӂ��O�̃|�W�V��������
            cherryFounded = false;
            for (int i = 0; i <= 3 && isPositionFounded == false; i++)
            {
                
                if ((CalcReelPosition(nowReelPosition, i) > maxExclusion || CalcReelPosition(nowReelPosition, i) < minExclusion)) //�����������_����+3�`0�̒n�_�����O�͈͊O�̎�
                {
                    reelPosition = CalcReelPosition(nowReelPosition, i);
                    isPositionFounded = true;
                }
            }

            if (stopCandidate[0] == CalcReelPosition(nowReelPosition, 6) && stopCandidate[0] != NONE && //�ړ��悪�����Ƃ�������1��₪���鎞
                (oneBeforePosition > maxExclusion || oneBeforePosition < minExclusion) &&
                (twoBeforePosition > maxExclusion || twoBeforePosition < minExclusion)) //��O�Ɠ�O�̈ʒu�����O�͈͊O�̎�
            {
                reelPosition = twoBeforePosition; //�I�����ꂽ�V���{�������̂Ȃ��ł����Ƃ��������A�ړI�̃V���{���������ɗ��Ȃ��悤�Ɏ~�܂���2�߂�
            }
            else if (stopCandidate[0] != nowReelPosition && stopCandidate[0] != NONE && //�ړ��O�ƈړ���̃��[���ʒu���s��v�ŁA��1��₪���鎞
                (oneBeforePosition > maxExclusion || oneBeforePosition < minExclusion)) //�܂��ЂƂO�����O�͈͊O�̎�
            {
                reelPosition = oneBeforePosition;
            }
            else if (stopCandidate[0] != NONE &&
                (stopCandidate[0] > maxExclusion || stopCandidate[0] < minExclusion)) //��₪���O�͈͊O�̎�
            {
                reelPosition = stopCandidate[0];
            }
            else if (stopCandidate[1] == CalcReelPosition(nowReelPosition, 6) && stopCandidate[1] != NONE && //�ړ��悪�����Ƃ�������2��₪���鎞
                (oneBeforePosition > maxExclusion || oneBeforePosition < minExclusion) &&
                (twoBeforePosition > maxExclusion || twoBeforePosition < minExclusion)) //��O�Ɠ�O�̈ʒu�����O�͈͊O�̎�
            {
                reelPosition = twoBeforePosition; //�I�����ꂽ�V���{�������̂Ȃ��ł����Ƃ��������A�ړI�̃V���{���������ɗ��Ȃ��悤�Ɏ~�܂���2�߂�
            }
            else if (stopCandidate[1] != nowReelPosition && stopCandidate[1] != NONE && //�ړ��O�ƈړ���̃��[���ʒu���s��v�ŁA��2��₪���鎞
                (oneBeforePosition > maxExclusion || oneBeforePosition < minExclusion)) //�܂��ЂƂO�����O�͈͊O�̎�
            {
                reelPosition = oneBeforePosition;
            }
            else if (stopCandidate[1] != NONE &&
                (stopCandidate[1] > maxExclusion || stopCandidate[1] < minExclusion))
            {
                reelPosition = stopCandidate[1];
            }

            leftPosition = reelPosition;

            lblArray.Text += "   CherryMax:" + maxExclusion.ToString() + " , CherryMin" + minExclusion.ToString() + " , next:" + reelPosition + " , now:" +  GetNowReelPosition(SelectReel.LEFT);

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

        //�萔�őI��
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
            leftPosition = Test(SelectReel.LEFT, role);


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