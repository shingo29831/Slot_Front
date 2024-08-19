using System;
using System.Diagnostics;


namespace Model;
public class Game : BusinessLogic
{
    static bool leftLanebtn = false;
    static bool centerLanebtn = false;
    static bool rightLanebtn = false;

    static int bonusReturn = 0;
    static bool nextBonusFlag = false;
    static bool bonusFlag = false;

    static int[] dispSymbol = new int[3];
    static int stopLaneCount = 0;

    const int TOP = 0;
    const int MIDDLE = 1;
    const int BOTTOM = 2;

    public static bool BonusLottery()
    {
        Random rnd = new Random();
        int rndnum = rnd.Next(1, 101);  //1以上100未満の値がランダムに出力
        if (rndnum <= Setting.getBonusProbability())
        {
            return true;
        }
        return false;
    }


    //リーチを探す処理で行が場所を示し、列に複数のシンボルのリーチが書かれている
    public static int[,] ReachSearch(int[] leftLane, int[] centerLane, int[] rightLane) 
    {
        int[,] reachRows = new int[3,2];

        // 左レーンの処理
        for (int row = 0; row < 3; row++)
        {
            if (leftLane == null && centerLane[row] == rightLane[row])
            {
                reachRows[row, 0] = centerLane[row];
            }
        }

        if (leftLane == null && centerLane[MIDDLE] == rightLane[BOTTOM])
        {
            reachRows[TOP, 1] = centerLane[MIDDLE];
        }

        if(leftLane == null && centerLane[MIDDLE] == rightLane[TOP])
        {
            reachRows[BOTTOM,1] = centerLane[MIDDLE];
        }




        //真ん中レーンの処理
        for (int row = 0; row < 3; row++)
        {
            if (centerLane == null && leftLane[row] == rightLane[row])
            {
                reachRows[row, 0] = leftLane[row];
            }
        }

        for(int row  = 0; row < 3;row++)
        {
            if(centerLane ==null && leftLane[row] == rightLane[2 - row])
            {
                reachRows[BOTTOM, row] = leftLane[row];
            }
        }

        //右レーンの処理
        for (int row = 0; row < 3; row++)
            {
                if (rightLane == null && centerLane[row] == leftLane[row])
                {
                    reachRows[row, 0] = centerLane[row];
                }
            }

        if (rightLane == null && centerLane[MIDDLE] == leftLane[BOTTOM])
        {
            reachRows[TOP, 1] = centerLane[MIDDLE];
        }
        else if (rightLane == null && centerLane[MIDDLE] == leftLane[TOP])
        {
            reachRows[BOTTOM, 1] = centerLane[MIDDLE];
        }


        return reachRows;
    }

    public static int[] DispSymbolLottery()
    {

        if(stopLaneCount == 3)
        {

            stopLaneCount = 0;
        }
        
        return dispSymbol;
    }

    private static void StopLaneCountup() { stopLaneCount++; }
}
