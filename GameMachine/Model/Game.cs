using System;
using static Constants;
using static Model.Setting;


//このクラスはゲームの内部処理を担当する
namespace Model;



public class Game
{
    static bool leftReelbtn = false;
    static bool centerReelbtn = false;
    static bool rightReelbtn = false;

    static int bonusReturn = 0;
    static bool nextBonusFlag = false;
    static bool bonusFlag = false;

    static int[] dispSymbol = new int[3];
    static int stopReelCount = 0;


    static Symbols[] leftReel = { Symbols.NONE, Symbols.NONE, Symbols.NONE };
    static Symbols[] centerReel = { Symbols.NONE, Symbols.NONE, Symbols.NONE };
    static Symbols[] rightReel = { Symbols.NONE, Symbols.NONE, Symbols.NONE };


    private static int nowLeftReel = 0;
    private static int nowCenterReel = 0;
    private static int nowRightReel = 0;

    private static bool leftReelMoving = true;
    private static bool centerReelMoving = true;
    private static bool rightReelMoving = true;


    private static Roles nowRole = Roles.NONE;
    private static Symbols symbolsAccordingRole = Symbols.NONE;

    //reachRowsはリーチとなる場所を3次元配列で格納する各リールのポジション(上・中・下)に二つまでの入ったら役が成立するシンボルを代入する
    private Symbols[,,] reachPositions = { { {Symbols.NONE,Symbols.NONE }, {Symbols.NONE,Symbols.NONE }, {Symbols.NONE,Symbols.NONE } }, //左リール : 0
                                  { {Symbols.NONE,Symbols.NONE }, {Symbols.NONE,Symbols.NONE }, {Symbols.NONE,Symbols.NONE } }, //中央リール : 1
                                  { {Symbols.NONE,Symbols.NONE }, {Symbols.NONE,Symbols.NONE }, {Symbols.NONE,Symbols.NONE } } }; //右リール : 2

    private static readonly int NOT = -1;


    //リールの現在の位置をオーバフローさせないように計算する 第一引数に移動前,第二引数に移動数を代入
    public static int CalcReelPosition(int reelPosition,int move)
    {
        reelPosition += move;
        if (reelPosition < 0)
        {
            reelPosition += 21;
        }else if (reelPosition > 20) 
        {
            reelPosition -= 21;
        }

        return reelPosition;
    }


    //リールを一つずつ移動させる　移動先が決定されている場合はリールのポジションを代入なければ NONE:-1 を代入
    public static void UpReelPosition(Reels selectReel,int destinationPosition)
    {
        switch (selectReel)
        {
            case Reels.LEFT:
                if(nowLeftReel == NONE)
                {
                    //NONE:-1が来たときは＋しない
                }
                else if(nowLeftReel != destinationPosition)
                {
                    nowLeftReel = CalcReelPosition (nowLeftReel, 1);
                }

                break;

            case Reels.CENTER:
                if (nowCenterReel == NONE)
                {
                    //NONE:-1が来たときは＋しない
                }
                else if (nowCenterReel != destinationPosition)
                {
                    nowCenterReel = CalcReelPosition (nowCenterReel, 1);
                }
                
                break;

            case Reels.RIGHT:
                if (nowRightReel == NONE)
                {
                    //NONE:-1が来たときは＋しない
                }
                else if (nowRightReel != destinationPosition)
                {
                    nowRightReel = CalcReelPosition (nowRightReel, 1);
                }
                
                break;
        }
    }


    //リールの今のポジションを取得する　引数に定数クラスのReels.LEFT,Reels.CENTER,Reels.RIGHT
    public static int GetNowReelPosition(Reels selectReel)
    {
        int nowPosition = NONE;
        switch(selectReel)
        {
            case Reels.LEFT:
                nowPosition = nowLeftReel;
                break;

            case Reels.CENTER:
                nowPosition = nowCenterReel;
                break;

            case Reels.RIGHT:
                nowPosition = nowRightReel;
                break;
        }
        return nowPosition;
    }


    //表示するリールの位置を取得する　引数に定数クラスのReels.LEFT,Reels.CENTER,Reels.RIGHTとTOP,MIDDLE,BOTTOM
    public static int GetDispSymbol(Reels selectReel,int dispPosition)
    {
        int reelPosition = NONE;
        switch (selectReel)
        {
            case Reels.LEFT:
                reelPosition = nowLeftReel + dispPosition;
                break;

            case Reels.CENTER:
                reelPosition = nowCenterReel + dispPosition;
                break;

            case Reels.RIGHT:
                reelPosition = nowRightReel + dispPosition;
                break;
        }

        if(reelPosition > 20)
        {
            reelPosition -= 21;
        }
        return reelPosition;
    }

