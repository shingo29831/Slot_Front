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

    //入ったら当選するシンボルを取得するtimeはFIRSTかSECONDが入り探索で1or2つ目に発見したか(正確には上から探索or下から探索)
    //positionは探索場所をTOP,MIDDLE,BOTTOMを代入する
    private static Symbols GetReachSymbol(Reels selectReel,Positions searchPosition,Times time)
    {
        sbyte startPosition = 0;
        bool isFromTop = true;
        sbyte difference = 1;
        Positions nextPosition = Positions.NONE;
        bool otherReelIsMoving = true;


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




        switch (time)
        {
            case Times.FIRST:
                difference = -1;
                startPosition = 2;
                isFromTop = true;
                break;
            case Times.SECOND:
                difference = 1;
                startPosition = 0;
                isFromTop = false;
                break;
        }


        for (int i = 0; i < 3; i++)
        {

        }

        return Symbols.NONE;
    }

    //入ったら当選するシンボルを指定したライン(TOP,MIDDLE,BOTTOM)を元に取得する
    //基本左リールを基準に選択したラインを探索TOPなら左リールの上、BOTTOMなら左リールの下。
    //左リールのシンボルを特定する場合は右リールを基準に探索する
    private static Symbols GetReachSymbolForLine(Reels selectReel ,Positions searchPosition , Positions line )
    {
        sbyte next = 0;
        bool isLeft = false;
        bool isCenter = false;
        bool isRight = false;

        bool isParallel = false;
        bool isNone = false;

        bool isTop = false;
        bool isMiddle = false;
        bool isBottom = false;

        Symbols lineFirstSymbol = Symbols.NONE;
        Symbols lineSecondSymbol = Symbols.NONE;

        Reels firstReel = Reels.NONE;
        Reels secondReel = Reels.NONE;

        switch(selectReel)
        {
            case Reels.LEFT:
                isLeft = true;
                break;
            case Reels.CENTER:
                isCenter = true;
                break;
            case Reels.RIGHT:
                isRight = true;
                break;
        }

        switch (searchPosition) //
        {
            case Positions.TOP:
                isTop = true;
                break;
            case Positions.MIDDLE:
                isMiddle = true;
                break;
            case Positions.BOTTOM:
                isBottom = true;
                break;
        }

        if (searchPosition == line) //探索ラインが特定するポジションと平行か否か
        {
            isParallel = true;
            switch (searchPosition) //平行の時比較先は全て同じ位置なので全ての上中下の位置を計算するための値を代入
            {
                case Positions.TOP:
                    next = 2;
                    break;
                case Positions.MIDDLE:
                    next = 1;
                    break;
                case Positions.BOTTOM:
                    next = 0;
                    break;
            }

            switch (selectReel) //選択外のリールのシンボルを比べるために代入する
            {
                case Reels.LEFT:
                    lineFirstSymbol = ReelOrder.centerReelOrder[CalcReelPosition(nowCenterReel, next)];
                    lineSecondSymbol = ReelOrder.rightReelOrder[CalcReelPosition(nowRightReel, next)];
                    break;
                case Reels.CENTER:
                    lineFirstSymbol = ReelOrder.leftReelOrder[CalcReelPosition(nowLeftReel, next)];
                    lineSecondSymbol = ReelOrder.rightReelOrder[CalcReelPosition(nowRightReel, next)];
                    break;
                case Reels.RIGHT:
                    lineFirstSymbol = ReelOrder.leftReelOrder[CalcReelPosition(nowLeftReel, next)];
                    lineSecondSymbol = ReelOrder.centerReelOrder[CalcReelPosition(nowCenterReel, next)];
                    break;
            }
        }


        if (isParallel == false && isLeft )
        {
            
        }

        if(isParallel && lineFirstSymbol == lineSecondSymbol) //平行なシンボルの並びを探索する場合二つとも同じシンボルの時、それを返す
        {
            return lineFirstSymbol;
        }

        if(isParallel == false && isCenter && searchPosition == Positions.MIDDLE && lineFirstSymbol == lineSecondSymbol) //特定するリールが中央の時に平行ではないならMIDDLEのみ比較する
        {
            return lineFirstSymbol;
        }

        if(isParallel == false && (isLeft || isRight) && line == Positions.MIDDLE) //特定するラインが平行でない時に特定するリールが左右のどちらかで探索ラインがMIDDLEの時NONEを返す
        {
            return Symbols.NONE;
        }


        return Symbols.NONE;
    }

}