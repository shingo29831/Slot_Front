using static Constants;
using static GameMachine.Model.Game;
using static GameMachine.Model.Setting;

namespace GameMachine
{
    public partial class TestForm : Form
    {
        bool leftStopBtn = false;
        bool centerStopBtn = false;
        bool rightStopBtn = false;


        sbyte bonusState = NONE;

        sbyte role = NONE;
        static sbyte leftPosition = 14;
        static sbyte centerPosition = 15;
        static sbyte rightPosition = 9;

        static sbyte leftNextPosition = NONE;
        static sbyte centerNextPosition = NONE;
        static sbyte rightNextPosition = NONE;

        sbyte stopBtnCount = 0;

        String uniqueID = "";

        bool stopAndResult = false;
        int lcnt = 0;
        public TestForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            MakeTableID();
            txtbox1.Text = GetTableID();


            dispReelsSymbols(Reels.LEFT);
            dispReelsSymbols(Reels.CENTER);
            dispReelsSymbols(Reels.RIGHT);


        }

        int icnt = 0;
        private void button1_Click(object sender, EventArgs e) //本番は約35で回す
        {

            PlayProcess();
            timer1.Enabled = true;
            stopAndResult = false;

        }


        
        private void button2_Click(object sender, EventArgs e)
        {

            lblArray.Text = GetBonusPosition(Reels.LEFT).ToString() +  " , " + GetBonusPosition(Reels.CENTER).ToString()  +" , "+ GetBonusPosition(Reels.RIGHT).ToString();

        }

        private static void SetNextReelPosition(in Reels selectReel, sbyte reelPosition)
        {
            switch (selectReel)
            {
                case Reels.LEFT:
                    leftNextPosition = reelPosition;
                    break;
                case Reels.CENTER:
                    centerNextPosition = reelPosition;
                    break;
                case Reels.RIGHT:
                    rightNextPosition = reelPosition;
                    break;
            }
        }


        private static sbyte GetFormNextReelPosition(in Reels selectReel)
        {
            switch (selectReel)
            {
                case Reels.LEFT:
                    return leftNextPosition;
                case Reels.CENTER:
                    return centerNextPosition;
                case Reels.RIGHT:
                    return rightNextPosition;
            }
            return 0;
        }



        private static void SetReelPosition(in Reels selectReel, sbyte reelPosition)
        {
            switch (selectReel)
            {
                case Reels.LEFT:
                    leftPosition = reelPosition;
                    break;
                case Reels.CENTER:
                    centerPosition = reelPosition;
                    break;
                case Reels.RIGHT:
                    rightPosition = reelPosition;
                    break;
            }
        }


        private static sbyte GetReelPosition(in Reels selectReel)
        {
            switch (selectReel)
            {
                case Reels.LEFT:
                    return leftPosition;
                case Reels.CENTER:
                    return centerPosition;
                case Reels.RIGHT:
                    return rightPosition;
            }
            return 0;
        }



        public static String RoleChangeToName(Roles role)
        {
            String value = "";
            switch (role)
            {
                case Roles.NONE:
                    value = "NONE";
                    break;
                case Roles.BELL:
                    value = "BELL";
                    break;
                case Roles.REPLAY:
                    value = "REPLAY";
                    break;
                case Roles.WATERMELON:
                    value = "WATERMELON";
                    break;
                case Roles.WEAK_CHERRY:
                    value = "WEAK_CHERRY";
                    break;
                case Roles.STRONG_CHERRY:
                    value = "STRONG_CHERRY";
                    break;
                case Roles.VERY_STRONG_CHERRY:
                    value = "VERY_STRONG_CHERRY";
                    break;
                case Roles.REGULAR:
                    value = "REG";
                    break;
                case Roles.BIG:
                    value = "BIG";
                    break;
            }
            return value;
        }


        private String PositionsChangeToNames(Positions positions)
        {
            String value = "";
            if (positions.HasFlag(Positions.TOP))
            {
                value += " ＆ TOP";
            }
            if (positions.HasFlag(Positions.MIDDLE))
            {
                value += " ＆ MIDDLE";
            }
            if (positions.HasFlag(Positions.BOTTOM))
            {
                value += " ＆ BOTTOM";
            }
            return value;
        }


        private String SymbolsChangeToNames(Symbols symbols)
        {
            String value = "";

            if (symbols.HasFlag(Symbols.BELL))
            {
                value += " ＆ BELL";
            }
            if (symbols.HasFlag(Symbols.REPLAY))
            {
                value += " ＆ REPLAY";
            }
            if (symbols.HasFlag(Symbols.WATERMELON))
            {
                value += " ＆ WATERMELON";
            }
            if (symbols.HasFlag(Symbols.CHERRY))
            {
                value += " ＆ CHERRY";
            }
            if (symbols.HasFlag(Symbols.BAR))
            {
                value += " ＆ BAR";
            }
            if (symbols.HasFlag(Symbols.SEVEN))
            {
                value += " ＆ SEVEN";
            }
            if (symbols.HasFlag(Symbols.REACH))
            {
                value += " ＆ REACH";
            }



            return value;
        }