    //ボーナス抽選開始後実行する"ボーナス抽選関数" bonusProbabilityに設定された確率に合わせて抽選する
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


    //ボーナス抽選当選後実行するレギュラーボーナスまたはビックボーナスを決定する
    public static Roles SelectBonusLottery()
    {
        Roles bonus = Roles.NONE;
        Random rnd = new Random();
        int regularProbabilityWeight = Setting.getBonusesProbabilityWeight(0);
        int bigProbabilityWeight = Setting.getBonusesProbabilityWeight(1);
        int sumWeight = regularProbabilityWeight + bigProbabilityWeight;
        int rndnum = rnd.Next(1, sumWeight+1);  //1以上sumWeight以下の値がランダムに出力

        if(regularProbabilityWeight <= rndnum)
        {
            bonus = Roles.REGULAR;
        }else if(regularProbabilityWeight > rndnum && sumWeight <= rndnum) 
        {
            bonus = Roles.BIG;
        }

        return bonus;
    }


    //役の抽選の関数　ボーナス以外の役が当選する
    public static int HitRoleLottery()
    {

        int role = 0;
        int sumWeight = 0;
        int lotteryRange = 6;


        for (int i = 0; i <= lotteryRange; i++)
        {
            sumWeight += Setting.getRoleWeight(i);
        }

        Random rnd = new Random();
        int rndnum = rnd.Next(1, sumWeight + 1);
        sumWeight = 0;
        int tmp = 0;
        for (int i = 0; i <= lotteryRange; i++)
        {
            tmp = sumWeight;
            sumWeight += Setting.getRoleWeight(i);


            if (tmp < rndnum && rndnum <= sumWeight)
            {
                role = i;
            }
        }


        return role;
    }


    //リールのシンボルの並びをReels.LEFT,Reels.CENTER,Reels.RIGHTで選択し取得する
    public static Symbols[] GetReelOrder(Reels selectReel) 
    {
        Symbols[] reelOrder = {Symbols.NONE };
        switch (selectReel)
        {
            case Reels.LEFT:
                reelOrder = ReelOrder.leftReelOrder;
                break;


            case Reels.CENTER:
                reelOrder = ReelOrder.centerReelOrder;
                break;


            case Reels.RIGHT:
                reelOrder = ReelOrder.rightReelOrder;
                break;
            
        }
        return reelOrder;
    }

   


    //どのリールが動いてるか代入する
    private static void SetReelMoving(Reels selectReel, bool isMoving)
    {
        switch(selectReel)
        {
            case Reels.LEFT:
                leftReelMoving = isMoving;
                break;
            case Reels.CENTER: 
                centerReelMoving = isMoving;
                break;
            case Reels.RIGHT:
                rightReelMoving = isMoving;
                break;
        }
    }

    //リールを全て動いている判定にする
    private static void ResetReelsMoving()
    {
        SetReelMoving(Reels.LEFT,true);
        SetReelMoving(Reels.CENTER,true);
        SetReelMoving(Reels.RIGHT,true);
    }


    //配列reachPositionsに、入ったら役が成立するポジションを代入する処理
    public static void SetReachPositions()
    {
        
    }



    //選択されたリールの表示される位置を返す
    private static int GetReelPosition(Reels selectReel,Times stopCount)
    {
        int reelPosition = NONE;
        switch (stopCount)
        {
            case Times.FIRST:
                reelPosition = GetFirstReelPosition(selectReel);
                break;
            case Times.SECOND:
                reelPosition = GetSecondReelPosition(selectReel);
                break;
            case Times.THIRD:
                reelPosition = GetThirdReelPostion(selectReel);
                break;
        }
        
        return reelPosition;
    }

