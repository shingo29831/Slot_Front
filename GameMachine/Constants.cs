

public static class Constants
{

    public static class Symbol
    {
        public static readonly int NONE = -1;
        public static readonly int BELL = 1;
        public static readonly int REPLAY = 2;
        public static readonly int WATERMELON = 3;
        public static readonly int CHERRY = 4;
        public static readonly int SEVEN = 5;
        public static readonly int BAR = 6;
    }

    public static class ReelOrder
    {
        public static readonly int[] leftReelOrder = { Symbol.BELL, Symbol.REPLAY, Symbol.BELL, Symbol.BAR, Symbol.CHERRY, Symbol.BELL, Symbol.REPLAY, Symbol.BELL, Symbol.WATERMELON, Symbol.SEVEN, Symbol.BELL, Symbol.REPLAY, Symbol.BELL, Symbol.CHERRY, Symbol.BAR, Symbol.BELL, Symbol.REPLAY, Symbol.BELL, Symbol.REPLAY, Symbol.SEVEN, Symbol.BELL };
        public static readonly int[] centerReelOrder = { Symbol.WATERMELON, Symbol.CHERRY, Symbol.BELL, Symbol.BAR, Symbol.REPLAY, Symbol.CHERRY, Symbol.BELL, Symbol.BELL, Symbol.REPLAY, Symbol.CHERRY, Symbol.BELL, Symbol.BAR, Symbol.REPLAY, Symbol.CHERRY, Symbol.BELL, Symbol.BELL, Symbol.REPLAY, Symbol.CHERRY, Symbol.BELL, Symbol.SEVEN, Symbol.BELL };
        public static readonly int[] rightReelOrder = { Symbol.WATERMELON, Symbol.CHERRY, Symbol.BELL, Symbol.BAR, Symbol.REPLAY, Symbol.CHERRY, Symbol.BELL, Symbol.BELL, Symbol.REPLAY, Symbol.CHERRY, Symbol.BELL, Symbol.BAR, Symbol.REPLAY, Symbol.CHERRY, Symbol.BELL, Symbol.BELL, Symbol.REPLAY, Symbol.CHERRY, Symbol.BELL, Symbol.SEVEN, Symbol.BELL };
    }

    public static class Role
    {
        public static readonly int NONE = -1;
        public static readonly int BELL = 1;
        public static readonly int REPLAY = 2;
        public static readonly int WATERMELON = 3;
        public static readonly int CHERRY = 4;
        public static readonly int REACH = 5;
        public static readonly int REGULAR = 6;
        public static readonly int BIG = 7;

    }

    public static class SelectReel
    {
        public static readonly int NONE = -1;
        public static readonly int LEFT = 1;
        public static readonly int CENTER = 2;
        public static readonly int RIGHT = 3;
    }

    public static class State
    {
        public static readonly int NONE = -1;
        public static readonly int REGULAR = 1;
        public static readonly int BIG = 2;
    }

    public static class Bonus
    {
        public static readonly int NONE = -1;
        public static readonly int REGULAR = 1;
        public static readonly int BIG = 2;
    }

    public static class Position
    {
        public static readonly int NONE = -1;
        public static readonly int TOP = 2;
        public static readonly int MIDDLE = 1;
        public static readonly int BOTTOM = 0;
    }
}

