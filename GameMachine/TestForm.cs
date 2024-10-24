using System;
using static GameMachine.Model.Setting;
using static Constants;
using static GameMachine.Model.Game;
using System.Collections;
using Model;

namespace GameMachine
{
    public partial class TestForm : Form
    {
        bool leftStopBtn = false;
        bool centerStopBtn = false;
        bool rightStopBtn = false;


        sbyte bonusState = NONE;

        sbyte role = NONE;
        static sbyte leftPosition = 0;
        static sbyte centerPosition = 0;
        static sbyte rightPosition = 0;

        static sbyte leftNextPosition = NONE;
        static sbyte centerNextPosition = NONE;
        static sbyte rightNextPosition = NONE;

        sbyte stopBtnCount = 0;

        String uniqueID = "";


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
                lblArray.Text =  "Coin:" + GetHasCoin().ToString() + "  ROLE:" + RoleChangeToName(GetNowRole()) + " Bonus:" + RoleChangeToName(GetNowBonus());
                timer1.Enabled = true;
                fcnt = 0;
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

        private void button2_Click(object sender, EventArgs e)
        {

            //timer1.Enabled=true;

            HitRolesLottery();
            lblArray.Text = "ROLE:" + RoleChangeToName(GetNowRole());
            Positions[] positions = { Positions.TOP, Positions.MIDDLE, Positions.BOTTOM };
            Lines[] lines = { Lines.upperToLower, Lines.upperToUpper, Lines.middleToMiddle, Lines.lowerToLower, Lines.lowerToUpper };
            Reels[] reels = { Reels.LEFT, Reels.CENTER, Reels.RIGHT };
            lblArray.Text = "GetReachPositions:";
            sbyte cnt = 0;
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


        private static sbyte GetNextReelPosition(in Reels selectReel)
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



        public String RoleChangeToName(Roles role)
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


        int fcnt = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            dispReelsSymbols(Reels.LEFT);
            dispReelsSymbols(Reels.CENTER);
            dispReelsSymbols(Reels.RIGHT);


            UpReelPosition(Reels.LEFT, leftNextPosition);
            UpReelPosition(Reels.CENTER, centerNextPosition);
            UpReelPosition(Reels.RIGHT, rightNextPosition);


            if (fcnt == 10)
            {

                fcnt = 20;
            }
            else if (fcnt < 10 && StopReelCount == 3)
            {
                fcnt++;
            }



        }

        private void PushStopReelPosition(Reels selectReel)
        {
            sbyte reelPosition = GetReelPosition(selectReel);
            SetNowReelPosition(selectReel, reelPosition);
            lblArray.Text += " 始:" + reelPosition.ToString();
            sbyte nextReelPosition = CalcNextReelPosition(selectReel); 
            SetNextReelPosition(selectReel, nextReelPosition); 
            SetReelMoving(selectReel, false);
            lblArray.Text += " 返:" + GetModelNextReelPosition(selectReel).ToString();

            if (StopReelCount == 3)
            {
                String leftValue = "";
                String centerValue = "";
                String rightValue = "";
                foreach (Lines line in LINES_ARRAY)
                {
                    leftValue += " " + SymbolChangeToName(GetSymbolForLine(Reels.LEFT, line, GetModelNextReelPosition(Reels.LEFT) ));
                    centerValue += " " + SymbolChangeToName(GetSymbolForLine(Reels.CENTER, line, GetModelNextReelPosition(Reels.CENTER) ));
                    rightValue += " " + SymbolChangeToName(GetSymbolForLine(Reels.RIGHT, line, GetModelNextReelPosition(Reels.RIGHT) ));
                }
                HitEstablishedRoles();
                CalcCoinReturned();
                //MessageBox.Show(GetModelNextReelPosition(Reels.LEFT).ToString()+" , "
                //    + GetModelNextReelPosition(Reels.CENTER).ToString()+" , " 
                //    + GetModelNextReelPosition(Reels.RIGHT).ToString()) ;
                //MessageBox.Show(leftValue+"\n"+centerValue+"\n"+rightValue);


                lblArray.Text += "  Role:" + RoleChangeToName(GetEstablishedRole()) + "  Coin:" +GetHasCoin().ToString();
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





    }
}