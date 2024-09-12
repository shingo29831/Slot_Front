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
            int[] reelOrder = Game.GetReelOrder(Constants.SelectReel.LEFT); //選択されたリールのシンボル配列を参照渡しする
            int[] symbolCandidate = { NONE, NONE, NONE, NONE, NONE, NONE, NONE };//ストップボタンを押した時点で表示する滑り4つ含めた候補を要素番号で
            int[] symbolsAccordingRole = { NONE, NONE };//roleを達成できるシンボルを格納する
            int[] stopCandidate = { NONE, NONE };
            int searchReelPosition = nowReelPosition;


            Random rnd = new Random();

            lblArray.Text = "";

            //現在のリールのポジションで下から上に7つを候補として代入
            for (int i = 0; i < symbolCandidate.Length; i++)
            {
                symbolCandidate[i] = searchReelPosition;
                searchReelPosition = Game.CalcReelPosition(searchReelPosition, 1);
            }



            //役を成立させるシンボルを代入、4以下のロールはそのままシンボルとして代入、REGとBIGは7と第二候補としてBARを代入
            if (role <= 4)
            {
                symbolsAccordingRole[0] = role;
            }
            else if (role == 6 || role == 7)
            {
                symbolsAccordingRole[0] = Constants.Symbol.SEVEN;
                symbolsAccordingRole[1] = Constants.Symbol.BAR;
            }

            

            //REGとBIGの場合7は即代入7が来なかった場合は7を返す、他の役は即代入
            for (int j = symbolCandidate.Length - 1; j >= 0; j--) //候補となるシンボルの数実行、現在位置から遠い順
            {
                if (reelOrder[symbolCandidate[j]] == symbolsAccordingRole[0]) //止まり先候補のシンボルとロールを成立させるシンボルを比較
                {
                    stopCandidate[0] = reelOrder[symbolCandidate[j]]; //停止候補に代入
                    reelPosition = symbolCandidate[j];
                    lblArray.Text += "Candi[" + j + "]:" + reelPosition + " ";
                }
                else if (reelOrder[symbolCandidate[j]] == symbolsAccordingRole[1])
                {
                    stopCandidate[1] = symbolCandidate[j];
                }
            }



            if (reelPosition == NONE && stopCandidate[1] != NONE) //リールの第一停止候補がなかった場合に第二候補を代入する
            {
                reelPosition = stopCandidate[1];
            }
            else if (reelPosition == NONE) //第一,第二停止候補がなかった場合、現在の位置を代入
            {
                reelPosition = nowReelPosition;
            }
            if (reelPosition == Game.CalcReelPosition(nowReelPosition, 6)) //選択されたシンボルが候補のなかでもっとも遠い時、目的のシンボルが中央に来ないように止まり先を2つ戻す
            {
                reelPosition = Game.CalcReelPosition(reelPosition, -2);
            }
            else if (reelPosition != nowReelPosition) //基本的に選ばれたシンボルが選択時下に来るため真ん中にくるように一つ戻す
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