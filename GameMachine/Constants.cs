using System;
using static Constants.Symbol;


//定数クラス
public static class Constants
{
    public static readonly int NONE = -1;
    public static class Symbol
    {
        public static readonly int BELL = 1;
        public static readonly int REPLAY = 2;
        public static readonly int WATERMELON = 3;
        public static readonly int CHERRY = 4;
        public static readonly int SEVEN = 5;
        public static readonly int BAR = 6;
    }

    public static class ReelOrder
    {
        public static readonly int[] leftReelOrder = { BELL, REPLAY, SEVEN, BELL, REPLAY, WATERMELON, BELL, REPLAY, BAR, CHERRY, WATERMELON, BELL, REPLAY, WATERMELON, SEVEN, WATERMELON, BELL, REPLAY, BAR, CHERRY, WATERMELON };
        public static readonly int[] centerReelOrder = { WATERMELON, BELL, SEVEN, REPLAY, WATERMELON, CHERRY, BELL, BAR, REPLAY, BELL, CHERRY, REPLAY, WATERMELON, REPLAY, BELL, SEVEN, REPLAY, WATERMELON, BELL, CHERRY, REPLAY };
        public static readonly int[] rightReelOrder = { REPLAY, CHERRY, BELL, REPLAY, BELL, WATERMELON, CHERRY, REPLAY, BELL, BAR, SEVEN, REPLAY, BELL, WATERMELON, CHERRY, REPLAY, BELL, WATERMELON, CHERRY, REPLAY, BELL };
    }

    public static class Role
    {
        public static readonly int BELL = 1;
        public static readonly int REPLAY = 2;
        public static readonly int WATERMELON = 3;
        public static readonly int WEAK_CHERRY = 4;
        public static readonly int STRONG_CHERRY = 5;
        public static readonly int VERY_STRONG_CHERRY = 6;
        public static readonly int REACH = 7;
        public static readonly int REGULAR = 8;
        public static readonly int BIG = 9;

        public static readonly int OTHER_BONUS = 6;　//6以下はボーナス以外の役という意味

    }

    public static class SelectReel
    {
        public static readonly int LEFT = 1;
        public static readonly int CENTER = 2;
        public static readonly int RIGHT = 3;
    }

    public static class State
    {
        public static readonly int REGULAR = 1;
        public static readonly int BIG = 2;
    }

    public static class Bonus
    {
        public static readonly int REGULAR = 1;
        public static readonly int BIG = 2;
    }

    public static class Position
    {
        public static readonly int TOP = 2;
        public static readonly int MIDDLE = 1;
        public static readonly int BOTTOM = 0;
    }
}

