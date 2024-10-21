using System;
using static Constants.Symbols;

public static class Constants
{
    public static readonly int NONE = -1;
    internal static readonly Symbols[] leftReelOrder;
    internal static readonly Symbols[] centerReelOrder;
    internal static readonly Symbols[] rightReelOrder;

    public static class ReelOrder
    {
        public static readonly Symbols[] leftReelOrder = { BELL, REPLAY, SEVEN, BELL, REPLAY, WATERMELON, BELL, REPLAY, BAR, CHERRY, WATERMELON, BELL, REPLAY, WATERMELON, SEVEN, WATERMELON, BELL, REPLAY, BAR, CHERRY, WATERMELON };
        public static readonly Symbols[] centerReelOrder = { WATERMELON, BELL, SEVEN, REPLAY, WATERMELON, CHERRY, BELL, BAR, REPLAY, BELL, CHERRY, REPLAY, WATERMELON, REPLAY, BELL, SEVEN, REPLAY, WATERMELON, BELL, CHERRY, REPLAY };
        public static readonly Symbols[] rightReelOrder = { REPLAY, CHERRY, BELL, REPLAY, BELL, WATERMELON, CHERRY, REPLAY, BELL, BAR, SEVEN, REPLAY, BELL, WATERMELON, CHERRY, REPLAY, BELL, WATERMELON, CHERRY, REPLAY, BELL };
    }

    public enum Symbols
    {
        NONE,
        BELL,
        REPLAY,
        WATERMELON,
        CHERRY,
        BAR,
        SEVEN
    }

    public enum Roles
    {
        NONE,
        BELL,
        REPLAY,
        WATERMELON,
        WEAK_CHERRY,
        STRONG_CHERRY,
        VERY_STRONG_CHERRY,
        REGULAR,
        BIG
    }


    //リール選択に使用
    public enum Reels
    {
        NONE,
        LEFT,
        CENTER,
        RIGHT
    }

    //リールの上中下の選択に使用
    public enum Positions
    {
        NONE,
        TOP,
        MIDDLE,
        BOTTOM,
    }

    public enum Times
    {
        NONE,
        FIRST,
        SECOND,
        THIRD
    }

}

