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
            if (stopBtnCount == 0)
            {
                lblArray.Text = "ROLE:" + roleChangeToName(Roles.VERY_STRONG_CHERRY);
                leftPosition = NONE;
                centerPosition = NONE;
                rightPosition = NONE;
                timer1.Enabled = true;
            }



        }

        private void button2_Click(object sender, EventArgs e)
        {

            //timer1.Enabled=true;


            Positions[] positions = { Positions.TOP, Positions.MIDDLE, Positions.BOTTOM };
            Lines[] lines = { Lines.upperToLower, Lines.upperToUpper, Lines.middleToMiddle, Lines.lowerToLower, Lines.lowerToUpper };
            lblArray.Text = "GetReachSymbols:";
            int cnt = 0;
            lblArray.Text += " " + cnt.ToString() + ":" + PositionsChangeToNames(GetRoleReachPositions()) + " , ";


            dispReelsSymbols(Reels.LEFT);



            dispReelsSymbols(Reels.CENTER);

            //UpReelPosition(Reels.RIGHT, rightPosition);
            dispReelsSymbols(Reels.RIGHT);
            UpReelPosition(Reels.RIGHT, leftPosition);
            if (nowRightReel == 20)
            {
                UpReelPosition(Reels.CENTER, centerPosition);
            }
        }

        private String roleChangeToName(Roles role)
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
                value += " Åï TOP";
            }
            if (positions.HasFlag(Positions.MIDDLE))
            {
                value += " Åï MIDDLE";
            }
            if (positions.HasFlag(Positions.BOTTOM))
            {
                value += " Åï BOTTOM";
            }
            return value;
        }


        private String SymbolsChangeToNames(Symbols symbols)
        {
            String value = "";

            if (symbols.HasFlag(Symbols.BELL))
            {
                value += " Åï BELL";
            }
            if (symbols.HasFlag(Symbols.REPLAY))
            {
                value += " Åï REPLAY";
            }
            if (symbols.HasFlag(Symbols.WATERMELON))
            {
                value += " Åï WATERMELON";
            }
            if (symbols.HasFlag(Symbols.CHERRY))
            {
                value += " Åï CHERRY";
            }
            if (symbols.HasFlag(Symbols.BAR))
            {
                value += " Åï BAR";
            }
            if (symbols.HasFlag(Symbols.SEVEN))
            {
                value += " Åï SEVEN";
            }
            if (symbols.HasFlag(Symbols.REACH))
            {
                value += " Åï REACH";
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

        //íËêîÇ≈ëIë
        private void dispReelsSymbols(Reels selectReel)
        {
            Symbols[] leftReelOrder = GetReelOrder(Reels.LEFT);
            Symbols[] centerReelOrder = GetReelOrder(Reels.CENTER);
            Symbols[] rightReelOrder = GetReelOrder(Reels.RIGHT);
            switch (selectReel)
            {
                case Reels.LEFT:
                    leftReelBot.Text = SymbolChangeToName(leftReelOrder[GetDispSymbol(Reels.LEFT, Positions.BOTTOM)]);
                    leftReelMid.Text = SymbolChangeToName(leftReelOrder[GetDispSymbol(Reels.LEFT, Positions.MIDDLE)]);
                    leftReelTop.Text = SymbolChangeToName(leftReelOrder[GetDispSymbol(Reels.LEFT, Positions.TOP)]);
                    break;

                case Reels.CENTER:
                    centerReelBot.Text = SymbolChangeToName(centerReelOrder[GetDispSymbol(Reels.CENTER, Positions.BOTTOM)]);
                    centerReelMid.Text = SymbolChangeToName(centerReelOrder[GetDispSymbol(Reels.CENTER, Positions.MIDDLE)]);
                    centerReelTop.Text = SymbolChangeToName(centerReelOrder[GetDispSymbol(Reels.CENTER, Positions.TOP)]);
                    break;

                case Reels.RIGHT:
                    rightReelBot.Text = SymbolChangeToName(rightReelOrder[GetDispSymbol(Reels.RIGHT, Positions.BOTTOM)]);
                    rightReelMid.Text = SymbolChangeToName(rightReelOrder[GetDispSymbol(Reels.RIGHT, Positions.MIDDLE)]);
                    rightReelTop.Text = SymbolChangeToName(rightReelOrder[GetDispSymbol(Reels.RIGHT, Positions.TOP)]);
                    break;
            }
        }

        private void leftStop_Click(object sender, EventArgs e)
        {
            leftPosition = PushStopReelPosition(Reels.LEFT);
        }

        private void centerStop_Click(object sender, EventArgs e)
        {
            centerPosition= PushStopReelPosition(Reels.CENTER);
        }

        private void rightStop_Click(object sender, EventArgs e)
        {
            rightPosition = PushStopReelPosition(Reels.RIGHT);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {


            Positions[] positions = { Positions.TOP, Positions.MIDDLE, Positions.BOTTOM };
            Lines[] lines = { Lines.upperToLower, Lines.upperToUpper, Lines.middleToMiddle, Lines.lowerToLower, Lines.lowerToUpper };
            lblArray.Text = "GetReachSymbols:";
            int cnt = 0;
                lblArray.Text += " " + cnt.ToString() + ":" + PositionsChangeToNames(GetRoleReachPositions()) + " , ";


            dispReelsSymbols(Reels.LEFT);



            dispReelsSymbols(Reels.CENTER);

            //UpReelPosition(Reels.RIGHT, rightPosition);
            dispReelsSymbols(Reels.RIGHT);
            UpReelPosition(Reels.RIGHT, leftPosition);
            if (nowRightReel == 20)
            {
                UpReelPosition(Reels.CENTER, centerPosition);
            }


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
                        value = "  LEFT:" + nowLeftReel.ToString();
                    }
                    break;
                case Reels.CENTER:
                    if (centerStopBtn == false)
                    {
                        centerStopBtn = true;
                        isExecution = true;
                        value = "  CENTER:" + nowCenterReel.ToString();
                    }
                    break;
                case Reels.RIGHT:
                    if (rightStopBtn == false)
                    {
                        rightStopBtn = true;
                        isExecution = true;
                        value = " RIGHT:" + nowRightReel.ToString();
                    }
                    break;
            }

            if (isExecution)
            {
                stopReelPosition = GetStopReelPosition(selectReel);

                lblArray.Text += value + " , " +  stopReelPosition.ToString() + " , ";
            }

            return stopReelPosition;
        }



    }
}