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
            int[] reelOrder = GetReelOrder(selectReel); //選択されたリールのシンボル配列を参照渡しする
            int[] symbolCandidate = { NONE, NONE, NONE, NONE, NONE, NONE, NONE }; //ストップボタンを押した時点で表示する滑り4つ含めた候補を要素番号で
            int[] symbolsAccordingRole = { NONE, NONE }; //roleを達成できるシンボルを格納する
            int[] stopCandidate = { NONE, NONE }; //roleを達成できるシンボルの位置を格納する 要素番号１はREGとBIGで7とBARで使用
            int searchReelPosition = nowReelPosition;

            int maxExclusion = NONE; //除外範囲の最大値を-1(リールより小さい値)に設定
            int minExclusion = 21; //除外範囲の最小値をリールより大きい値に設定

            int oneBeforePosition = NONE;
            int twoBeforePosition = NONE;

            bool cherryFounded = false;
            bool isLeft = false;
            bool isPositionFounded = false;

            Random rnd = new Random();

            if (selectReel == LEFT)
            {
                isLeft = true; //レフトリールフラグをtrue
            }

            //現在のリールのポジションで下から上に7つを候補として代入
            for (int i = 0; i < symbolCandidate.Length; i++)
            {
                if (reelOrder[CalcReelPosition(searchReelPosition, i)] == Symbol.CHERRY && isLeft && cherryFounded == false && (role < Role.WEAK_CHERRY || role > Role.VERY_STRONG_CHERRY))
                {
                    cherryFounded = true; //チェリー発見フラグをtrue
                    maxExclusion = searchReelPosition; //チェリーシンボルがある位置を候補除外範囲の最大値として代入
                    minExclusion = CalcReelPosition(searchReelPosition, -2); //チェリーシンボルから2つ下を候補除外範囲の最小値として代入
                }
                symbolCandidate[i] = searchReelPosition;
                searchReelPosition = CalcReelPosition(searchReelPosition, 1);
            }



            if (isLeft && cherryFounded && maxExclusion < minExclusion)//役がチェリー以外の時、最大値と最小値が反対なら交換する
            {
                int tmp = maxExclusion;
                maxExclusion = minExclusion;
                minExclusion = tmp;
            }



            //役を成立させるシンボルを代入、4以下(ボーナス以外)のロールはそのままシンボルとして代入、強・最強チェリーはCHERRYを代入REGとBIGは7と第二候補としてBARを代入
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




            //REGとBIGの場合7は即代入7が来なかった場合は7を返す、他の役は即代入
            for (int j = symbolCandidate.Length - 1; j >= 0; j--) //候補となるシンボルの数実行、現在位置から遠い順
            {
                if (reelOrder[symbolCandidate[j]] == symbolsAccordingRole[0] && //止まり先候補のシンボルとロールを成立させるシンボルを比較
                    (symbolCandidate[j] > maxExclusion || symbolCandidate[j] < minExclusion)) //また除外範囲外であるか比較 
                {
                    stopCandidate[0] = symbolCandidate[j]; //停止候補に代入
                }
                else if (reelOrder[symbolCandidate[j]] == symbolsAccordingRole[1] &&
                    (symbolCandidate[j] > maxExclusion || symbolCandidate[j] < minExclusion)) //また除外範囲外であるか比較
                {
                    stopCandidate[1] = symbolCandidate[j];
                }
            }
            oneBeforePosition = CalcReelPosition(stopCandidate[0], -1); //ひとつ前のポジションを代入
            twoBeforePosition = CalcReelPosition(stopCandidate[0], -2); //ふたつ前のポジションを代入
            cherryFounded = false;
            for (int i = 0; i <= 3 && isPositionFounded == false; i++)
            {
                
                if ((CalcReelPosition(nowReelPosition, i) > maxExclusion || CalcReelPosition(nowReelPosition, i) < minExclusion)) //押下した時点から+3～0の地点が除外範囲外の時
                {
                    reelPosition = CalcReelPosition(nowReelPosition, i);
                    isPositionFounded = true;
                }
            }

            if (stopCandidate[0] == CalcReelPosition(nowReelPosition, 6) && stopCandidate[0] != NONE && //移動先がもっとも遠く第1候補がある時
                (oneBeforePosition > maxExclusion || oneBeforePosition < minExclusion) &&
                (twoBeforePosition > maxExclusion || twoBeforePosition < minExclusion)) //一つ前と二つ前の位置が除外範囲外の時
            {
                reelPosition = twoBeforePosition; //選択されたシンボルが候補のなかでもっとも遠い時、目的のシンボルが中央に来ないように止まり先を2つ戻す
            }
            else if (stopCandidate[0] != nowReelPosition && stopCandidate[0] != NONE && //移動前と移動先のリール位置が不一致で、第1候補がある時
                (oneBeforePosition > maxExclusion || oneBeforePosition < minExclusion)) //またひとつ前が除外範囲外の時
            {
                reelPosition = oneBeforePosition;
            }
            else if (stopCandidate[0] != NONE &&
                (stopCandidate[0] > maxExclusion || stopCandidate[0] < minExclusion)) //候補が除外範囲外の時
            {
                reelPosition = stopCandidate[0];
            }
            else if (stopCandidate[1] == CalcReelPosition(nowReelPosition, 6) && stopCandidate[1] != NONE && //移動先がもっとも遠く第2候補がある時
                (oneBeforePosition > maxExclusion || oneBeforePosition < minExclusion) &&
                (twoBeforePosition > maxExclusion || twoBeforePosition < minExclusion)) //一つ前と二つ前の位置が除外範囲外の時
            {
                reelPosition = twoBeforePosition; //選択されたシンボルが候補のなかでもっとも遠い時、目的のシンボルが中央に来ないように止まり先を2つ戻す
            }
            else if (stopCandidate[1] != nowReelPosition && stopCandidate[1] != NONE && //移動前と移動先のリール位置が不一致で、第2候補がある時
                (oneBeforePosition > maxExclusion || oneBeforePosition < minExclusion)) //またひとつ前が除外範囲外の時
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

        //定数で選択
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