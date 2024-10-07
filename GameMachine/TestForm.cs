using System;
using static Model.Setting;
using static Constants;
using static Model.Game;
using System.Collections;


namespace GameMachine
{
    public partial class TestForm : Form
    {
        int leftNowReelPosition = GetNowReelPosition(Reels.LEFT);
        bool leftStopBtn = false;
        bool centerStopBtn = false;
        bool rightStopBtn = false;


        int bonusState = NONE;

        int role = NONE;
        int leftPosition = NONE;
        int centerPosition = NONE;
        int rightPosition = NONE;

        int stopBtnCount = 0;

        String uniqueID = "";
        public TestForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            makeTableID();
            txtbox1.Text = getTableID();


            dispReelsSymbols(Reels.LEFT);
            dispReelsSymbols(Reels.CENTER);
            dispReelsSymbols(Reels.RIGHT);


        }


        private void button1_Click(object sender, EventArgs e)
        {
            if (stopReelCount == 0)
            {
                lblArray.Text = "ROLE:" + RoleChangeToName(Roles.VERY_STRONG_CHERRY);
                leftPosition = NONE;
                centerPosition = NONE;
                rightPosition = NONE;
                timer1.Enabled = true;
            }
            if(stopReelCount == 3)
            {
                ResetReelsMoving();
            }



        }

        private void button2_Click(object sender, EventArgs e)
        {

            //timer1.Enabled=true;


            Positions[] positions = { Positions.TOP, Positions.MIDDLE, Positions.BOTTOM };
            Lines[] lines = { Lines.upperToLower, Lines.upperToUpper, Lines.middleToMiddle, Lines.lowerToLower, Lines.lowerToUpper };
            Reels[] reels = { Reels.LEFT, Reels.CENTER, Reels.RIGHT }; 
            lblArray.Text = "GetReachPositions:";
            int cnt = 0;


            for(int gap = 0; gap <= 4 ; gap++)
            {
                //int reelPosition = CalcReelPosition(nowLeftReel, gap);
                //lblArray.Text += cnt.ToString() + ":" + GetIsExclusion(Reels.LEFT, reelPosition).ToString() + " , ";
                cnt++;
            }

            dispReelsSymbols(Reels.LEFT);
            dispReelsSymbols(Reels.CENTER);
            dispReelsSymbols(Reels.RIGHT);

            //UpReelPosition(Reels.RIGHT, rightPosition);



            UpReelPosition(Reels.LEFT, rightPosition);
            if (1 == 20)
            {
                UpReelPosition(Reels.CENTER, centerPosition);
            }
        }



        private String RoleChangeToName(Roles role)
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
        private String LinesChangeToNames(Lines lines)
        {
            String value = "";
            if (lines.HasFlag(Lines.upperToLower))
            {
                value += " • _";
            }
            if (lines.HasFlag(Lines.upperToUpper))
            {
                value += " • P";
            }
            if (lines.HasFlag(Lines.middleToMiddle))
            {
                value += " • [";
            }
            if (lines.HasFlag(Lines.lowerToLower))
            {
                value += " • Q";
            }
            if (lines.HasFlag(Lines.lowerToUpper))
            {
                value += " • ^";
            }

            return value;
        }


        private String PositionsChangeToNames(Positions positions)
        {
            String value = "";
            if (positions.HasFlag(Positions.TOP))
            {
                value += " • TOP";
            }
            if (positions.HasFlag(Positions.MIDDLE))
            {
                value += " • MIDDLE";
            }
            if (positions.HasFlag(Positions.BOTTOM))
            {
                value += " • BOTTOM";
            }
            return value;
        }