    //一つ目にストップさせるリールの処理
    private static int GetFirstReelPosition(Reels selectReel)
    {
        Symbols[] reelOrder = GetReelOrder(selectReel); //選択されたリールの並びを参照
        symbolsAccordingRole = GetSymbolsAccordingRole(nowRole);
        int reelPosition = NONE;
        int nowReelPosition = GetNowReelPosition(selectReel);
        int stopCandidate = GetStopCandidate(reelOrder,nowReelPosition,Positions.NONE);
        int secondStopCandidate = NONE;
        bool[] isExclusions = { false, false, false, false, false}; //除外する位置を代入する。リールを止める位置だけのため、BOTTOMにくる5つの範囲
        if (nowRole == Roles.BIG || nowRole == Roles.REGULAR)
        {
            secondStopCandidate = GetBarPosition(reelOrder,nowReelPosition,Positions.NONE);
        }
        int searchPostion = nowReelPosition;
        for(int gap = 0; gap < 7; gap++)
        {
            isExclusions[gap] = getIsExclusion( reelOrder, selectReel, searchPostion, gap);
            searchPostion = CalcReelPosition(searchPostion, 1);

        }


        return reelPosition;
    }

    //二つ目にストップさせるリールの処理
    private static int GetSecondReelPosition(Reels selectReel)
    {
        int reelPosition = NONE;
        int nowReelPosition = GetNowReelPosition(selectReel);

        return reelPosition;
    }

    //三つ目にストップさせるリールの処理
    private static int GetThirdReelPostion(Reels selectReel)
    {
        int reelPostion = NONE;
        int nowReelPosition = GetNowReelPosition(selectReel);

        return reelPostion;
    }


    //役を成立させるシンボルを取得
    public static Symbols GetSymbolsAccordingRole(Roles role)
    {
        switch (role)
        {
            case Roles.BELL:
                return Symbols.BELL;

            case Roles.REPLAY:
                return Symbols.REPLAY;

            case Roles.WATERMELON:
                return Symbols.WATERMELON;

            case Roles.WEAK_CHERRY:
                return Symbols.CHERRY;

            case Roles.STRONG_CHERRY:
                return Symbols.CHERRY;

            case Roles.VERY_STRONG_CHERRY:
                return Symbols.CHERRY;

            case Roles.REGULAR:
                return Symbols.SEVEN;

            case Roles.BIG:
                return Symbols.SEVEN;

            default:
                return Symbols.NONE;
        }

        
    }

    //選択したポジションに入れることが可能な現在のポジションに最も近い役を成立させるシンボルを取得
    private static int GetStopCandidate(Symbols[] reelOrder, int nowReelPosition, Positions position)
    {
        int maxRange = 6;
        int repetitions = 7;
        switch (position)
        {
            case Positions.TOP:
                maxRange = 6;
                repetitions = 5;
                break;

            case Positions.MIDDLE:
                maxRange = 5;
                repetitions = 5;
                break;

            case Positions.BOTTOM:
                maxRange = 4;
                repetitions = 5;
                break;
        }
        int searchReelPosition = CalcReelPosition(nowReelPosition, maxRange);
        int findedPostion = NONE;
        for(int i = 0; i < repetitions; i++)
        {
            searchReelPosition = CalcReelPosition(searchReelPosition, -1);
            if (reelOrder[searchReelPosition] == symbolsAccordingRole)
            {
                 findedPostion = searchReelPosition;
            }
        }
        return findedPostion;
    }

    //選択したポジションに入れることが可能な現在のポジションに最も近いBARを取得
    private static int GetBarPosition(Symbols[] reelOrder, int nowReelPosition , Positions position)
    {
        int maxRange = 6;
        int repetitions = 7;
        switch (position)
        {
            case Positions.TOP:
                maxRange = 6;
                repetitions = 5;
                break;

            case Positions.MIDDLE:
                maxRange = 5;
                repetitions = 5;
                break;

            case  Positions.BOTTOM:
                maxRange = 4;
                repetitions = 5;
                break;
        }
        int searchReelPosition = CalcReelPosition(nowReelPosition, maxRange);
        int barPostion = NONE;

        for (int i = 0; i < repetitions; i++)
        {
            searchReelPosition = CalcReelPosition(searchReelPosition, -1);
            if (reelOrder[searchReelPosition] == Symbols.BAR)
            {
                barPostion = searchReelPosition;
            }
        }
        return barPostion;
    }