        private String SymbolChangeToName(Symbols symbol)
        {
            String value = "";
            switch (symbol)
            {
                case Symbols.NONE:
                    value = "NONE";
                    break;
                case Symbols.BELL:
                    value = "BELL";
                    break;
                case Symbols.REPLAY:
                    value = "REPLAY";
                    break;
                case Symbols.WATERMELON:
                    value = "WATERMELON";
                    break;
                case Symbols.CHERRY:
                    value = "CHERRY";
                    break;
                case Symbols.BAR:
                    value = "BAR";
                    break;
                case Symbols.SEVEN:
                    value = "SEVEN";
                    break;
                case Symbols.REACH:
                    value = "REACH";
                    break;
            }
            return value;
        }

        //定数で選択
        private void dispReelsSymbols(Reels selectReel)
        {
            switch (selectReel)
            {
                case Reels.LEFT:
                    leftReelBot.Text = SymbolChangeToName(GetSymbol(Reels.LEFT, Positions.BOTTOM));
                    leftReelMid.Text = SymbolChangeToName(GetSymbol(Reels.LEFT, Positions.MIDDLE));
                    leftReelTop.Text = SymbolChangeToName(GetSymbol(Reels.LEFT, Positions.TOP));
                    break;

                case Reels.CENTER:
                    centerReelBot.Text = SymbolChangeToName(GetSymbol(Reels.CENTER, Positions.BOTTOM));
                    centerReelMid.Text = SymbolChangeToName(GetSymbol(Reels.CENTER, Positions.MIDDLE));
                    centerReelTop.Text = SymbolChangeToName(GetSymbol(Reels.CENTER, Positions.TOP));
                    break;

                case Reels.RIGHT:
                    rightReelBot.Text = SymbolChangeToName(GetSymbol(Reels.RIGHT, Positions.BOTTOM));
                    rightReelMid.Text = SymbolChangeToName(GetSymbol(Reels.RIGHT, Positions.MIDDLE));
                    rightReelTop.Text = SymbolChangeToName(GetSymbol(Reels.RIGHT, Positions.TOP));
                    break;
            }
        }

        private void leftStop_Click(object sender, EventArgs e)
        {


            PushStopReelPosition(Reels.LEFT);

        }

        private void centerStop_Click(object sender, EventArgs e)
        {

            PushStopReelPosition(Reels.CENTER);

        }

        private void rightStop_Click(object sender, EventArgs e)
        {

            PushStopReelPosition(Reels.RIGHT);

        }