        private String SymbolsChangeToNames(Symbols symbols)
        {
            String value = "";

            if (symbols.HasFlag(Symbols.BELL))
            {
                value += " • BELL";
            }
            if (symbols.HasFlag(Symbols.REPLAY))
            {
                value += " • REPLAY";
            }
            if (symbols.HasFlag(Symbols.WATERMELON))
            {
                value += " • WATERMELON";
            }
            if (symbols.HasFlag(Symbols.CHERRY))
            {
                value += " • CHERRY";
            }
            if (symbols.HasFlag(Symbols.BAR))
            {
                value += " • BAR";
            }
            if (symbols.HasFlag(Symbols.SEVEN))
            {
                value += " • SEVEN";
            }
            if (symbols.HasFlag(Symbols.REACH))
            {
                value += " • REACH";
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

        //’è”‚Å‘I‘ð
        private void dispReelsSymbols(Reels selectReel)
        {
            Symbols[] leftReelOrder = GetReelOrder(Reels.LEFT);
            Symbols[] centerReelOrder = GetReelOrder(Reels.CENTER);
            Symbols[] rightReelOrder = GetReelOrder(Reels.RIGHT);
            switch (selectReel)
            {
                case Reels.LEFT:
                    leftReelBot.Text = SymbolChangeToName(leftReelOrder[GetSymbolForPosition(Reels.LEFT, Positions.BOTTOM)]);
                    leftReelMid.Text = SymbolChangeToName(leftReelOrder[GetSymbolForPosition(Reels.LEFT, Positions.MIDDLE)]);
                    leftReelTop.Text = SymbolChangeToName(leftReelOrder[GetSymbolForPosition(Reels.LEFT, Positions.TOP)]);
                    break;

                case Reels.CENTER:
                    centerReelBot.Text = SymbolChangeToName(centerReelOrder[GetSymbolForPosition(Reels.CENTER, Positions.BOTTOM)]);
                    centerReelMid.Text = SymbolChangeToName(centerReelOrder[GetSymbolForPosition(Reels.CENTER, Positions.MIDDLE)]);
                    centerReelTop.Text = SymbolChangeToName(centerReelOrder[GetSymbolForPosition(Reels.CENTER, Positions.TOP)]);
                    break;

                case Reels.RIGHT:
                    rightReelBot.Text = SymbolChangeToName(rightReelOrder[GetSymbolForPosition(Reels.RIGHT, Positions.BOTTOM)]);
                    rightReelMid.Text = SymbolChangeToName(rightReelOrder[GetSymbolForPosition(Reels.RIGHT, Positions.MIDDLE)]);
                    rightReelTop.Text = SymbolChangeToName(rightReelOrder[GetSymbolForPosition(Reels.RIGHT, Positions.TOP)]);
                    break;
            }
        }

        private void leftStop_Click(object sender, EventArgs e)
        {

            lblArray.Text +=" Žn:" + GetNowReelPosition(Reels.LEFT).ToString();
            SetReelMoving(Reels.LEFT, false);
            leftPosition = GetReelPosition(Reels.LEFT);
            lblArray.Text +=" •Ô:" +leftPosition.ToString();

        }

        private void centerStop_Click(object sender, EventArgs e)
        {
            lblArray.Text +=" Žn:" + GetNowReelPosition(Reels.CENTER).ToString();
            SetReelMoving(Reels.CENTER, false);
            centerPosition = GetReelPosition(Reels.LEFT);
            lblArray.Text += " •Ô:"+centerPosition.ToString();
        }

        private void rightStop_Click(object sender, EventArgs e)
        {
            int position = GetReelPosition(Reels.RIGHT);
            lblArray.Text += " Žn:"+ position.ToString();
            rightPosition = position;
            SetReelMoving(Reels.RIGHT, false);
            
            lblArray.Text += " •Ô:"+rightPosition.ToString();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            dispReelsSymbols(Reels.LEFT);
            dispReelsSymbols(Reels.CENTER);
            dispReelsSymbols(Reels.RIGHT);


            UpReelPosition(Reels.LEFT, leftPosition);
            UpReelPosition(Reels.CENTER, centerPosition);
            UpReelPosition(Reels.RIGHT, rightPosition);



        }

        private int PushStopReelPosition(Reels selectReel)
        {
            bool isExecution = false;
            int stopReelPosition = NONE;
            String value = "";


            switch (selectReel)
            {
                case Reels.LEFT:
                    if (leftStopBtn == false)
                    {
                        leftStopBtn = true;
                        isExecution = true;
                        value = "  LEFT:" + 1l.ToString();
                    }
                    break;
                case Reels.CENTER:
                    if (centerStopBtn == false)
                    {
                        centerStopBtn = true;
                        isExecution = true;
                        value = "  CENTER:" + 1.ToString();
                    }
                    break;
                case Reels.RIGHT:
                    if (rightStopBtn == false)
                    {
                        rightStopBtn = true;
                        isExecution = true;
                        value = " RIGHT:" + 1.ToString();
                    }
                    break;
            }


            return stopReelPosition;
        }



    }
}