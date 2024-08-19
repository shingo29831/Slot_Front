using System;
using System.Diagnostics;


namespace Model;
public class Game: BusinessLogic
{
    static bool leftLanebtn = false;
    static bool centerLanebtn = false;
    static bool rightLanebtn = false;
    static int bigBonusCount = 0;
    static int regulerBonusCount = 0;
    static int betweenBonus = 0;
    static bool nextBonusFlag = false;
    static bool bonusFlag = false;

    public static bool bonusLottery()
    {
        Random rnd = new Random();
        int rndnum = rnd.Next(1, 101);  //1以上100未満の値がランダムに出力
        if (rndnum <= Setting.getBonusProbability())
        {
            return true;
        }
        return false;
    }

    public static int[] dispSymbolSelecter()
    {

    }
}
