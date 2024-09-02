using System;
using System.Diagnostics;
using System.Security.Permissions;

namespace Model;
public class Game : BusinessLogic
{
    static bool leftReelbtn = false;
    static bool centerReelbtn = false;
    static bool rightReelbtn = false;

    static int bonusReturn = 0;
    static bool nextBonusFlag = false;
    static bool bonusFlag = false;

    static int[] dispSymbol = new int[3];
    static int stopReelCount = 0;

    const int TOP = 0;
    const int MIDDLE = 1;
    const int BOTTOM = 2;



    public static bool BonusLottery()
    {
        Random rnd = new Random();
        int rndnum = rnd.Next(1, 101);  //1以上101未満の値がランダムに出力
        if (rndnum <= Setting.getBonusProbability())
        {
            return true;
        }
        return false;
    }


    public static int SelectBonusLottery()
    {
        int bonus = 0;
        Random rnd = new Random();
        int regularProbabilityWeight = Setting.getBonusesProbabilityWeight(REGULAR_BONUS);
        int bigProbabilityWeight = Setting.getBonusesProbabilityWeight(BIG_BONUS);
        int sumWeight = regularProbabilityWeight + bigProbabilityWeight;
        int rndnum = rnd.Next(1, sumWeight+1);  //1以上sumWeight以下の値がランダムに出力

        if(regularProbabilityWeight <= rndnum)
        {
            bonus = REGULAR_HIT_STATE;
        }else if(regularProbabilityWeight > rndnum && sumWeight <= rndnum) 
        {
            bonus = BIG_HIT_STATE;
        }

        return bonus;
    }


    //役の抽選の関数 5はリーチ6はボーナス
    public static int HitRoleLottery(int bonusState)
    {

        int role = 0;
        int sumWeight = 0;
        int lotteryRange = 4;
        if (bonusState == NONE)
        {
            lotteryRange = 4;
        }
        else if(bonusState != NONE)
        {
            lotteryRange = 6;
        }


        for(int i = 0; i <= lotteryRange; i++)
        {
            if (i <= 5)
            {
                sumWeight += Setting.getRoleWeight(i);
            }
            else if (i == 6 && bonusState == REGULAR_HIT_STATE)
            {
                sumWeight += Setting.getRoleWeight(REGULAR_ROLE);
            }
            else if(i == 6 && bonusState == BIG_HIT_STATE)
            {
                sumWeight += Setting.getRoleWeight(BIG_ROLE);
            }

        }

        Random rnd = new Random();
        int rndnum = rnd.Next(1, sumWeight + 1);


        sumWeight = 0;
        for(int i = 0;i < lotteryRange; i++)
        {
            if (i <= 5)
            {
                sumWeight += Setting.getRoleWeight(i);
            }
            else if (i == 6 && bonusState == REGULAR_HIT_STATE)
            {
                sumWeight += Setting.getRoleWeight(REGULAR_ROLE);
            }
            else if (i == 6 && bonusState == BIG_HIT_STATE)
            {
                sumWeight += Setting.getRoleWeight(BIG_ROLE);
            }

            if (sumWeight >= rndnum)
            {
                role = i;
            }
        }


        return role;
    }

    //test
    
    //リールで表示されるシンボルの選択をする関数
    //第一引数にはストップを押下した時点のシンボルの位置を0～20中央の値を代入
    //第二引数には処理するリールの位置をREFT,CENTER,RIGHT_REELで選択
    //第三引数には決まった役（ロール）
    //処理内容にはリーチの行の把握とレギュラーボーナス時にはBARと7がどこにあるか把握する必要もあり
    //一つ目のレーンの処理
    public static int GetFarstReelPosition(int nowReelPosition,int selectReel,int role)
    {
        int reelPosition = NONE;
        int[] reelOrder = { NONE, NONE, NONE, NONE, NONE, NONE, NONE, NONE, NONE, NONE, NONE, NONE, NONE, NONE, NONE, NONE, NONE, NONE, NONE, NONE, NONE };
        int[] symbolCandidate = { NONE, NONE, NONE, NONE, NONE, NONE, NONE };//ストップボタンを押した時点で表示する滑り4つ含めた候補を要素番号で
        int[] symbolsAccordingRole = { NONE, NONE };//roleを達成できるシンボルを格納する
        int[] stopCandidate = { NONE, NONE };
        int findSymbol = 0;
        int searchReelPosition = nowReelPosition;


        Random rnd = new Random();

        //現在のリールのポジションから下から上に7つを候補として代入
        for(int i = 0; i < symbolCandidate.Length; i++)
        {
            symbolCandidate[i] = searchReelPosition;
            if (searchReelPosition == 20)
            {
                searchReelPosition = 0;
            }
            else if (searchReelPosition < 20)
            {
                searchReelPosition++;
            }
        }



        //4以下のロールはそのままシンボルとして代入、REGとBIGは7とBARを代入
        if(role <= 4)
        {
            symbolsAccordingRole[0] = role;
        }
        else if(role == 6 || role == 7)
        {
            symbolsAccordingRole[0] = SEVEN;
            symbolsAccordingRole[1] = BAR;
        }


        //REGとBIGの場合7は即代入7が来なかった場合は7を返す、他の役は即代入
        for(int i = 0; i <= 20; i++)
        {
            for(int j = 6; j < symbolCandidate.Length; j--)
            {
                if(reelOrder[symbolCandidate[j]] == symbolsAccordingRole[0])
                {
                    stopCandidate[0] = reelOrder[symbolCandidate[j]];
                    findSymbol += 1;
                    reelPosition = stopCandidate[0];
                }
                else if(reelOrder[symbolCandidate[j]] == symbolsAccordingRole[1])
                {
                    stopCandidate[1] = reelOrder[symbolCandidate[j]];
                    findSymbol += 2;
                }
            }
        }

        
        if(reelPosition == NONE && stopCandidate[1] != NONE)
        {
            reelPosition = stopCandidate[1];
        }
        else if(reelPosition == NONE)
        {
            reelPosition = nowReelPosition;
        }
        else if (reelPosition == symbolCandidate[6])
        {
            reelPosition -= 2;
        }
        else if(reelPosition != nowReelPosition)
        {
            reelPosition --;
        }

        return reelPosition;
    }


