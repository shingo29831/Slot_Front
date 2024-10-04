using System;
using static Constants.Symbols;

public static class Constants
{
    public static readonly int NONE = -1;
    public static class ReelOrder
    {
        public static readonly Symbols[] leftReelOrder = { BELL, REPLAY, SEVEN, BELL, REPLAY, WATERMELON, BELL, REPLAY, BAR, CHERRY, WATERMELON, BELL, REPLAY, WATERMELON, SEVEN, WATERMELON, BELL, REPLAY, BAR, CHERRY, WATERMELON };
        public static readonly Symbols[] centerReelOrder = { WATERMELON, BELL, SEVEN, REPLAY, WATERMELON, CHERRY, BELL, BAR, REPLAY, BELL, CHERRY, REPLAY, WATERMELON, REPLAY, BELL, SEVEN, REPLAY, WATERMELON, BELL, CHERRY, REPLAY };
        public static readonly Symbols[] rightReelOrder = { REPLAY, CHERRY, BELL, REPLAY, BELL, WATERMELON, CHERRY, REPLAY, BELL, BAR, SEVEN, REPLAY, BELL, WATERMELON, CHERRY, REPLAY, BELL, WATERMELON, CHERRY, REPLAY, BELL };
    }

    public enum Symbols
    {
        NONE = 0,
        BELL = 1,
        REPLAY = 2,
        WATERMELON = 4,
        CHERRY = 8,
        BAR = 16,
        SEVEN = 32,

        REACH = 64, //リーチ目の時に使う値
    }

    public enum Roles : int
    {
        NONE = 0,
        BELL = 1,
        REPLAY = 2,
        WATERMELON = 4,
        WEAK_CHERRY = 8,
        STRONG_CHERRY = 16,
        VERY_STRONG_CHERRY = 32,
        REGULAR = 64,
        BIG = 128,
    }


    //リール選択に使用
    public enum Reels : int
    {
        NONE = 0,
        LEFT = 1,
        CENTER = 2,
        RIGHT = 4,
    }

    //リールの上中下の選択に使用
    public enum Positions
    {
        NONE = 0,
        TOP = 1,
        MIDDLE = 2,
        BOTTOM = 4,
    }

    public enum Times
    {
        NONE,
        FIRST,
        SECOND,
        THIRD
    }

}