        int tcnt = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            tcnt++;
            if (tcnt % 4 == 0 && tcnt < 13)
            {
                PlayProcess();
            }else if (tcnt == 13 && stopAndResult)
            {
                StopAndResult();
                tcnt = 0;
                timer1.Enabled = false;
            }
            else if (tcnt == 13 )
            {
                PlayProcess();
                tcnt = 0;
            }
            ShowDisplay();
            UpReel();





        }

        private void PushStopReelPosition(Reels selectReel)
        {
            sbyte reelPosition = GetReelPosition(selectReel);
            SetNowReelPosition(selectReel, reelPosition);
            lblArray.Text += " 始:" + reelPosition.ToString();
            sbyte nextReelPosition = CalcNextReelPosition(selectReel);
            SetNextReelPosition(selectReel, nextReelPosition);
            SetReelMoving(selectReel, false);
            lblArray.Text += " 返:" + GetNextReelPosition(selectReel).ToString();

            if (StopReelCount == 3)
            {
                String leftValue = "";
                String centerValue = "";
                String rightValue = "";
                foreach (Lines line in LINES_ARRAY)
                {
                    leftValue += " " + SymbolChangeToName(GetSymbolForLine(Reels.LEFT, line, GetNextReelPosition(Reels.LEFT)));
                    centerValue += " " + SymbolChangeToName(GetSymbolForLine(Reels.CENTER, line, GetNextReelPosition(Reels.CENTER)));
                    rightValue += " " + SymbolChangeToName(GetSymbolForLine(Reels.RIGHT, line, GetNextReelPosition(Reels.RIGHT)));
                }
                HitEstablishedRoles();
                CalcCoinReturned();
                SwitchingBonus();


                lblArray.Text += "  Role:" + RoleChangeToName(GetEstablishedRole()) + "  Coin:" + GetHasCoin().ToString();
                RoleInspection();
            }






        }

        private static Symbols GetSymbol(Reels selectReel, Positions position)
        {
            Symbols[] reelOrder = GetReelOrder(selectReel);
            sbyte reelPosition = GetReelPosition(selectReel);
            sbyte gap = 0;
            switch (position)
            {
                case Positions.TOP:
                    gap = 2;
                    break;
                case Positions.MIDDLE:
                    gap = 1;
                    break;
                case Positions.BOTTOM:
                    gap = 0;
                    break;
            }

            return reelOrder[CalcReelPosition(reelPosition, gap)];
        }


        //リールを一つずつ上げる、第二引数まで上げるがNONEの場合は無限に上げる
        private static void UpReelPosition(Reels selectReel, sbyte destinationPosition)
        {

            switch (selectReel)
            {
                case Reels.LEFT:
                    if (leftPosition != destinationPosition || destinationPosition == NONE)
                    {
                        leftPosition = CalcReelPosition(leftPosition, 1);
                    }

                    break;
                case Reels.CENTER:
                    if (centerPosition != destinationPosition || destinationPosition == NONE)
                    {
                        centerPosition = CalcReelPosition(centerPosition, 1);
                    }

                    break;
                case Reels.RIGHT:
                    if (rightPosition != destinationPosition || destinationPosition == NONE)
                    {
                        rightPosition = CalcReelPosition(rightPosition, 1);
                    }

                    break;
            }


        }


        private void PlayProcess()
        {
            if (StopReelCount == 3)
            {
                CalcCoinCollection();
                SetEstablishedRole(Roles.NONE);
                HitRolesLottery();
                BonusLottery();
                SetNextReelPosition(Reels.LEFT, NONE);
                SetNextReelPosition(Reels.CENTER, NONE);
                SetNextReelPosition(Reels.RIGHT, NONE);

                ResetReelsMoving();
                lblArray.Text = "Coin:" + GetHasCoin().ToString() + "  ROLE:" + RoleChangeToName(GetNowRole()) + " Bonus:" + RoleChangeToName(GetNowBonus());
                timer1.Enabled = true;
                icnt = 0;
            }

            switch (icnt)
            {
                case 1:
                    PushStopReelPosition(Reels.LEFT);
                    break;
                case 2:
                    PushStopReelPosition(Reels.CENTER);
                    break;
                case 3:
                    PushStopReelPosition(Reels.RIGHT);
                    break;
            }
            if (icnt > -1)
            {

                icnt++;
            }

            if (icnt == -1)
            {
                lblArray.Text = "ROLE:" + RoleChangeToName(GetNowRole());
                timer1.Enabled = true;
                icnt++;
            }
        }


        private void RoleInspection()
        {
            Roles nowRole = GetNowRole();
            Roles establishedRole = GetEstablishedRole();
            if (!(nowRole == Roles.NONE && establishedRole == Roles.NONE) && (!(nowRole == establishedRole || establishedRole == Roles.NONE)
                && !((nowRole == Roles.VERY_STRONG_CHERRY || nowRole == Roles.STRONG_CHERRY) && establishedRole == Roles.WEAK_CHERRY)))
            {
                tcnt = 0;
                timer1.Enabled = false;
            }
        }


        private void StopAndResult()
        {
            MessageBox.Show(tcnt.ToString());
            tcnt = 0;
            timer1.Enabled = false;
            
        }

        private void ShowDisplay()
        {
            dispReelsSymbols(Reels.LEFT);
            dispReelsSymbols(Reels.CENTER);
            dispReelsSymbols(Reels.RIGHT);
        }

        private void UpReel()
        { 
            UpReelPosition(Reels.LEFT, leftNextPosition);
            UpReelPosition(Reels.CENTER, centerNextPosition);
            UpReelPosition(Reels.RIGHT, rightNextPosition);
        }

        //マックスベットが押された時の処理
        private void OnPushedMaxBet()
        {
            CalcCoinCollection(); //コイン回収
            SetEstablishedRole(Roles.NONE); //現在の役をなしに設定
            HitRolesLottery(); //役の抽選
            BonusLottery(); //ボーナスの抽選(レア役がでた時のみ)
            ResetReelsMoving(); //全てのリールを動いているフラグにする
        }

        //ストップボタンが押された時の処理
        private void OnPushedStopBtn(Reels selectReel,in sbyte reelPosition)
        {
            SetNowReelPosition(selectReel, reelPosition); //現在のリールの位置を設定
            CalcNextReelPosition(selectReel); //現在のリールの位置を元に計算　こいつの返り値を代入してViewに反映すること
            SetReelMoving(selectReel, false); //選択したリールを停止

            //三つ目のリールが停止した時
            if(StopReelCount == 3)
            {
                HitEstablishedRoles(); //達成された役を探索
                CalcCoinReturned(); //達成された役を元にコインを還元
                SwitchingBonus(); //ボーナスの状態を(達成したボーナスに突入・停止・次のボーナスに)移行
            }
        }
    }
}