    //あとで編集
    //どこのリールであるか、一つ目のリールのシンボルの並びと場所の引数が必要
    //一つ目のリールの情報を元に
    public static int GetSecondReelPosition(int nowReelPosition, int selectReel, int role)
    {
        int reelPosition = NONE;
        int[] reelOrder = { NONE, NONE, NONE, NONE, NONE, NONE, NONE, NONE, NONE, NONE, NONE, NONE, NONE, NONE, NONE, NONE, NONE, NONE, NONE, NONE, NONE };
        int[] symbolCandidate = { NONE, NONE, NONE, NONE, NONE, NONE, NONE };//ストップボタンを押した時点で表示する滑り4つ含めた候補を要素番号で
        int[] symbolsAccordingRole = { NONE, NONE };//roleを達成できるシンボルを格納する
        int[] stopCandidate = { NONE, NONE };
        int findSymbol = 0;
        int searchReelPosition = nowReelPosition;


        Random rnd = new Random();

        //現在のリールのポジションから下から上に7つを候補として代入
        for (int i = 0; i < symbolCandidate.Length; i++)
        {
            symbolCandidate[i] = searchReelPosition;
            if (searchReelPosition == 20)
            {
                searchReelPosition = 0;
            }
            else if (searchReelPosition < 20)
            {
                searchReelPosition++;
            }
        }



        //4以下のロールはそのままシンボルとして代入、REGとBIGは7とBARを代入
        if (role <= 4)
        {
            symbolsAccordingRole[0] = role;
        }
        else if (role == 6 || role == 7)
        {
            symbolsAccordingRole[0] = SEVEN;
            symbolsAccordingRole[1] = BAR;
        }


        //REGとBIGの場合7は即代入7が来なかった場合は7を返す、他の役は即代入
        for (int i = 0; i <= 20; i++)
        {
            for (int j = 6; j < symbolCandidate.Length; j--)
            {
                if (reelOrder[symbolCandidate[j]] == symbolsAccordingRole[0])
                {
                    stopCandidate[0] = reelOrder[symbolCandidate[j]];
                    findSymbol += 1;
                    reelPosition = stopCandidate[0];
                }
                else if (reelOrder[symbolCandidate[j]] == symbolsAccordingRole[1])
                {
                    stopCandidate[1] = reelOrder[symbolCandidate[j]];
                    findSymbol += 2;
                }
            }
        }


        if (reelPosition == NONE && stopCandidate[1] != NONE)
        {
            reelPosition = stopCandidate[1];
        }
        else if (reelPosition == NONE)
        {
            reelPosition = nowReelPosition;
        }
        else if (reelPosition == symbolCandidate[6])
        {
            reelPosition -= 2;
        }
        else if (reelPosition != nowReelPosition)
        {
            reelPosition--;
        }

        return reelPosition;
    }



    //シンボルが決まっていないところはNONEを代入すること
    //リーチを探す処理で行が場所を示し、列に複数のシンボルのリーチが書かれている
    public static int[,] GetReachRows(int[] leftReel, int[] centerReel, int[] rightReel) 
    {
        int[,] reachRows = new int[3,2];

        // 左リールの処理
        for (int row = 0; row < 3; row++)
        {
            if (leftReel == null && centerReel[row] == rightReel[row])
            {
                reachRows[row, 0] = centerReel[row];
            }
        }

        if (leftReel == null && centerReel[MIDDLE] == rightReel[BOTTOM])
        {
            reachRows[TOP, 1] = centerReel[MIDDLE];
        }

        if(leftReel == null && centerReel[MIDDLE] == rightReel[TOP])
        {
            reachRows[BOTTOM,1] = centerReel[MIDDLE];
        }




        //真ん中リールの処理
        for (int row = 0; row < 3; row++)
        {
            if (centerReel == null && leftReel[row] == rightReel[row])
            {
                reachRows[row, 0] = leftReel[row];
            }
        }

        for(int row  = 0; row < 3;row++)
        {
            if(centerReel ==null && leftReel[row] == rightReel[2 - row])
            {
                reachRows[BOTTOM, row] = leftReel[row];
            }
        }

        //右レーンの処理
        for (int row = 0; row < 3; row++)
            {
                if (rightReel == null && centerReel[row] == leftReel[row])
                {
                    reachRows[row, 0] = centerReel[row];
                }
            }

        if (rightReel == null && centerReel[MIDDLE] == leftReel[BOTTOM])
        {
            reachRows[TOP, 1] = centerReel[MIDDLE];
        }
        else if (rightReel == null && centerReel[MIDDLE] == leftReel[TOP])
        {
            reachRows[BOTTOM, 1] = centerReel[MIDDLE];
        }


        return reachRows;
    }


    public static int[] DispSymbolLottery()
    {

        if(stopReelCount == 3)
        {

            stopReelCount = 0;
        }
        
        return dispSymbol;
    }

    private static void StopReelCountup() { stopReelCount++; }
}
