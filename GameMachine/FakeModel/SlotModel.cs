using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameMachine.FakeModel
{
    //#############################################//
    //                                             //
    //              仮のデータです！！！           //
    //                                             //
    //#############################################//
    class SlotModel
    {
        public const int NONE = -1;
        public const int BELL = 1;
        public const int REPLAY = 2;
        public const int WATERMELON = 3;
        public const int CHERRY = 4;
        public const int SEVEN = 5;
        public const int BAR = 6;
        //左側のリールのみ
        public static int[] leftReelOrder = { BELL, REPLAY, BELL, BAR, CHERRY, BELL, REPLAY, BELL, WATERMELON, SEVEN, BELL, REPLAY, BELL, CHERRY, BAR, BELL, REPLAY, BELL, REPLAY, SEVEN, BELL };

        public static int[] centerReelOrder = { WATERMELON, CHERRY, BELL, BAR, REPLAY, CHERRY, BELL, BELL, REPLAY, CHERRY, BELL, BAR, REPLAY, CHERRY, BELL, BELL, REPLAY, CHERRY, BELL, SEVEN, BELL };

        public static int[] rightReelOrder = { WATERMELON, CHERRY, BELL, BAR, REPLAY, CHERRY, BELL, BELL, REPLAY, CHERRY, BELL, BAR, REPLAY, CHERRY, BELL, BELL, REPLAY, CHERRY, BELL, SEVEN, BELL };
    }
}