    //除外範囲か否かを返す
    private static bool getIsExclusion(Symbols[] reelOrder, Reels selectReel, int reelPosition, int gap)
    {
        int range = 1; //探索開始位置を含めた探索範囲を代入する
        int bottom = 0; //reelPositionと探索開始位置の差を代入する
        bool isWeakCherry = false;
        bool isStrongCherry = false;
        Symbols[] topExclusionSymbols = { Symbols.NONE, Symbols.NONE };
        Symbols[] midleExclusionSymbols = { Symbols.NONE, Symbols.NONE };
        Symbols[] bottomExclusionSymbols = { Symbols.NONE, Symbols.NONE };

        switch (nowRole)
        {
            case Roles.WEAK_CHERRY:
                isWeakCherry = true; 
                break;
            case Roles.STRONG_CHERRY:
                isStrongCherry= true;
                range = 1;
                bottom = 1;
                break;
            default:
                break;
        }

        if(selectReel == Reels.LEFT && isWeakCherry == false) //左リールの時
        {
            range = 3;
            bottom = 0;
        }

        if(selectReel == Reels.CENTER && isStrongCherry == false) //中央リールの時に強チェリーではないなら中央に探索ポイントにする
        {
            range = 1;
            bottom = 1;
        }

        int searchPosition = CalcReelPosition(reelPosition, bottom);

        for (int i = bottom; i < range; i++)
        {
            if (reelOrder[searchPosition] == Symbols.CHERRY)
            {
                return true;
            }
            searchPosition = CalcReelPosition(searchPosition, 1);
            
        }

        //ここにシンボル
        

        return false;
    }

    //指定したリールのポジションでリーチのシンボルを取得する
    //searchPositionは探索場所をTOP,MIDDLE,BOTTOMを代入する
    private static Symbols GetReachSymbolsForPositions(Reels selectReel,Positions searchPosition)
    {
        bool otherReelIsMoving = true;
        Symbols reachSymbols = Symbols.NONE;


        switch (selectReel)
        {
            case Reels.LEFT:
                otherReelIsMoving = centerReelMoving; //centerReelMovingを代入する
                otherReelIsMoving |= rightReelMoving; //oterReelIsMovingかrightReelMovingがtrueの時trueを代入する
                break;
            case Reels.CENTER:
                otherReelIsMoving = leftReelMoving;
                otherReelIsMoving |= rightReelMoving;
                break;
            case Reels.RIGHT:
                otherReelIsMoving = leftReelMoving;
                otherReelIsMoving |= centerReelMoving;
                break;
        }

        if (otherReelIsMoving) //選択外のリールが動いていた時NONEを返す
        {
            return Symbols.NONE;
        }


        if(selectReel == Reels.LEFT)
        {
            switch (searchPosition)
            {
                case Positions.TOP:
                    reachSymbols = GetReachSymbolForLine(selectReel, Lines.upperToLower);
                    reachSymbols |= GetReachSymbolForLine(selectReel, Lines.upperToUpper);
                    break;
                case Positions.MIDDLE:
                    reachSymbols = GetReachSymbolForLine(selectReel, Lines.middleToMiddle);
                    break;
                case Positions.BOTTOM:
                    reachSymbols = GetReachSymbolForLine(selectReel, Lines.lowerToLower);
                    reachSymbols |= GetReachSymbolForLine(selectReel, Lines.lowerToUpper);
                    break;
            }
        }

        if (selectReel == Reels.CENTER)
        {
            switch (searchPosition)
            {
                case Positions.TOP:
                    reachSymbols = GetReachSymbolForLine(selectReel, Lines.upperToUpper);
                    break;
                case Positions.MIDDLE:
                    reachSymbols = GetReachSymbolForLine(selectReel, Lines.upperToLower);
                    reachSymbols |= GetReachSymbolForLine(selectReel, Lines.middleToMiddle);
                    reachSymbols |= GetReachSymbolForLine(selectReel, Lines.lowerToUpper);
                    break;
                case Positions.BOTTOM:
                    reachSymbols |= GetReachSymbolForLine(selectReel, Lines.lowerToLower);
                    break;
            }
        }

        if (selectReel == Reels.RIGHT)
        {
            switch (searchPosition)
            {
                case Positions.TOP:
                    reachSymbols = GetReachSymbolForLine(selectReel, Lines.lowerToUpper);
                    reachSymbols |= GetReachSymbolForLine(selectReel, Lines.upperToUpper);
                    break;
                case Positions.MIDDLE:
                    reachSymbols = GetReachSymbolForLine(selectReel, Lines.middleToMiddle);
                    break;
                case Positions.BOTTOM:
                    reachSymbols = GetReachSymbolForLine(selectReel, Lines.lowerToLower);
                    reachSymbols |= GetReachSymbolForLine(selectReel, Lines.upperToLower);
                    break;
            }
        }

        return reachSymbols;
    }

    enum Lines :int
    {
        NONE = 0,
        upperToLower = 1, //左上から右下
        upperToUpper = 2, //左上から右上
        middleToMiddle = 4, //左中から右中
        lowerToLower = 8, //左下から右下
        lowerToUpper = 16, //左下から右上
    }

    //入ったら当選するシンボルを指定したLinesを元に取得する
    //左リールを基準にし、upperToLowerは左上から右下
    private static Symbols GetReachSymbolForLine(Reels selectReel , Lines line )
    {

        Symbols reachSymbol = Symbols.NONE; //リーチになっているシンボルが入る,OR演算で複数代入可能
        Symbols lineSymbols = Symbols.NONE; //選択したライン上にあるシンボルを代入する

        Reels stoppedReels = Reels.NONE;

        switch(selectReel)
        {
            case Reels.LEFT:
                stoppedReels = Reels.CENTER | Reels.RIGHT;
                break;
            case Reels.CENTER:
                stoppedReels = Reels.LEFT | Reels.RIGHT;
                break;
            case Reels.RIGHT:
                stoppedReels = Reels.LEFT | Reels.CENTER;
                break;
        }

        if (stoppedReels.HasFlag(Reels.LEFT)) //止まっているリールに左リールが含まれている時
        {
            switch(line)
            {
                case Lines.upperToLower: //左上から右下のライン
                    lineSymbols = leftReel[CalcReelPosition(nowLeftReel, 2)];
                    break;
                case Lines.upperToUpper: //左上から右上のライン
                    lineSymbols = leftReel[CalcReelPosition(nowLeftReel, 2)];
                    break;
                case Lines.middleToMiddle: //左中から右中のライン
                    lineSymbols = leftReel[CalcReelPosition(nowLeftReel, 1)];
                    break;
                case Lines.lowerToLower: //左下から右下のライン
                    lineSymbols = leftReel[nowLeftReel];
                    break;
                case Lines.lowerToUpper: //左下から右上のライン
                    lineSymbols = leftReel[nowLeftReel];
                    break;
            }
        }

        if (stoppedReels.HasFlag(Reels.CENTER)) //止まっているリールに中央リールが含まれている時
        {
            switch (line)
            {
                case Lines.upperToLower:
                    lineSymbols |= centerReel[CalcReelPosition(nowCenterReel, 1)]; // "|="で左辺と右辺のOR演算の結果を代入する
                    break;
                case Lines.upperToUpper:
                    lineSymbols |= centerReel[CalcReelPosition(nowCenterReel, 2)];
                    break;
                case Lines.middleToMiddle:
                    lineSymbols |= centerReel[CalcReelPosition(nowCenterReel, 1)];
                    break;
                case Lines.lowerToLower:
                    lineSymbols |= centerReel[CalcReelPosition(nowCenterReel, 1)];
                    break;
                case Lines.lowerToUpper:
                    lineSymbols |= centerReel[nowCenterReel];
                    break;
            }
        }

        if (stoppedReels.HasFlag(Reels.LEFT)) //止まっているリールに左リールが含まれている時
        {
            switch (line)
            {
                case Lines.upperToLower: //左上から右下のライン
                    lineSymbols |= rightReel[nowRightReel];
                    break;
                case Lines.upperToUpper: //左上から右上のライン
                    lineSymbols |= rightReel[CalcReelPosition(nowRightReel, 2)];
                    break;
                case Lines.middleToMiddle: //左中から右中のライン
                    lineSymbols |= rightReel[CalcReelPosition(nowRightReel, 1)];
                    break;
                case Lines.lowerToLower: //左下から右下のライン
                    lineSymbols |= rightReel[nowRightReel];
                    break;
                case Lines.lowerToUpper: //左下から右上のライン
                    lineSymbols |= rightReel[CalcReelPosition(nowRightReel, 2)];
                    break;
            }
        }

        switch (lineSymbols)
        {
            case Symbols.BELL:
            case Symbols.REPLAY:
            case Symbols.WATERMELON:
            case Symbols.CHERRY: 
            case Symbols.BAR:
            case Symbols.SEVEN: //フォールスルー　ここから上のcaseの場合以下の処理をbreakまで実行する
                reachSymbol = lineSymbols;
                break;

            default:
                reachSymbol = Symbols.NONE;
                break;
        }

        if(lineSymbols.HasFlag(Symbols.BAR) && lineSymbols.HasFlag(Symbols.SEVEN)) //リーチ目になりそうな状態 
        {
            reachSymbol = Symbols.REACH; //REGが来た場合、優先で7を入れること BIGがきた場合は7以外を揃えること
        }



        return reachSymbol;
    }

}